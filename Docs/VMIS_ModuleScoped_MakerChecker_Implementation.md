# Module-Scoped Maker–Checker + MISC — Implementation Record

**Delivered:** 2026-08-13
**Database:** `VigilanceMISDB`
**Implements:** `VMIS_ModuleScoped_MakerChecker_Plan.md` (Option B, the recommended design)
**Companions:** `VMIS_IAC_MakerChecker_Implementation.md`, `VMIS_Vigilance_MakerChecker_Implementation.md`,
`VMIS_MakerChecker_Rollout_Plan.md`

---

## 1. What this delivers

Two things, in one release:

1. **Checker authorisation now has a module dimension.** Vigilance and IAC are checked by one set
   of people; Complaint and MISC by another. Previously a checker mapped to a zone automatically
   checked *every* module in that zone.
2. **MISC is on the maker–checker workflow** — the fourth module, and the first to arrive already
   module-scoped.

---

## 2. How an admin manages it

`Admin/UserCreation.aspx`, exactly where checkers were already created. Search a PF number or
enter a new one, set **Location/Role = CHECKER**, and two lists appear:

| Control | What it is |
|---|---|
| **Modules to Check** | The checker groups. Each shows its member modules — "Vigilance & IAC (IAC, Vigilance)" — so the admin never has to know the module registry |
| **Zones** | Unchanged from before: the SOL-coded zones, from `spCircleMaster_Ddl` |

A grant is the **cross product**: ticking *Vigilance & IAC* plus two zones grants that user those
two modules in those two zones, and nothing else. Both lists open pre-ticked with what the user
currently holds, so the screen shows the truth rather than an empty form.

Saving with a group but no zone (or vice versa) is refused with a message, because that
combination grants nothing and would leave the checker staring at an empty inbox with no
explanation.

To **revoke** a module group, untick it and save — the mapping is deactivated, not deleted, so
the row remains as the record of what was granted. Changing the role away from CHECKER still
removes the role and deactivates every mapping, as before.

**To change which modules travel together** — say, to split RTI into its own group later — no
admin work is needed at all: it is one `UPDATE` on `WORKFLOW_MODULE.GroupCode`, and every
inbox, badge and authorisation check follows immediately (§4).

---

## 3. Deployment

Run in order against `VigilanceMISDB`:

```
1. Database/Scripts/2026-08-13_MakerChecker_ModuleScope.sql
2. Database/Scripts/2026-08-13_MISC_MakerChecker.sql
```

Then build and deploy the application. Both scripts are idempotent and safe to re-run — with one
caveat: if step 3 of script 1 *errors*, inspect `MakerCheckerMapping` before re-running (see the
note at the end of §9).

> ### ⚠️ `QUOTED_IDENTIFIER` must be ON when these scripts run
>
> Both scripts now begin with `SET QUOTED_IDENTIFIER ON; SET ANSI_NULLS ON;`, and that must not be
> removed. The setting is **captured into each procedure's metadata at creation time** and applies
> whenever it runs, regardless of the caller. `System.Data.SqlClient` connects with
> `QUOTED_IDENTIFIER ON`, so a module created with it OFF fails at runtime with:
>
> ```
> SELECT failed because the following SET options have incorrect settings: 'QUOTED_IDENTIFIER'.
> ```
>
> **`sqlcmd` defaults this to OFF** (SSMS defaults to ON). If deploying with sqlcmd, pass `-I` as
> well. After deploying, this must return no rows:
>
> ```sql
> SELECT o.name FROM sys.sql_modules m JOIN sys.objects o ON o.object_id = m.object_id
> WHERE m.uses_quoted_identifier = 0;
> ```

**Already applied to the development database** (`localhost\SQLEXPRESS`, `VigilanceMISDB`) on
2026-08-13 and exercised through 20 scenarios — see §9.

**Nobody's access changes at deployment.** Every existing active checker grant is fanned out to
one row per group, so a checker who saw every module yesterday still does. Narrowing a user is
then a deliberate admin action on the screen above. Details and rationale in §5.2.

### Three decisions baked in — change them here if the business disagrees

| Decision | Where | Alternative |
|---|---|---|
| **Existing checkers keep access to everything** until an admin narrows them | script 1, step 3a | Skip the fan-out and every checker loses access until re-mapped — schedule the admin work *with* the release if that is wanted |
| **Pre-existing MISC records are grandfathered Approved** | script 2, step 2 | Change the `'A'` literal to `'P'`. That floods the checker inbox and locks every existing MISC record from editing |
| **Bulk-imported MISC records are exempt from checking** (`ImportApprovalStatus = 'A'`) | script 2, step 1 | See §7 — this one has a prerequisite, do not just flip it |

---

## 4. The design, in one function

Modules belong to a **checker group**; a checker is granted a group in a zone.

```
WORKFLOW_MODULE_GROUP      VIG = 'Vigilance & IAC'      CMP = 'Complaint & MISC'
WORKFLOW_MODULE.GroupCode  IAC->VIG  VIGILANCE->VIG  COMPLAINT->CMP  MISC->CMP
MakerCheckerMapping.GroupCode   which group a grant covers
```

Six places used to ask "is this user a checker for this zone?". They now all resolve through one
inline table-valued function:

```sql
SELECT ModuleCode, ZoneSolID, GroupCode FROM dbo.fnCheckerScope(@UserPF)
```

| # | Call site | Kind |
|---|---|---|
| 1 | `spCase_CheckerAction` guard 6 | 🔴 authorisation |
| 2 | `spComplaint_CheckerAction` | 🔴 authorisation |
| 3 | `spCase_CheckerQueue` | inbox |
| 4 | `frmComplaintChecker.aspx.cs` | inbox |
| 5 | `Default.aspx.cs` × 2 | landing-page badges |
| 6 | `vw_CASE_APPROVAL_ORPHANS` | monitor |

**Having one definition is the point.** When a module moves group or a group is deactivated,
there is one place for that to take effect rather than six — and no way to update five of them.

The function is an inline TVF, so it folds into each caller's query plan; there is no measurable
cost over the hand-written join it replaced.

### Why groups rather than a `ModuleCode` on the mapping

The requirement is about modules *moving together*. A group says that in the data; a per-module
key leaves it as a convention the admin re-applies by hand every time, and it drifts. Row counts
stay at `users × zones × groups` instead of `× modules`. A module needing its own distinct
checkers just gets a single-module group — so this expresses everything a per-module key could.
Any module registered without a group at deployment automatically got one (script 1, step 2).

### The orphan monitor had to change too

Once grants are module-scoped, a record can be pending in a zone that **has** checkers — just not
for its module. That record is invisible to every inbox *and* locked from editing by the maker.
The old zone-only view reported it as healthy. `vw_CASE_APPROVAL_ORPHANS` now distinguishes three
causes:

```sql
SELECT * FROM dbo.vw_CASE_APPROVAL_ORPHANS;
-- OrphanReason: 'No zone recorded on the case'
--             | 'No active checker mapped to this zone'
--             | 'This zone has checkers, but none for this module's group'
```

---

## 5. Database changes — script 1

### 5.1 New and altered objects

| Object | Change |
|---|---|
| `WORKFLOW_MODULE_GROUP` | **new** — `GroupCode`, `GroupName`, `IsActive`. Seeded `VIG`, `CMP` |
| `WORKFLOW_MODULE.GroupCode` | **new column**, `NOT NULL`, FK to the group table. A module with no group could be checked by nobody, so the constraint is what stops one being registered by accident |
| `WORKFLOW_MODULE` row `COMPLAINT` | **new registry row only.** Complaint's approval state still lives in its own six columns, not `CASE_APPROVAL`; the row exists so `fnCheckerScope` can return `'COMPLAINT'`. It has no `CASE_APPROVAL` rows, so it changes no count and no queue |
| `MakerCheckerMapping.GroupCode` | **new column** |
| `UQ_MakerCheckerMapping` | dropped; replaced by unique **index** `UQ_MakerCheckerMapping_Scope (UserPF, ZoneSolID, GroupCode)`. An index rather than a constraint because `GroupCode` is nullable on the retired pre-migration rows and a unique index treats NULLs as comparable — which is what is wanted |
| `IX_MakerCheckerMapping_Lookup` | rebuilt to include `GroupCode` |
| `fnCheckerScope` | **new** — the single resolution point |
| `spCheckerGroup_Ddl` / `spCheckerScope_Get` / `spCheckerScope_Save` | **new** — admin screen support |
| `spCase_CheckerAction`, `spCase_CheckerQueue`, `spComplaint_CheckerAction`, `vw_CASE_APPROVAL_ORPHANS` | repointed at `fnCheckerScope` |

Every other guard is untouched — self-approval block, still-pending re-check inside the
transaction, mandatory remarks, module-registered check. This change narrows *one* guard; it
removes none.

### 5.2 Migration — fan out, do not treat NULL as "all"

Each existing active checker row becomes one row per active group; the ungrouped originals are
deactivated.

A `NULL`-means-everything rule would have made deployment a no-op too, but it leaves two
authorisation semantics live at once and every call site carrying
`(GroupCode IS NULL OR GroupCode = …)` forever. **One forgotten `OR` is a silent authorisation
hole.** Fanning out makes every grant explicit, the query single-form, and the admin screen show
what is actually true. After migration, an active checker row with a `NULL` `GroupCode` grants
nothing — the model fails closed.

### 5.3 `spCheckerScope_Save` also fixes a live race

The old `SaveMakerCheckerMapping` did `SELECT COUNT(*)` then UPDATE-or-INSERT, once per zone. Two
concurrent saves produced duplicate mappings, and a duplicate mapping **duplicated every row in
that checker's inbox**. It is now one `MERGE` in one round trip, with the unique index making the
race impossible. Anything no longer ticked is deactivated by the same statement.

---

## 6. MISC maker–checker — script 2

Follows the recipe in `VMIS_IAC_MakerChecker_Implementation.md` §9. No column was added to `MISC`
or `MISC_HISTORY`, so the `INSERT INTO MISC_HISTORY SELECT * FROM MISC` statements stay ordinally
safe. MISC is registered in group **CMP** with `TableName/KeyColumn/RefColumn/ZoneColumn` =
`MISC` / `CODE` / `RNO` / `NEWZONE`.

### `spMiscStructure_Update`

**No parameter added or removed** — the existing C# call site works unchanged.

Insert wraps in a transaction, captures `SCOPE_IDENTITY()` and writes a `CASE_APPROVAL` row at
`'P'` plus a `SUBMITTED` audit row. Update reads the prior status first:

| Prior status | Behaviour |
|---|---|
| `'X'` Rejected | **Refused** — `@o_ERRCODE = 5` |
| `'P'` Pending | **Refused** — not the maker's to change while the checker holds it |
| `'C'` Changes Requested | proceeds → back to `'P'`, logs `RESUBMITTED` |
| `'A'` Approved | proceeds → back to `'P'`, logs `RESUBMITTED` (the edit invalidates the decision) |
| `NULL` no approval row | onboarded now at `'P'`, logs `SUBMITTED` |

On resubmit, `RecordRef` and `ZoneSolID` are refreshed from the form, so a zone change re-routes
the record.

**Zone (New) is now mandatory for a maker** (`@o_ERRCODE = 4`). The zone is the only thing that
routes a record to a checker; saved without one the record would sit `'P'` forever, in no inbox
and locked from editing. Refusing is the only outcome that is not a silent trap.

New `@o_ERRCODE` values: `4` zone missing, `5` record locked. Existing values unchanged.
The `VMIS_DESKUSER` branch is unchanged — desk-user annotations still bypass approval, as on
every other module.

### `spMiscStructure_View`

All three branches (`LIST`, `SEARCH`, single-record) `LEFT JOIN CASE_APPROVAL` and return
`APPROVALSTATUS`, `APPROVALSTATUSTEXT`, `CHECKERREMARKS`. `LEFT JOIN`, so records that predate
the workflow still list with a NULL status.

### `spMISCExcel_Import`

Registers each imported row and logs an `IMPORTED` audit row (kept distinct from `SUBMITTED` so
uploads can be told apart in reporting). Two optional parameters were **appended with defaults**
(`@p_NEWZONESOLID`, `@p_NEWCIRCLESOLID`), so the existing call site keeps working. It also now
opens a transaction only when `@@TRANCOUNT = 0` and never commits or rolls back one it did not
start.

One incidental correction: the history copy was `WHERE RNO = @p_RNO`, which could pick up a
soft-deleted row with the same R number. It is now keyed on the new `CODE`.

---

## 7. ⚠️ MISC imports are exempt from checking, and why

`WORKFLOW_MODULE.ImportApprovalStatus` is **`'A'` for MISC** — unlike IAC, which is `'P'`.

The MISC upload sheet has a free-text `ZONE` column but **no SOL-coded zone**; the import proc had
no zone parameter at all before this delivery. Under `'P'`, every imported row would land Pending
with no zone: in no inbox, and not editable by the maker. `'A'` registers them Approved with
`CheckerUser = 'SYSTEM'` and an audit note — an explicit, audited exemption rather than a silent
gap. Any later edit re-queues the record for verification like anything else.

**To make MISC uploads require checking:**

1. Add a `NEWZONESOLID` column (and optionally `NEWCIRCLESOLID`) to the MISC upload sheet. The
   proc and `funcExcelImport_MISC` already accept them — the C# passes them only when the column
   is present, so adding it needs no code change.
2. `UPDATE dbo.WORKFLOW_MODULE SET ImportApprovalStatus = 'P' WHERE ModuleCode = 'MISC';`

Rows without a zone are then refused (`@o_ERRCODE = -2`). **Do not do step 2 without step 1.**

Note also that the checker inbox still has **no bulk action** (carried gap, see §10), so
switching MISC to `'P'` puts every uploaded row in front of a checker one at a time.

---

## 8. Application changes

### New

| File | Purpose |
|---|---|
| `VMISP/Mis/frmMiscChecker.aspx` (+ `.cs`, `.designer.cs`) | MISC checker inbox — Bootstrap 5 card design, matching the IAC and Vigilance inboxes |
| `VMISP/Mis/frmMiscCheckerView.aspx` (+ `.cs`, `.designer.cs`) | Read-only MISC record + Accept / Push Back / Reject with mandatory remarks |

Both follow the settled UI convention (`VMIS_IAC_MakerChecker_Implementation.md` §6.0): the inbox
is Bootstrap 5; the verification page is Bootstrap 3 mirroring the MISC entry form's field order
and labels exactly, with the pinned `.checker-action-bar`. `NATURE` stores a code, so it is
resolved to its description via `NATURECASE` rather than showing the checker a number.

Neither page needed a new proc — `spCase_CheckerQueue` and `spCase_CheckerAction` are generic, and
the IAC and Vigilance checker pages inherit the new module scoping with **no code change at all**.
That is the payoff of the central design.

### Modified

| File | Change |
|---|---|
| `Admin/UserCreation.aspx` (+ `.designer.cs`) | `chkModuleGroups` list and a proper `Zones` row. The old `chkZones` sat **outside any `<tr>`**, as a direct child of the `<table>` — invalid markup that browsers hoist out of the table. Both are now in labelled rows |
| `Admin/UserCreation.aspx.cs` | `BindModuleGroups`, `ShowCheckerScope`; `SaveMakerCheckerMapping` → `SaveCheckerScope` (validating, via `MERGE`); `LoadCheckerZones` → `LoadCheckerScope`. A failed save now returns before reporting "User Updated" |
| `Default.aspx.cs` | Both count queries → `fnCheckerScope`; Vigilance and MISC added to `checkerInboxPages`, which had only IAC — **the Vigilance badge has been un-clickable since that module shipped** |
| `Mis/frmComplaintChecker.aspx.cs` | Inbox query → `fnCheckerScope`, module `'COMPLAINT'` |
| `Mis/frmMiscStructure.aspx` | `OnRowDataBound` wired up; "Checker Status" and "Checker Remarks" grid columns |
| `Mis/frmMiscStructure.aspx.cs` | `gvMain_RowDataBound` row lock; `funcApplyCheckerLock` on fetch-by-number; `funcControlsUserRights` label check; `funcSave` now surfaces the proc's real message |
| `Upload/frmExcelUpload.aspx.cs` | `funcExcelImport_MISC` passes the zone columns when the sheet has them, and reports `-2` |
| `Web.sitemap` | MISC Checker Inbox node; the MISC group node opened to `VMIS_CHECKER` and its maker/report children given explicit roles so a checker-only user sees only the inbox |
| `VMISP.csproj` | Four new page files registered |

### `funcSave` was hiding its own error messages

`frmMiscStructure.aspx.cs` did `if (cmdSave.ExecuteNonQuery() > 0)`. The new guards refuse a save
**without running any DML**, so that test reads as failure and the guard's message would never
have reached the user. It now reads `@o_ERRCODE` and clears the form only on `1` (saved) or `2`
(updated). The same fix was made for IAC and Vigilance in their deliveries.

---

## 9. Verification performed

**C# compiles clean — zero errors.** Visual Studio is not installed on the development machine
(`Microsoft.WebApplication.targets` missing, so MSBuild cannot build a web project), so the
csproj's full `Compile` item list — 210 files — was compiled directly with Roslyn against the
project's own references. It **succeeds and produces an assembly**. The only excluded files are
`Reports/frmCaseRegister.aspx.cs` and `Upload/frmAccessUpload.aspx.cs`, which need Office COM
interops absent from this machine and are unrelated to this work. This is the same method and the
same two exclusions as the IAC and Vigilance deliveries.

That covers the code-behind and its binding to the designer files. It does **not** cover the
`.aspx` markup, which only `aspnet_compiler` validates — and the designer files for the two new
MISC pages were **hand-written from the markup**, not generated, so they should be regenerated in
Visual Studio and diffed before release.

### Database — both scripts deployed and exercised on `localhost\SQLEXPRESS`

Run against the development `VigilanceMISDB` (SQL Server 2025 Express, compatibility level 160).
Test data was removed afterwards; `MISC`, `MISC_HISTORY` and the MISC rows of `CASE_APPROVAL` are
back to 0 and the identity is reseeded. The two pre-existing IAC records were untouched.

| # | Scenario | Result |
|---|---|---|
| 1 | Migration fan-out | checker `5224503` (zone `100002`) went from one ungrouped grant to explicit `VIG` + `CMP` — **access unchanged**, as intended |
| 2 | `fnCheckerScope('5224503')` | returns COMPLAINT, IAC, MISC, VIGILANCE for zone `100002` |
| 3 | MISC saved with no zone | refused, code 4 |
| 4 | MISC saved with zone | saved, `CASE_APPROVAL` at `'P'`, `SUBMITTED` logged |
| 5 | Checker queue, module MISC | record returned with module name and view page |
| 6 | **Admin narrows the user to `VIG` only** | scope becomes IAC + Vigilance; the CMP row is deactivated |
| 7 | MISC queue for that user | **empty** |
| 8 | MISC action by that user | **refused** — "You are not authorized to act on this record." |
| 9 | Orphan monitor | reports the MISC record as *"This zone has checkers, but none for this module's group"* — the failure the old zone-only view would have called healthy |
| 10 | `CMP` granted back | scope restored; queue and actions work again |
| 11 | Maker approves own record | refused — "Maker and checker cannot be the same user." |
| 12 | Maker edits while pending | refused, code 5 |
| 13 | Push back → correct → resubmit → approve | each step correct; status `C` → `P` → `A` |
| 14 | Second action on the same record | refused — "already been actioned" |
| 15 | Audit trail | `SUBMITTED → PUSHED_BACK → RESUBMITTED → APPROVED` |
| 16 | View `LIST` and `GET` branches | both return `APPROVALSTATUS` / `APPROVALSTATUSTEXT` / `CHECKERREMARKS` |
| 17 | Import under `ImportApprovalStatus = 'A'` | registered Approved, `CheckerUser = 'SYSTEM'`, `IMPORTED` logged with the exemption note |
| 18 | Import with no zone after switching to `'P'` | refused, code −2, **and no partial MISC row left behind** |
| 19 | Import with a zone under `'P'` | imported, registered Pending |
| 20 | Orphan monitor after cleanup | empty |

> **A deployment defect was found by running the application.** All 11 objects were first created
> through sqlcmd, which defaults `QUOTED_IDENTIFIER` to **OFF** — and that setting is stored with
> the module. The admin screen's first call, `spCheckerGroup_Ddl`, then failed from
> `System.Data.SqlClient` with *"SELECT failed because the following SET options have incorrect
> settings: 'QUOTED_IDENTIFIER'"*, because that proc used an **XML data type method**
> (`FOR XML PATH(...).value()`), which requires the option ON. Fixed twice over: both scripts now
> set the option explicitly (see the box in §3), and `spCheckerGroup_Ddl` was rewritten to use
> `STRING_AGG`, which has no XML dependency at all. All 11 objects were redeployed and now report
> `uses_quoted_identifier = 1`.
>
> **One script defect was found and fixed by running it.** The fan-out in step 3 ran *before* the
> old `UQ_MakerCheckerMapping (UserPF, ZoneSolID)` constraint was dropped, so it failed on its
> first row with a unique-key violation. The constraint drop now precedes the fan-out (steps 3a →
> 3b → 3c). Note that the failure was **not** cleanly re-runnable: the `UPDATE` that retires the
> ungrouped rows still executed, so a plain re-run found no active ungrouped rows and skipped the
> fan-out, leaving the checker with no grants at all. That state was repaired by reactivating the
> ungrouped row and re-running. **If a run of step 3 ever errors, check
> `MakerCheckerMapping` before re-running** — the recovery is the `UPDATE ... SET IsActive = 1
> WHERE GroupCode IS NULL AND IsChecker = 1` shown in §12.

**Still to do:** an end-to-end run through the running application — the admin screen, the MISC
maker form, the MISC inbox and the verification page. §11 is the matrix; cases 11–15 there are the
ones only the UI can cover.

---

## 10. Known gaps

| # | Item |
|---|---|
| 1 | **`.aspx` markup not compiler-validated**, and the two new designer files are hand-written. Open in Visual Studio, regenerate, diff |
| 2 | **Menu trimming is still role-based.** A Vigilance-only checker still *sees* the Complaint, IAC and MISC inbox nodes; those inboxes come up empty and any action is refused server side. Untidy, not a hole. Fixing it properly needs runtime trimming from `fnCheckerScope` (plan §5.2) |
| 3 | **No bulk action in any checker inbox** — carried from the IAC delivery, and the thing standing between the build and practical daily use where volume arrives by upload |
| 4 | **Complaint is still on the old inline-columns mechanism.** It is now module-*scoped*, but not migrated. Two mechanisms remain live; migrating it is specified in `VMIS_IAC_MakerChecker_Implementation.md` §"Migrating Complaint" |
| 5 | **`spComplaintUser_Update` and `spVigilanceUser_Update` bypasses** — unchanged here. `spVigilanceUser_Update` was closed in the Vigilance delivery; Complaint's is still open |
| 6 | **Desk-user annotations bypass approval** on every module. Pre-existing |
| 7 | **A rejected record is locked permanently.** No reopen path exists for anyone |
| 8 | **`STRING_SPLIT`** in `spCheckerScope_Save` needs database compatibility level 130+ |
| 9 | **Makers are not module-scoped.** Any `VMIS_MISUSER` can enter any module in any zone; `MakerCheckerMapping.IsMaker` is written as `0` everywhere and read by nothing. If module-scoped makers are wanted, `fnCheckerScope`'s shape would serve it, but it is separate work |

---

## 11. Test matrix

Zones `Z1`, `Z2`. Checker `C1` granted **VIG** in `Z1`. Checker `C2` granted **CMP** in `Z1`.
Maker `M1`, never the same account as the checker.

### Module scoping

| # | Scenario | Expected |
|---|---|---|
| 1 | `C1` opens the IAC and Vigilance inboxes | Z1 records listed in both |
| 2 | `C1` opens the Complaint and MISC inboxes | **both empty** |
| 3 | `C1` posts a MISC action directly, bypassing the menu | **refused** — "not authorized" |
| 4 | `C2` opens the MISC inbox | Z1 MISC records listed |
| 5 | `C2` actions a Complaint record in Z1 | success |
| 6 | `C1` actions an IAC record in **Z2** | **refused** — zone scoping still applies |
| 7 | `C1` is the maker on their own Z1 Vigilance record | **refused** — self-approval guard intact |
| 8 | `Default.aspx` badges for `C1` | IAC + Vigilance only, no Complaint or MISC |
| 9 | Pending MISC record in a zone with only VIG checkers | appears in `vw_CASE_APPROVAL_ORPHANS` with the "no checker for this module's group" reason |
| 10 | `WORKFLOW_MODULE_GROUP` set `IsActive = 0` for VIG | `C1`'s inboxes empty, actions refused, records show as orphans |
| 11 | Pre-migration checker, post-deployment, no admin edit | access **unchanged** |
| 12 | Admin unticks CMP for a user and saves | Complaint and MISC access stops; VIG unaffected |
| 13 | Save with a group ticked but no zone | refused with a message; nothing written |
| 14 | Two admins save the same user concurrently | no duplicate rows, no duplicated inbox rows |
| 15 | Move MISC to VIG (`UPDATE WORKFLOW_MODULE SET GroupCode='VIG' WHERE ModuleCode='MISC'`) | `C1` sees MISC, `C2` does not — with no application change |

### MISC workflow

| # | Scenario | Expected |
|---|---|---|
| 16 | Save a MISC record with no Zone (New) | refused, code 4 |
| 17 | Save with a zone | saved, `CASE_APPROVAL` row at `'P'`, `SUBMITTED` logged |
| 18 | Maker edits while pending | refused, code 5; grid button reads "Pending" and is disabled |
| 19 | Checker pushes back | status `'C'`; maker's grid button reads "Edit" and shows the remarks |
| 20 | Maker corrects and resubmits | back to `'P'`, checker fields cleared, `RESUBMITTED` logged |
| 21 | Checker approves, maker then edits | re-queued to `'P'`, `RESUBMITTED` logged |
| 22 | Checker rejects, maker retries | refused, code 5; button reads "Rejected" |
| 23 | Second action on the same record | refused — "already been actioned" |
| 24 | `LIST` / `SEARCH` / `GET` view branches | all three return the three new columns |
| 25 | Excel import under `ImportApprovalStatus = 'A'` | imported, registered Approved, `CheckerUser = 'SYSTEM'`, `IMPORTED` logged |
| 26 | Same, after switching to `'P'` with no zone column in the sheet | row refused, code −2, **no partial MISC row left behind** |
| 27 | Maker edits an exempt imported record | re-queued to `'P'` — the exemption applies to the import, not forever |
| 28 | Audit trail | `SUBMITTED → PUSHED_BACK → RESUBMITTED → APPROVED → RESUBMITTED → REJECTED` |
| 29 | Full-table check | `SELECT * FROM dbo.vw_CASE_APPROVAL_ORPHANS` empty |

Run 1–10 and 16–29 in SQL first; 11–15 need the admin screen.

---

## 12. Rollback

The database changes are additive. To revert the module scoping while keeping MISC:

```sql
-- Restore the four repointed objects from the 2026-07-27 foundation script and the
-- 2026-07-25 Complaint script, then:
DROP FUNCTION dbo.fnCheckerScope;
DROP PROC dbo.spCheckerScope_Save;
DROP PROC dbo.spCheckerScope_Get;
DROP PROC dbo.spCheckerGroup_Ddl;

-- Restore the pre-migration mappings (the originals were deactivated, not deleted):
UPDATE dbo.MakerCheckerMapping SET IsActive = 1 WHERE GroupCode IS NULL AND IsChecker = 1;
UPDATE dbo.MakerCheckerMapping SET IsActive = 0 WHERE GroupCode IS NOT NULL;
```

`WORKFLOW_MODULE_GROUP` and the two `GroupCode` columns can be left in place — nothing reads them
once `fnCheckerScope` is gone. The application must be rolled back at the same time, since five
files call the function.
