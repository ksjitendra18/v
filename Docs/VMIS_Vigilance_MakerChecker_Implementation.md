# Vigilance Maker–Checker — Implementation Record

**Module:** Vigilance (`Mis/frmVigilance.aspx` and `Mis/Vigilance.aspx`)
**Delivered:** 2026-07-29
**Database:** `VigilanceMISDB`
**Companions:** `VMIS_IAC_MakerChecker_Implementation.md` (the recipe this follows), `VMIS_MakerChecker_Rollout_Plan.md`, `VMIS_Technical_Overview.md`, `VMIS_Database_Inventory.md`

---

## 1. What this is

The second module onto the central `CASE_APPROVAL` registry built for IAC on 2026-07-27,
following the "adding the next module" recipe in `VMIS_IAC_MakerChecker_Implementation.md` §9.

**No foundation objects were changed.** `WORKFLOW_MODULE`, `CASE_APPROVAL`,
`CASE_APPROVAL_HISTORY`, `spCase_CheckerAction` and `spCase_CheckerQueue` are already generic;
Vigilance is registered against them and reuses both without modification.

**IAC and Complaint are not affected.** Complaint remains on its own older per-table mechanism
(six columns on `COMPLAINT`), unchanged.

---

## 2. Why Vigilance is the awkward one

`VIGILANCE` and `VIGILANCE_HISTORY` are **171 columns each**, `spVigilance_Update` takes
**144 parameters**, and three procedures copy history with
`INSERT INTO VIGILANCE_HISTORY SELECT * FROM VIGILANCE`. That statement works only while both
tables stay in exact column count *and ordinal order*.

The central design is what makes this safe: **not one column was added to either table**, so the
ordinal trap cannot be sprung. Had the Complaint per-table pattern been replicated here, six
columns would have had to be appended to both 171-column tables in identical order, with silent
history corruption from the first mistake onward.

Two further Vigilance-specific wrinkles:

| Wrinkle | How it is handled |
|---|---|
| **Two live entry pages** — `frmVigilance.aspx` (old) and `Vigilance.aspx` (new) are both on the menu | Both call the same `spVigilance_Update`, so putting the rule in the procedure covers both automatically. Both grids and both code-behinds were still updated, so the lock is visible in each |
| **`spVigilanceExcel_Import` was uncallable** | Repaired as part of this work — see §5.4 |

---

## 3. Files delivered

### New — database scripts

| File | Contents |
|---|---|
| `Database/Scripts/2026-07-29_Vigilance_MakerChecker.sql` | Registry row, backfill, and 3 modified procs |
| `Database/Scripts/2026-07-29_Vigilance_MakerChecker_ExcelImport.sql` | `spVigilanceExcel_Import` — workflow registration plus the repair in §5.4 |

Both are idempotent (`CREATE OR ALTER`, existence guards, `MERGE`) and safe to re-run.

### New — web pages

| File | Purpose |
|---|---|
| `VMISP/Mis/frmVigilanceChecker.aspx` (+ `.cs`, `.designer.cs`) | Checker inbox: every Vigilance record pending this checker's verification |
| `VMISP/Mis/frmVigilanceCheckerView.aspx` (+ `.cs`, `.designer.cs`) | Read-only Vigilance record + Accept / Push Back / Reject with mandatory remarks |

### Modified

| File | Change |
|---|---|
| `VMISP/Mis/Vigilance.aspx` | `OnRowDataBound` wired up; "Checker Status" and "Checker Remarks" grid columns |
| `VMISP/Mis/Vigilance.aspx.cs` | Row-level edit lock, checker lock on fetch-by-number, save now surfaces the proc's message |
| `VMISP/Mis/frmVigilance.aspx` | Same two grid columns |
| `VMISP/Mis/frmVigilance.aspx.cs` | Same three changes, adapted to this page's image button |
| `VMISP/Upload/frmExcelUpload.aspx.cs` | `funcExcelImport_Vigilance` — passes the zone, commits on rows actually imported, reports skipped rows with reasons |
| `VMISP/Upload/Files/ExcelImport/VIGILANCE.xlsx` | Five columns added (see §5.4) |
| `VMISP/Web.sitemap` | Vigilance Checker Inbox node; `VMIS_CHECKER` on the Vigilance group; explicit roles on its maker/report children |
| `VMISP/VMISP.csproj` | Registered the six new page files |

---

## 4. Registry entry

One row added to `dbo.WORKFLOW_MODULE`:

| Column | Value | Note |
|---|---|---|
| `ModuleCode` | `VIGILANCE` | the discriminator used everywhere |
| `ModuleName` | `Vigilance` | label in the inbox |
| `TableName` | `VIGILANCE` | |
| `KeyColumn` | `CODE` | `int IDENTITY` surrogate → `CASE_APPROVAL.RecordCode` |
| `RefColumn` | `RNO` | the R Number a user sees → `CASE_APPROVAL.RecordRef` |
| `ZoneColumn` | `NEWZONE` | `varchar(10)` SOL code → `CASE_APPROVAL.ZoneSolID`, which routes to a checker |
| `ViewPage` | `~/Mis/frmVigilanceCheckerView.aspx` | |
| `ImportApprovalStatus` | `P` | bulk-imported records require checking. See §5.4 |

> Note the key differs from IAC's: IAC keys on `SNO`, Vigilance on `CODE`. The rollout plan's §2
> table is the reference for each module — they are not consistent.

---

## 5. Procedures modified

### 5.1 `spVigilance_Update` — the write path

**No parameters were added or removed.** Both existing C# call sites work unchanged.

**Insert (`@p_MODE = 'I'`)**
Wrapped in a transaction. After the `INSERT INTO VIGILANCE`, captures `SCOPE_IDENTITY()` and
writes a `CASE_APPROVAL` row at `'P'` plus a `SUBMITTED` audit row.

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
sit `'P'` forever, invisible to every inbox and locked from editing.

**The `VMIS_DESKUSER` branch is unchanged** — desk-user annotations still bypass approval
entirely. Pre-existing behaviour, and an open question for the business (§9).

**New `@o_ERRCODE` values:** `4` = zone missing, `5` = record locked. Existing values unchanged
(`0` error, `1` saved, `2` updated, `3` duplicate R number).

### 5.2 `spVigilance_View` — the read path

All three branches (`LIST`, `SEARCH`, single-record `GET`/`VIEW`) now `LEFT JOIN` `CASE_APPROVAL`
and return three extra columns: `APPROVALSTATUS`, `APPROVALSTATUSTEXT`, `CHECKERREMARKS`.

`LEFT JOIN`, so records with no approval row still list, with `NULL` status.

The base table is now aliased `V` in all three branches, and the `SEARCH` branch's `@STRCOND` seed
changed from `ACTIVE='Y'` to `V.ACTIVE='Y'` to match. The per-field search predicates it appends
stay unqualified and remain unambiguous — no `CASE_APPROVAL` column name collides with a
`VIGILANCE` one.

### 5.3 `spVigilanceUser_Update` — bypass closed

`Mis/frmVigilanceUpdate.aspx` (MISUSER-only) changes Basic Pay, DA_CO/ZO/HO, Register or Penalty
Proceeding on an existing record. It **never touched the approval state**, so a maker could alter
an *approved* record and it stayed approved and unverified. Now:

- a rejected record is refused (`@o_ERRCODE = 3`)
- an approved or pushed-back record is re-queued to `'P'` with a `RESUBMITTED` audit row naming
  the field that changed, e.g. `"BASICPAY changed via Vigilance Update screen."`

Two optional parameters (`@p_USERROLE`, `@p_USERIP`) were appended **with defaults**, so the
existing nine-parameter call site works untouched.

> The equivalent bypass still exists in `spComplaintUser_Update`. Same shape, same fix. Out of
> scope here.

### 5.4 `spVigilanceExcel_Import` — and a defect found on the way

**Vigilance Excel upload did not work before this change, and had not for some time.** Three
independent faults, any one of which fails the run:

1. `funcExcelImport_Vigilance` reads `row["TMSACREFNO"]`, `row["BANKNAME"]` and
   `row["DESK_USER_REMARKS"]`. **None of those columns exist in `VIGILANCE.xlsx`**, so the sheet
   throws on the first row.
2. The same function passes `@p_DESK_USER_REMARKS` and `@p_BANKNAME`, which the procedure **did
   not declare**.
3. It omits `@p_TABLENAME` and `@p_USER`, which **had no defaults**.

On top of that the procedure never wrote `NEWZONE` at all — so even once it ran, every imported
record would have been unroutable to any checker.

Rather than put a new control on top of a broken path, the import was repaired:

- `@p_TABLENAME` and `@p_USER` defaulted, so the existing call site binds
- `@p_BANKNAME`, `@p_DESK_USER_REMARKS`, `@p_NEWZONESOLID`, `@p_NEWCIRCLESOLID` added, all
  defaulted, and written to `VIGILANCE`
- `VIGILANCE.xlsx` gained the five columns the code expects:
  `TMSACREFNO`, `BANKNAME`, `DESK_USER_REMARKS`, `NEWZONESOLID`, `NEWCIRCLESOLID`
- `funcExcelImport_Vigilance` now passes the two zone columns

The workflow changes then follow the IAC pattern exactly. The import registers every row it
inserts in `CASE_APPROVAL` and logs an `IMPORTED` audit row.

**Whether imported records need checking is configuration, not code** —
`WORKFLOW_MODULE.ImportApprovalStatus`:

| Value | Behaviour | Trade-off |
|---|---|---|
| `'P'` **(set here)** | Imported records are Pending and appear in the checker inbox like any other | The control covers the bulk path. But a large upload puts every row in front of a checker, and **the maker cannot edit any of them until actioned**. Practical only with a bulk action in the inbox — see §10 |
| `'A'` | Imported records are registered Approved, with `CheckerUser = 'SYSTEM'` and an audit note | An explicit, audited exemption instead of a silent gap. Records still carry a visible status, appear in reporting, and **re-enter the workflow the moment anyone edits them** |

```sql
-- exempt bulk uploads from checking
UPDATE dbo.WORKFLOW_MODULE SET ImportApprovalStatus = 'A' WHERE ModuleCode = 'VIGILANCE';
```

**Rows with no zone are rejected when imports require checking** (`@o_ERRCODE = -2`), for the same
reason the manual form refuses. Under `'A'` no zone is needed, since nothing has to be routed.

**Transaction handling.** `funcExcelImport_Vigilance` wraps the whole sheet in one transaction, so
the proc only opens its own when `@@TRANCOUNT = 0`, and never commits or rolls back a transaction
it did not start. On error it re-throws, so the caller's catch still rolls the sheet back and
reports the offending row.

**The same last-row commit bug IAC had was present here too**, and is fixed the same way:
`funcExcelImport_Vigilance` committed on `intErrCode`, which held only the *last* row's result, so
one rejected row at the end of a sheet silently discarded the whole upload. It now commits when
**any** row imported, and reports each skipped row's reason (first 10, then "and N more").

---

## 6. Application changes

### 6.1 UI convention — as settled for IAC

The convention fixed in `VMIS_IAC_MakerChecker_Implementation.md` §6.0 is followed exactly. No new
visual design was invented for this module.

| Page | Style |
|---|---|
| **Checker inbox** (`frmVigilanceChecker.aspx`) | Bootstrap 5 card design — gradient header, `card-custom`, `table-hover`, badge status chips, client-side search box |
| **Verification page** (`frmVigilanceCheckerView.aspx`) | Bootstrap 3, mirroring `Mis/Vigilance.aspx` — `panel panel-primary`, `col-sm-12 alert alert-dark`, `form-group row` of `col-sm-3` cells, `form-control input-sm`, `label label-*` status chips |
| **Action bar** | Pinned `.checker-action-bar` at the bottom of the viewport |

`GetStatusClass()` therefore returns Bootstrap 5 badge modifiers in the inbox code-behind and
Bootstrap 3 `label label-*` classes in the verification code-behind. That is intentional — each
matches the framework its own page loads. **Do not load Bootstrap 5 on the verification page.**

### 6.2 `frmVigilanceChecker.aspx` — the inbox

Calls `spCase_CheckerQueue` with `@p_MODULE = 'VIGILANCE'`. Columns: R No, Zone, Submitted By,
Submitted On, Status badge, and a View link carrying `?id=<CODE>`.

### 6.3 `frmVigilanceCheckerView.aspx` — verification and decision

Read-only view of the record **in `Mis/Vigilance.aspx`'s exact field order and with its exact
labels** — 96 fields, from R Number / R Number 1 / Name & Particulars / Name down to Status and
Dealing Officer Remarks. A short header strip shows R Number, Checker Status, Submitted By and
Submitted On, then a Checker Decision section with a mandatory remarks box.

- The query string carries `VIGILANCE.CODE`, which is what `CASE_APPROVAL.RecordCode` holds.
- Every field is a `ReadOnly` TextBox with a grey background.
- Action buttons and the "verify carefully" note appear **only** while the record is `'P'`. The
  remarks box stays visible either way, so a past decision can still be read.
- `lblMsg` sits in the bar but **outside** `pnlActions`, so outcomes and refusals still show after
  the record has been actioned.
- Remarks are enforced client-side, in the code-behind, and again in the proc.
- All three buttons route to `spCase_CheckerAction` — the C# holds none of the control logic.

**Master lookups resolved.** IAC's known fidelity gap — showing stored codes where the entry form
shows descriptions — is closed here. The page resolves `NATURECASE`, `STATUSCODE`, `REGISTER`,
`SCALE`, `PENALTYPROCEEDING`, `DISAUTHORITYSCIRCLE`, `LETTERSENTTO`, `NEWZONE` and `NEWCIRCLE`
against their masters, with `ISNULL` falling back to the raw code if a master row has been removed,
so nothing silently disappears from the record. The remaining dropdowns (`Circle`, `Zone`, `Final`,
`Lodi Case`, `State`, `Penalty Type`) store their *text* rather than a code, so no lookup is needed.

**The markup and its `.designer.cs` were generated from one field spec**, so the two cannot drift.
Cross-checked afterwards: every `runat="server"` control in the markup has a declaration, and the
designer declares nothing the markup lacks.

### 6.4 The two maker screens

Both got the same treatment.

**Grid** — two new columns, "Checker Status" and "Checker Remarks".

**Row-level edit lock** (`gvMain_RowDataBound`):

| Status | `Vigilance.aspx` (text button) | `frmVigilance.aspx` (image button) |
|---|---|---|
| `P` | disabled, "Pending", warning styling | disabled, tooltip explains why |
| `C` | enabled, "Edit", info styling | enabled |
| `X` | disabled, "Rejected", danger styling | disabled, tooltip explains why |
| `A` / `NULL` | enabled, "Edit" | enabled |

`frmVigilance.aspx` opens a row with an `ImageButton`, so there is no label to carry the signal —
the lock reads as a tooltip there instead. Neither page's `funcControlsUserRights()` re-enables
grid buttons after binding (unlike IAC's), so no label check was needed.

**Fetch-by-number lock** (`funcApplyCheckerLock`) — the "Get" button loads a record straight into
the form, bypassing the grid. Pending and rejected records now hide the Update button and explain
why; a pushed-back record shows the checker's remarks. Convenience only — the proc refuses these
saves regardless.

**Save now reports the real reason.** Both `funcSave` implementations previously did
`if (ExecuteNonQuery() > 0)`. The new guards refuse a save *without running any DML*, so their
message would never have reached the user. Both now read `@o_ERRCODE` and display `@o_EERMSG`,
clearing the form only on `1` (saved) or `2` (updated).

### 6.5 `Web.sitemap`

- `VMIS_CHECKER` added to the **Vigilance** group node
- New node: `~/Mis/frmVigilanceChecker.aspx`, `roles="VMIS_CHECKER"`
- The maker/report children under that group were given explicit roles
  (`VMIS_MISUSER,VMIS_DESKUSER,VMIS_VIEWUSER`) so a checker-only user sees **only** the inbox

The top-level ancestor node already carries `VMIS_CHECKER` from the IAC delivery.

---

## 7. Verification performed

### Database — 18 workflow scenarios, all passed

Executed against live `VigilanceMISDB`; test data removed afterwards (`VIGILANCE`,
`VIGILANCE_HISTORY` and the `VIGILANCE` rows of `CASE_APPROVAL` / `CASE_APPROVAL_HISTORY` all back
to 0, identity reseeded).

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
| 12 | `spVigilanceUser_Update` on the approved record | re-queued to `'P'` |
| 13 | Checker rejects, then maker retries via the update screen **and** the entry form | both refused |
| 14 | Audit trail | `SUBMITTED → PUSHED_BACK → RESUBMITTED → APPROVED → RESUBMITTED → REJECTED` |
| 15 | `LIST` / `SEARCH` / `GET` view branches | all three return the three new columns |
| 16 | Orphan monitor | empty, as expected |
| 17 | Backfill grandfathers a pre-existing row | registered `'A'` with `CheckerUser = 'SYSTEM'` |
| 18 | Backfill re-run | 0 new rows — idempotent |

### Database — Excel import, 10 further scenarios, all passed

| # | Scenario | Result |
|---|---|---|
| 1 | Import with zone, `ImportApprovalStatus = 'P'` | saved, registered Pending |
| 2 | Import with **blank** zone under `'P'` | rejected, code -2, **and no partial VIGILANCE row left behind** |
| 3 | Duplicate R number | rejected, code -1 (unchanged behaviour) |
| 4 | Imported record in the checker queue | appears alongside typed records |
| 5 | Checker approves the imported record | success |
| 6 | Import with blank zone under `ImportApprovalStatus = 'A'` | saved, registered Approved, `CheckerUser = 'SYSTEM'` with the exemption note |
| 7 | State of all imported records | statuses and checker fields as configured |
| 8 | Maker edits an exempt imported record | re-queued to Pending — the exemption applies to the import, not forever |
| 9 | Audit trail | `IMPORTED → APPROVED` and `IMPORTED → RESUBMITTED` recorded correctly |
| 10 | Orphan monitor | empty |

### Application — compiles clean

**C# compilation passes with zero errors.** Visual Studio is not installed on the development
machine, so MSBuild cannot build a web project (`Microsoft.WebApplication.targets` missing).
Compiling the csproj's full `Compile` item list directly with Roslyn (206 files) **succeeds and
produces an assembly**. The only excluded files are `Reports/frmCaseRegister.aspx.cs` and
`Upload/frmAccessUpload.aspx.cs`, which need Office COM interops absent from this machine and are
unrelated to this work.

That covers the code-behind and its binding to the designer files. It does **not** cover the
`.aspx` markup, which only `aspnet_compiler` validates — though both new designer files were
generated from the same spec as their markup and cross-checked against it, so the two cannot drift.

> **Open item: build in Visual Studio and smoke-test the two new checker pages, both maker screens,
> and one real Excel upload.** No end-to-end run through the running application has happened for
> this module.

---

## 8. Deployment

Run in order against `VigilanceMISDB`:

```
1. Database/Scripts/2026-07-27_CaseApproval_Central_MakerChecker_Foundation.sql   (if not already applied)
2. Database/Scripts/2026-07-27_IAC_MakerChecker_ExcelImport.sql                   (if not already applied -- adds ImportApprovalStatus)
3. Database/Scripts/2026-07-29_Vigilance_MakerChecker.sql
4. Database/Scripts/2026-07-29_Vigilance_MakerChecker_ExcelImport.sql
```

Scripts 3 and 4 re-create `ImportApprovalStatus` if step 2 has not been run, so they are safe in
either order relative to it.

Then build and deploy the application, **including the updated `VIGILANCE.xlsx` template** — users
must re-download it, because uploads of the old sheet will now fail on the five missing columns.

### Three decisions baked in — change them here if the business disagrees

**Pre-existing Vigilance records are grandfathered as Approved.** Step 1 of the Vigilance script
marks every existing active row `'A'` with `CheckerUser = 'SYSTEM'` and a `GRANDFATHERED` audit row,
on the basis that those records predate the control. Marking them `'P'` instead would flood the
checker inbox and lock every existing record from editing. `VIGILANCE` was empty on the development
database, so this was a no-op there and **will not be in production** — change the literal in step 1
before deploying if the opposite is wanted.

**Self-approval is blocked** by `spCase_CheckerAction` (foundation guard 5). If UAT uses a single
account holding both `VMIS_MISUSER` and `VMIS_CHECKER`, that account cannot approve its own entries.

**Bulk-imported Vigilance records default to Pending** (`ImportApprovalStatus = 'P'`), and uploads
will start rejecting rows whose `NEWZONESOLID` is blank. One `UPDATE` switches it (§5.4).

### Prerequisites for a working end-to-end test

- At least one user in `MakerCheckerMapping` with `IsChecker = 1`, `IsActive = 1`, and a
  `ZoneSolID` matching the `NEWZONE` used on the record
- **A different** user holding `VMIS_MISUSER` to act as maker
- The reference masters the Vigilance form depends on must be populated (`STATUS`, `SCALE`,
  `NATURECASE`, `REGISTER`, `PENALTYPROCEEDING`, `BRANCH_MASTER`, `BRANCH_MASTER_NEW`, …).
  `VMIS_MakerChecker_Rollout_Plan.md` §7 records these as empty on the development database

### Rollback

The DB changes are additive; no case-table DDL was touched.

```sql
-- restore the three Vigilance procs from source control, then:
DELETE FROM dbo.CASE_APPROVAL_HISTORY WHERE ModuleCode = 'VIGILANCE';
DELETE FROM dbo.CASE_APPROVAL         WHERE ModuleCode = 'VIGILANCE';
DELETE FROM dbo.WORKFLOW_MODULE       WHERE ModuleCode = 'VIGILANCE';
```

Leave the foundation objects in place — IAC still uses them.

---

## 9. Known gaps and open questions

| # | Item |
|---|---|
| 1 | **No bulk action in the checker inbox.** With `ImportApprovalStatus = 'P'`, a checker must open and action every imported record one at a time. Carried over from the IAC delivery; the fix is the same one page for both modules |
| 2 | **No end-to-end run through the running application.** C# compiles clean and the database behaviour is verified in 28 scenarios, but the markup has not been rendered and no upload has been done through the UI |
| 3 | **Desk-user annotations bypass approval.** `VMIS_DESKUSER` appends HO Status and remarks with no verification. Pre-existing; unchanged here |
| 4 | **A rejected record is locked permanently.** No reopen path exists for anyone |
| 5 | **`spComplaintUser_Update` still has the bypass** §5.3 closed for Vigilance and IAC |
| 6 | **Complaint is still on the old mechanism.** A checker with both roles has two inboxes until Complaint migrates. `spComplaintExcel_Import` also still bypasses approval |
| 7 | **`spVigilance_Delete` is not wired to the workflow.** Same situation as `spIACStructure_Delete` — it soft-deletes and leaves the `CASE_APPROVAL` row behind |
| 8 | **`VigilanceMonitoring` and `PenaltyCharge` are separate modules**, not covered by this work despite sitting under the same menu group |

### Questions for the business

1. Should a rejected record be permanently locked, or should a supervisor be able to reopen it?
2. Should desk-user annotations be checked?
3. Is a second approval level (zonal → HO) anticipated?
4. Should Vigilance bulk uploads require checking (`'P'`, as configured) or be exempted (`'A'`)?
   The same decision was taken as `'P'` for IAC on 2026-07-27.

---

## 10. Next module

The recipe in `VMIS_IAC_MakerChecker_Implementation.md` §9 still applies unchanged. Suggested
order for what remains of Tier A: **MISC → RTI → NOC → RRB → Vigilance Monitoring.**

Two things this delivery adds to that recipe:

- **Check the module's Excel import actually works before adding workflow to it.** Vigilance's had
  been broken by a template/parameter mismatch; a control layered on a dead path proves nothing.
- **Generate the verification page's markup and `.designer.cs` from a single field spec.** For a
  96-field form, hand-writing both and hoping they match is not a reasonable bet.

`spRRB_Update` and `spRRB_Operation` are different procedures and **both** need the change when RRB
is rolled out.
