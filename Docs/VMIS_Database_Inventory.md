# VMISP — Database Inventory (live server)

**Server:** `localhost\SQLEXPRESS` (machine `Dell`), Windows Authentication
**Surveyed:** 2026-07-25, directly against the running instance
**Companion to:** `VMIS_Technical_Overview.md`

---

## 1. Instance overview

| Database | Purpose | Tables | Procs | Views | Functions | State |
|---|---|---:|---:|---:|---:|---|
| **VigilanceMISDB** | All business/case data | **68** | **217** | 0 | 3 | Schema complete, **data essentially empty** |
| **UserManagementDB** | ASP.NET Membership / Roles / Profile | 14 | 42 | 32 | 1 | **Fully populated (real data)** |
| **Common** | Shared branch/circle/zone lookups | 3 | 0 | 0 | 0 | Populated (small) |
| `EofficeAlpha_P3` | Unrelated application | — | — | — | — | Not part of VMISP |

Also present: `master`, `tempdb`, `model`, `msdb` (system).

### Structural characteristics of `VigilanceMISDB`

| Feature | Count | Comment |
|---|---:|---|
| User tables | 68 | 61 real + 7 Excel-import staging (`*$`) |
| Stored procedures | 217 | |
| Scalar functions | 3 | |
| **Views** | **0** | All read logic lives in procs |
| **Triggers** | **0** | History is written explicitly by procs, not by triggers |
| **Foreign keys** | **0** | No referential integrity enforced anywhere |
| Non-clustered indexes | 84 | |
| Primary keys | 30 | **38 of 68 tables have no PK** — including every `*_HISTORY` table |

This is a *procedure-centric* design: no views, no triggers, no FKs. Every rule — history snapshots, soft deletes, status transitions, maker–checker authorisation — is enforced inside stored procedure bodies. If you change a proc, nothing else in the database will catch a mistake for you.

---

## 2. ⚠️ Read this before planning any work

Four findings materially affect what you can build and test right now.

### 2.1 The business database has no data

Almost every table is empty:

| Table | Rows |
|---|---:|
| `COMPLAINT` | **3** |
| `COMPLAINT_HISTORY` | 5 |
| `COMPLAINT_APPROVAL_HISTORY` | 7 |
| `MakerCheckerMapping` | **1** |
| `USER_TRACE` | 36 |
| `USER_CONCURRENT_LOGIN` | 21 |
| `BRANCH_MASTER_NEW` | 28 |
| **Everything else** | **0** |

That includes every case table (`VIGILANCE`, `IAC`, `SR`, `WB`, `RRB`, `RTI`, `MISC`, `NOC`, `LODI`, `ABBFF`, `SANCTION*`, `OPERATIONALREF`, `PENALTY_CHARGE`, `VIGILANCEMIS`) **and every reference master** (`STATUS`, `SCALE`, `NATURECASE`, `SOURCEREF`, `PENALTYTYPE`, `PENALTYPROCEEDING`, `REGISTER`, `CVOADVICE`, `EMAIL_MASTER`, `DIVISIONMASTER`, `BRANCH_MASTER`, `STATE_MASTER`, `Fgm_Master`, `ZONE_CHIEF_MANAGER`, `EMPLOYEE_MASTER`).

**Consequence:** every dropdown on every entry form will render empty, and status/scale/nature lookups will fail. The only workflow that can be exercised end-to-end today is the Complaint maker–checker path, because someone seeded exactly enough rows for it. Ask for a masters data extract from UAT/production before starting feature work.

### 2.2 37 stored procedures called by the application do not exist

These pages will throw `Could not find stored procedure` the moment the relevant button is pressed:

| Page | Missing procedure(s) |
|---|---|
| `Master\BranchMaster.aspx.cs` | `spBranchMaster_Ddl`, `spBranchMaster_Operation`, `spBranchMaster_View` |
| `Master\CircleMaster.aspx.cs` | `spCircleMaster_Operation`, `spCircleMaster_View` |
| `Search\frmEOSearch.aspx.cs` | `spEOSearch_View` |
| `Mis\VigilanceMonitoring.aspx.cs` | `spVigilanceMIS_Update` |
| `Mis\SanctionDataUpload.aspx.cs` | `spSanctionDataVerify`, `spSanctionForInvestigation_Upload`, `spSanctionForProsecution_Upload` |
| `Reports\VigilanceCaseStatus.aspx.cs` | `spVigilanceCaseStatus_Report` |
| `Reports\VigilanceMonitoringReports.aspx.cs` | `spVigilanceMonitoring_Report` |
| `Upload\frmExcelUpload.aspx.cs` | `spComplaintExcel_Import`, `spNOCExcel_Import`, `spSFIExcel_Import`, `spSFPCExcel_Import`, `spVIGMExcel_Import` |
| `Upload\frmExcelPFEODetails.aspx.cs` | `spExcelImportDetailsEO_Get` |
| `DataAccessLayer\MasterData.cs` | `spBranchCircle_Ddl`, `spBranchCircle_Get`, `spBranchMaster_Ddl`, `spCircleBranchMaster_Ddl`, `spCircleZoneMaster_Ddl`, `spDelete`, `spFogetPasswordUIDValidate`, `spForgetPasswordMailStatus_Update`, `spMasterEmailID_Get`, `spPasswordUserID_Get`, `spRoleDescription_Ddl`, `spScaleMaster_Ddl`, `spStatusMaster_Ddl`, `spTableFormat_Get`, `spUplodedFile_Get`, `spUserEmaild_Get`, `spUserRole_Ddl`, `spUserType_Ddl`, `spZoneCircleMaster_Ddl`, `spZoneCode_Get` |

Note that `MasterData.cs` alone accounts for 20 of them — the entire shared master-data layer is unusable against this database. **Excel upload for COMPLAINT is broken** even though upload for IAC, MISC, LODI, RRB, RTI, SR, VIGILANCE and WB works.

### 2.3 A referenced table is missing: `CBS_BRANCH_MASTER`

Eight procedures reference `dbo.CBS_BRANCH_MASTER`, which does not exist in this database:

```
spDisciplinaryAuthority_Ddl   spMaster_Ddl        spNOC_Ddl      spRRB_Ddl
spRTI_Ddl                     spSR_Ddl            spVigilanceMIS_Ddl   spWB_Ddl
```

SQL Server allows a proc to be created against a missing table (deferred name resolution) — it only fails at execution. So the **dropdown-population step of the NOC, RRB, RTI, SR, WB and Vigilance-Monitoring entry forms will fail at runtime**, as will the shared Disciplinary Authority dropdown used across several forms (`CommonFunction.funcDisciplinaryAuthority`).

This is almost certainly a CBS integration table that exists in UAT/production but was not included in the schema you were given.

### 2.4 `vmismainscript.sql` is not a faithful copy of this database

The checked-in script contains 61 tables and **2 procedures**. The live database has 68 tables and **217 procedures**. The 215 missing procedure definitions exist only on the server. Script them out and commit them — that is the single highest-value housekeeping task in this repo.

```powershell
# quick scripting of every proc/function to one file
sqlcmd -S "localhost\SQLEXPRESS" -E -C -h -1 -y 0 -Q "SET NOCOUNT ON; USE VigilanceMISDB; SELECT definition + CHAR(13)+CHAR(10)+'GO'+CHAR(13)+CHAR(10) FROM sys.sql_modules" -o Database\Scripts\_all_programmability.sql
```

---

## 3. `VigilanceMISDB` — tables

### 3.1 Case entities (the core)

| Table | Cols | Rows | History table | Child table |
|---|---:|---:|---|---|
| `COMPLAINT` | 66 | 3 | `COMPLAINT_HISTORY` (66) | `COMPLAINT_EO_DETAILS` (17) |
| `VIGILANCE` | **171** | 0 | `VIGILANCE_HISTORY` (171) | — |
| `RRB` | **159** | 0 | `RRB_HISTORY` (159) | — |
| `MISC` | 66 | 0 | `MISC_HISTORY` (66) | `MISC_EO_DETAILS` (17) |
| `IAC` | 58 | 0 | `IAC_HISTORY` (58) | — |
| `RTI` | 57 | 0 | `RTI_HISTORY` (57) | — |
| `WB` | 56 | 0 | `WB_HISTORY` (56) | — |
| `OPERATIONALREF` | 55 | 0 | `OPERATIONALREF_HISTORY` (55) | — |
| `VIGILANCEMIS` | 55 | 0 | `VIGILANCEMIS_HISTORY` (50) | — |
| `SR` | 48 | 0 | `SR_HISTORY` (48) | — |
| `ABBFF` | 48 | 0 | — | `ABBFF_EO_DETAILS` (20) |
| `SANCTION_FOR_PROSECUTION` | 45 | 0 | `..._HISTORY` (46) | — |
| `SANCTION_FOR_INVESTIGATION` | 41 | 0 | `..._HISTORY` (40) | — |
| `NOC` | 37 | 0 | `NOC_HISTORY` (41) | — |
| `LODI` | 32 | 0 | `LODI_HISTORY` (33) | — |
| `PENALTY_CHARGE` | 27 | 0 | `PENALTY_CHARGE_HISTORY` (29) | — |
| `SANCTION` | 16 | 0 | `SANCTION_HISTORY` (16) | — |
| `COMPLAINT_060622` | 60 | 0 | *(dated backup copy — dead)* | |

> Column counts drift between a table and its history twin (`VIGILANCEMIS` 55 vs 50, `NOC` 37 vs 41, `PENALTY_CHARGE` 27 vs 29, `SANCTION_FOR_INVESTIGATION` 41 vs 40). The update procs use `INSERT INTO X_HISTORY SELECT * FROM X`, which **breaks the moment the column counts or order diverge**. If you add a column to any case table, you must add it to the history table in the same position. This is a live trap.

### 3.2 Maker–checker (Complaint workflow)

| Table | Cols | Rows | Purpose |
|---|---:|---:|---|
| `MakerCheckerMapping` | 9 | 1 | Which PF number checks which zone |
| `COMPLAINT_APPROVAL_HISTORY` | 8 | 7 | Append-only log of SUBMITTED / APPROVED / REJECTED / PUSHED_BACK / RESUBMITTED |

**Current live state:**

```
MakerCheckerMapping:  Id=2  UserPF=5224503  ZoneSolID=100002  IsMaker=0 IsChecker=1 IsActive=1  CreatedBy=5224579

COMPLAINT:
  CODE=1  RNO=123  NEWZONE=100002  APPROVALSTATUS=A  maker=5224563  checker=5224503
  CODE=2  RNO=456  NEWZONE=100002  APPROVALSTATUS=X  maker=5224563  checker=5224503
  CODE=3  RNO=789  NEWZONE=100002  APPROVALSTATUS=P  maker=5224563  checker=5224503

COMPLAINT_APPROVAL_HISTORY:
  1 | code 1 | SUBMITTED   | 5224563 | 2026-07-24 23:24 | VMIS_MISUSER
  2 | code 1 | APPROVED    | 5224503 | 2026-07-25 00:12 | VMIS_CHECKER
  3 | code 2 | SUBMITTED   | 5224563 | 2026-07-25 00:16 | VMIS_MISUSER
  4 | code 2 | REJECTED    | 5224503 | 2026-07-25 00:18 | VMIS_CHECKER
  6 | code 3 | SUBMITTED   | 5224563 | 2026-07-25 15:27 | VMIS_MISUSER
  7 | code 3 | PUSHED_BACK | 5224503 | 2026-07-25 15:29 | VMIS_CHECKER
  8 | code 3 | RESUBMITTED | 5224563 | 2026-07-25 15:30 | VMIS_MISUSER
```

All three checker outcomes plus the push-back → resubmit loop have been exercised. Only **one** checker is mapped, to **one** zone (`100002`), so multi-zone and multi-checker behaviour is untested.

### 3.3 Reference masters (all empty)

`BRANCH_MASTER` (15 cols) · `BRANCH_MASTER_NEW` (14 cols, **28 rows** — the only populated master) · `DIVISIONMASTER` (12) · `STATE_MASTER` (3) · `EMPLOYEE_MASTER` (16) · `Fgm_Master` (12) · `STATUS` (8) · `SCALE` (8) · `NATURECASE` (8) · `SOURCEREF` (8) · `PENALTYTYPE` (9) · `PENALTYPROCEEDING` (10) · `REGISTER` (10) · `CVOADVICE` (9) · `EMAIL_MASTER` (12) · `ZONE_CHIEF_MANAGER` (12) + `_HISTORY` (16) · `tbl_UserCreationDD` (3)

### 3.4 Security, audit and logging

| Table | Cols | Rows | Written by |
|---|---:|---:|---|
| `USER_TRACE` | 13 | 36 | `spUserTrace_Update`, `spUserTrace_Updatenew`, `spUserLoginTrace_Operation`, `spLogout` |
| `USER_CONCURRENT_LOGIN` | 4 | 21 | `spUserConcurrent_InsertSession` / `_Deactivate` / `_CheckSession` |
| `VMISP_LOG` | 13 | 0 | `spErrorLog_Update` ← `VMISP_Error_Log.HandleException` |
| `VMIS_ERROR_LOG` | 12 | 0 | `spLockedError` |

> Note the crossover: the C# helper named `VMISP_Error_Log` writes to the table named **`VMISP_LOG`**, while the table named `VMIS_ERROR_LOG` is written by `spLockedError` (login lockout events). Easy to confuse.

### 3.5 Excel-import staging tables

Seven tables whose names end in `$` are artefacts of the SQL Server Import Wizard / OLEDB Excel provider (an Excel sheet named `IAC` imports as `IAC$`):

`C$` (3 cols) · `IAC$` (4) · `M$` (3) · `SOI$` (4) · `SOP$` (4) · `VIG$` (4) · `VigilanceMonitoring$` (4)

All empty, none referenced by any procedure or by the application. They are leftovers from a manual data load and can be dropped.

---

## 4. `VigilanceMISDB` — scalar functions (3)

| Function | Signature | What it does |
|---|---|---|
| `ReverseColumnValue_Function` | `(@p_COLUMNNAME varchar(250), @p_WHERECONDITION varchar(50), @p_TABLENAME varchar(50))` → `varchar(250)` | Status/reason columns store an append-only pipe-delimited history (`newest \| older \| oldest`). This returns **just the newest segment** — the text after the last `\|`. Falls back to the whole string if there is no pipe. Dispatches on `@p_TABLENAME` over 12 hard-coded modules: `COMPLAINT, IAC, MISC, NOC, OR, RRB, SR, VIG, VIGMIS, WB, RTI, SANCTION`. Used by every `_View` proc to produce `SHORTSTATUS` / `SHORTREASONSFORCLOSURE`. |
| `SolName_Function` | `(@p_SOLID varchar(10))` → `varchar(250)` | `SOLID + ' - ' + Branch_name` from `BRANCH_MASTER`. Used by `spIACOutstanding_Report`, `spMISCOutstanding_Report`, `spVigilanceOutstanding_Report`. |
| `Status_Function` | `(@p_CODE varchar(10), @p_FORM varchar(20))` → `varchar(250)` | `STS_CODE + ' - ' + STS_STATUS` from `STATUS` where `STS_TABLE = @p_FORM`. Used by the IAC/MISC outstanding reports. |

> **Design note that matters for any new requirement:** the pipe-delimited status history in `COMPLAINT.STATUS` is how the desk-user annotation works — `spComplaint_Update` does `@p_HOSTATUS + ' | ' + old STATUS`. If a stakeholder asks for status history or reporting on status transitions, the data is in a delimited string, not in rows. `ReverseColumnValue_Function` is the only thing that parses it, and it only returns the latest entry.

---

## 5. `VigilanceMISDB` — stored procedures (217)

### 5.1 By category

| Category | Count | Naming |
|---|---:|---|
| SSRS report datasets | 45 | `sp*_Report` |
| Case CRUD — read | 17 | `sp*_View` |
| Case CRUD — write | 20 | `sp*_Update`, `sp*_Operation`, `spLodi`, `spPenaltyCharge`, `spSanctionFor*` |
| Case CRUD — delete | 9 | `sp*_Delete` |
| Dropdown / lookup | 22 | `sp*_Ddl` |
| Excel / Access import | 9 | `sp*Excel_Import`, `spACCESSSR_Import` |
| Master maintenance | 20 | `sp<Master>_View` / `_Update` |
| Child (EO details) | 9 | `sp*EO_Add` / `_View` / `_Delete` |
| Dashboard | 16 | `spDashboard*`, `spDashbaord_Ddl`, `spVIGDashboard*` |
| Search | 7 | `spTableWiseSearch_View`, `spHistoryTableWiseSearch_View`, `spFieldWiseSearch_View`, `spCustomize_Report`, `spTableColumn_Get`, `spHistoryTableColumn_Get`, `spUploadTableColumn_Get` |
| Auth / session / audit | 8 | `spUser*`, `spValidateSingleUserLogin`, `spLogout`, `spErrorLog_Update`, `spLockedError` |
| Internal history helpers | 6 | `sp*_History` (called *by other procs*, never by C#) |
| Maker–checker | 1 | `spComplaint_CheckerAction` |

### 5.2 Widest procedures (parameter counts)

Useful to know before you touch them:

| Procedure | Params |
|---|---:|
| `spRRB_Update` | **156** |
| `spVigilance_Update` | **144** |
| `spVigilanceExcel_Import` | 88 |
| `spRRB_Operation` | 57 |
| `spMiscStructure_Update` | 50 |
| `spRTI_Update` | 50 |
| `spIACStructure_Update` | 49 |
| `spComplaint_Update` | 47 |
| `spMISCExcel_Import` | 47 |
| `spRRBExcel_Import` | 44 |

### 5.3 Recently modified (the maker–checker work)

Only three procedures carry a `modify_date` of **2026-07-25**; everything else is 2026-07-24 (the day the DB was restored):

```
spComplaint_CheckerAction   (8 params)   — new
spComplaint_Update         (47 params)   — changed
spComplaint_View           (12 params)   — changed
```

These match the three scripts in `Database\Scripts\` exactly. Both scripts and server are in sync.

---

## 6. Page → stored procedure map

Generated by scanning every non-designer `.cs` file for procedure references and intersecting with the live catalogue.

### Authentication & shell

| File | Procedures |
|---|---|
| `Login.aspx.cs` | `spValidateSingleUserLogin`, `spUserConcurrent_Deactivate`, `spUserConcurrent_InsertSession`, `spUserLoginTrace_Operation`, `spUserTrace_Update`, `spUserTrace_Updatenew` |
| `LoginSSO.aspx.cs` | `spUserTrace_Update` |
| `SiteMaster.Master.cs` | `spUserConcurrent_CheckSession` |
| `Code\VMISP_Error_Log.cs` | `spErrorLog_Update` |
| `Code\CommonFunction.cs` | `spBranchName_Get`, `spDisciplinaryAuthority_Ddl`, `spEmailMaster_Get`, `spZoneCircle_Ddl` |
| `DataAccessLayer\MasterData.cs` | `spCircleMaster_Ddl`, `spEmailMaster_Get`, `spLockedError`, `spLogout`, `spZoneMaster_Ddl`, `spZoneTypeCM_Ddl` *(+ 20 missing — see §2.2)* |

### Dashboard

| File | Procedures |
|---|---|
| `Default.aspx.cs` | `spDashbaord_Ddl`, `spDashboard_Outstanding`, `spDashboard_OutstandingData`, `spDashboardCompalint_Outstanding`, `spDashboardCompalintDayWise_Outstanding`, `spDashboardCompalintIACVigilanceNPA_OutstandingData`, `spDashboardComplaintPendingatDesk_Outstanding`, `spDashboardComplaintSourceRef_Outstanding`, `spDashboardIAC_Outstanding`, `spDashboardIACDayWise_Outstanding`, `spDashboardIACPendingatDesk_Outstanding`, `spDashboardNPA_Outstanding`, `spDashboardNPADayWise_Outstanding`, `spDashboardVigilance_Outstanding`, `spDashboardVigNonVig_Outstanding` |

*(plus an inline ad-hoc query `GetPendingComplaintCount` joining `COMPLAINT` to `MakerCheckerMapping`)*

### Case modules (`Mis\`)

| File | Procedures |
|---|---|
| `frmComplaint.aspx.cs` | `spComplaint_View`, `spComplaint_Update`, `spComplaint_Ddl`, `spComplaintEO_Add/_View/_Delete`, `spIACStructure_View` |
| `frmComplaintView.aspx.cs` | `spComplaint_View`, `spComplaintEO_View` |
| `frmComplaintChecker.aspx.cs` | *(inline query — `COMPLAINT` ⋈ `MakerCheckerMapping`)* |
| `frmComplaintCheckerView.aspx.cs` | **`spComplaint_CheckerAction`** *(+ inline SELECT for display)* |
| `frmComplaintUpdate.aspx.cs` | `spComplaintUser_Update`, `spCircleOffice_Ddl` |
| `frmIACStructure.aspx.cs` | `spIACStructure_View`, `spIACStructure_Update`, `spIAC_Ddl` |
| `frmIACUpdate.aspx.cs` | `spIACUser_Update` |
| `frmVigilance.aspx.cs` | `spVigilance_View`, `spVigilance_Update`, `spVigilance_Ddl`, `spVigilance_Delete` |
| `Vigilance.aspx.cs` | `spVigilance_View`, `spVigilance_Update`, `spVigilance_Ddl` |
| `frmVigilanceUpdate.aspx.cs` | `spVigilanceUser_Update`, `spMasterForm_Ddl`, `spCircleOffice_Ddl` |
| `VigilanceMonitoring.aspx.cs` | `spVigilanceMIS_View`, `spVigilanceMIS_Ddl` *(+ missing `spVigilanceMIS_Update`)* |
| `frmSRStructure.aspx.cs` | `spSRStructure_View`, `spSRStructure_Update`, `spSRStructure_Delete`, `spSR_Ddl` |
| `SR.aspx.cs` | `spSRStructure_View`, `spSRStructure_Update`, `spSR_Ddl` |
| `frmWBStructure.aspx.cs` | `spWBStructure_View`, `spWBStructure_Update`, `spWBStructure_Delete`, `spWB_Ddl` |
| `WB.aspx.cs` | `spWBStructure_View`, `spWBStructure_Update`, `spWB_Ddl` |
| `frmRRB.aspx.cs` | `spRRB_View`, `spRRB_Update`, `spRRB_Delete`, `spMaster_Ddl`, `spMasterForm_Ddl`, `spCircleOffice_Ddl` |
| `RRB.aspx.cs` | `spRRB_View`, `spRRB_Operation`, `spRRB_Ddl` |
| `frmRTI.aspx.cs` | `spRTI_View`, `spRTI_Update`, `spRTI_Ddl` |
| `frmMiscStructure.aspx.cs` | `spMiscStructure_View`, `spMiscStructure_Update`, `spMISC_Ddl`, `spMiscEO_Add/_View/_Delete` |
| `frmNoc.aspx.cs` | `spNOC_View`, `spNOC_Update`, `spNOC_Delete`, `spNOC_Ddl` |
| `NOC.aspx.cs` | `spNOC_View`, `spNOC_Update`, `spNOC_Ddl` |
| `Lodi.aspx.cs` | `spLodi`, `spLodi_View`, `spLodi_Ddl` |
| `frmABBFF.aspx.cs` | `spABBFFStructure_View`, `spABBFFStructure_Update`, `spABBFFEO_Add/_View/_Delete`, `spMISC_Ddl` |
| `frmOperationalRef.aspx.cs` | `spOperationalRef_View`, `spOperationalRef_Update`, `spOperationalRef_Ddl` |
| `PenaltyCharge.aspx.cs` | `spPenaltyCharge`, `spPenaltyCharge_View`, `spPenaltyCharge_Ddl` |
| `frmSanction.aspx.cs` | `spSanction_View`, `spSanction_Update` |
| `SanctionForInvestigation.aspx.cs` | `spSanctionForInvestigation`, `_View`, `_Ddl` |
| `SanctionForProsecution.aspx.cs` | `spSanctionForProsecution`, `_View`, `_Ddl` |
| `SanctionDataUpload.aspx.cs` | `spSanctionFileFormat` *(+ 3 missing)* |

### Masters (`Master\`, `Admin\`)

| File | Procedures |
|---|---|
| `Master\frmStatus.aspx.cs` | `spStatus_View`, `spStatus_Update` |
| `Master\frmScale.aspx.cs` | `spScale_View`, `spScale_Update` |
| `Master\frmNatureCase.aspx.cs` | `spNatureCase_View`, `spNatureCase_Update` |
| `Master\frmSourceRef.aspx.cs` | `spSourceRef_View`, `spSourceRef_Update` |
| `Master\frmPenaltyType.aspx.cs` | `spPenaltyType_View`, `spPenaltyType_Update` |
| `Master\frmPenaltyProceding.aspx.cs` | `spPenaltyProceding_View`, `spPenaltyProceding_Update` |
| `Master\frmRegister.aspx.cs` | `spRegister_View`, `spRegister_Update` |
| `Master\frmCVOAdvice.aspx.cs` | `spCVOAdvice_View`, `spCVOAdvice_Update` |
| `Master\EmailMaster.aspx.cs` | `spEmailMaster` |
| `Master\ZoneChiefManager.aspx.cs` | `spZoneChiefManager_View`, `spZoneChiefManager_Operation` |
| `Master\LodiDisable.aspx.cs` | `spLodiDisable_View`, `spLodi_Disable` |
| `Master\CircleMaster.aspx.cs` | `spCircleMaster_Ddl` *(+ 2 missing)* |
| `Master\BranchMaster.aspx.cs` | *(all 3 missing)* |
| `Admin\BranchMaster.aspx.cs` | `spBranchMaster_Update` |
| `Admin\frmCircleHead.aspx.cs` | `spCircleHead_Update` |
| `Admin\UserCreation.aspx.cs` | `spCircleMaster_Ddl` *(+ inline `MakerCheckerMapping` upserts)* |

### Search, upload, reports

| File | Procedures |
|---|---|
| `Search\frmTableWiseSearch.aspx.cs` | `spTableColumn_Get`, `spTableWiseSearch_View` |
| `Search\frmCustomizeReports.aspx.cs` | `spTableColumn_Get`, `spCustomize_Report` |
| `Search\frmAuditTrailSearch.aspx.cs` | `spHistoryTableColumn_Get`, `spHistoryTableWiseSearch_View` |
| `Search\frmFieldWiseSearch.aspx.cs` | `spFieldWiseSearch_View` |
| `Search\frmVigilanceStatusSearch.aspx.cs` | `spVigilanceStatus_View` |
| `Search\RetirementCases.aspx.cs` | `spRetirementCases_Details` |
| `Search\frmEOSearch.aspx.cs` | *(missing `spEOSearch_View`)* |
| `Upload\frmExcelUpload.aspx.cs` | `spExcelVerify_Get`, `spUploadTableColumn_Get`, `spIACExcel_Import`, `spLodiExcel_Import`, `spMISCExcel_Import`, `spRRBExcel_Import`, `spRTIExcel_Import`, `spSRExcel_Import`, `spVigilanceExcel_Import`, `spWBExcel_Import` *(+ 5 missing)* |
| `Upload\frmExcelPF.aspx.cs` | `spExcelImportDetails_Get`, `spUploadTableColumn_Get` |
| `Upload\frmExcelPFEODetails.aspx.cs` | `spUploadTableColumn_Get` *(+ 1 missing)* |
| `Upload\frmAccessUpload.aspx.cs` | `spACCESSSR_Import` |
| `Reports\frmCaseRegister.aspx.cs` | `spCaseRegister_View` |
| `Reports\LodiReport.aspx.cs` | `spLodi_Report` |
| `Reports\MISCReport.aspx.cs` | `spMISC_Report` |
| `Reports\PenaltyChargeReport.aspx.cs` | `spPenaltyCharge_Report` |
| `Reports\SanctionForInvestigationReports.aspx.cs` | `spSanctionForInvestigation_Report` |
| `Reports\SanctionForProsecutionReports.aspx.cs` | `spSanctionForProsecution_Report` |

---

## 7. The 66 procedures not called from C#

They are not dead code — most belong to SSRS.

### 7.1 SSRS datasets (45) — called by the report server, not the app

The naming maps 1:1 onto the `ReportPath` values in `Reports\*.aspx.cs`:

| SSRS report path | Dataset procedure |
|---|---|
| `/VMIS_Reports/ComplaintOutstanding` | `spComplaintOutstanding_Report` |
| `/VMIS_Reports/ComplaintOutstandingAsOnDate` | `spComplaintOutstandingAsOnDate_Report` |
| `/VMIS_Reports/ComplaintOutstandingCVC` | `spComplaintOutstandingCVC_Report` |
| `/VMIS_Reports/ComplaintOutstandingOthers` | `spComplaintOutstandingOthers_Report` |
| `/VMIS_Reports/ComplaintReportToMD` | `spComplaintReportToMD_Report` |
| `/VMIS_Reports/ComplaintStatus` | `spComplaintStatus_Report` |
| `/VMIS_Reports/Complaints`, `/rptComplaints` | `spComplaintNo_Report`, `spComplaintNo_Copy_Report` |
| `/VMIS_Reports/IACOutstanding` \| `IACStatus` \| `IACReportToMD` \| `IACRetirement` | `spIACOutstanding_Report`, `spIACStatus_Report`, `spIACReportToMD_Report`, `spIACRetirement_Report` |
| `/VMIS_Reports/Vigilance*` (12 paths) | `spVigilanceOutstanding_Report`, `spVigilanceStatus_Report`, `spVigilanceFirstStagePending_Report`, `spVigilanceFirstStagePendingAtDesk_Report`, `spVigilanceSecondStagePending_Report`, `spVigilanceSecondStagePendingAtDA_Report`, `spVigilanceChargeSheetNotServed_Report`, `spVigilanceEoPoNotAppointed_Report`, `spVigilanceReconsiderViewAwiatedFromDA_Report`, `spVigilanceEnquiryIsInProgress_Report`, `spVigilanceRetirement_Report`, `spVigilanceFinalOrderAwaited_Report`, `spVigilanceMinorChargeSheet_Report` |
| `/VMIS_Reports/SR*`, `WB*`, `RRB*`, `RTI*`, `MISC*`, `OR*` | `spSROutstanding_Report`, `spSRStatus_Report`, `spSRReportToMD_Report`, `spWBOutstanding_Report`, `spWBStatus_Report`, `spWBReportToMD_Report`, `spRRBOutstanding_Report`, `spRRBStatus_Report`, `spRRBProgress_Report`, `spRTIOutstanding_Report`, `spRTIStatus_Report`, `spMISCOutstanding_Report`, `spMISCStatus_Report`, `spMISCReportToMD_Report`, `spOperationalRefOutstanding_Report`, `spOperationalRefStatus_Report` |
| `/VMIS_Reports/DepartmentalEnquiries` \| `Investigation` \| `NatureProcedings` \| `PenaltyProcedings` \| `DFSReport` \| `DFSDetailsReport` | `spDepartmentalEnqNo_Report`, `spInvestigation_Report`, `spNatureProceedings_Report`, `spPenaltyProcedings_Report`, `spDFSMonthly_Report`, `spDFSMonthlyDetails_Report` |

### 7.2 Internal helpers (6) — called by other procedures

`spLodi_History` ← `spLodi` · `spNOC_History` ← `spNOC_Update` · `spPenaltyCharge_History` ← `spPenaltyCharge` · `spSanctionForInvestigation_History` ← `spSanctionForInvestigation` · `spSanctionForProsecution_History` ← `spSanctionForProsecution` · `spZoneChiefManager_History` ← `spZoneChiefManager_Operation`

These are the "snapshot into the history table" helpers for the modules that factored it out instead of inlining it.

### 7.3 Genuinely unreferenced (10) — candidates for review

| Procedure | Assessment |
|---|---|
| `spComplaint_Delete` | Delete proc never wired to a UI button |
| `spIACStructure_Delete` | Same |
| `spMiscStructure_Delete` | Same |
| `spOperationalRef_Delete` | Same |
| `spRTI_Delete` | Same |
| `spVIGDashboardDAWiseData` | Dashboard drill-down never wired up |
| `spVIGDashboardDaysWiseData` | Same |
| `spVIGDashboardScaleWiseData` | Same |
| `spVMISPCaseDetails_Get` | Possibly an external/API consumer |
| `spTMSACIACDetails_Report`, `spCVCAdvice_Report`, `spProsecutionSanctions_Report` | SSRS reports that exist on the server but aren't linked from any page |

Note the asymmetry: `spVigilance_Delete`, `spRRB_Delete`, `spSRStructure_Delete`, `spWBStructure_Delete`, `spNOC_Delete` **are** wired up, but the equivalent for Complaint, IAC, MISC, OR and RTI is not. Deleting a complaint is not possible through the UI today.

---

## 8. `UserManagementDB` — identity store

Standard ASP.NET SQL Membership schema, **shared with other bank applications** (360 roles, only 6 of which are VMIS).

| Table | Rows | Notes |
|---|---:|---|
| `aspnet_Users` | 12 639 | |
| `aspnet_Membership` | **46 506** | More than `aspnet_Users` — multiple applications |
| `aspnet_Roles` | **360** | Roles for many apps; VMIS uses 6 |
| `aspnet_UsersInRoles` | 23 545 | |
| `aspnet_Profile` | 12 436 | Backs `WebProfile` — `sol`, `solname`, `nameofuser`, `changepwd` |
| `aspnet_Applications` | 2 | |
| `aspnet_Users_54` | 56 635 | Backup/staging copy |
| `aspnet_Profile_bck` | 12 424 | Backup copy |
| `PWD_LOG` | 4 798 | Password change log |
| `USER_TRAIL_PAYFEE` | 46 410 | Belongs to a different application |
| `ECS_UserTempTable`, `ECSUserTempTable`, `UAT$` | 68 / 68 / 8 | Other apps / leftovers |

### VMIS role membership (live)

| Role | Users |
|---|---:|
| `VMIS_MISUSER` | 10 |
| `VMIS_DESKUSER` | 8 |
| `VMIS_ADMIN` | 3 |
| `VMIS_CHECKER` | **2** |
| `VMIS_SUPERUSER` | 1 |
| `VMIS_VIEWUSER` | **0** |

### Actual user → role assignments

```
302010   VMIS_MISUSER          5180079  VMIS_MISUSER
303630   VMIS_MISUSER          5202694  VMIS_SUPERUSER
36156    VMIS_MISUSER          5202714  VMIS_MISUSER
57402    VMIS_MISUSER          5213381  VMIS_ADMIN
76109    VMIS_MISUSER          5224503  VMIS_CHECKER + VMIS_DESKUSER   ← dual role
97698    VMIS_MISUSER          5224563  VMIS_MISUSER
5153748  VMIS_MISUSER          5224579  VMIS_ADMIN
513500   VMIS_ADMIN            5224580  VMIS_CHECKER                    ← checker only
340919   VMIS_DESKUSER         65776    VMIS_DESKUSER
340987   VMIS_DESKUSER         82527    VMIS_DESKUSER
344845   VMIS_DESKUSER         3448451  VMIS_DESKUSER
5167639  VMIS_DESKUSER
```

This confirms two things I flagged in the code review:

- **`5224503` holds two primary roles** (`VMIS_CHECKER` + `VMIS_DESKUSER`). `funcSSOLogin` sets `Session["role"] = roles.FirstOrDefault(r => PrimaryRoles.Contains(r))` — it iterates the *provider's* role array, which comes back alphabetically, so `VMIS_CHECKER` wins. That is correct here only by accident of alphabetical ordering, not by design. Add an explicit precedence rule before more dual-role users are created.
- **`5224580` is checker-only** — and per `Web.sitemap`, `VMIS_CHECKER` is absent from the two ancestor nodes above "Complaint Checker Inbox", so this user sees **no menu items at all**. They can only reach their inbox via the Default.aspx modal (whose link is broken — it points at the non-existent `ComplaintApproval.aspx`) or by typing the URL.

`Web.config` connects to this database as `AuthDB`.

---

## 9. `Common` — shared lookups

| Table | Rows |
|---|---:|
| `BranchMaster` | 16 |
| `CircleMaster` | 8 |
| `ZoneMaster` | 4 |

Connected as `dbCommon`. Small and populated; a plausible source for reseeding the empty `VigilanceMISDB` masters.

---

## 10. Practical notes for the next change

1. **Adding a column to a case table** — you must add it to the `*_HISTORY` twin in the same ordinal position, because every update proc does `INSERT INTO X_HISTORY SELECT * FROM X`. Four tables are already out of sync (§3.1).
2. **No FKs, no triggers, no views** — integrity and history are proc-only. A new write path that bypasses the proc bypasses the audit trail entirely.
3. **`spComplaint_View` is the pattern to copy for a new list screen**, but note its SEARCH branch builds dynamic SQL by string concatenation and runs `EXEC(@SQL)` — injectable. If you extend it, parameterise with `sp_executesql`.
4. **Maker–checker joins on `NEWZONE`**, not `ZONE`. A complaint with `NEWZONE = NULL` will never appear in any checker's inbox and will sit at `APPROVALSTATUS='P'` forever. Any new case type that adopts this workflow needs the same zone column populated on insert.
5. **Only the Complaint module has maker–checker.** Extending it to another module means: 6 new columns + an approval-history table (or a shared one keyed by module), a `sp<Module>_CheckerAction`, the `_Update` proc resubmit logic, a checker inbox page, a checker view page, sitemap entries for `VMIS_CHECKER`, and the `MakerCheckerMapping` zone scoping.
6. **Before demoing anything** — load the reference masters, restore `CBS_BRANCH_MASTER`, and create the 37 missing procedures. Nothing beyond the complaint workflow will run without them.

---

## 11. How this was collected

```powershell
sqlcmd -S "localhost\SQLEXPRESS" -E -C -Q "<query>"
```

Windows Authentication, `-C` to trust the self-signed server certificate. Catalogue views used: `sys.databases`, `sys.objects`, `sys.tables`, `sys.columns`, `sys.partitions`, `sys.procedures`, `sys.parameters`, `sys.sql_modules`, `sys.sql_expression_dependencies`, `sys.indexes`, `sys.key_constraints`, `sys.foreign_keys`, `sys.triggers`.

The page↔proc map was produced by scanning every non-designer `.cs` file under `VMISP\` (excluding `obj\`) for `sp[A-Z]\w+` tokens and intersecting with the live procedure catalogue.
