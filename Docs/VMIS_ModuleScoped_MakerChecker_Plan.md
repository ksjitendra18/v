# Module-Scoped Maker–Checker — Design Plan

**Requirement:** different checkers for different modules. Vigilance and IAC share one set of
checkers; Complaint and MISC share another.
**Prepared:** 2026-08-13
**Status:** design — not implemented
**Companions:** `VMIS_MakerChecker_Rollout_Plan.md`, `VMIS_IAC_MakerChecker_Implementation.md`,
`VMIS_Vigilance_MakerChecker_Implementation.md`

---

## 1. The problem in one line

**Checker authorisation today is scoped by zone only. It has no module dimension at all.**

`MakerCheckerMapping` is `(UserPF, ZoneSolID, IsMaker, IsChecker, IsActive)`. Every place that
asks "may this user check this record?" asks only "is this user an active checker for this
record's zone?":

```sql
-- spCase_CheckerAction, guard 6
IF NOT EXISTS (SELECT 1 FROM dbo.MakerCheckerMapping
               WHERE UserPF = @p_USER AND ZoneSolID = @ZONE
                 AND IsChecker = 1 AND IsActive = 1)
```

So a checker mapped to zone `100002` today automatically checks **every** module in that
zone — IAC, Vigilance, Complaint, and every module added later. There is no way to say
"this person checks Vigilance and IAC but not Complaint."

That is exactly what the new requirement asks for.

---

## 2. Where the module dimension has to be added

Six call sites. Missing any one of them leaves a hole rather than a control — the two marked
🔴 are authorisation, the rest are visibility.

| # | Call site | What it does today | Kind |
|---|---|---|---|
| 1 | `spCase_CheckerAction` guard 6 | zone-only `EXISTS` on `MakerCheckerMapping` | 🔴 **authorisation** |
| 2 | `frmComplaintCheckerView.aspx.cs` → `spComplaint_CheckerAction` | Complaint's own proc, same zone-only check | 🔴 **authorisation** |
| 3 | `spCase_CheckerQueue` | inbox — `INNER JOIN MakerCheckerMapping ON ZoneSolID` | visibility |
| 4 | `frmComplaintChecker.aspx.cs` (inline SQL, lines 33–38) | Complaint inbox, same join | visibility |
| 5 | `Default.aspx.cs` `GetOtherPendingCheckerCounts` + `GetPendingComplaintCount` | landing-page pending badges, same join | visibility |
| 6 | `vw_CASE_APPROVAL_ORPHANS` | "no active checker mapped to this zone" — must become "…for this **module** in this zone" | monitor |

Plus two application-side pieces:

| # | Piece | Change |
|---|---|---|
| 7 | `Admin/UserCreation.aspx` + `.cs` (`SaveMakerCheckerMapping`, `LoadCheckerZones`, `DeactivateCheckerMappings`) | today a zone `CheckBoxList` only — needs a module-group selector |
| 8 | `Web.sitemap` | a Vigilance-only checker still sees the Complaint and IAC inbox nodes; role trimming cannot express module scope |

> **Note on #6.** The orphan monitor is not cosmetic. Once mappings are module-scoped, a
> record can be pending in a zone that *has* checkers — just not for its module. That row is
> invisible to every inbox **and** locked from editing by the maker. The current view would
> report it as healthy. Module-scoping the monitor is part of the change, not a follow-up.

---

## 3. Three options

### Option A — `ModuleCode` column on `MakerCheckerMapping`

Grain becomes `(UserPF, ZoneSolID, ModuleCode)`.

- Most direct, finest-grained.
- Row count is `users × zones × modules`. A checker covering 5 zones × 14 modules is 70 rows,
  and the admin screen has to manage them.
- The stated requirement is about *groups of modules moving together*. Expressing "Vigilance
  and IAC are checked by the same people" as 2× rows per user per zone means the pairing lives
  nowhere in the data — it is a convention the admin must re-apply every time, and it drifts.

### Option B — module **group**, mapping keyed by group ✅ recommended

Modules are assigned to a **checker group**; mappings are keyed `(UserPF, ZoneSolID, GroupCode)`.

- Reads exactly like the requirement: group `VIG` = {VIGILANCE, IAC}, group `CMP` = {COMPLAINT, MISC}.
- Row count is `users × zones × groups` — with 2–4 groups this stays small and the admin screen
  stays usable.
- Re-grouping later (e.g. splitting RTI out of a group) is one `UPDATE` on `WORKFLOW_MODULE`,
  not a re-mapping of every user.
- A module needing its own distinct checkers simply gets a single-module group. **Option B can
  express everything Option A can** — it just costs one row in `WORKFLOW_MODULE_GROUP` to do it.

### Option C — group by default, per-module override

Option B plus an optional `ModuleCode` on the mapping row that narrows a grant to one module.

- Maximum flexibility, two resolution rules to reason about, and an admin screen that has to
  explain the difference. **Not recommended now** — Option B's single-module-group escape hatch
  covers the same ground without a second mechanism. It stays available later; Option B's schema
  is a strict subset.

**Recommendation: Option B.**

---

## 4. Recommended design

### 4.1 New table — `dbo.WORKFLOW_MODULE_GROUP`

```sql
CREATE TABLE dbo.WORKFLOW_MODULE_GROUP
(
    GroupCode  varchar(20)  NOT NULL PRIMARY KEY,   -- 'VIG', 'CMP'
    GroupName  varchar(100) NOT NULL,               -- 'Vigilance & IAC'  (shown in the admin screen)
    IsActive   bit          NOT NULL DEFAULT(1)
);
```

Seed:

| GroupCode | GroupName | Modules |
|---|---|---|
| `VIG` | Vigilance & IAC | `VIGILANCE`, `IAC` |
| `CMP` | Complaint & MISC | `COMPLAINT`, `MISC` |

`COMPLAINT` and `MISC` are not in `WORKFLOW_MODULE` yet — Complaint is still on its inline-columns
mechanism and MISC is not built. See §7.

### 4.2 `dbo.WORKFLOW_MODULE` — one new column

```sql
ALTER TABLE dbo.WORKFLOW_MODULE
    ADD GroupCode varchar(20) NULL
        CONSTRAINT FK_WORKFLOW_MODULE_GROUP REFERENCES dbo.WORKFLOW_MODULE_GROUP(GroupCode);
```

Nullable for the deployment step only; made `NOT NULL` once every registered module is assigned
(same script, after the `UPDATE`s). **Every module must belong to exactly one group** — a module
with no group can be checked by nobody, which is the silent-limbo failure this design exists to
avoid. The `NOT NULL` is what enforces that going forward.

### 4.3 `dbo.MakerCheckerMapping` — one new column, new unique key

```sql
ALTER TABLE dbo.MakerCheckerMapping ADD GroupCode varchar(20) NULL;   -- FK added after backfill

-- replaces UQ_MakerCheckerMapping (UserPF, ZoneSolID)
ALTER TABLE dbo.MakerCheckerMapping DROP CONSTRAINT UQ_MakerCheckerMapping;
ALTER TABLE dbo.MakerCheckerMapping
    ADD CONSTRAINT UQ_MakerCheckerMapping UNIQUE (UserPF, ZoneSolID, GroupCode);

DROP INDEX IX_MakerCheckerMapping_Lookup ON dbo.MakerCheckerMapping;
CREATE INDEX IX_MakerCheckerMapping_Lookup
    ON dbo.MakerCheckerMapping (UserPF, IsChecker, IsActive) INCLUDE (ZoneSolID, GroupCode);
```

**Backfill — fan out, do not leave NULL.** Every existing active checker row is expanded into one
row per active group, preserving today's behaviour exactly:

```sql
-- existing (UserPF, Zone) -> (UserPF, Zone, every active group)
INSERT INTO dbo.MakerCheckerMapping (UserId, UserPF, ZoneSolID, GroupCode, IsMaker, IsChecker, IsActive, CreatedBy)
SELECT M.UserId, M.UserPF, M.ZoneSolID, G.GroupCode, M.IsMaker, 1, 1, 'MODULE_SCOPE_MIGRATION'
FROM   dbo.MakerCheckerMapping M
       CROSS JOIN dbo.WORKFLOW_MODULE_GROUP G
WHERE  M.IsChecker = 1 AND M.IsActive = 1 AND M.GroupCode IS NULL AND G.IsActive = 1;

-- the old ungrouped rows are then retired
UPDATE dbo.MakerCheckerMapping SET IsActive = 0 WHERE GroupCode IS NULL AND IsChecker = 1;
```

> **Why fan out rather than treat `NULL` as "all groups".** A `NULL`-means-everything rule would
> make the deployment a no-op, but it leaves two authorisation semantics live at once, and every
> one of the six call sites in §2 would have to carry `(GroupCode IS NULL OR GroupCode = …)`
> forever. One forgotten `OR` is a silent authorisation hole. Fanning out makes every grant
> explicit, the query single-form, and the admin screen show the truth. **After migration,
> `GroupCode NULL` on an active checker row means nothing is granted — fail closed.**
>
> The cost is that a business wanting the *new* restriction must go and untick groups per user
> after deployment. That is the right default: deployment changes no one's access, and every
> narrowing is a deliberate, audited admin action.

### 4.4 One resolution function — the heart of this change

Rather than repeat the predicate at six call sites, define it once:

```sql
CREATE OR ALTER FUNCTION dbo.fnCheckerScope (@p_USER varchar(50))
RETURNS TABLE
AS
RETURN
    SELECT  WM.ModuleCode,
            M.ZoneSolID
    FROM    dbo.MakerCheckerMapping M
            INNER JOIN dbo.WORKFLOW_MODULE_GROUP G
                    ON G.GroupCode = M.GroupCode AND G.IsActive = 1
            INNER JOIN dbo.WORKFLOW_MODULE WM
                    ON WM.GroupCode = G.GroupCode AND WM.IsActive = 1
    WHERE   M.UserPF    = @p_USER
      AND   M.IsChecker = 1
      AND   M.IsActive  = 1;
```

An inline TVF, so it inlines into the caller's plan — no performance cost over the hand-written
join. Every call site in §2 becomes a join to `dbo.fnCheckerScope(@user)` on
`(ModuleCode, ZoneSolID)`. **When a module is re-grouped or a group is deactivated, all six
call sites change together, because there is only one of them.**

Complaint is not in `WORKFLOW_MODULE` yet, so call sites 2 and 4 need the literal `'COMPLAINT'`
until it is registered — see §7.

### 4.5 Resulting authorisation rule

> A user may act on a record if they hold an **active checker mapping** for **that record's zone**
> **in the group that owns that record's module**.

Every existing guard stays exactly as it is — self-approval block, still-pending check,
transaction, mandatory remarks. This change narrows *one* guard; it removes none.

---

## 5. Application changes

### 5.1 `Admin/UserCreation.aspx` — the admin screen

Today: a single `chkZones` `CheckBoxList`, saved by `SaveMakerCheckerMapping` as one row per
ticked zone.

Needed: **module group × zone**. Two workable shapes —

| Shape | Description | Trade-off |
|---|---|---|
| **Two lists** (recommended) | A `chkModuleGroups` list above the existing `chkZones` list. Save writes the cross product | Simple, matches the existing markup, one extra control. Cannot express "Vigilance in zone A, Complaint in zone B" for one user |
| **Grid** | Groups as rows, zones as columns, a checkbox per cell | Fully expressive; a much bigger UI change and awkward with many zones |

Start with two lists. The grid is only needed if the business says one person must check
different modules in different zones — **worth confirming before building** (§9, Q2).

`SaveMakerCheckerMapping` also needs the fix already noted in the rollout plan: it does
`SELECT COUNT(*)` then UPDATE-or-INSERT per row, which is a race. With the cross product it fires
`groups × zones` times. Replace the whole loop with **one `MERGE`** against a table-valued
parameter or a values list — fewer round trips and race-free.

`LoadCheckerZones` becomes `LoadCheckerScope` and ticks both lists.
`DeactivateCheckerMappings` needs no change — it already clears every row for the user.

### 5.2 Menu visibility (`Web.sitemap`)

Security trimming is role-based. A Vigilance-only checker will still see the Complaint and IAC
inbox nodes and get an empty (or refusing) page. Three ways out:

| Approach | Notes |
|---|---|
| **Leave it** | Inboxes are empty and `spCase_CheckerAction` refuses. Untidy, not a security hole |
| **Per-group roles** — `VMIS_CHECKER_VIG`, `VMIS_CHECKER_CMP` | Trimming works natively, but role membership and `MakerCheckerMapping` are now two sources of truth that can disagree. **Not recommended** |
| **Runtime trimming** — a `SiteMapProvider` filter, or hide nodes in the master page from `fnCheckerScope` | One source of truth. More code, correct behaviour |

Recommended: ship with "leave it", add runtime trimming as a follow-up. It is cosmetic and should
not gate the authorisation change.

### 5.3 `Default.aspx.cs`

Both count queries join `MakerCheckerMapping` on zone alone — repoint both at `fnCheckerScope`.
The `checkerInboxPages` dictionary is unaffected.

---

## 6. Deliverables

### New

| File | Contents |
|---|---|
| `Database/Scripts/2026-08-XX_MakerChecker_ModuleScope.sql` | `WORKFLOW_MODULE_GROUP`; `GroupCode` on `WORKFLOW_MODULE` and `MakerCheckerMapping`; seed; backfill; new unique key + index; `fnCheckerScope`; repointed `spCase_CheckerAction`, `spCase_CheckerQueue`, `vw_CASE_APPROVAL_ORPHANS`. Idempotent, one script |
| `Docs/VMIS_ModuleScoped_MakerChecker_Implementation.md` | Implementation record, written on delivery |

### Modified

| File | Change |
|---|---|
| `VMISP/Admin/UserCreation.aspx` | `chkModuleGroups` CheckBoxList |
| `VMISP/Admin/UserCreation.aspx.cs` | Bind groups; `SaveMakerCheckerMapping` → group × zone via `MERGE`; `LoadCheckerZones` → `LoadCheckerScope` |
| `VMISP/Default.aspx.cs` | Both count queries → `fnCheckerScope` |
| `VMISP/Mis/frmComplaintChecker.aspx.cs` | Inline inbox SQL → `fnCheckerScope`, module `'COMPLAINT'` |
| `VMISP/Mis/frmComplaintCheckerView.aspx.cs` / `spComplaint_CheckerAction` | Module-scope the authorisation guard |

Not touched: the IAC and Vigilance checker pages. They call `spCase_CheckerQueue` and
`spCase_CheckerAction`, so they inherit the new scoping with no code change — which is the
payoff of the central design.

---

## 7. Complaint and MISC — the wrinkle

The requirement pairs **Complaint with MISC**, and neither is on the central registry:

- **Complaint** still uses the six inline columns, its own `spComplaint_CheckerAction`, and
  inline SQL in `frmComplaintChecker.aspx.cs`.
- **MISC** has no maker–checker workflow at all yet.

Two ways to satisfy the requirement:

| | Path 1 — scope in place | Path 2 — migrate Complaint first ✅ |
|---|---|---|
| Work | Add the group check to `spComplaint_CheckerAction` and the two Complaint pages separately, hard-coding `'COMPLAINT'` | Migrate Complaint onto `CASE_APPROVAL` (recipe already written: `VMIS_IAC_MakerChecker_Implementation.md` §"Migrating Complaint"), then group-scoping is free |
| Result | Two authorisation mechanisms, both needing every future change applied twice | One mechanism, one inbox, one guard |
| Effort | Lower now | Higher now, lower from then on |

**Recommendation: Path 2**, since the migration is already specified and is the prerequisite for
retiring the duplicate mechanism anyway. If the requirement is urgent, Path 1 is a valid stopgap
— but it should be booked as debt, not treated as done.

**MISC still has to be built** (rollout plan §2.1, recipe in the IAC doc §9) before "Complaint and
MISC share checkers" is more than a configuration waiting for a module. **Group `CMP` will contain
only Complaint until then** — which is correct and harmless; the group is ready when MISC lands.

Suggested sequence: **module scoping (this plan) → migrate Complaint → build MISC**. Scoping
first means Vigilance/IAC get the real control immediately, and both later pieces arrive already
scoped rather than needing a second pass.

---

## 8. Test matrix

Zones `Z1`, `Z2`. Checker `C1` mapped to `VIG`/`Z1`. Checker `C2` mapped to `CMP`/`Z1`.
Maker `M1` (never the same user as the checker).

| # | Scenario | Expected |
|---|---|---|
| 1 | `C1` opens the IAC inbox | Z1 IAC records listed |
| 2 | `C1` opens the Vigilance inbox | Z1 Vigilance records listed |
| 3 | `C1` opens the Complaint inbox | **empty** |
| 4 | `C1` posts a Complaint action directly (URL/postback, bypassing the menu) | **refused** — "not authorized" |
| 5 | `C2` opens the IAC and Vigilance inboxes | **both empty** |
| 6 | `C2` actions a Complaint record in Z1 | success |
| 7 | `C1` actions an IAC record in **Z2** | **refused** — zone scoping still applies |
| 8 | `C1` = maker on their own Z1 IAC record | **refused** — self-approval guard intact |
| 9 | `Default.aspx` badges for `C1` | IAC + Vigilance counts only, no Complaint |
| 10 | Pending Vigilance record in a zone with only `CMP` checkers | appears in `vw_CASE_APPROVAL_ORPHANS` |
| 11 | Group `VIG` set `IsActive = 0` | `C1`'s inboxes empty, actions refused; records show as orphans |
| 12 | Pre-migration checker, post-deployment, no admin edit | access **unchanged** — sees every module, as before |
| 13 | Admin unticks `CMP` for a user, saves | that user's Complaint access stops immediately; `VIG` unaffected |
| 14 | Two admins save the same user concurrently | no duplicate rows (`MERGE` + unique key) |
| 15 | Module registered with `GroupCode` NULL | insert **refused** by `NOT NULL` — cannot create an uncheckable module |

Run 1–11 in SQL against `VigilanceMISDB` first; 12–15 need the admin screen.

---

## 9. Decisions needed before build

1. **Confirm the groups.** `VIG` = {Vigilance, IAC} and `CMP` = {Complaint, MISC} is what was
   stated. Where do the remaining 12 modules go as they roll out — RTI, NOC, RRB, Vigilance
   Monitoring, the Sanction trio, and the rest? A default of "one group per module" is safest;
   they can be merged later with one `UPDATE`.
2. **Can one user check different modules in different zones?** ("Vigilance in Delhi, Complaint in
   Mumbai.") **No** → two checkbox lists, simple screen. **Yes** → a group × zone grid. This is
   the single biggest driver of UI effort — worth answering before anything is built.
3. **Complaint: Path 1 or Path 2** (§7)? Recommendation is Path 2.
4. **Migration default.** Existing checkers keep access to everything until an admin narrows them
   (§4.3). Confirm — the alternative is that every checker loses access at deployment until
   re-mapped, which needs the admin work scheduled *with* the release.
5. **Should the maker side be scoped too?** This plan scopes checkers only. Makers are controlled
   by the `VMIS_MISUSER` role and the sitemap — any MISUSER can enter any module in any zone.
   `MakerCheckerMapping.IsMaker` exists but is written as `0` everywhere and is read by nothing.
   If module-scoped *makers* are also wanted, that is a separate piece of work and should be said
   now, because the same `fnCheckerScope` shape would serve it.
6. **Group naming** as it appears to admins — `GroupName` is the only label they see.

---

## 10. Effort

Assumes decisions in §9 are settled and Path 2 (§7) is deferred to its own piece of work.

| Piece | Estimate |
|---|---|
| SQL script — tables, backfill, `fnCheckerScope`, 3 repointed objects | 1 day |
| `UserCreation.aspx` two-list version + `MERGE` rewrite | 1–1.5 days |
| `Default.aspx.cs` + Complaint call sites (Path 1 stopgap) | 0.5 day |
| Test matrix §8 | 1 day |
| Documentation | 0.5 day |
| **Total** | **≈ 4–4.5 days** |

Add ~2 days if §9 Q2 is "yes" (grid UI). Migrating Complaint (Path 2) and building MISC are
separate, already-specified pieces of work and are **not** in this estimate.
