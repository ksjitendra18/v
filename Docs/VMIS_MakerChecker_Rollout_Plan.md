# Maker–Checker Rollout — Candidate Pages and Table Strategy

**Scope:** extend the Complaint maker–checker workflow to the remaining VMISP modules
**Prepared:** 2026-07-25
**Based on:** live `VigilanceMISDB` on `localhost\SQLEXPRESS` + code review
**Companions:** `VMIS_Technical_Overview.md`, `VMIS_Database_Inventory.md`

> **Status update — 2026-07-27.** The Option 2 recommendation in §5 was accepted and built.
> The central foundation (`WORKFLOW_MODULE`, `CASE_APPROVAL`, `CASE_APPROVAL_HISTORY`,
> `spCase_CheckerAction`, `spCase_CheckerQueue`) is live, and **IAC** is the first module on it.
> See **`VMIS_IAC_MakerChecker_Implementation.md`** for what was built, what was verified, and
> the recipe for the next module. Phasing in §8 below is superseded by that document's §9.
>
> **Status update — 2026-07-29.** **Vigilance** is the second module on the central registry.
> See **`VMIS_Vigilance_MakerChecker_Implementation.md`**. It also closed the
> `spVigilanceUser_Update` bypass listed in §3.1 and repaired `spVigilanceExcel_Import`, which
> had been non-functional. Remaining on the old per-table mechanism: **Complaint**.
> Remaining Tier A: MISC, RTI, NOC, RRB, Vigilance Monitoring.

---

## 1. Summary

There are **14 case modules** besides Complaint. All are genuine candidates, but they are not equally ready. The deciding factor is whether the table already carries a **SOL-coded zone column**, because that is what `MakerCheckerMapping` scopes a checker to.

| Tier | Modules | Why | Effort per module |
|---|---|---|---|
| **A — ready** | IAC, Vigilance, MISC, RTI, RRB, NOC, Vigilance Monitoring | Already have `NEWZONE varchar(10)` | Low |
| **B — rename only** | ABBFF, LODI, Penalty Charge | Have a zone column under a different name (`NEW_ZONE`, `LODI_ZONE`, `PC_ZONE`) | Low + mapping |
| **C — no coded zone** | SR, WB, Operational Ref | Only free-text `ZONE varchar(100)` — cannot join to `ZoneSolID` | Medium — needs data migration |
| **D — no zone at all** | Sanction, Sanction for Investigation, Sanction for Prosecution | No zone column; only `SFI_CIRCLE` / `SFP_CIRCLE` | High — needs a routing decision from the business |

**Before any of this is worth doing, three bypass routes must be closed.** As things stand today, the Complaint control that is already live can be circumvented — see §3. Rolling the same pattern out to 14 more modules without fixing that multiplies a hole rather than a control.

**Architectural recommendation:** do **not** replicate the per-table column approach 14 times. Use a **central approval table** keyed by module + record. Rationale and migration path in §5.

---

## 2. Candidate page inventory

Every page below writes to a case table and is therefore in scope. The "Write proc" column is what actually has to change.

### 2.1 Tier A — has `NEWZONE varchar(10)`, ready today

| Module | Entry page(s) | Write proc(s) | Table (cols) | History | Business key |
|---|---|---|---|---:|---|
| **IAC** | `Mis/frmIACStructure.aspx` | `spIACStructure_Update` | `IAC` (58) | `IAC_HISTORY` (58) | `SNO` / `CODE` |
| **Vigilance** | `Mis/frmVigilance.aspx`<br>`Mis/Vigilance.aspx` | `spVigilance_Update` *(both pages)* | `VIGILANCE` (**171**) | `VIGILANCE_HISTORY` (171) | `RNO` / `CODE` |
| **MISC** | `Mis/frmMiscStructure.aspx` | `spMiscStructure_Update` | `MISC` (66) | `MISC_HISTORY` (66) | `RNO` / `CODE` |
| **RTI** | `Mis/frmRTI.aspx` | `spRTI_Update` | `RTI` (57) | `RTI_HISTORY` (57) | `RTINO` / `CODE` |
| **RRB** | `Mis/frmRRB.aspx`<br>`Mis/RRB.aspx` | `spRRB_Update` **and** `spRRB_Operation` ⚠️ *(two different procs)* | `RRB` (**159**) | `RRB_HISTORY` (159) | `RNO` / `CODE` |
| **NOC** | `Mis/Noc.aspx`<br>`Mis/frmNoc.aspx` | `spNOC_Update` *(both pages)* | `NOC` (37) | `NOC_HISTORY` (41) | `SNO` / `CODE` |
| **Vigilance Monitoring** | `Mis/VigilanceMonitoring.aspx` | `spVigilanceMIS_Update` ⚠️ **missing from DB** | `VIGILANCEMIS` (55) | `VIGILANCEMIS_HISTORY` (50) | `VIGM_RNO` / `VIGM_CODE` |

### 2.2 Tier B — zone exists under another name

| Module | Entry page | Write proc | Zone column | Note |
|---|---|---|---|---|
| **ABBFF** | `Mis/frmABBFF.aspx` | `spABBFFStructure_Update` | `NEW_ZONE varchar(50)` | ⚠️ **No `ABBFF_HISTORY` table exists.** The proc writes into `MISC` and `MISC_HISTORY` — almost certainly copy-paste legacy. Must be fixed before adding workflow. |
| **LODI** | `Mis/Lodi.aspx` | `spLodi` (→ `spLodi_History`) | `LODI_ZONE varchar(10)` | Uses explicit-column history (safe). Business question: is a LODI addition/deletion a "case" needing approval, or is it already governed by `Master/LodiDisable.aspx`? |
| **Penalty Charge** | `Mis/PenaltyCharge.aspx` | `spPenaltyCharge` (→ `spPenaltyCharge_History`) | `PC_ZONE varchar(10)` | Explicit-column history (safe). |

### 2.3 Tier C — free-text zone, needs data migration

| Module | Entry page(s) | Write proc | Zone column | Problem |
|---|---|---|---|---|
| **SR** | `Mis/frmSRStructure.aspx`<br>`Mis/SR.aspx` | `spSRStructure_Update` *(both)* | `ZONE varchar(100)` | Free text — no SOL code to join on |
| **WB** | `Mis/frmWBStructure.aspx`<br>`Mis/WB.aspx` | `spWBStructure_Update` *(both)* | `ZONE varchar(100)` | Same |
| **Operational Ref** | `Mis/frmOperationalRef.aspx` | `spOperationalRef_Update` | `ZONE varchar(100)` | Same |

These need `NEWZONE varchar(10)` / `NEWCIRCLE varchar(10)` added and back-filled, exactly as was done for Complaint/IAC/MISC/RTI at some earlier point. The entry forms also need the cascading Zone→Circle dropdown (`ddlZoneNew_SelectedIndexChanged`) that the Tier A forms already have.

### 2.4 Tier D — no zone; routing decision required

| Module | Entry page | Write proc | Available geography |
|---|---|---|---|
| **Sanction for Investigation** | `Mis/SanctionForInvestigation.aspx` | `spSanctionForInvestigation` | `SFI_CIRCLE varchar(10)` only |
| **Sanction for Prosecution** | `Mis/SanctionForProsecution.aspx` | `spSanctionForProsecution` | `SFP_CIRCLE varchar(10)` only |
| **Sanction** | `Mis/frmSanction.aspx` | `spSanction_Update` | **none** |

Sanction cases are head-office processes. Zone-based checker assignment may be the wrong model entirely — these probably need either a fixed HO checker group or circle-based routing. **This needs a stakeholder decision before design.**

### 2.5 Out of scope (recommend excluding)

| Page | Reason |
|---|---|
| `Master/*` (12 forms), `Admin/*` | Reference data, already restricted to ADMIN/SUPERUSER. Maker–checker on masters is a separate, larger governance question. |
| `Search/*`, `Reports/*` | Read-only. |
| `Mis/frmComplaintView.aspx` | Read-only view. |

---

## 3. 🔴 Close these bypass routes first

The Complaint workflow as deployed today can be circumvented three ways. Each one applies to every module you extend to, so fix the pattern once, now.

### 3.1 The bulk field-update forms write straight past the workflow

`spComplaintUser_Update` — called by `Mis/frmComplaintUpdate.aspx` (MISUSER-only menu item) — updates `CIRCLEOFFICE` or `SENTTO` and **never touches `APPROVALSTATUS`**:

```sql
IF(@p_FIELD = 'CIRCLE')
    UPDATE COMPLAINT SET CIRCLEOFFICE=@p_CIRCLEOFFICE, MODUSER=@p_USER, MODDATE=GETDATE()
    WHERE RNO=@p_RNO;      -- APPROVALSTATUS untouched
```

So a maker can alter an **approved** complaint's circle office and it stays approved, unverified. The equivalent procs exist for two more modules and have the same shape:

| Proc | Page | Module |
|---|---|---|
| `spComplaintUser_Update` | `Mis/frmComplaintUpdate.aspx` | Complaint |
| `spIACUser_Update` | `Mis/frmIACUpdate.aspx` | IAC |
| `spVigilanceUser_Update` | `Mis/frmVigilanceUpdate.aspx` | Vigilance |

**Fix:** apply the same "prior status `A` or `C` → reset to `P` + log `RESUBMITTED`" logic that `spComplaint_Update` already has, or remove these forms from the MISUSER menu.

### 3.2 Excel import creates records that no checker ever sees

`COMPLAINT.APPROVALSTATUS` is `char(1) NOT NULL DEFAULT ('P')` — good, an import that omits the column still lands as Pending. **But `NEWZONE` is nullable**, and the checker inbox joins on it:

```sql
FROM COMPLAINT C
INNER JOIN MakerCheckerMapping UZM ON C.NEWZONE = UZM.ZoneSolID
WHERE ... AND C.APPROVALSTATUS = 'P'
```

An `INNER JOIN` on a NULL `NEWZONE` yields nothing. Imported rows therefore sit at `'P'` **forever, invisible to every checker and locked from editing** (the maker's grid disables the button for `'P'`). Silent, permanent limbo.

**Fix (both needed):**
1. Make the import procs populate `NEWZONE`, or reject rows without it.
2. Add an **orphan monitor** — a query/report for `APPROVALSTATUS='P' AND (NEWZONE IS NULL OR NEWZONE NOT IN (SELECT ZoneSolID FROM MakerCheckerMapping WHERE IsChecker=1 AND IsActive=1))`. Without this, nothing surfaces the problem.

Affected import procs: `spIACExcel_Import`, `spMISCExcel_Import`, `spRRBExcel_Import`, `spRTIExcel_Import`, `spSRExcel_Import`, `spVigilanceExcel_Import`, `spWBExcel_Import`, `spLodiExcel_Import`, `spACCESSSR_Import` (+ `spComplaintExcel_Import`, `spNOCExcel_Import`, `spSFIExcel_Import`, `spSFPCExcel_Import`, `spVIGMExcel_Import` once restored — see §7).

### 3.3 Modules with two entry forms need both covered

Five modules have an old and a new page live simultaneously, both on the menu:

| Module | Old page | New page | Same write proc? |
|---|---|---|---|
| SR | `frmSRStructure.aspx` | `SR.aspx` | Yes — `spSRStructure_Update` |
| WB | `frmWBStructure.aspx` | `WB.aspx` | Yes — `spWBStructure_Update` |
| Vigilance | `frmVigilance.aspx` | `Vigilance.aspx` | Yes — `spVigilance_Update` |
| NOC | `frmNoc.aspx` | `NOC.aspx` | Yes — `spNOC_Update` |
| **RRB** | `frmRRB.aspx` | `RRB.aspx` | **No — `spRRB_Update` vs `spRRB_Operation`** |

Where the proc is shared, enforcing in the proc covers both pages automatically — a good argument for putting the rule in SQL rather than C#. **RRB is the exception and needs both procs changed.**

> **Design principle for this rollout:** put the status transition inside the write procedure, not in the page. There are 20+ write paths across 14 modules and at least 5 duplicate pages; C#-side enforcement will be missed somewhere.

---

## 4. Structural readiness of the write procedures

Good news — the modules are structurally near-identical to Complaint, so the `spComplaint_Update` change transplants cleanly.

| Proc | `@p_MODE` | MISUSER branch | DESKUSER branch | `@p_USERIP` |
|---|:--:|:--:|:--:|:--:|
| `spComplaint_Update` *(reference)* | ✅ | ✅ | ✅ | ✅ |
| `spIACStructure_Update` | ✅ | ✅ | ✅ | ✅ |
| `spVigilance_Update` | ✅ | ✅ | ✅ | ✅ |
| `spMiscStructure_Update` | ✅ | ✅ | ✅ | ✅ |
| `spRTI_Update` | ✅ | ✅ | ✅ | ✅ |
| `spRRB_Update` / `spRRB_Operation` | ✅ | ✅ | ✅ | ✅ |
| `spNOC_Update` | ✅ | ✅ | ✅ | ✅ |
| `spSRStructure_Update` | ✅ | ✅ | ✅ | ✅ |
| `spWBStructure_Update` | ✅ | ✅ | ✅ | ✅ |
| `spOperationalRef_Update` | ✅ | ✅ | ✅ | ✅ |
| `spSanctionForInvestigation` | ✅ | ✅ | ✅ | ✅ |
| `spSanctionForProsecution` | ✅ | ✅ | ✅ | ✅ |
| `spABBFFStructure_Update` | ✅ | ✅ | ❌ | ✅ |
| `spLodi` | ✅ | ✅ | ❌ | ✅ |
| `spPenaltyCharge` | ✅ | ✅ | ❌ | ✅ |
| `spSanction_Update` | ✅ | ❌ | ✅ | ❌ | 

`spSanction_Update` is the outlier — no MISUSER branch and no IP parameter. It needs restructuring before it can carry the workflow.

### ⚠️ The `SELECT *` history trap

**21 procedures** copy history with `INSERT INTO X_HISTORY SELECT * FROM X`. That works **only** while the two tables have identical column count *and ordinal order*:

```
spABBFFStructure_Update   spComplaint_CheckerAction  spComplaint_Update     spComplaintUser_Update
spIACStructure_Update     spIACUser_Update           spMISCExcel_Import     spMiscStructure_Update
spOperationalRef_Update   spRRB_Operation            spRRB_Update           spRRBExcel_Import
spRTI_Update              spRTIExcel_Import          spSanction_Update      spSRExcel_Import
spSRStructure_Update      spVigilance_Update         spVigilanceUser_Update spWBExcel_Import
spWBStructure_Update
```

The Complaint implementation handled this correctly — the 6 workflow columns were appended at positions **61–66 in both `COMPLAINT` and `COMPLAINT_HISTORY`, in identical order**. Replicate that discipline exactly.

Six modules use explicit column lists instead and are safe either way: **LODI, NOC, Penalty Charge, Sanction for Investigation, Sanction for Prosecution, Zone Chief Manager**.

> Correction to an earlier note in `VMIS_Database_Inventory.md`: the column-count differences between `NOC`/`NOC_HISTORY` (37 vs 41) and the other three pairs are **by design**, not a defect — those procs use explicit column lists and the history tables carry extra trail columns (`NOC_TRAIL_USER`, `NOC_TRAIL_DATE`, `NOC_TRAIL_PAGE`, `NOC_TRAIL_OPERATION`). Only the `SELECT *` procs above are exposed to the ordinal trap.

---

## 5. Table strategy

### 5.1 Two options

**Option 1 — replicate the Complaint pattern per module**
6 columns on each case table + 6 on each history table + a `sp<Module>_CheckerAction` + an approval-history table each.

**Option 2 — central approval tables (recommended)**
One `CASE_APPROVAL` row per case record, keyed by module + record id. No schema change to the case tables at all.

| | Option 1 (per-table) | Option 2 (central) |
|---|---|---|
| DDL touched | 28 tables (14 case + 14 history) | 2 new tables |
| `SELECT *` ordinal risk | **High** — 11 of 14 modules affected | **None** |
| Touching `VIGILANCE` (171 cols) / `RRB` (159 cols) | Yes | No |
| Checker action procs | 14 | 1 generic |
| Checker inbox pages | 14 | 1, with a module filter |
| Cross-module "my pending queue" | Needs 14-way `UNION` | Single query |
| Reporting on approval SLAs | Per-module | Uniform |
| List-screen filtering | Column already on the row | One `JOIN` |
| Consistency with existing Complaint code | Matches | Complaint needs migrating (or bridging) |

**Recommendation: Option 2.** The decisive factors are that it avoids touching two 160+ column tables and their `SELECT *` history procs entirely, and it collapses 14 checker inboxes into one. The only real cost is a join on list screens, which these procs already do freely.

### 5.2 Recommended schema

```sql
-- Registry of which modules participate, and how to locate/scope a record
CREATE TABLE dbo.WORKFLOW_MODULE
(
    ModuleCode      varchar(20)  NOT NULL PRIMARY KEY,   -- 'COMPLAINT','IAC','VIGILANCE',...
    ModuleName      varchar(100) NOT NULL,               -- shown in the checker inbox
    TableName       sysname      NOT NULL,               -- 'COMPLAINT'
    KeyColumn       sysname      NOT NULL,               -- 'CODE'      (surrogate)
    RefColumn       sysname      NOT NULL,               -- 'RNO'       (what the user sees)
    ZoneColumn      sysname      NOT NULL,               -- 'NEWZONE' / 'NEW_ZONE' / 'PC_ZONE'
    ViewPage        varchar(200) NOT NULL,               -- '~/Mis/frmComplaintCheckerView.aspx'
    IsActive        bit          NOT NULL DEFAULT(1)
);

-- One row per case record under workflow
CREATE TABLE dbo.CASE_APPROVAL
(
    ApprovalId      bigint IDENTITY(1,1) NOT NULL PRIMARY KEY,
    ModuleCode      varchar(20)  NOT NULL,
    RecordCode      bigint       NOT NULL,      -- case table surrogate key
    RecordRef       varchar(50)  NOT NULL,      -- human-readable (RNO/SNO/RTINO...)
    ZoneSolID       varchar(10)  NULL,          -- snapshot at submit; drives checker routing
    ApprovalStatus  char(1)      NOT NULL DEFAULT('P'),   -- P / A / C / X
    MakerUser       varchar(50)  NOT NULL,
    MakerDate       datetime     NOT NULL DEFAULT(GETDATE()),
    CheckerUser     varchar(50)  NULL,
    CheckerDate     datetime     NULL,
    CheckerRemarks  varchar(max) NULL,
    CONSTRAINT UQ_CASE_APPROVAL UNIQUE (ModuleCode, RecordCode),
    CONSTRAINT CK_CASE_APPROVAL_STATUS CHECK (ApprovalStatus IN ('P','A','C','X'))
);

CREATE INDEX IX_CASE_APPROVAL_Queue
    ON dbo.CASE_APPROVAL (ApprovalStatus, ZoneSolID, ModuleCode)
    INCLUDE (RecordCode, RecordRef, MakerUser, MakerDate);

-- Append-only audit, replaces/absorbs COMPLAINT_APPROVAL_HISTORY
CREATE TABLE dbo.CASE_APPROVAL_HISTORY
(
    Id          bigint IDENTITY(1,1) NOT NULL PRIMARY KEY,
    ModuleCode  varchar(20)  NOT NULL,
    RecordCode  bigint       NOT NULL,
    ActionType  varchar(20)  NOT NULL,   -- SUBMITTED|APPROVED|REJECTED|PUSHED_BACK|RESUBMITTED
    ActionBy    varchar(50)  NOT NULL,
    ActionDate  datetime     NOT NULL DEFAULT(GETDATE()),
    Remarks     varchar(max) NULL,
    UserRole    varchar(50)  NULL,
    UserIP      varchar(30)  NULL
);

CREATE INDEX IX_CASE_APPROVAL_HISTORY_Record
    ON dbo.CASE_APPROVAL_HISTORY (ModuleCode, RecordCode, Id);
```

### 5.3 Fixes to `MakerCheckerMapping` (needed either way)

Three defects in the existing table:

```sql
-- 1. Join-type mismatch: ZoneSolID is varchar(6) but COMPLAINT.NEWZONE is varchar(10)
--    (and ABBFF.NEW_ZONE is varchar(50)). Any zone code longer than 6 chars can never match.
ALTER TABLE dbo.MakerCheckerMapping ALTER COLUMN ZoneSolID varchar(10) NOT NULL;

-- 2. No unique constraint, yet UserCreation.aspx.cs does SELECT COUNT(*) then UPDATE-or-INSERT.
--    That is a race; two concurrent saves produce duplicate mappings and duplicate inbox rows.
ALTER TABLE dbo.MakerCheckerMapping
    ADD CONSTRAINT UQ_MakerCheckerMapping UNIQUE (UserPF, ZoneSolID);

-- 3. Only a PK on Id — every checker inbox query scans the table.
CREATE INDEX IX_MakerCheckerMapping_Lookup
    ON dbo.MakerCheckerMapping (UserPF, IsChecker, IsActive) INCLUDE (ZoneSolID);
```

Replace the read-then-write in `SaveMakerCheckerMapping` with a single `MERGE` (or rely on the new unique constraint + `IF EXISTS` inside a transaction).

### 5.4 If you choose Option 1 anyway

Keep the Complaint template exactly:

```sql
-- Append to the case table AND its history table in the SAME ORDER, at the END.
ALTER TABLE dbo.<TABLE> ADD
    APPROVALSTATUS char(1)      NOT NULL CONSTRAINT DF_<TABLE>_APPR DEFAULT('P'),
    MAKERUSER      varchar(50)  NULL,
    MAKERDATE      datetime     NULL,
    CHECKERUSER    varchar(50)  NULL,
    CHECKERDATE    datetime     NULL,
    CHECKERREMARKS varchar(max) NULL;

ALTER TABLE dbo.<TABLE>_HISTORY ADD
    APPROVALSTATUS char(1)      NULL,   -- history: nullable, no default
    MAKERUSER      varchar(50)  NULL,
    MAKERDATE      datetime     NULL,
    CHECKERUSER    varchar(50)  NULL,
    CHECKERDATE    datetime     NULL,
    CHECKERREMARKS varchar(max) NULL;
```

Then verify ordinal alignment before deploying — this check must return zero rows:

```sql
SELECT c.column_id, c.name AS CaseCol, h.name AS HistCol
FROM sys.columns c
FULL JOIN sys.columns h
       ON h.object_id = OBJECT_ID('<TABLE>_HISTORY') AND h.column_id = c.column_id
WHERE c.object_id = OBJECT_ID('<TABLE>')
  AND (c.name <> h.name OR c.name IS NULL OR h.name IS NULL);
```

Tier C additionally needs:

```sql
ALTER TABLE dbo.SR  ADD NEWZONE varchar(10) NULL, NEWCIRCLE varchar(10) NULL;  -- + SR_HISTORY
-- then back-fill from BRANCH_MASTER_NEW by matching the free-text ZONE, and
-- add the cascading Zone/Circle dropdowns to the entry forms.
```

---

## 6. Procedure and UI changes per module

### 6.1 Write proc — three edits (pattern from `spComplaint_Update`)

```sql
-- (a) INSERT branch (@p_MODE='I'): register the record as pending
INSERT INTO CASE_APPROVAL (ModuleCode, RecordCode, RecordRef, ZoneSolID, ApprovalStatus, MakerUser)
VALUES ('<MODULE>', SCOPE_IDENTITY(), @p_<REF>, @p_ZONENEW, 'P', @p_USER);

INSERT INTO CASE_APPROVAL_HISTORY (ModuleCode, RecordCode, ActionType, ActionBy, UserRole, UserIP)
VALUES ('<MODULE>', SCOPE_IDENTITY(), 'SUBMITTED', @p_USER, @p_USERROLE, @p_USERIP);

-- (b) UPDATE branch (@p_MODE='U'): re-queue an already-decided record
DECLARE @OLD char(1);
SELECT @OLD = ApprovalStatus FROM CASE_APPROVAL WHERE ModuleCode='<MODULE>' AND RecordCode=@p_CODE;

IF (@OLD IN ('C','A'))
BEGIN
    UPDATE CASE_APPROVAL
       SET ApprovalStatus='P', CheckerUser=NULL, CheckerDate=NULL
     WHERE ModuleCode='<MODULE>' AND RecordCode=@p_CODE;

    INSERT INTO CASE_APPROVAL_HISTORY (ModuleCode, RecordCode, ActionType, ActionBy, UserRole, UserIP)
    VALUES ('<MODULE>', @p_CODE, 'RESUBMITTED', @p_USER, @p_USERROLE, @p_USERIP);
END

-- (c) Block edits on rejected records at the data layer, not just in the grid
IF (@OLD = 'X')
BEGIN
    SET @o_ERRCODE = 0;
    SET @o_EERMSG  = 'This record has been rejected and cannot be edited.';
    RETURN;
END
```

> Edit (c) is **not** in the current Complaint implementation — rejection is enforced only by disabling a button in `gvMain_RowDataBound`. A direct postback or a second entry page still gets through. Add it.

### 6.2 One generic checker action proc replaces 14

```sql
CREATE OR ALTER PROC dbo.spCase_CheckerAction
(
    @p_MODULE   varchar(20),
    @p_CODE     bigint,
    @p_ACTION   char(1),          -- 'A' approve | 'X' reject | 'C' push back
    @p_REMARKS  varchar(max),
    @p_USER     varchar(50),
    @p_USERROLE varchar(50),
    @p_USERIP   varchar(30),
    @o_EERMSG   varchar(max) OUTPUT,
    @o_ERRCODE  int          OUTPUT
)
```

Same guard sequence the Complaint proc already uses — validate action code, require remarks, confirm the record exists, confirm the caller is an active checker for that record's zone, confirm status is still `'P'` (prevents double-action), then update + log. Two additions worth making:

- **Wrap in a transaction.** `spComplaint_CheckerAction` currently does three writes (history snapshot, update, audit insert) with no `BEGIN TRAN`. A failure between them leaves the audit trail inconsistent.
- **Reject self-approval:** `IF @p_USER = <MakerUser> RETURN 'Maker and checker cannot be the same user.'` The current proc does not check this. A user holding both `VMIS_MISUSER` and `VMIS_CHECKER` — which `AssignRole()` explicitly permits, since `VMIS_CHECKER` is a *secondary* role — can today approve their own complaint. **This defeats the entire purpose of maker–checker and should be treated as the highest-priority fix in this whole plan.**

### 6.3 UI work

| Item | Option 1 | Option 2 |
|---|---|---|
| Checker inbox | 14 new pages | 1 page + module dropdown (generalise `frmComplaintChecker.aspx`) |
| Checker detail/action view | 14 new pages | 1 per module *(field layouts differ)* — or reuse each module's existing entry form in read-only mode with an action bar |
| Maker grid status column | Per module | Per module (join `CASE_APPROVAL`) |
| `Web.sitemap` | Add `VMIS_CHECKER` nodes | Add one node |
| `Default.aspx` pending modal | Extend count query | Single count across all modules |

**Recommended UI shortcut:** rather than building 14 checker *detail* pages, add a read-only mode plus an action bar to each module's existing entry form. `funcControlsUserRights()` already implements exactly this shape for `VMIS_VIEWUSER` — add a `VMIS_CHECKER` branch that calls `DisableAllControls(this.Page)` and shows Accept/Reject/Push-Back plus a remarks box. That reuses the field layout, validation and lookups for free.

---

## 7. Prerequisites

These block the work regardless of design choice.

| # | Blocker | Impact |
|---|---|---|
| 1 | **Reference masters are empty** (`STATUS`, `SCALE`, `NATURECASE`, `SOURCEREF`, `PENALTYTYPE`, `REGISTER`, `BRANCH_MASTER`, …) | Cannot open, save or test any entry form. Need a UAT masters extract. |
| 2 | **`CBS_BRANCH_MASTER` table missing** | Breaks the `_Ddl` dropdown procs for NOC, RRB, RTI, SR, WB, Vigilance Monitoring — six of the target modules will not load. |
| 3 | **`spVigilanceMIS_Update` missing** | Vigilance Monitoring has no write path at all today. |
| 4 | **37 procs missing from this DB** (§2.2 of the inventory) | Includes the whole `MasterData.cs` layer. |
| 5 | **215 proc definitions not in source control** | You cannot safely diff or roll back proc changes. Script them out first. |
| 6 | **`spABBFFStructure_Update` writes to `MISC`/`MISC_HISTORY`; no `ABBFF_HISTORY` exists** | ABBFF has no working history mechanism to hook into. |
| 7 | **Checker-only users have no menu** (`VMIS_CHECKER` absent from the sitemap ancestor nodes) and the Default.aspx modal links to non-existent `ComplaintApproval.aspx` | Every new checker inbox will be unreachable the same way. Fix once, centrally. |

---

## 8. Suggested phasing

**Phase 0 — harden what exists (do this first, ~1 sprint)**
Self-approval block · transaction wrapper · server-side reject lock · the three `*User_Update` bypasses · `MakerCheckerMapping` type/unique/index fixes · sitemap `VMIS_CHECKER` nodes · fix the modal link · orphan-pending monitor · script all procs into `Database/Scripts/`.

**Phase 1 — foundation (~1 sprint)**
`WORKFLOW_MODULE`, `CASE_APPROVAL`, `CASE_APPROVAL_HISTORY` · generic `spCase_CheckerAction` · generic checker inbox · migrate Complaint onto it (backfill from `COMPLAINT.APPROVALSTATUS` + `COMPLAINT_APPROVAL_HISTORY`, keep the existing columns in place initially as a read-only fallback).

**Phase 2 — Tier A rollout**
IAC → MISC → RTI → NOC → Vigilance → RRB *(both procs)* → Vigilance Monitoring. Start with IAC: smallest table, explicit dependencies, closest in shape to Complaint. Vigilance and RRB last — 171 and 159 columns, 144 and 156 proc parameters.

**Phase 3 — Tier B**
Penalty Charge and LODI (both already use safe explicit-column history). ABBFF only after its history defect is fixed.

**Phase 4 — Tier C**
SR, WB, Operational Ref — add and back-fill `NEWZONE`/`NEWCIRCLE`, add cascading dropdowns to six pages, then apply the standard pattern.

**Phase 5 — Tier D**
Sanction ×3, pending the routing decision in §9.

---

## 9. Questions for the stakeholder

1. **Which modules are actually in scope?** "Other forms" could mean all 14 or just the high-volume ones (IAC, Vigilance, MISC). Effort ranges from ~2 sprints to ~8.
2. **How should Sanction cases route?** They have no zone. Fixed HO checker group, circle-based, or excluded?
3. **Should a maker be allowed to be a checker at all?** Currently `VMIS_CHECKER` is additive, and one live user (`5224503`) holds `VMIS_CHECKER` + `VMIS_DESKUSER`. If dual-holding is intended, the self-approval block in §6.2 is mandatory. If not, make `VMIS_CHECKER` a primary (exclusive) role.
4. **Does a rejected record stay permanently locked?** Today `'X'` means read-only forever, with no reopen path. Is that intended, or should a supervisor be able to reopen?
5. **Do bulk Excel imports need approval too?** If yes, imported rows must land as `'P'` with a populated zone and a defined maker — which will make imports far slower to clear. If no, they need an explicit `'A'` with an audit note recording the exemption.
6. **Do the desk-user annotations need checking?** `VMIS_DESKUSER` appends HO Status and remarks without any approval. Is that deliberate?
7. **Multi-level approval?** Current model is single checker. Is a second level (e.g. zonal → HO) anticipated? Cheaper to design for now than to retrofit.
8. **Do updates need checking, or only creation?** Complaint currently re-queues on every edit of an approved record. Confirm that is wanted everywhere — it will roughly double checker workload on active cases.
