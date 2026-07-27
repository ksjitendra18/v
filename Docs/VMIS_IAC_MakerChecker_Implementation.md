# IAC Maker–Checker — Implementation Record

**Module:** IAC (`Mis/frmIACStructure.aspx`)
**Delivered:** 2026-07-27
**Database:** `VigilanceMISDB`
**Companions:** `VMIS_MakerChecker_Rollout_Plan.md` (the design study this implements), `VMIS_Technical_Overview.md`, `VMIS_Database_Inventory.md`

---

## 1. What this is

The Complaint module already had a maker–checker workflow, built by putting six workflow
columns (`APPROVALSTATUS`, `MAKERUSER`, `MAKERDATE`, `CHECKERUSER`, `CHECKERDATE`,
`CHECKERREMARKS`) directly on `COMPLAINT` and `COMPLAINT_HISTORY`.

This delivery extends the same workflow to **IAC**, but on a **central approval registry**
instead of per-table columns. IAC is the first module on the new mechanism; the foundation
built here is reusable for the remaining 13 modules.

**Complaint was not changed and is not affected.** It keeps its inline columns and its own
`spComplaint_CheckerAction`. The two mechanisms run side by side. Migrating Complaint onto
the central registry is a separate, later decision (see §9).

---

## 2. Why central instead of per-table — the decision

`VMIS_MakerChecker_Rollout_Plan.md` §5 recommended the central table. Verifying it against
the live database confirmed it:

| Factor | Per-table (Complaint pattern) | Central (what was built) |
|---|---|---|
| DDL to roll out all 14 modules | 28 tables (14 case + 14 history) | 2 tables, once |
| `SELECT *` history risk | **High.** 21 procs copy history with `INSERT INTO <T>_HISTORY SELECT * FROM <T>`, which only works while both tables have identical column count *and ordinal order* | **None.** No case table is altered |
| Touching `VIGILANCE` (171 cols) / `RRB` (159 cols) | Required | Not required |
| Checker action procs | 14 | 1 |
| Checker inbox queries | 14 | 1 |
| Cross-module "everything pending for me" | 14-way `UNION` | single query |
| Cost | — | one `LEFT JOIN` on list screens |

The deciding evidence: `IAC` and `IAC_HISTORY` are 58 columns each and **currently in exact
ordinal alignment**, and `spIACStructure_Update` copies history with `INSERT INTO IAC_HISTORY
SELECT * FROM IAC`. Appending columns to one table and not the other — or appending in a
different order — silently corrupts every history row from that moment on. The central design
removes that failure mode entirely rather than relying on discipline 14 more times.

---

## 3. Files delivered

### New — database scripts

| File | Contents |
|---|---|
| `Database/Scripts/2026-07-27_CaseApproval_Central_MakerChecker_Foundation.sql` | The reusable foundation: 3 tables, 1 view, 2 generic procs, `MakerCheckerMapping` fixes |
| `Database/Scripts/2026-07-27_IAC_MakerChecker.sql` | IAC-specific: backfill + 3 modified procs |
| `Database/Scripts/2026-07-27_IAC_MakerChecker_ExcelImport.sql` | `WORKFLOW_MODULE.ImportApprovalStatus` + `spIACExcel_Import` |

All three are idempotent (`CREATE OR ALTER`, existence guards, `MERGE`) and safe to re-run.

### New — web pages

| File | Purpose |
|---|---|
| `VMISP/Mis/frmIACChecker.aspx` (+ `.cs`, `.designer.cs`) | Checker inbox: every IAC record pending this checker's verification |
| `VMISP/Mis/frmIACCheckerView.aspx` (+ `.cs`, `.designer.cs`) | Read-only IAC record + Accept / Reject / Push Back with mandatory remarks |

### Modified

| File | Change |
|---|---|
| `VMISP/Mis/frmIACStructure.aspx` | Added `OnRowDataBound`; added "Checker Status" and "Checker Remarks" grid columns |
| `VMISP/Mis/frmIACStructure.aspx.cs` | Row-level edit lock; checker lock on fetch-by-number; save now surfaces the proc's message |
| `VMISP/Upload/frmExcelUpload.aspx.cs` | `funcExcelImport_IAC` — commits on rows actually imported rather than the last row's result, and reports skipped rows with reasons |
| `VMISP/Web.sitemap` | IAC Checker Inbox node; **fixed the ancestor role trimming that made every checker inbox unreachable** |
| `VMISP/VMISP.csproj` | Registered the four new page files |

---

## 4. Database objects created

### 4.1 `dbo.WORKFLOW_MODULE` — module registry

Which modules participate, and how to locate and route a record. One row per module.

| Column | Purpose |
|---|---|
| `ModuleCode` | PK. `'IAC'`. Used as the discriminator everywhere |
| `ModuleName` | Label for the inbox |
| `TableName` / `KeyColumn` / `RefColumn` / `ZoneColumn` | Documents how a module maps onto the registry (`IAC` / `SNO` / `IACNO` / `NEWZONE`) |
| `ViewPage` | Checker detail page, e.g. `~/Mis/frmIACCheckerView.aspx` |
| `IsActive` | Turns a module's workflow off without dropping data |
| `ImportApprovalStatus` | `'P'` or `'A'` — what **bulk-imported** records land as. See §5.4. Defaults to `'P'` |

Seeded with one row: **IAC**.

### 4.2 `dbo.CASE_APPROVAL` — current approval state

One row per case record under workflow. **This is the table that replaces the six inline columns.**

| Column | Notes |
|---|---|
| `ApprovalId` | identity PK |
| `ModuleCode` + `RecordCode` | **unique together.** `RecordCode` holds the case table's surrogate key — for IAC that is `IAC.SNO` |
| `RecordRef` | human-readable key (`IAC.IACNO`), kept in step on edit |
| `ZoneSolID` | snapshot of `IAC.NEWZONE`. **This is what routes the record to a checker** |
| `ApprovalStatus` | `P` Pending · `A` Approved · `C` Changes Requested · `X` Rejected. CHECK-constrained |
| `MakerUser` / `MakerDate` | who submitted, when |
| `CheckerUser` / `CheckerDate` / `CheckerRemarks` | who decided, when, why |

Status codes deliberately match what `spComplaint_View` already used, so the two modules read
the same way. Note Complaint's reject code is `'X'`, not `'R'`.

Two indexes: `IX_CASE_APPROVAL_Queue` (inbox) and `IX_CASE_APPROVAL_Record` (list-screen join).

### 4.3 `dbo.CASE_APPROVAL_HISTORY` — append-only audit

Never updated, only inserted. `ActionType` is one of:

`SUBMITTED` · `IMPORTED` · `RESUBMITTED` · `APPROVED` · `REJECTED` · `PUSHED_BACK` · `GRANDFATHERED`

`IMPORTED` is kept distinct from `SUBMITTED` so bulk uploads can be told apart from typed entry
in audit and reporting.

Also records `ActionBy`, `ActionDate`, `Remarks`, `UserRole`, `UserIP`.

### 4.4 `dbo.vw_CASE_APPROVAL_ORPHANS` — monitor

Pending records **no checker can ever see** — either no zone on the record, or no active
checker mapped to that zone. Such a record is invisible to every inbox *and* locked from
editing by the maker: permanent, silent limbo.

```sql
SELECT * FROM dbo.vw_CASE_APPROVAL_ORPHANS;
```

**Run this after every bulk import.** Nothing else surfaces the problem.

### 4.5 `dbo.spCase_CheckerAction` — generic action proc (one for all modules)

```sql
EXEC dbo.spCase_CheckerAction
     @p_MODULE, @p_CODE, @p_ACTION, @p_REMARKS,
     @p_USER, @p_USERROLE, @p_USERIP,
     @o_EERMSG OUTPUT, @o_ERRCODE OUTPUT;
```

`@p_ACTION`: `'A'` approve · `'X'` reject · `'C'` push back.
`@o_ERRCODE`: `1` = success, `0` = refused (reason in `@o_EERMSG`).

Guards, in order — cheap checks first, authorisation before state, "still pending" last:

1. Action code must be `A` / `X` / `C`
2. Remarks are mandatory
3. Module must be registered and active
4. Record must exist in `CASE_APPROVAL`
5. **Maker and checker must be different users**
6. Caller must be an active checker mapped to the record's zone
7. Status must still be `'P'` — prevents double-action from a double-submit

Then updates state and writes the audit row **inside a transaction**, with the pending check
repeated in the `UPDATE ... WHERE ApprovalStatus = 'P'` so two concurrent checkers cannot both win.

> **Guard 5 is the important one.** `VMIS_CHECKER` is a *secondary* role — `AssignRole()`
> explicitly lets a user hold `VMIS_MISUSER` and `VMIS_CHECKER` at the same time. Without this
> check that user can approve their own entry, which defeats the entire control. It is a single
> clearly-commented block; see §8 if it blocks single-account UAT.

### 4.6 `dbo.spCase_CheckerQueue` — generic inbox (one for all modules)

```sql
EXEC dbo.spCase_CheckerQueue @p_USER = '5224503', @p_MODULE = 'IAC', @p_STATUS = 'P';
```

`@p_MODULE = NULL` returns every module the user checks — this is the cross-module inbox the
central design exists to enable. `@p_STATUS = NULL` returns every status.

Scoping is done by joining `MakerCheckerMapping`, so the calling page needs no zone logic.

### 4.7 `dbo.MakerCheckerMapping` — three defects fixed

These were latent bugs affecting Complaint too, and had to be fixed regardless of design:

1. **Type mismatch.** `ZoneSolID` was `varchar(6)` while `COMPLAINT.NEWZONE` and `IAC.NEWZONE`
   are `varchar(10)`. Any SOL code longer than 6 characters could never match, so those zones
   silently had no checker. Widened to `varchar(10)`.
2. **Missing unique key.** `Admin/UserCreation.aspx.cs` does `SELECT COUNT(*)` then
   UPDATE-or-INSERT — a race. Two concurrent saves produce duplicate mappings, which duplicate
   every row in the inbox. Added `UNIQUE (UserPF, ZoneSolID)`.
3. **Missing index.** Only a PK on `Id` existed, so every inbox query scanned the table. Added
   `IX_MakerCheckerMapping_Lookup (UserPF, IsChecker, IsActive) INCLUDE (ZoneSolID)`.

---

## 5. Procedures modified for IAC

### 5.1 `spIACStructure_Update` — the write path

**No parameters were added or removed.** The existing C# call site works unchanged.

**Insert (`@p_MODE = 'I'`)**
Wrapped in a transaction. After the `INSERT INTO IAC`, captures `SCOPE_IDENTITY()` and writes
a `CASE_APPROVAL` row at `'P'` plus a `SUBMITTED` audit row.

**Update (`@p_MODE = 'U'`)**
Reads the current `ApprovalStatus` *before* touching anything, then:

| Prior status | Behaviour |
|---|---|
| `'X'` Rejected | **Refused** — `@o_ERRCODE = 5` |
| `'P'` Pending | **Refused** — not the maker's to change while the checker holds it |
| `'C'` Changes Requested | Update proceeds → back to `'P'`, logs `RESUBMITTED` |
| `'A'` Approved | Update proceeds → back to `'P'`, logs `RESUBMITTED` (the edit invalidates the earlier decision) |
| `NULL` no approval row | Record predates the workflow, or came from an import. Onboarded now at `'P'`, logs `SUBMITTED` |

On resubmit, `RecordRef` and `ZoneSolID` are refreshed from the form so a zone change re-routes
the record to the correct checker.

**New validation — Zone (New) is mandatory for a maker (`@o_ERRCODE = 4`).**
The zone is the only thing that routes a record to a checker. A record saved without one would
sit `'P'` forever, invisible to every inbox and locked from editing. Refusing the save is the
only outcome that is not a silent trap.

**The `VMIS_DESKUSER` branch is unchanged** — desk-user annotations still bypass approval
entirely. That is pre-existing behaviour and an open question for the business (§9).

**New `@o_ERRCODE` values:** `4` = zone missing, `5` = record locked. Existing values are unchanged
(`0` error, `1` saved, `2` updated, `3` duplicate IAC number).

### 5.2 `spIACStructure_View` — the read path

All three branches (`LIST`, `SEARCH`, and the single-record `GET`/`VIEW`) now `LEFT JOIN`
`CASE_APPROVAL` and return three extra columns:

- `APPROVALSTATUS` — the raw code, drives the grid button
- `APPROVALSTATUSTEXT` — display text
- `CHECKERREMARKS` — why the checker decided as they did

`LEFT JOIN`, so records with no approval row still list, with `NULL` status.

### 5.3 `spIACUser_Update` — bypass closed

`Mis/frmIACUpdate.aspx` (MISUSER-only) changes the DA on an existing record. It **never touched
the approval state**, so a maker could alter an *approved* record and it stayed approved and
unverified. Now:

- a rejected record is refused (`@o_ERRCODE = 3`)
- an approved or pushed-back record is re-queued to `'P'` with a `RESUBMITTED` audit row reading
  `"DA changed via IAC Update screen."`

Two optional parameters (`@p_USERROLE`, `@p_USERIP`) were appended **with defaults**, so the
existing three-parameter call site still works untouched.

> The equivalent bypass still exists in `spComplaintUser_Update` and `spVigilanceUser_Update`.
> Same shape, same fix. Not done here — out of scope for IAC.

### 5.4 `spIACExcel_Import` — the bulk path, and why it matters most

**For IAC, Excel upload is the dominant way records arrive.** Leaving it outside the workflow
would have meant the control applied only to the minority of records that are typed in — and
would have handed anyone a trivial way around it: upload a one-row sheet instead of using the form.

The import now registers every row it inserts in `CASE_APPROVAL`, and logs an `IMPORTED` audit row.

**Whether imported records need checking is configuration, not code** —
`WORKFLOW_MODULE.ImportApprovalStatus`, changeable per module with one `UPDATE`:

| Value | Behaviour | Trade-off |
|---|---|---|
| `'P'` **(default)** | Imported records are Pending and appear in the checker inbox like any other | The control genuinely covers the dominant path. But a 500-row upload puts 500 records in front of a checker, and **the maker cannot edit any of them until they are actioned**. Practical only with a bulk action in the inbox — see §10 |
| `'A'` | Imported records are registered Approved, with `CheckerUser = 'SYSTEM'` and an audit note | An explicit, audited exemption instead of a silent gap. Records still carry a visible status, appear in reporting, and **re-enter the workflow the moment anyone edits them** |

```sql
-- exempt bulk uploads from checking
UPDATE dbo.WORKFLOW_MODULE SET ImportApprovalStatus = 'A' WHERE ModuleCode = 'IAC';
```

**Rows with no zone are rejected when imports require checking** (`@o_ERRCODE = -2`). The zone is
the only thing that routes a record to a checker; imported without one, the row would sit Pending
forever — in no inbox, and not editable. The manual form already refuses for the same reason, so
this also closes the upload-instead-of-typing bypass. Under `'A'` no zone is needed, since nothing
has to be routed.

If IAC is absent from `WORKFLOW_MODULE` or has `IsActive = 0`, the proc imports exactly as it did
before and registers nothing.

**Transaction handling.** `funcExcelImport_IAC` wraps the whole sheet in one transaction. The proc
therefore only opens its own when `@@TRANCOUNT = 0`, and never commits or rolls back a transaction
it did not start — doing so would silently decide the fate of the caller's entire batch. On error
it re-throws, exactly as the unhandled version did, so the caller's existing catch still rolls the
sheet back and reports the offending row.

**No parameters changed**, so the existing call site works unchanged — it already passed
`@p_USER`, `@p_ADDUSERIP` and `@p_NEWZONESOLID`.

#### A batch-loss bug this exposed

`funcExcelImport_IAC` committed based on `intErrCode`, which held **only the last row's result**:

```csharp
if (intErrCode.Equals(1)) { txn.Commit(); }
```

So a single rejected row *at the end of a sheet* silently discarded the entire upload — no commit,
no error, just a batch that vanished. This was pre-existing (a duplicate IAC number on the last row
did it too), but adding a second rejection reason would have made it fire far more often. It now:

- commits when **any** row imported, rolls back when none did
- counts skipped rows and reports each one's reason (first 10, then "and N more")

---

## 6. Application changes

### 6.0 UI convention — settled, use this for every future module

The two checker pages deliberately use **different** styling, because they do different jobs.
This split was chosen by the business on 2026-07-27 after seeing both, and is the pattern to
follow for MISC, RTI, NOC and the rest. **Do not invent a new design per module.**

| Page | Style | Why |
|---|---|---|
| **Checker inbox** (`frmIACChecker.aspx`) | **Bootstrap 5 card design** — gradient page header, `card-custom`, `table-hover`, badge status chips, client-side search box. Copied from `frmComplaintChecker.aspx` | It is a new kind of screen with no equivalent in the old app, so it does not need to imitate anything. The card design reads better as a worklist |
| **Verification page** (`frmIACCheckerView.aspx`) | **Bootstrap 3, mirroring the module's own entry form** — `panel panel-primary`, inner `col-sm-12 alert alert-dark`, `form-group row` rows of `col-sm-3` cells, `form-control input-sm`, `label label-*` status chips | A checker reads the same record the maker keyed in. Same field order, same labels, same shape means nothing has to be re-learned and nothing is missed |
| **Action bar** (verification page) | **Pinned to the bottom of the viewport**, always visible while scrolling | An IAC record is long. Accept / Push Back / Reject must be reachable without scrolling back |

**Do not load Bootstrap 5 on the verification page.** It runs on Bootstrap 3 to match the entry
form, and loading both breaks the grid. That is why the pinned bar is the hand-rolled
`.checker-action-bar` rule rather than Bootstrap 5's `.sticky-bottom`:

```css
.checker-action-bar { position: fixed; left: 0; right: 0; bottom: 0; z-index: 1000;
                      background: #fff; border-top: 1px solid #ddd;
                      box-shadow: 0 -2px 10px rgba(0,0,0,.15); padding: 10px 20px; }
.checker-page       { padding-bottom: 80px; }   /* keep the last fields clear of the bar */
```

Consequently `GetStatusClass()` returns **Bootstrap 5 badge modifiers** in the inbox code-behind
and **Bootstrap 3 `label label-*` classes** in the verification code-behind. That is intentional,
not an oversight — each matches the framework its own page loads.

### 6.1 `frmIACChecker.aspx` — the inbox

Calls `spCase_CheckerQueue` with `@p_MODULE = 'IAC'`. Bootstrap 5 card design matching
`frmComplaintChecker.aspx` (§6.0).

Columns: IAC No, Zone, Submitted By, Submitted On, Status badge, and a View link carrying
`?id=<SNO>`. A client-side search box filters the rendered rows without a postback. A running
total sits in the page header.

### 6.2 `frmIACCheckerView.aspx` — verification and decision

Read-only view of the IAC record **in the entry form's exact field order and with its exact
labels** — IAC No, Received Date, Circle Name, Branch Name, then Vigilance Number, Closure Date,
Accused, DA View, and so on down to Status and Dealing Officer Remarks. A short header strip above
it shows IAC Number, Checker Status, Submitted By and Submitted On, then a Checker Decision section
with a mandatory remarks box.

Accept / Push Back / Reject live in the **pinned bottom bar** (§6.0), alongside a Back to Inbox
link and `lblMsg`.

- The query string carries `IAC.SNO`, which is what `CASE_APPROVAL.RecordCode` holds.
- Every field is a `ReadOnly` TextBox with a grey background, so the record cannot be altered here.
- Action buttons and the "verify carefully" note appear **only** while the record is `'P'`. The
  remarks box itself stays visible either way, so the decision recorded against an already-actioned
  record can still be read.
- `lblMsg` sits in the bar but **outside** `pnlActions`, so outcomes and refusals still show after
  the record has been actioned and the buttons have gone.
- Remarks are enforced client-side (confirm dialog), in the code-behind, and again in the proc.
- All three buttons route to `spCase_CheckerAction` — the C# holds none of the control logic.

> **Known fidelity gap:** the entry form renders Status Code, Letter Sent To, Nature Case, Scale
> and Bank Name as dropdowns showing descriptions, whereas `IAC` stores codes for some of them.
> The checker page shows what is stored. Nature Case is resolved to its description via a join to
> `NATURECASE`; the others would each need their own master lookup. Low impact, easy to add later.

### 6.3 `frmIACStructure.aspx` / `.cs` — the maker screen

**Grid** — two new columns, "Checker Status" and "Checker Remarks".

**Row-level edit lock** (`gvMain_RowDataBound`, newly wired up in the markup — the handler
existed but was never bound, so its hover behaviour was dead code):

| Status | Button |
|---|---|
| `P` | disabled, "Pending", warning styling |
| `C` | enabled, "Edit", info styling |
| `X` | disabled, "Rejected", danger styling |
| `A` / `NULL` | enabled, "Edit" |

The button **label** is the lock signal: `funcControlsUserRights()` re-enables grid buttons for
`VMIS_DESKUSER` after binding, and now only does so when the label is still `"Edit"`. Previously
it re-enabled every row unconditionally.

**Fetch-by-number lock** (`funcApplyCheckerLock`) — the "Get" button loads a record straight into
the form, bypassing the grid. Pending and rejected records now hide the Update button and explain
why; a pushed-back record shows the checker's remarks. This is convenience only — the proc
refuses these saves regardless.

**Save now reports the real reason.** `funcSave` previously did `if (ExecuteNonQuery() > 0)` and
otherwise showed a fixed `"Error in IAC Insert/ Update."`. The new guards refuse a save *without
running any DML*, so their message would never have reached the user. It now reads `@o_ERRCODE`
and displays `@o_EERMSG`, clearing the form only on `1` (saved) or `2` (updated).

### 6.4 `Web.sitemap` — checker inboxes were unreachable

Security trimming is on (`securityTrimmingEnabled="true"`). **`VMIS_CHECKER` appeared on no
ancestor node**, so a checker-only user saw no menu at all — the already-shipped Complaint
Checker Inbox was unreachable too. Fixed:

- `VMIS_CHECKER` added to the top-level wrapper node
- `VMIS_CHECKER` added to the **IAC** and **Complaint** group nodes
- The maker/report children under those two groups were given explicit roles
  (`VMIS_MISUSER,VMIS_DESKUSER,VMIS_VIEWUSER`) so a checker-only user sees **only** the inbox,
  not the data-entry form

Plus the new node: `~/Mis/frmIACChecker.aspx`, `roles="VMIS_CHECKER"`.

---

## 7. Verification performed

### Database — 16 scenarios, all passed

Executed against live `VigilanceMISDB`; test data removed afterwards (`IAC`, `IAC_HISTORY`,
`CASE_APPROVAL`, `CASE_APPROVAL_HISTORY` all returned to 0 rows, identity reseeded).

| # | Scenario | Result |
|---|---|---|
| 1 | Insert with no zone | refused, code 4 |
| 2 | Insert with zone `100002` | saved, `CASE_APPROVAL` row at `'P'` |
| 3 | Maker edits while pending | refused, code 5 |
| 4 | `spCase_CheckerQueue` for the mapped checker | record returned with module name and view page |
| 5 | Maker approves own record | refused — "Maker and checker cannot be the same user." |
| 6 | Action with empty remarks | refused |
| 7 | Unmapped user acts | refused — "You are not authorized to act on this record." |
| 8 | Mapped checker pushes back | success, status `'C'` |
| 9 | Second action on the same record | refused — "already been actioned" |
| 10 | Maker corrects and resubmits | updated, back to `'P'`, checker fields cleared |
| 11 | Checker approves | success, status `'A'` |
| 12 | `spIACUser_Update` on the approved record | re-queued to `'P'` |
| 13 | Checker rejects, then maker retries via the update screen | refused |
| 14 | Audit trail | `SUBMITTED → PUSHED_BACK → RESUBMITTED → APPROVED → RESUBMITTED → REJECTED` |
| 15 | `LIST` / `SEARCH` / `GET` view branches | all three return the three new columns |
| 16 | Orphan monitor | empty, as expected |

### Database — Excel import, 10 further scenarios, all passed

| # | Scenario | Result |
|---|---|---|
| 1 | Import with zone, `ImportApprovalStatus = 'P'` | saved, registered Pending |
| 2 | Import with **blank** zone under `'P'` | rejected, code -2, **and no partial IAC row left behind** |
| 3 | Duplicate IAC number | rejected, code -1 (unchanged behaviour) |
| 4 | Imported record in the checker queue | appears alongside typed records |
| 5 | Checker approves the imported record | success |
| 6 | Import with blank zone under `ImportApprovalStatus = 'A'` | saved, registered Approved, `CheckerUser = 'SYSTEM'` with the exemption note |
| 7 | State of all imported records | statuses and checker fields as configured |
| 8 | Maker edits an exempt imported record | re-queued to Pending — exemption applies to the import, not forever |
| 9 | Audit trail | `IMPORTED → APPROVED` and `IMPORTED → RESUBMITTED` recorded correctly |
| 10 | Orphan monitor | empty |

### Application — compiles clean; one real end-to-end run

**C# compilation passes with zero errors.** Visual Studio is not installed on the development
machine, so MSBuild cannot build a web project (`Microsoft.WebApplication.targets` missing).
Compiling the csproj's full `Compile` item list directly with Roslyn **succeeds and produces an
assembly**. The only excluded files are `Reports/frmCaseRegister.aspx.cs` and
`Upload/frmAccessUpload.aspx.cs`, which need Office COM interops absent from this machine and are
unrelated to this work.

That covers the code-behind and its binding to the designer files. It does **not** cover the
`.aspx` markup, which only `aspnet_compiler` validates — though both new designer files were
generated directly from their markup, so the two cannot drift.

While this work was in progress, an IAC record (`IACNO 123`, `CHANNEL = 'MANUAL ENTRY'`) was
created through the running application by user `5224563` and **approved by checker `5224503`
with remarks**, producing a correct `SUBMITTED → APPROVED` audit trail. That exercises the maker
form, the checker inbox and the verification page against the real UI. That record was left in
place; only the synthetic test rows were removed.

That run predates both the Excel-import changes (§5.4) and the layout rework (§6), so it covers
neither.

> **Open item: build in Visual Studio and smoke-test the two reworked checker pages.**

---

## 8. Deployment

Run in order against `VigilanceMISDB`:

```
1. Database/Scripts/2026-07-27_CaseApproval_Central_MakerChecker_Foundation.sql
2. Database/Scripts/2026-07-27_IAC_MakerChecker.sql
3. Database/Scripts/2026-07-27_IAC_MakerChecker_ExcelImport.sql
```

Then build and deploy the application.

### Three decisions baked in — change them here if the business disagrees

**Pre-existing IAC records are grandfathered as Approved.** Step 1 of the IAC script marks every
existing active IAC row `'A'` with `CheckerUser = 'SYSTEM'` and a `GRANDFATHERED` audit row, on
the basis that those records predate the control. Marking them `'P'` instead would flood the
checker inbox and lock every existing record from editing. The IAC table was empty on the
development database, so this was a no-op there and **will not be in production** — change the
literal in step 1 before deploying if the opposite is wanted.

**Self-approval is blocked.** If UAT is done with a single account holding both `VMIS_MISUSER`
and `VMIS_CHECKER`, that account cannot approve its own entries — by design. To relax it for
testing, comment out the single marked block in `spCase_CheckerAction` (§4.5 guard 5). It should
be restored before production.

**Bulk-imported IAC records default to Pending** (`ImportApprovalStatus = 'P'`). Since most IAC
records arrive by upload, this is the setting that makes the control real — and also the one with
the largest operational impact: a large sheet puts every one of its rows in front of a checker,
and locks the maker out of them until actioned. Uploads will also start rejecting rows whose
`NEWZONESOLID` is blank. **Decide this before the first production upload**, and see §10 on bulk
approval. One `UPDATE` switches it (§5.4).

### Prerequisites for a working end-to-end test

- At least one user in `MakerCheckerMapping` with `IsChecker = 1`, `IsActive = 1`, and a
  `ZoneSolID` matching the `NEWZONE` used on the IAC record
- **A different** user holding `VMIS_MISUSER` to act as maker
- The reference masters the IAC form depends on must be populated (`STATUS`, `SCALE`,
  `NATURECASE`, `BRANCH_MASTER`, …). `VMIS_MakerChecker_Rollout_Plan.md` §7 records these as
  empty on the development database

### Rollback

The DB changes are additive. To revert:

```sql
-- restore the three IAC procs from source control, then:
DROP PROC dbo.spCase_CheckerAction;
DROP PROC dbo.spCase_CheckerQueue;
DROP VIEW dbo.vw_CASE_APPROVAL_ORPHANS;
DROP TABLE dbo.CASE_APPROVAL_HISTORY;
DROP TABLE dbo.CASE_APPROVAL;      -- FK to WORKFLOW_MODULE, drop this first
DROP TABLE dbo.WORKFLOW_MODULE;
```

The `MakerCheckerMapping` fixes should be **kept** — they fix real defects independent of this work.

---

## 9. Adding the next module

The foundation is done; a further module is now mostly repetition. Using MISC as the example:

1. **Register it** — insert a `WORKFLOW_MODULE` row (`MISC` / `MISC` / `RNO` / `CODE` / `NEWZONE` /
   `~/Mis/frmMiscCheckerView.aspx`). Check the plan's §2 table for each module's real key columns;
   they are not consistent.
2. **Backfill** — copy step 1 of the IAC script, swapping table and column names.
3. **Write proc** — apply the four edits from §5.1 to `spMiscStructure_Update`: register on
   insert, guard on update, re-queue on resubmit, zone mandatory.
4. **View proc** — add the `LEFT JOIN CASE_APPROVAL` and three columns to every branch.
5. **Import proc** — apply §5.4 to the module's `sp<Module>Excel_Import`: read
   `ImportApprovalStatus`, register the row, reject rows with no zone when checking is required,
   and use the ambient-transaction pattern. **Do not skip this** — for several modules the import
   is the dominant path, exactly as it is for IAC. Check the caller in
   `Upload/frmExcelUpload.aspx.cs` for the same last-row commit bug described in §5.4.
6. **Pages** — copy both IAC checker pages and follow the settled UI convention in **§6.0**:
   the inbox keeps the Bootstrap 5 card design as-is (change `ModuleCode`, the grid columns and
   the link target); the verification page **mirrors that module's own entry form
   field-for-field** — same order, same labels, same Bootstrap 3 panel markup — and keeps the
   pinned `.checker-action-bar`. Familiarity is the point: do not introduce a new visual design
   per module. Generate the `.designer.cs` from the finished markup rather than hand-writing it,
   then diff the two to confirm they match.
7. **Maker screen** — grid columns, `RowDataBound` lock, `funcControlsUserRights` label check,
   `funcSave` error surfacing.
8. **Sitemap + csproj** — one node, four file registrations.

**No new checker action proc and no new inbox query are needed.** `spCase_CheckerAction` and
`spCase_CheckerQueue` are already generic.

### Watch out for

- **Modules with two live entry pages** — SR, WB, Vigilance, NOC and RRB each have an old and a
  new form on the menu. Where they share a write proc, changing the proc covers both. **RRB is
  the exception:** `spRRB_Update` and `spRRB_Operation` are different procs and both need the change.
- **Tier C modules** (SR, WB, Operational Ref) have only free-text `ZONE varchar(100)` and no
  SOL code to route on. They need `NEWZONE`/`NEWCIRCLE` added and back-filled first.
- **Tier D modules** (the three Sanction forms) have no zone at all and need a routing decision
  from the business before design.
- **ABBFF** has no `ABBFF_HISTORY` table — `spABBFFStructure_Update` writes into `MISC`/`MISC_HISTORY`.
  Fix that before adding workflow.

Suggested order (smallest and closest in shape to what is proven, first):
**MISC → RTI → NOC → Vigilance → RRB → Vigilance Monitoring.**

---

## 10. Known gaps and open questions

### Carried into this delivery

| # | Item |
|---|---|
| 1 | **No bulk action in the checker inbox — now a confirmed requirement.** The business decided on 2026-07-27 that imported records *do* require checking (`'P'`). Since IAC's volume arrives by upload, a checker must currently open and action every imported record one at a time. This is the main thing standing between the current build and practical daily use — see below |
| 2 | **`.aspx` markup not compiler-validated** — no Visual Studio on the development machine. The C# compiles clean and one real end-to-end run happened through the running app, but that run predates the import changes and the layout rework (§7) |
| 3 | **Desk-user annotations bypass approval.** `VMIS_DESKUSER` appends HO Status and remarks with no verification. Pre-existing; unchanged here |
| 4 | **A rejected record is locked permanently.** No reopen path exists for anyone |
| 5 | **Same bypass still open on two other modules** — `spComplaintUser_Update` and `spVigilanceUser_Update` have the shape §5.3 fixed for IAC |
| 6 | **Other modules' imports are still unregistered.** `spMISCExcel_Import`, `spRTIExcel_Import`, `spRRBExcel_Import`, `spSRExcel_Import`, `spWBExcel_Import`, `spVigilanceExcel_Import`, `spLodiExcel_Import`, `spACCESSSR_Import` need the §5.4 treatment when their module is rolled out |
| 7 | **Complaint still on the old mechanism.** Two mechanisms live at once. A checker with both roles has two inboxes until Complaint migrates. `spComplaintExcel_Import` also still bypasses approval |
| 8 | **`spIACStructure_Delete` is not wired to the workflow.** It soft-deletes (`ACTIVE='N'`) and leaves the `CASE_APPROVAL` row behind. It has **no call site in the application** and appears to be dead, which is why it was left alone — but it would need handling if ever used |

### On gap 1 — bulk approval

If `ImportApprovalStatus` stays at `'P'`, the inbox needs a way to action many records at once.
The data model already supports it: `spCase_CheckerAction` is per-record and idempotent-safe
(guard 7 makes a second action on the same record a no-op), so a bulk action is a loop over
selected `RecordCode`s in one transaction, plus checkboxes and a "Approve selected" button on
`frmIACChecker.aspx`. Remarks would apply to the whole selection.

If instead the business exempts uploads (`'A'`), this is not needed — the inbox only ever holds
typed records, which are low volume.

**These two decisions are the same decision, and it is worth making before rollout rather than after.**

### Questions for the business

1. Should a maker ever be allowed to be a checker? Currently blocked. If dual-holding is intended
   for some users, that block is what makes it safe; if not, `VMIS_CHECKER` should be made a
   primary (exclusive) role in `AssignRole()`.
2. Should a rejected record be permanently locked, or should a supervisor be able to reopen it?
3. ~~Do bulk Excel imports need approval?~~ **Decided 2026-07-27: yes.** `ImportApprovalStatus`
   stays at `'P'` for IAC — imported records go to the checker like any other. This makes gap 1
   above (bulk approval in the inbox) a real requirement rather than an option, since upload is
   IAC's dominant path. **Not yet built.**
4. Should desk-user annotations be checked?
5. Is a second approval level (zonal → HO) anticipated? Cheaper to design for now than to retrofit —
   the central table makes this materially easier than the per-table design would have.

### Migrating Complaint onto the central registry — when wanted

Data is small and the shape is a direct match:

1. `INSERT INTO CASE_APPROVAL` from `COMPLAINT` — `RecordCode = CODE`, `RecordRef = RNO`,
   `ZoneSolID = NEWZONE`, plus the existing `APPROVALSTATUS`/maker/checker columns
2. `INSERT INTO CASE_APPROVAL_HISTORY` from `COMPLAINT_APPROVAL_HISTORY` (column names map 1:1)
3. Register `COMPLAINT` in `WORKFLOW_MODULE`
4. Repoint `spComplaint_Update` and `spComplaint_View` at `CASE_APPROVAL`; retire
   `spComplaint_CheckerAction` in favour of `spCase_CheckerAction`
5. Repoint `frmComplaintChecker.aspx.cs` at `spCase_CheckerQueue` (it currently runs inline SQL)
   and `frmComplaintCheckerView.aspx.cs` at `spCase_CheckerAction`
6. Leave the six columns on `COMPLAINT` in place initially as a read-only fallback; drop them
   only after a period of parallel running — and if they are ever dropped, **drop the matching
   columns from `COMPLAINT_HISTORY` in the same order**, or the `SELECT *` history copy breaks

Migrating Complaint also collapses the two inboxes into one: `spCase_CheckerQueue` with
`@p_MODULE = NULL` already returns every module a user checks.
