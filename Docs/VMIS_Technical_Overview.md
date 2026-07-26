# Vigilance MIS Portal (VMISP) — Technical Overview

**Repository:** `D:\Jitendra\projects\vmisprod`
**Solution:** `VMISP.sln` → single web project `VMISP/VMISP.csproj`
**Report generated:** 2026-07-25
**Audience:** developer new to the project

---

## 1. What this system is

VMISP is the **Vigilance Management Information System** of Punjab National Bank. It is the case-management and MIS backbone for the bank's Vigilance Department. Everything in the system revolves around one idea: a *case* — a complaint, an enquiry, a disciplinary proceeding, an RTI request, a whistle-blower report — is registered against an **accused employee** at a **branch**, routed through a **zone/circle** hierarchy, progressed through statuses, and finally closed. On top of that sit dashboards, SSRS reports, bulk Excel/Access imports, and a recently-added **maker–checker approval workflow** for complaints.

There are **15 case-type modules** (Complaint, IAC, Vigilance, SR, WB, RRB, RTI, MISC, NOC, LODI, ABBFF, Sanction, Operational Reference, Penalty-not-Commensurate-with-Charges, Vigilance Monitoring), each backed by its own table plus a mirror `*_HISTORY` table.

---

## 2. Technology stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET **Web Forms**, .NET Framework **4.7.2** |
| Language | C# (code-behind, `*.aspx.cs`) |
| UI | Web Forms server controls + AjaxControlToolkit + Bootstrap + jQuery (`vendor/`, `Scripts/`, `Js/`) |
| Charts | DotNet.Highcharts (dashboard) |
| Reports | **SSRS** via `Microsoft.ReportViewer.WebForms` 15.0 (server reports, not RDLC) |
| Excel | ClosedXML + OLEDB (Jet/ACE) for `.xls`/`.xlsx` import |
| Data access | Raw `SqlConnection`/`SqlCommand` + stored procedures. Entity Framework 6 exists (`Models/VMISModel.edmx`) but is used only for `ABBFF` |
| Auth | ASP.NET **Membership / Role / Profile** providers (SQL) + Forms Authentication + external **SSO** |
| Logging | **NLog** (`NLog.config` → `VMISP/logs/`) + custom DB error log |
| Databases | `VigilanceMISDB` (business data), `UserManagementDB` (membership/roles/profile), `Common` |

### Project layout

```
VMISP/
├── Login.aspx / LoginSSO.aspx / Logout.aspx    # authentication entry points
├── Default.aspx (+ .cs, 192 KB)                # main landing page = Dashboard
├── SiteMaster.Master                           # master page used by 158 pages
├── Site.Master                                 # legacy master, 6 pages
├── Web.sitemap                                 # role-trimmed navigation menu
├── Web.config                                  # providers, connection strings, auth
├── Global.asax.cs                              # headers, cookie hardening
├── WebProfile.cs                               # strongly-typed profile wrapper
├── Code/                                       # shared helpers
│   ├── CommonFunction.cs   (30 methods)
│   ├── SSO.cs              (SSO token exchange)
│   ├── VMISP_Error_Log.cs  (central exception handler)
│   ├── Dashboard.cs        (chart DTOs)
│   └── HRMS.cs             (HRMS employee DTOs)
├── DataAccessLayer/MasterData.cs               # 27 master-data lookups
├── Models/                                     # EF model (ABBFF only)
├── Admin/     (4 forms)   user & circle-head administration
├── Master/    (12 forms)  reference-data masters
├── Mis/       (30 forms)  case-entry / case-workflow forms
├── Reports/   (20 forms)  SSRS + grid reports
├── Search/    (7 forms)   generic & specialised search
├── Upload/    (4 forms)   Excel / MS-Access bulk import
├── vmismainscript.sql                          # full DB script (UTF-16, 19 231 lines)
└── obj/Release/Package/PackageTmp/             # BUILD OUTPUT — ignore, it duplicates every page
```

> ⚠️ **Important when navigating the repo:** `VMISP/obj/Release/Package/PackageTmp/` contains a *complete duplicate* of every `.aspx`. Always edit the copy in the source folder, never the one under `obj/`.

**Scale:** 93 `.aspx` pages in the source tree, **1 111 methods** across 93 code-behind files, 61 tables, ~190 stored procedures, 47 SSRS report paths.

---

## 3. Authentication and session management

### 3.1 The two login paths

**A. SSO (the production path)** — `Login.aspx.cs` → `Page_Load`

1. External SSO portal POSTs `enc_token` + `userid` to `Login.aspx`.
2. `SSOLayer.GETSSOData()` (`Code/SSO.cs`) calls the token API (`SSO_TokenAPI_URL`) and gets back an `SSOResponse` (`Username`, `returnURL`).
3. The posted `userid` is compared to `ssodata.Username`; mismatch → redirect back to SSO with an error.
4. `funcSSOLogin(userId, returnUrl)` completes the login.

**B. Local form login (legacy)** — `aspLogin_Authenticate` / `Login1_LoggingIn` / `Login1_LoggedIn`
Uses the `Login1` control with a client-side obfuscated password (every 3rd character is real, plus a GUID suffix), validated against `Membership.ValidateUser`. Forces a password change every 90 days or when `Profile.changepwd == "1"`.

### 3.2 `funcSSOLogin` — what a successful login actually does

```
WebProfile.GetProfile(userId)          → sol, solname, nameofuser, changepwd
   ↓ (null → "User not present in VigilanceMIS")
Session: solname, nameofuser, changepwd, userid, sol, solid, hosol
   ↓
Cache["VMISessionId"+userid] = SessionID          (concurrent-login guard)
   ↓
Roles.GetRolesForUser(userId)
   ↓ pick first role present in PrimaryRoles[]    → Session["role"]
   ↓ Session["IsChecker"] = roles.Contains("VMIS_CHECKER")
   ↓ Session["Roles"]     = all roles
   ↓
Session["AuthToken"] = new GUID  + cookie "AuthToken"
   ↓
funcValidateSingleUserLogin()   → spValidateSingleUserLogin   (find live session)
funcTerminateUserAlreadyLogin() → spUserLoginTrace_Operation  (kill it)
funcDeactivateAllSessions()     → spUserConcurrent_Deactivate
funcInsertCurrentSession()      → spUserConcurrent_InsertSession
funcUpdateUserTrace()           → spUserTrace_Updatenew       (audit row)
   ↓
FormsAuthentication.SetAuthCookie() → redirect ~/Default.aspx
```

If the user has **no** primary role, login is refused and the user is bounced to the SSO page with a message.

### 3.3 Session enforcement on every page — `SiteMaster.Master.cs`

Every page using `SiteMaster.Master` (158 of them) is guarded twice:

- **`Page_Init`** — if `Session["userid"]` is null → `FormsAuthentication.SignOut()`, abandon session, redirect to SSO. Also sets up the anti-XSRF ViewState user key (note: the cookie-writing half is commented out, so XSRF validation is effectively inert).
- **`Page_Load`** — requires `Session["AuthToken"]` **and** the `AuthToken` cookie **and** `funcCheckCurrentSession()` (→ `spUserConcurrent_CheckSession`) to all agree. Any mismatch clears the cache entry, kills the session and redirects to SSO. On success it paints the header (`solname`, `nameofuser`, role display name, timestamp) and adds `no-cache` headers.

This is a genuine **single-active-session-per-user** design: logging in from a second machine terminates the first session in the DB, and the first machine is thrown out on its next page load.

### 3.4 Web.config security posture

```xml
<authentication mode="Forms">
  <forms loginUrl="~/Login.aspx" protection="Encryption" defaultUrl="~/Default.aspx" />
</authentication>
<authorization><deny users="?" /></authorization>   <!-- anonymous denied globally -->
<sessionState mode="InProc" timeout="60" />
```

Membership: hashed passwords, min length 6, `maxInvalidPasswordAttempts="50"`, password reset enabled, no security question.
Site map: `securityTrimmingEnabled="true"` — the menu hides nodes the user's role cannot see.
`Global.asax.cs` strips `Server`, `X-AspNet-Version`, `X-Powered-By` headers and writes `HttpOnly; Secure; SameSite=Strict` cookies.

---

## 4. Roles — the authoritative list

Roles live in `UserManagementDB` (SqlRoleProvider). The code recognises **six**:

| Role | Display name (`SiteMaster.Master.cs`) | Purpose |
|---|---|---|
| `VMIS_SUPERUSER` | Vigilance Super User | Full access to everything |
| `VMIS_ADMIN` | Vigilance Admin User | User administration + reference masters + audit trail |
| `VMIS_MISUSER` | Vigilance Mis User | **Maker** — creates and edits all case records; bulk uploads |
| `VMIS_DESKUSER` | Vigilance Desk User | Dealing officer — read-only on case data, but may append HO status + remarks |
| `VMIS_VIEWUSER` | Vigilance View User | Read-only across all case modules |
| `VMIS_CHECKER` | *(no display name)* | **Checker** — approves / rejects / pushes back complaints for assigned zones |

### 4.1 Primary vs secondary roles

Declared identically in `Login.aspx.cs` and `Admin/UserCreation.aspx.cs`:

```csharp
PrimaryRoles   = { VMIS_ADMIN, VMIS_CHECKER, VMIS_DESKUSER,
                   VMIS_MISUSER, VMIS_SUPERUSER, VMIS_VIEWUSER };  // mutually exclusive
SecondaryRoles = { VMIS_CHECKER };                                 // additive
```

`AssignRole()` in `UserCreation.aspx.cs`:
- Assigning a **secondary** role just adds it (`Roles.AddUserToRole`).
- Assigning a **primary** role first strips every *other* primary role, then adds it.

`VMIS_CHECKER` appears in **both** arrays, so its behaviour depends on which branch runs first — `AssignRole` checks `SecondaryRoles` first, so assigning `VMIS_CHECKER` is always additive (a user can be `VMIS_MISUSER` **and** `VMIS_CHECKER`). But `funcSSOLogin` picks `Session["role"]` as the **first** match against `PrimaryRoles` in the order roles come back from the provider — so for a dual-role user, which role drives per-form permissions is not deterministic. See §14.

### 4.2 Three independent layers of authorisation

| # | Layer | Where | What it does |
|---|---|---|---|
| 1 | **Menu trimming** | `Web.sitemap` + `securityTrimmingEnabled` | Hides menu nodes whose `roles=` doesn't include the user's role |
| 2 | **Page-entry gate** | `CommonFunction.funcCheckUserRights(FORMNAME)` | Called in `Page_Load`; `false` → `Response.Redirect("~/Logout.aspx")` |
| 3 | **Control-level gate** | `funcControlsUserRights()` in **21 MIS forms** | Disables/hides Submit/Update/Delete and greys out fields per role |

**Layer 2 in full (`Code/CommonFunction.cs`):**

```csharp
VMIS_SUPERUSER → true for every FORMNAME
VMIS_CHECKER   → true for every FORMNAME          // NOTE: same breadth as superuser
VMIS_ADMIN     → BRANCH_MASTER, CIRCLE_MASTER, EMAIL_MASTER,
                 ZONE_CHIEF_MANAGER, LODI_DISABLE, DASHBOARD
VMIS_MISUSER   → BRANCH_MASTER, DASHBOARD
VMIS_DESKUSER  → BRANCH_MASTER, DASHBOARD
VMIS_VIEWUSER  → BRANCH_MASTER, DASHBOARD
```

Only six `FORMNAME` values are actually checked, so this layer only really guards masters and the dashboard. The bulk of MIS forms rely on layers 1 and 3.

**Layer 3, the common pattern** (`Mis/frmComplaint.aspx.cs`):

```csharp
if (USERROLE == "VMIS_VIEWUSER") {
    DisableAllControls(this.Page);      // recursive: TextBox, DropDownList, CheckBox, Button, Calendar
    // re-enable only the search boxes
    btnSubmit.Visible = btnUpdate.Visible = btnCancel.Visible = false;
}
else if (USERROLE == "VMIS_DESKUSER") {
    DisableAllControls(this.Page);
    // re-enable search boxes + HO-status panel + Dealing-Officer-Remarks + Update
}
// MISUSER / SUPERUSER: nothing disabled → full CRUD
```

Simpler modules (e.g. `frmVigilance`) only special-case `VMIS_VIEWUSER`.

---

## 5. Role → capability matrix

Derived from `Web.sitemap` (menu), `funcCheckUserRights` (page gate) and `funcControlsUserRights` (control gate).

| Area | SUPERUSER | ADMIN | MISUSER | DESKUSER | VIEWUSER | CHECKER |
|---|:--:|:--:|:--:|:--:|:--:|:--:|
| **User Maintenance** (create user, user list) | ✅ | ✅ | ❌ | ❌ | ❌ | ❌ |
| **Master Maintenance** (12 masters, circle-head) | ✅ | ✅ | ❌ | ❌ | ❌ | ❌ |
| **Dashboard** | ✅ | ❌¹ | ✅ | ✅ | ✅ | ❌¹ |
| **Case entry forms** — create | ✅ | ❌ | ✅ | ❌ | ❌ | ❌ |
| **Case entry forms** — edit/update | ✅ | ❌ | ✅ | HO status + remarks only | ❌ | ❌ |
| **Case entry forms** — view/search | ✅ | ❌ | ✅ | ✅ | ✅ | ❌ |
| **Bulk field update** (`frmComplaintUpdate`, `frmIACUpdate`, `frmVigilanceUpdate`) | ✅ | ❌ | ✅ | ❌ | ❌ | ❌ |
| **Complaint Checker Inbox** (approve / reject / push back) | ❌ | ❌ | ❌ | ❌ | ❌ | ✅ |
| **Excel / Access upload** | ✅ | ❌ | ✅ | ❌ | ❌ | ❌ |
| **Search (Form-wise, Customize, Vigilance Status, Retirement, EO)** | ✅ | ❌ | ✅ | ✅ | ✅ | ❌ |
| **Module reports (SSRS)** | ✅ | ❌ | ✅ | ✅ | ✅ | ❌ |
| **Audit Trail Report** | ❌ | ✅ | ❌ | ❌ | ❌ | ❌ |

¹ Menu node lists `VMIS_SUPERUSER,VMIS_MISUSER,VMIS_DESKUSER,VMIS_VIEWUSER` — `VMIS_ADMIN` is excluded from the Dashboard menu even though `funcCheckUserRights("DASHBOARD")` returns `true` for admin.

**In one sentence per role:**

- **SUPERUSER** — everything, no restrictions anywhere.
- **ADMIN** — creates/removes users and assigns roles, maintains reference masters (branch/circle/zone/email/LODI-disable), reads the audit trail. Does *not* touch case data.
- **MISUSER** — the data-entry workhorse. Creates and edits every case type, runs bulk Excel/Access imports, uses the bulk field-update forms, runs searches and reports.
- **DESKUSER** — the dealing officer. Sees every case but can only append an **HO Status** line and **Dealing Officer Remarks**; `spComplaint_Update` prepends their status to the existing status (`@p_HOSTATUS + ' | ' + old STATUS`) and stamps `DESK_USER_ID / _IP / _ADDDATE / _ROLE`.
- **VIEWUSER** — pure read-only; every control on the page is disabled except search boxes.
- **CHECKER** — sees only complaints in zones mapped to them in `MakerCheckerMapping`, and approves / rejects / pushes back.

---

## 6. Navigation structure (`Web.sitemap`)

Menu rendered by `<asp:Menu ID="NavigationMenu" DataSourceID="SiteMapDataSource1">` in `SiteMaster.Master`.

```
Home (~/Default.aspx)
└── [SUPERUSER, ADMIN, MISUSER, DESKUSER, VIEWUSER]
    ├── User Maintenance ......................... [SUPERUSER, ADMIN]
    │   ├── User Creation ........................ Admin/UserCreation.aspx
    │   └── User List ........................... Admin/frmUserList.aspx
    ├── Dashboard ............................... [SUPERUSER, MISUSER, DESKUSER, VIEWUSER]
    │   └── Dashboard ........................... Dashboard.aspx
    ├── Master Maintenance ...................... [SUPERUSER, ADMIN]
    │   ├── Circle Head Update .................. Admin/frmCircleHead.aspx
    │   ├── Branch Master (circle) .............. Master/CircleMaster.aspx
    │   ├── Branch Master ....................... Master/BranchMaster.aspx
    │   ├── Status Master ....................... Master/frmStatus.aspx
    │   ├── Scale Master ........................ Master/frmScale.aspx
    │   ├── Nature Case Master .................. Master/frmNatureCase.aspx
    │   ├── Source Ref Master ................... Master/frmSourceRef.aspx
    │   ├── Penalty Type Master ................. Master/frmPenaltyType.aspx
    │   ├── Register ............................ Master/frmRegister.aspx
    │   ├── Penalty Proceeding .................. Master/frmPenaltyProceding.aspx
    │   ├── Email Master ........................ Master/EmailMaster.aspx
    │   ├── Zone Chief Manager Master ........... Master/ZoneChiefManager.aspx
    │   └── Disable Lodi Details ................ Master/LodiDisable.aspx
    ├── Complaint ............................... [MISUSER, DESKUSER, VIEWUSER]
    │   ├── Complaint ........................... Mis/frmComplaint.aspx
    │   ├── Complaint Update .................... Mis/frmComplaintUpdate.aspx      [MISUSER]
    │   ├── Complaint Checker Inbox ............. Mis/frmComplaintChecker.aspx     [CHECKER]
    │   └── Report .............................. Reports/frmComplaintReports.aspx
    ├── IAC ..................................... frmIACStructure / frmIACUpdate[MISUSER] / frmIACReports
    ├── LODI .................................... Mis/Lodi.aspx / Reports/LodiReport.aspx
    ├── ABBFF ................................... Mis/frmABBFF.aspx / Reports/ABBFFReport.aspx
    ├── Misc .................................... frmMiscStructure / MISCReport / frmMISCReports
    ├── NOC ..................................... Mis/Noc.aspx
    ├── Operational Ref ......................... frmOperationalRef / frmORReports
    ├── RRB ..................................... frmRRB / RRB (new) / frmRRBReports
    ├── RTI ..................................... frmRTI / frmRTIReports
    ├── SR ...................................... frmSRStructure / SR (new) / frmSRReports
    ├── Sanction ................................ frmSanction, SanctionForInvestigation,
    │                                             SanctionForProsecution, SanctionDataUpload + 2 reports
    ├── Vigilance ............................... frmVigilance / Vigilance (new) / VigilanceMonitoring /
    │                                             PenaltyCharge / frmVigilanceUpdate[MISUSER] /
    │                                             VigilanceCaseStatus / frmVigilanceReports / PenaltyChargeReport
    ├── WB ...................................... frmWBStructure / WB (new) / frmWBReports
    ├── Search .................................. [MISUSER, DESKUSER, VIEWUSER]
    │   ├── Form Wise Search .................... Search/frmTableWiseSearch.aspx
    │   ├── Customize Report .................... Search/frmCustomizeReports.aspx
    │   ├── Vigilance Status .................... Search/frmVigilanceStatusSearch.aspx
    │   ├── Retirement Cases .................... Search/RetirementCases.aspx
    │   └── Accused / EO Search ................. Search/frmEOSearch.aspx
    ├── Upload .................................. [MISUSER]
    │   ├── Excel Upload For Form ............... Upload/frmExcelUpload.aspx
    │   ├── Excel Upload of PF .................. Upload/frmExcelPF.aspx
    │   └── Excel Upload of EO PF ............... Upload/frmExcelPFEODetails.aspx
    ├── Report .................................. [MISUSER, DESKUSER, VIEWUSER]
    │   ├── Case Register ....................... Reports/frmCaseRegister.aspx
    │   ├── Complaint Report .................... Reports/frmComplaintRpt.aspx
    │   └── MISC Report ......................... Reports/frmMiscellaneousReports.aspx
    └── Report .................................. [ADMIN]
        └── Audit Trail Report .................. Search/frmAuditTrailSearch.aspx
```

Pages present in the project but **not** on the menu: `Mis/frmComplaintCheckerView.aspx` (reached from the checker inbox), `Mis/frmComplaintView.aspx`, `Mis/frmNoc.aspx`, `Mis/frmSanction.aspx`, `Search/frmFieldWiseSearch.aspx`, `Upload/frmAccessUpload.aspx`, `secretpagexyz.aspx`, `WebForm1.aspx`, `NewLogin.aspx`, `Home.aspx`, `About.aspx`, the `Account/` scaffolding, `changePwd.aspx`, `frmChangePassword.aspx`.

---

## 7. The maker–checker workflow (Complaint module)

This is the newest feature in the codebase and the one currently under active change (see `git status` and `Database/Scripts/`).

### 7.1 Data model

**`MakerCheckerMapping`** — which user checks which zone:

| Column | Type | Meaning |
|---|---|---|
| `Id` | int identity | PK |
| `UserId` | uniqueidentifier | Membership `ProviderUserKey` |
| `UserPF` | varchar(10) | PF number = the login id |
| `ZoneSolID` | varchar(6) | Zone this user checks |
| `IsMaker` / `IsChecker` | bit | Currently only `IsChecker=1, IsMaker=0` rows are written |
| `IsActive` | bit | Soft-delete |
| `CreatedOn` / `CreatedBy` | | Audit |

**`COMPLAINT`** gained six workflow columns: `APPROVALSTATUS`, `MAKERUSER`, `MAKERDATE`, `CHECKERUSER`, `CHECKERDATE`, `CHECKERREMARKS`.

**`COMPLAINT_APPROVAL_HISTORY`** — append-only audit of every workflow transition: `COMPLAINTCODE`, `ACTIONTYPE`, `ACTIONBY`, `ACTIONDATE`, `REMARKS`, `USERROLE`, `USERIP`.

### 7.2 Status codes

| Code | Text (`spComplaint_View`) | Maker's Edit button (`gvMain_RowDataBound`) |
|---|---|---|
| `P` | Pending Approval | disabled, "Pending" (yellow) |
| `A` | Approved | enabled, "Edit" — editing re-queues it as `P` |
| `C` | Changes Requested | enabled, "Edit" (blue) — saving re-queues it as `P` |
| `X` | Rejected | **disabled**, "Rejected" (red) — locked from editing |
| `NULL` | *(legacy record)* | enabled, "Edit" — pre-workflow rows keep old behaviour |

> Historical note worth knowing: reject was originally coded `'R'`, which didn't match `spComplaint_View`'s `CASE` and silently fell through to the "editable" bucket. `Database/Scripts/2026-07-25_Complaint_Reject_Code_Fix_And_Remarks_Column.sql` changed it to `'X'` and includes a one-time `UPDATE COMPLAINT SET APPROVALSTATUS='X' WHERE APPROVALSTATUS='R'`.

### 7.3 Flow

```
MAKER (VMIS_MISUSER)                    CHECKER (VMIS_CHECKER)
─────────────────────                   ──────────────────────
Mis/frmComplaint.aspx
  btnSubmit_Click → funcSave("I")
    → spComplaint_Update @p_MODE='I'
       INSERT COMPLAINT (APPROVALSTATUS='P',
                         MAKERUSER, MAKERDATE)
       INSERT COMPLAINT_APPROVAL_HISTORY 'SUBMITTED'
                                   ─────►  Default.aspx Page_Load
                                           GetPendingComplaintCount(userPF)
                                           → modal "N complaint(s) pending"

                                           Mis/frmComplaintChecker.aspx
                                             BindComplaints():
                                               COMPLAINT c
                                               JOIN MakerCheckerMapping m
                                                 ON c.NEWZONE = m.ZoneSolID
                                               WHERE m.UserPF=@me
                                                 AND m.IsChecker=1 AND m.IsActive=1
                                                 AND c.APPROVALSTATUS='P'

                                           Mis/frmComplaintCheckerView.aspx?id=<RNO>
                                             btnAccept_Click   → TakeAction("A", …)
                                             btnReject_Click   → TakeAction("X", …)
                                             btnPushBack_Click → TakeAction("C", …)
                                               ↓ remarks mandatory (client + SP)
                                               spComplaint_CheckerAction
                                                 • validate action ∈ {A,X,C}
                                                 • remarks not empty
                                                 • complaint exists & ACTIVE='Y'
                                                 • caller is an active checker
                                                   for the complaint's NEWZONE
                                                 • status is still 'P' (no double-action)
                                                 • snapshot → COMPLAINT_HISTORY
                                                 • UPDATE APPROVALSTATUS, CHECKERUSER,
                                                          CHECKERDATE, CHECKERREMARKS
                                                 • INSERT COMPLAINT_APPROVAL_HISTORY
                                                   'APPROVED' | 'REJECTED' | 'PUSHED_BACK'
  ◄─────────────────────────────────────────────
Maker sees CHECKERREMARKS as a grid column.
  'C' or 'A' → Edit allowed
    btnUpdate_Click → funcSave("U")
      → spComplaint_Update @p_MODE='U'
         snapshot → COMPLAINT_HISTORY
         if old status ∈ {'C','A'} → APPROVALSTATUS='P'
                                   + 'RESUBMITTED' history row
  'X' → locked
```

**Why editing an Approved complaint re-queues it:** `2026-07-25_Complaint_Approved_Edit_Resubmit_Fix.sql`. Before that fix, a maker could edit an already-approved complaint and the edit was never re-verified.

### 7.4 Assigning a checker

`Admin/UserCreation.aspx`:
1. `DDLocation_SelectedIndexChanged` — selecting `VMIS_CHECKER` reveals `chkZones` and calls `BindZones()` (→ `spCircleMaster_Ddl`).
2. `BtnSubmit_Click` → `AssignRole()` adds `VMIS_CHECKER` (secondary → additive).
3. `SaveMakerCheckerMapping(userPF)` — sets `IsActive=0` on all the user's existing rows, then upserts one `IsChecker=1, IsMaker=0, IsActive=1` row per ticked zone.
4. `DeactivateCheckerMappings(userPF)` / `RemoveSecondaryRole()` — used when the checker role is withdrawn.
5. `LoadCheckerZones()` — re-ticks the boxes when an existing user is searched.

---

## 8. Functional modules

Every case module follows the same shape. Naming convention: `frmXxx.aspx` / `frmXxxStructure.aspx` is the original form; a bare `Xxx.aspx` (`SR.aspx`, `WB.aspx`, `RRB.aspx`, `Vigilance.aspx`, `NOC.aspx`) is a newer redesign of the same module — **both are live and both are on the menu.**

| Module | Entry form(s) | Table | Notes |
|---|---|---|---|
| **Complaint** | `Mis/frmComplaint.aspx` | `COMPLAINT` (+ `COMPLAINT_EO_DETAILS`) | The only module with maker–checker. Child grid of Accused/EO details. Has an "Fetch IAC" lookup (`btnFetchIAC_Click`). |
| **IAC** (Internal Advisory Committee) | `Mis/frmIACStructure.aspx` | `IAC` | Meeting-based: `MEETNO`, `IACNO`, DA/IAC/CVO views, ABBFF cross-reference fields |
| **Vigilance** | `Mis/frmVigilance.aspx`, `Mis/Vigilance.aspx` | `VIGILANCE` | **The largest entity — ~180 columns.** Full disciplinary lifecycle: charge sheet → CVC 1st-stage advice → enquiry (PO/EO/CDI) → CVC 2nd-stage advice → DA order → penalty → appeal → closure. CBI RC numbers, FIR, suspension/revocation, LODI linkage |
| **Vigilance Monitoring** | `Mis/VigilanceMonitoring.aspx` | `VIGILANCEMIS` | Condensed monitoring view of vigilance cases |
| **SR** (Staff Reference) | `Mis/frmSRStructure.aspx`, `Mis/SR.aspx` | `SR` | ZM view / IC view / final action |
| **WB** (Whistle Blower) | `Mis/frmWBStructure.aspx`, `Mis/WB.aspx` | `WB` | Same column set as `COMPLAINT` |
| **RRB** (Regional Rural Bank) | `Mis/frmRRB.aspx`, `Mis/RRB.aspx` | `RRB` | Mirrors `VIGILANCE` for RRB staff |
| **RTI** | `Mis/frmRTI.aspx` | `RTI` | RTI applications routed through vigilance |
| **MISC** | `Mis/frmMiscStructure.aspx` | `MISC` (+ `MISC_EO_DETAILS`) | Miscellaneous / NPA cases; carries `NPADATE`, `ZONE_CM`, `ZONE_TYPE` |
| **NOC** | `Mis/Noc.aspx`, `Mis/frmNoc.aspx` | `NOC` | Vigilance clearance for an employee |
| **LODI** (List of Doubtful Integrity) | `Mis/Lodi.aspx` | `LODI` | Add/delete from LODI with reason; deletions gated by `Master/LodiDisable.aspx` |
| **ABBFF** | `Mis/frmABBFF.aspx` | `ABBFF` (+ `ABBFF_EO_DETAILS`) | Advisory Board for Banking & Financial Frauds — fraud/FMR/RBI reporting. **Only module using Entity Framework** |
| **Sanction** | `Mis/frmSanction.aspx`, `SanctionForInvestigation.aspx`, `SanctionForProsecution.aspx`, `SanctionDataUpload.aspx` | `SANCTION`, `SANCTION_FOR_INVESTIGATION`, `SANCTION_FOR_PROSECUTION` | Sanction for investigation / prosecution, plus a dedicated Excel upload with verify-then-submit |
| **Operational Reference** | `Mis/frmOperationalRef.aspx` | `OPERATIONALREF` | Same shape as `COMPLAINT` |
| **Penalty Charge** | `Mis/PenaltyCharge.aspx` | `PENALTY_CHARGE` | "Penalty not commensurate with charges" tracking |

### Bulk field-update forms (MISUSER only)

`frmComplaintUpdate.aspx`, `frmIACUpdate.aspx`, `frmVigilanceUpdate.aspx` — pick a field from a dropdown (`ddlField_SelectedIndexChanged` re-labels the value control), enter a value, apply across records via `spComplaintUser_Update` / `spIACUser_Update` / `spVigilanceUser_Update`.

---

## 9. Reference-data masters (`Master/`, `Admin/`)

| Form | Table | Stored procs |
|---|---|---|
| `Master/BranchMaster.aspx` | `BRANCH_MASTER`, `BRANCH_MASTER_NEW` | `spBranchMaster_View/_Update/_Operation/_Ddl` |
| `Master/CircleMaster.aspx` | `DIVISIONMASTER` | `spCircleMaster_View/_Operation/_Ddl` |
| `Master/frmStatus.aspx` | `STATUS` | `spStatus_View/_Update` |
| `Master/frmScale.aspx` | `SCALE` | `spScale_View/_Update` |
| `Master/frmNatureCase.aspx` | `NATURECASE` | `spNatureCase_View/_Update` |
| `Master/frmSourceRef.aspx` | `SOURCEREF` | `spSourceRef_View/_Update` |
| `Master/frmPenaltyType.aspx` | `PENALTYTYPE` | `spPenaltyType_View/_Update` |
| `Master/frmPenaltyProceding.aspx` | `PENALTYPROCEEDING` | `spPenaltyProceding_View/_Update` |
| `Master/frmRegister.aspx` | `REGISTER` | `spRegister_View/_Update` |
| `Master/frmCVOAdvice.aspx` | `CVOADVICE` | `spCVOAdvice_View/_Update` |
| `Master/EmailMaster.aspx` | `EMAIL_MASTER` | `spEmailMaster`, `spEmailMaster_Get` |
| `Master/ZoneChiefManager.aspx` | `ZONE_CHIEF_MANAGER` (+ history) | `spZoneChiefManager_View/_Operation` |
| `Master/LodiDisable.aspx` | `LODI` | `spLodiDisable_View`, `spLodi_Disable` |
| `Admin/frmCircleHead.aspx` | `DIVISIONMASTER` | `spCircleHead_Update` |
| `Admin/BranchMaster.aspx` | `BRANCH_MASTER` | `spBranchMaster_*` |
| `Admin/UserCreation.aspx` | Membership + `MakerCheckerMapping` | `spCircleMaster_Ddl`, `spUserType_Ddl`, `spRoleDescription_Ddl` |
| `Admin/frmUserList.aspx` | Membership | `spUserRole_Ddl` |

Masters all share the same 8–14 method skeleton: `Page_Load`, `funcSave`, `funcShow`, `funcBindControl`, `funcClear`, `btnSubmit_Click`, `btnUpdate_Click`, `btnCancel_Click`, `btnGet_Click`, `imgSearch_LIST_Click`, `gvMain_RowCommand`, `gvMain_PageIndexChanging`, `gvMain_Sorting`, `gvMain_RowDataBound`.

---

## 10. Search

| Form | What it does |
|---|---|
| `Search/frmTableWiseSearch.aspx` | Generic search over any module table. `ddlTableName_SelectedIndexChanged` → `spTableColumn_Get` builds the column list → `spTableWiseSearch_View` runs it. Excel + PDF export |
| `Search/frmCustomizeReports.aspx` | User picks table + columns; `spCustomize_Report` returns an ad-hoc result set. Excel + PDF |
| `Search/frmFieldWiseSearch.aspx` | Field-level search (`spFieldWiseSearch_View`), sortable/paged grid |
| `Search/frmAuditTrailSearch.aspx` | **ADMIN only.** Searches the `*_HISTORY` tables via `spHistoryTableColumn_Get` + `spHistoryTableWiseSearch_View` — shows who changed what |
| `Search/frmEOSearch.aspx` | Accused / Enquiry-Officer search across modules (`spEOSearch_View`) |
| `Search/RetirementCases.aspx` | Employees with pending cases nearing retirement (`spRetirementCases_Details`) |
| `Search/frmVigilanceStatusSearch.aspx` | Vigilance status by employee; also calls the **HRMS web service** (`funcHRMSEmployee` → `CBS_HRMS_SERVICE_EMP`) to pull live employee details |

---

## 11. Bulk import

| Form | What it does |
|---|---|
| `Upload/frmExcelUpload.aspx` | **The big one — 33 methods.** For each of 13 modules there is a `funcExcelVerify_XXX` (dry-run validation, `spExcelVerify_Get`) and a `funcExcelImport_XXX` (commit, `spXXXExcel_Import`). Covers COMPLAINT, IAC, MISC, LODI, NOC, RRB, RTI, SR, VIG, VigilanceMIS, WB, SanctionForInvestigation, SanctionForProsecution. `funcDownloadExcelFormat` emits the blank template (`spSanctionFileFormat` / `spTableFormat_Get`). Two-step: **Verify → Upload** |
| `Upload/frmExcelPF.aspx` | Bulk PF-number correction on case records |
| `Upload/frmExcelPFEODetails.aspx` | Bulk PF-number correction on Accused/EO child rows |
| `Upload/frmAccessUpload.aspx` | Legacy MS-Access `.mdb` import for COMPLAINTS / MISC / OPERATIONALREFERENCE / RRB / SR (`spACCESSSR_Import`). Not on the menu |
| `Mis/SanctionDataUpload.aspx` | Dedicated verify-then-submit for the two Sanction tables (`spSanctionDataVerify`, `spSanctionForInvestigation_Upload`, `spSanctionForProsecution_Upload`) |

Uploaded files land in `ExcelFolderPath = Files\ExcelImport\` and are read through the Jet/ACE OLEDB providers configured as `Excel03ConString` / `Excel07ConString`.

---

## 12. Reporting

### 12.1 SSRS server reports (`Reports/frm*Reports.aspx`)

Twelve pages host a `ReportViewer` in **server-report** mode. Each `lnkXxx_Click` handler sets `rvMain.ServerReport.ReportPath` to a path under `/VMIS_Reports/` and refreshes. Credentials come from `report_uid` / `report_pwd` in `Web.config` via an `IReportServerCredentials` implementation defined *per page* (`MyConfigFileCredentials`, `MyConfigFileCredentials2`, `MyConfigFileCredentials3`, …).

**All 47 report paths:**

```
Complaint      : ComplaintOutstanding, ComplaintOutstandingAsOnDate, ComplaintOutstandingCVC,
                 ComplaintOutstandingOthers, ComplaintReportToMD, ComplaintStatus,
                 Complaints, rptComplaints
IAC            : IACOutstanding, IACReportToMD, IACRetirement, IACStatus
Vigilance      : VigilanceOutstanding, VigilanceStatus, VigilanceFirstStagePending,
                 VigilanceFirstStagePendingatDesk, VigilanceSecondStagePending,
                 VigilanceSecondStagePendingAtDA, VigilanceChargeSheetNotServed,
                 VigilanceEoPoNotAppointed, VigilanceReconsiderViewAwiatedFromDA,
                 VigilanceEnquiryIsInProgress, VigilanceRetirement,
                 VigilanceFinalOrderAwaited, VigilanceMinorChargeSheet
SR             : SROutstanding, SRStatus, SRReportToMD
WB             : WBOutstanding, WBStatus, WBReportToMD
RRB            : RRBOutstanding, RRBStatus, ProgressOfRRBReport
RTI            : RTIOutstanding, RTIStatus
MISC           : MISCOutstanding, MISCStatus, MISCReportToMD
Operational Ref: OROutstanding, ORStatus
Other          : DepartmentalEnquiries, Investigation, NatureProcedings,
                 PenaltyProcedings, DFSReport, DFSDetailsReport
```

> ⚠️ These pages read `AppSettings["report_ipaddress"]` and `AppSettings["report_serverstring"]`, but **`Web.config` defines `report_serverip` and `report_servername`** — the keys don't match, so `reportServerPath` is built from nulls. SSRS reports will fail against the current config. See §14.

### 12.2 In-page grid reports (`Reports/*.aspx` without ReportViewer)

`frmCaseRegister.aspx` (Complaint / IAC / Vigilance registers with sort, page and Excel download), `LodiReport.aspx`, `MISCReport.aspx`, `ABBFFReport.aspx`, `PenaltyChargeReport.aspx`, `VigilanceCaseStatus.aspx`, `VigilanceMonitoringReports.aspx`, `SanctionForInvestigationReports.aspx`, `SanctionForProsecutionReports.aspx` — these query stored procs directly and render a `GridView`, with `funcConvertToExcel` / `funcConvertToPDF` export.

---

## 13. Dashboard (`Default.aspx` — 116 methods)

`Default.aspx` is both the landing page and the main dashboard. Structure:

- **`Page_Load`** — `funcCheckUserRights("DASHBOARD")` gate → `funcbindDropdown()` (`spDashbaord_Ddl`) → `GetPendingComplaintCount(userPF)`; if > 0, registers a startup script that opens the *Pending Complaint Approval* modal.
- **`funcHideUnhide(VIEW)`** — swaps between five panels: `OUTSTANDING`, `COMPLAINT_OUTSTANDING`, `IAC_OUTSTANDING`, `VIGILANCE_OUTSTANDING`, `NPA_OUTSTANDING`, `ABBFF`.
- **~20 `getXxx…` methods** — call the `spDashboard*_Outstanding` family and return `List<ChartData>` DTOs (defined in `Code/Dashboard.cs`).
- **~18 `funcOutstanding…Charts` methods** — build DotNet.Highcharts column/pie charts from those DTOs.
- **~70 `btnXxx_Click` handlers** — every chart segment and KPI tile is clickable and drills through to a detail grid (`spDashboard_OutstandingData`, `spDashboardCompalintIACVigilanceNPA_OutstandingData`), exportable via `funcConvertToExcelCOD`.
- **Four `ddlDealingCM*_SelectedIndexChanged`** — filter each panel by dealing Chief Manager.
- **`Page_PreInit`** — disables partial rendering (`sm.EnablePartialRendering = false`) so Highcharts renders correctly.

`Dashboard.aspx` is a separate, much smaller page (menu item "Dashboard").

---

## 14. The canonical page pattern

Once you understand one MIS form you understand thirty. Method-name frequency across the 93 code-behinds:

| Method | Count | Role |
|---|---:|---|
| `Page_Load` | 93 | Entry: rights check, dropdown binding, JS wiring, `ViewState["USERROLE"]` |
| `funcClear` | 43 | Reset all controls to blank/default |
| `funcShow` | 36 | Load a record or list via `spXxx_View` |
| `btnSubmit_Click` | 36 | → `funcSave("I")` (insert) |
| `funcSave` | 35 | Single method handling both insert and update via a `MODE` char |
| `btnUpdate_Click` | 34 | → `funcSave("U")` (update) |
| `btnCancel_Click` | 32 | → `funcClear()` |
| `btnGet_Click` | 31 | Fetch by primary reference number |
| `gvMain_RowCommand` | 29 | Grid row actions (Edit / View / Delete) |
| `funcBindControl` | 26 | Map a `DataRow` onto form controls |
| `funcbindDropdown` | 22 | Bind all lookups from `spXxx_Ddl` |
| `gvMain_RowDataBound` | 21 | Per-row formatting / button state |
| `funcControlsUserRights` | 21 | **Role-based control enable/disable** |
| `gvMain_Sorting` / `_PageIndexChanging` | 19 each | Grid sort/page |
| `tabMain_ActiveTabChanged` | 18 | Tab switch (Entry ⇄ List) |
| `funcConvertToExcel` | 17 | Export grid to `.xls` |
| `ddlZoneNew_SelectedIndexChanged` | 15 | Cascade Zone → Circle → Branch |
| `funcValidation` | 12 | Server-side validation |

**Standard data-access idiom** (repeated hundreds of times):

```csharp
SqlConnection con = new SqlConnection(WebConfigurationManager
        .ConnectionStrings["dbVIGILANCEMIS"].ConnectionString);
SqlCommand cmd = new SqlCommand();
try {
    con.Open();
    cmd.Connection  = con;
    cmd.CommandType = CommandType.StoredProcedure;
    cmd.CommandText = "[dbo].[spXxx_Update]";
    cmd.Parameters.Clear();
    cmd.Parameters.AddWithValue("@p_...", value);
    SqlParameter outMsg  = new SqlParameter("@o_EERMSG",  SqlDbType.VarChar, -1) { Direction = ParameterDirection.Output };
    SqlParameter outCode = new SqlParameter("@o_ERRCODE", SqlDbType.Int)         { Direction = ParameterDirection.Output };
    cmd.Parameters.Add(outMsg); cmd.Parameters.Add(outCode);
    cmd.CommandTimeout = 0;                      // no timeout, everywhere
    cmd.ExecuteNonQuery();
    lblMsg.Text = Convert.ToString(outMsg.Value);
}
catch (Exception ex) { VMISP.VMISP_COMM_ERROR_TRACK.VMISP_Error_Log.HandleException(ex); }
finally { cmd.Dispose(); con.Close(); con.Dispose(); }
```

Every write proc returns `@o_EERMSG` (message shown to the user) and `@o_ERRCODE` (`1` = inserted, `2` = updated, `3` = duplicate key, `-1` = not found, `0` = failure).

---

## 15. Shared code reference

### `Code/CommonFunction.cs` (30 methods)

Conversion — `convertToInt`, `convertToInt(ToolTip)`, `convertToDecimal`, `convertToDateTime`
Dropdowns — `ddlSelectedText`, `ddlSelectedValue`, `ddlSelectedValue_Scale`, `ddlSetData`, `ddlSetDataValue`, `ddlSetDataValue_Scale`, `bindDropdownList`, `bindDropdownList_SELECT`
Control state — `disableControlsTextBox`, `enableControlsTextBox`, `disableControlsDropDownList`, `disableControlsCheckBox`, `chkSelected`, `chkSetData`, **`DisableAllControls`**, **`EnableAllControls`**
String utils — `removeTextBoxFirstComma`, `removeStringLastComma`, `removeStringLastPipe`
Context — **`funcGetUserIP`** (stamped on every write), **`funcCheckUserRights`**
Lookups — `funcDisciplinaryAuthority`, `funcMasterEmail_Get`, `funcZoneCircleMaster`, `funcGetBranchName`

### `DataAccessLayer/MasterData.cs` (27 methods)

`funcRoleMaster`, `funcRoleDescriptionMaster`, `funcCircleZone`, `funcCircleZoneMaster`, `funcCircleBranch`, `funcBranchMaster`, `funcGetBranchCircle`, `funcBindBranchCircle`, `funcUserEmailID`, `funcCircleMaster`, `funcMasterEmailID`, `funcUpdateMailStatusofForgetPassword`, `funcGetPasswordUserID`, `funcValidateForgetPasswordUID`, `funcUploadedDocument`, `funcLogout`, `funcGetTableFormat`, `funcDelete`, `funcStatusMaster` (×2 overloads), `funcMasterEmail_Get`, `funcZoneCircle`, `funcZoneMaster`, `funcScaleMaster`, `funcUserType_Ddl`, `funcZoneTypeCM`, `funcLockedError`

### `Code/SSO.cs`
`GETSSOData(json)` — POSTs to `SSO_TokenAPI_URL`, returns the decrypted payload. `RemoteServerCertificateValidationCallback` — **accepts any TLS certificate**.

### `Code/VMISP_Error_Log.cs`
`HandleException(ex)` — the single catch-all used in essentially every `catch` block. Writes to `VMIS_ERROR_LOG` via `spErrorLog_Update`.

### `WebProfile.cs`
Strongly-typed wrapper over the SqlProfileProvider: `sol`, `solname`, `nameofuser`, `changepwd`, `pocategory`, `deptlist`. `GetProfile(userId)` is what turns a PF number into a branch/user identity at login.

---

## 16. Database

### 16.1 Tables (61)

**Case entities + history pairs**
`COMPLAINT` / `COMPLAINT_HISTORY` / `COMPLAINT_EO_DETAILS` / `COMPLAINT_APPROVAL_HISTORY` / `COMPLAINT_060622` (backup)
`IAC` / `IAC_HISTORY` · `VIGILANCE` / `VIGILANCE_HISTORY` · `VIGILANCEMIS` / `VIGILANCEMIS_HISTORY`
`SR` / `SR_HISTORY` · `WB` / `WB_HISTORY` · `RRB` / `RRB_HISTORY` · `RTI` / `RTI_HISTORY`
`MISC` / `MISC_HISTORY` / `MISC_EO_DETAILS` · `NOC` / `NOC_HISTORY` · `LODI` / `LODI_HISTORY`
`OPERATIONALREF` / `OPERATIONALREF_HISTORY` · `PENALTY_CHARGE` / `PENALTY_CHARGE_HISTORY`
`ABBFF` / `ABBFF_EO_DETAILS` · `SANCTION` / `SANCTION_HISTORY`
`SANCTION_FOR_INVESTIGATION` (+`_HISTORY`) · `SANCTION_FOR_PROSECUTION` (+`_HISTORY`)

**Masters** — `BRANCH_MASTER`, `BRANCH_MASTER_NEW`, `DIVISIONMASTER`, `STATE_MASTER`, `EMPLOYEE_MASTER`, `Fgm_Master`, `STATUS`, `SCALE`, `NATURECASE`, `SOURCEREF`, `PENALTYTYPE`, `PENALTYPROCEEDING`, `REGISTER`, `CVOADVICE`, `EMAIL_MASTER`, `ZONE_CHIEF_MANAGER` (+`_HISTORY`), `tbl_UserCreationDD`

**Security / audit** — `MakerCheckerMapping`, `USER_TRACE`, `USER_CONCURRENT_LOGIN`, `VMIS_ERROR_LOG`, `VMISP_LOG`

### 16.2 Scalar functions

- `ReverseColumnValue_Function(value, refNo, tableName)` — returns a shortened/derived form of long text columns (used for `SHORTSTATUS`, `SHORTREASONSFORCLOSURE`)
- `SolName_Function(sol)` — SOL id → branch name
- `Status_Function(...)` — status code → description

### 16.3 Stored-procedure naming convention

| Suffix | Meaning | Example |
|---|---|---|
| `_View` | SELECT (list / search / single record, switched on `@p_VIEW`) | `spComplaint_View` |
| `_Update` | INSERT **or** UPDATE, switched on `@p_MODE` (`I`/`U`) | `spComplaint_Update` |
| `_Ddl` | Dropdown/lookup dataset (often multi-result) | `spVigilance_Ddl` |
| `_Delete` | Soft delete (`ACTIVE='N'`) | `spVigilance_Delete` |
| `_Operation` | Combined CRUD driven by a type parameter | `spBranchMaster_Operation` |
| `_Import` | Excel/Access bulk load | `spComplaintExcel_Import` |
| `_Report` | Report dataset | `spLodi_Report` |
| `_Get` | Small scalar/metadata fetch | `spTableColumn_Get` |

### 16.4 Full stored-procedure inventory (~190, as referenced from C#)

<details>
<summary>Click to expand</summary>

**Auth / session / audit**
`spValidateSingleUserLogin`, `spUserConcurrent_CheckSession`, `spUserConcurrent_Deactivate`, `spUserConcurrent_InsertSession`, `spUserLoginTrace_Operation`, `spUserTrace_Update`, `spUserTrace_Updatenew`, `spLogout`, `spLockedError`, `spErrorLog_Update`, `spFogetPasswordUIDValidate`, `spForgetPasswordMailStatus_Update`, `spPasswordUserID_Get`, `spUserEmaild_Get`, `spUserRole_Ddl`, `spUserType_Ddl`, `spRoleDescription_Ddl`

**Complaint** `spComplaint_View`, `spComplaint_Update`, `spComplaint_Ddl`, `spComplaint_CheckerAction`, `spComplaintUser_Update`, `spComplaintEO_Add`, `spComplaintEO_View`, `spComplaintEO_Delete`, `spComplaintExcel_Import`

**IAC** `spIACStructure_View`, `spIACStructure_Update`, `spIAC_Ddl`, `spIACUser_Update`, `spIACExcel_Import`

**Vigilance** `spVigilance_View`, `spVigilance_Update`, `spVigilance_Ddl`, `spVigilance_Delete`, `spVigilanceUser_Update`, `spVigilanceExcel_Import`, `spVigilanceStatus_View`, `spVigilanceCaseStatus_Report`, `spVigilanceMIS_View`, `spVigilanceMIS_Update`, `spVigilanceMIS_Ddl`, `spVigilanceMonitoring_Report`, `spVIGMExcel_Import`

**SR** `spSRStructure_View`, `spSRStructure_Update`, `spSRStructure_Delete`, `spSR_Ddl`, `spSRExcel_Import`, `spACCESSSR_Import`
**WB** `spWBStructure_View`, `spWBStructure_Update`, `spWBStructure_Delete`, `spWB_Ddl`, `spWBExcel_Import`
**RRB** `spRRB_View`, `spRRB_Update`, `spRRB_Ddl`, `spRRB_Delete`, `spRRB_Operation`, `spRRBExcel_Import`
**RTI** `spRTI_View`, `spRTI_Update`, `spRTI_Ddl`, `spRTIExcel_Import`
**MISC** `spMiscStructure_View`, `spMiscStructure_Update`, `spMISC_Ddl`, `spMISC_Report`, `spMiscEO_Add`, `spMiscEO_View`, `spMiscEO_Delete`, `spMISCExcel_Import`
**NOC** `spNOC_View`, `spNOC_Update`, `spNOC_Ddl`, `spNOC_Delete`, `spNOCExcel_Import`
**LODI** `spLodi`, `spLodi_View`, `spLodi_Ddl`, `spLodi_Report`, `spLodi_Disable`, `spLodiDisable_View`, `spLodiExcel_Import`
**ABBFF** `spABBFFStructure_View`, `spABBFFStructure_Update`, `spABBFFEO_Add`, `spABBFFEO_View`, `spABBFFEO_Delete`
**Operational Ref** `spOperationalRef_View`, `spOperationalRef_Update`, `spOperationalRef_Ddl`
**Penalty Charge** `spPenaltyCharge`, `spPenaltyCharge_View`, `spPenaltyCharge_Ddl`, `spPenaltyCharge_Report`
**Sanction** `spSanction_View`, `spSanction_Update`, `spSanctionForInvestigation`, `spSanctionForInvestigation_View`, `spSanctionForInvestigation_Ddl`, `spSanctionForInvestigation_Report`, `spSanctionForInvestigation_Upload`, `spSanctionForProsecution`, `spSanctionForProsecution_View`, `spSanctionForProsecution_Ddl`, `spSanctionForProsecution_Report`, `spSanctionForProsecution_Upload`, `spSanctionDataVerify`, `spSanctionFileFormat`, `spSFIExcel_Import`, `spSFPCExcel_Import`

**Masters** `spBranchMaster_View/_Update/_Operation/_Ddl`, `spBranchCircle_Ddl`, `spBranchCircle_Get`, `spBranchName_Get`, `spCircleMaster_View/_Operation/_Ddl`, `spCircleBranchMaster_Ddl`, `spCircleZoneMaster_Ddl`, `spCircleOffice_Ddl`, `spCircleHead_Update`, `spZoneMaster_Ddl`, `spZoneCircle_Ddl`, `spZoneCircleMaster_Ddl`, `spZoneCode_Get`, `spZoneTypeCM_Ddl`, `spZoneChiefManager_View/_Operation`, `spStatus_View/_Update`, `spStatusMaster_Ddl`, `spScale_View/_Update`, `spScaleMaster_Ddl`, `spNatureCase_View/_Update`, `spSourceRef_View/_Update`, `spPenaltyType_View/_Update`, `spPenaltyProceding_View/_Update`, `spRegister_View/_Update`, `spCVOAdvice_View/_Update`, `spEmailMaster`, `spEmailMaster_Get`, `spMasterEmailID_Get`, `spMaster_Ddl`, `spMasterForm_Ddl`, `spDisciplinaryAuthority_Ddl`, `spDelete`

**Search / reports** `spTableWiseSearch_View`, `spTableColumn_Get`, `spTableFormat_Get`, `spHistoryTableWiseSearch_View`, `spHistoryTableColumn_Get`, `spFieldWiseSearch_View`, `spEOSearch_View`, `spCustomize_Report`, `spCaseRegister_View`, `spRetirementCases_Details`

**Upload helpers** `spExcelVerify_Get`, `spExcelImportDetails_Get`, `spExcelImportDetailsEO_Get`, `spUploadTableColumn_Get`, `spUplodedFile_Get`

**Dashboard** `spDashbaord_Ddl`, `spDashboard_Outstanding`, `spDashboard_OutstandingData`, `spDashboardCompalint_Outstanding`, `spDashboardCompalintDayWise_Outstanding`, `spDashboardCompalintIACVigilanceNPA_OutstandingData`, `spDashboardComplaintPendingatDesk_Outstanding`, `spDashboardComplaintSourceRef_Outstanding`, `spDashboardIAC_Outstanding`, `spDashboardIACDayWise_Outstanding`, `spDashboardIACPendingatDesk_Outstanding`, `spDashboardNPA_Outstanding`, `spDashboardNPADayWise_Outstanding`, `spDashboardVigilance_Outstanding`, `spDashboardVigNonVig_Outstanding`

</details>

> **Note on `vmismainscript.sql`:** the file is **UTF-16LE** encoded (19 231 lines). Standard `grep` will find nothing unless you convert it first: `iconv -f UTF-16LE -t UTF-8 vmismainscript.sql > vmis-utf8.sql`. It contains the schema and only two procedures (`spValidateSingleUserLogin`, `spVigilanceStatus_View`) — **the remaining ~190 procedures are not in source control.** They exist only on the database server. This is the single biggest gap in the repository.

---

## 17. Cross-cutting concerns

**Audit trail.** Every case table carries `ADDUSER`, `ADDDATE`, `ADDUSERIP`, `MODUSER`, `MODDATE`, `MODUSERIP`, plus `DESK_USER_ID / _IP / _ADDDATE / _ROLE` for desk-user annotations. Every update proc snapshots the whole row into the matching `*_HISTORY` table *before* writing. `Search/frmAuditTrailSearch.aspx` (ADMIN) reads those history tables.

**Soft delete.** Records are never physically removed — `ACTIVE='Y'/'N'` (or `LODI_ACTIVE`, `PC_ACTIVE`). Every `_View` proc filters `ACTIVE='Y'`.

**Login trace.** `USER_TRACE` records every login/logout with IP, SOL, role, status and auth token. `USER_CONCURRENT_LOGIN` backs the single-session rule.

**Error logging.** Two channels: NLog to `VMISP/logs/`, and `VMISP_Error_Log.HandleException` → `VMIS_ERROR_LOG` table.

**Zone/Circle migration.** Case tables carry both the old `ZONE`/`CIRCLEOFFICE` (free text) and the newer `NEWZONE`/`NEWCIRCLE` (SOL-id coded) columns. The maker–checker join uses `NEWZONE`, so **a complaint with a null `NEWZONE` will never appear in any checker's inbox.**

---

## 18. Issues worth knowing about before you change anything

These are real findings from reading the code, ordered by severity. None are speculative.

### 🔴 Critical

**1. Hardcoded login backdoor in `Login.aspx.cs:78-86`.**
`Page_Load` runs this *unconditionally*, after the SSO branch and outside any `if`:

```csharp
Session["returnURL"] = "https://10.192.3.99/ssouat/sso.php";
funcSSOLogin("5224563", "https://10.192.3.99/ssouat/sso.php"); //mis
```

Anyone who reaches `Login.aspx` is silently logged in as PF `5224563`. Several other PF numbers sit commented out just above it (admin, checker, desk user) — clearly a developer convenience for role-switching during testing. **This must not reach production.**

**2. SQL injection in `spComplaint_View` (SEARCH branch).** Search parameters are concatenated into a `VARCHAR(MAX)` and run through `EXEC(@SQL)`:

```sql
SET @STRCOND = @STRCOND + 'AND ACCUSED LIKE''%' + @p_ACCUSED + '%'''
...
EXEC(@SQL);
```

The C# side passes these as proper `SqlParameter`s, so the injection point is inside the procedure, not the page — but it is fully reachable from `frmComplaint.aspx`'s search boxes. The same dynamic-SQL pattern appears in the generic search procs (`spTableWiseSearch_View`, `spCustomize_Report`, `spFieldWiseSearch_View`).

**3. Live credentials committed in `Web.config`.** `report_pwd`, `CBSServicePassword`, and several connection strings with `User ID=sa; Password=...` (commented and uncommented). These should move to encrypted config sections or a secret store, and the exposed passwords should be rotated.

### 🟠 High

**4. `VMIS_CHECKER` has superuser-level form access.** In `funcCheckUserRights`, `VMIS_CHECKER` returns `true` for *every* `FORMNAME`, exactly like `VMIS_SUPERUSER`. A checker should only need the checker inbox. Since `funcControlsUserRights` doesn't special-case checkers either, a checker who navigates directly to `Mis/frmVigilance.aspx` gets a fully-enabled entry form.

**5. A checker-only user has no menu.** `Web.sitemap`'s intermediate node (line 7-8) lists `VMIS_SUPERUSER, VMIS_ADMIN, VMIS_MISUSER, VMIS_DESKUSER, VMIS_VIEWUSER` — **`VMIS_CHECKER` is absent** — and the "Complaint" parent node likewise omits it. With security trimming on, the Checker Inbox child node is unreachable through the menu for a user whose only role is `VMIS_CHECKER`. In practice checkers get in via the Default.aspx modal or a direct URL. Add `VMIS_CHECKER` to both ancestor nodes.

**6. The dashboard modal links to a page that does not exist.** `Default.aspx:567`:

```html
<a href="ComplaintApproval.aspx" class="btn btn-primary">Review Now</a>
```

There is no `ComplaintApproval.aspx` anywhere in the project. The correct target is `Mis/frmComplaintChecker.aspx`.

**7. SSRS report config keys don't match.** Report pages read `AppSettings["report_ipaddress"]` and `AppSettings["report_serverstring"]`; `Web.config` defines `report_serverip` and `report_servername`. The `Uri` is therefore constructed from nulls and every SSRS report will throw.

**8. `Session["returnURL"]` is dereferenced without a null check.** `SiteMaster.Master.cs:29` does `Session["returnURL"].ToString()` at the top of `Page_Load`. If the session has expired but the page is still reachable, this throws `NullReferenceException` before the redirect logic can run. Likewise `SiteMaster.Master.cs:68` calls `Request.Cookies["VMISessionId"].ToString()` on a cookie that may be absent, inside the *logout* path.

**9. Cookie path app-setting name mismatch.** `Global.asax.cs` reads `UserDefinedCookiePathFilter` (defined in `Web.config`); `SiteMaster.Master.cs:69` reads `UserDefiniedCookiePathFilter` (misspelled, not defined) — so the logout cookie gets a null path.

**10. TLS certificate validation is disabled.** `RemoteServerCertificateValidationCallback` appears 13 times (SSO, HRMS, every report page) and unconditionally returns `true`. Combined with `SecurityProtocolType.Ssl3` being enabled, this defeats transport security for all outbound calls.

### 🟡 Medium

**11. `Web.config` is in development configuration.** `<compilation debug="true">`, `<customErrors mode="Off">` (stack traces shown to users), `requestValidationMode="2.0"` (weakens built-in XSS filtering), `maxRequestLength="2147483647"` (2 GB uploads), `executionTimeout="999999999"`, and connection strings pointing at `localhost\SQLEXPRESS`.

**12. Anti-XSRF protection is inert.** `SiteMaster.Master.cs` sets `Page.ViewStateUserKey` only when the cookie already exists, but the code that *creates* the cookie is commented out (lines 151-171), as is the `Page.PreLoad += master_Page_PreLoad` hook that would validate it.

**13. Swallowed exceptions.** `Login.aspx.cs:210-213` — `funcSSOLogin`'s entire body is wrapped in a `catch` with a commented-out handler, so any SSO failure fails silently with a blank page. Similar empty catches exist elsewhere.

**14. ~190 stored procedures are not in source control.** Only the schema plus two procs are in `vmismainscript.sql`. All business logic — every `_View`, `_Update`, `_Ddl`, `_Import` — lives only on the server. Script them out and commit them; without that, the repository cannot rebuild a working system.

**15. `cmd.CommandTimeout = 0` everywhere.** No query can ever time out; a bad plan on `VIGILANCE` (180 columns, dynamic SQL) will hold a worker thread indefinitely.

**16. `obj/Release/Package/PackageTmp/` is committed.** A full duplicate of every page. Easy to edit the wrong file. Should be in `.gitignore`.

**17. Duplicated form generations.** `frmSRStructure` vs `SR`, `frmWBStructure` vs `WB`, `frmRRB` vs `RRB`, `frmVigilance` vs `Vigilance`, `frmNoc` vs `NOC` — old and new versions of the same module, both wired into the menu, both writing to the same tables. Clarify which is canonical before making changes.

**18. `secretpagexyz.aspx` is deployed.** An unlinked utility page with three button handlers. Worth reviewing and removing.

---

## 19. Where to start reading

If you have an hour, read these in order:

1. `VMISP/Web.config` — providers, connection strings, auth mode
2. `VMISP/Web.sitemap` — the whole feature map with role gates, in 194 lines
3. `VMISP/Login.aspx.cs` → `funcSSOLogin` — how identity and `Session["role"]` are established
4. `VMISP/SiteMaster.Master.cs` — the per-request guard every page inherits
5. `VMISP/Code/CommonFunction.cs` → `funcCheckUserRights` + `DisableAllControls`
6. `VMISP/Mis/frmComplaint.aspx.cs` — the canonical MIS form (24 methods, all the patterns)
7. `Database/Scripts/2026-07-25_Complaint_Checker_Workflow.sql` — the newest feature, well commented
8. `VMISP/Default.aspx.cs` lines 1-200 — dashboard entry and the checker notification

Once `frmComplaint.aspx.cs` makes sense, the other 29 MIS forms are variations on it.
