# VMIS Portal — External Front-End Dependency Audit & Consolidation Plan

**Date:** 2026-07-27
**Scope:** VMISP web application (185 `.aspx` pages, 3 master pages)
**Nature of this document:** Read-only audit. No code was changed while producing this report.
**Goal:** Give an accurate map of every third-party JS/CSS dependency in the codebase, show which version each page actually loads, flag dead weight and broken references, and estimate the effort to consolidate onto one version per library without changing the visual design.

---

## 1. Executive summary

The portal has **no build system, no package manager for front-end assets, and no shared dependency version** — every page (or small template it was copy-pasted from) drags in its own copy of jQuery, Bootstrap, jQuery UI, etc. Over the years this produced:

- **9 different jQuery builds** in the tree (versions 1.4.1 → 3.4.1), **4 of them loaded in the browser today**, and on 41 pages **three different jQuery versions are loaded on the same page** (only the last one survives — the first two are pure wasted downloads).
- **6 different Bootstrap builds** (v3.0.0, v3.3.7, v3.4.1, v4.0.0, v4.0.0‑beta.2, v5.3.7‑via‑CDN), **5 of them still live** on one page or another, sometimes CSS from one major version paired with JS from another on the *same page*.
- A large `Scripts/Scripts/` and `Scripts/Highcharts-4.0.1/` vendor tree (Bootstrap 4.3.1, DataTables, Moment, SweetAlert, jsZip, pdfmake, Popper, jQuery 3.4.1 — dozens of files) that **is not referenced by a single `.aspx`, `.master`, or `.cs` file**. It appears to be a leftover admin-template drop that was never wired in.
- **Two broken/dead script references on `Login.aspx`** (the single most important page in the app) and one on `NewLogin.aspx` — paths that point at files which do not exist anywhere in the repository.
- A brand-new feature (the IAC/Complaint "Checker" Maker-Checker screens added in this same branch) pulls **Bootstrap 5.3.7 from a public CDN** (`cdn.jsdelivr.net`), which is architecturally a 5th/6th Bootstrap version and, for a banking application, an external-network dependency that the other 181 pages don't have.
- Of 185 pages, **84 pages carry page-level `<script>`/`<link>` includes**; the other **101 pages have zero page-level includes** and rely only on the sitewide `Theme1` theme (set globally in `Web.config`) plus whatever `AjaxControlToolkit`/`ScriptManager` emits automatically.

None of this is visually broken by accident today only because the pages that share a template happen to share the same CSS version. But it means **any shared-library upgrade today is not one change — it is up to 6 independent, untested upgrades**, and it's easy to introduce a mismatch (as already happened on `Login.aspx`, see §5).

---

## 2. How this map was built

- Enumerated every `.js`/`.css` file under `VMISP/` (excluding `obj`/`bin` build output) and read version banners embedded in each file (`Bootstrap vX`, `jQuery vX`, `DataTables X`, etc.) rather than trusting filenames alone.
- Searched all 185 `.aspx` files and the 3 `.master` files for `src="...js"` / `href="...css"` attributes to see what each page *actually requests*.
- Cross-referenced every inventoried vendor file's name against the whole tree (`.aspx`, `.master`, `.cs`, `.ascx`) to find files that are never requested by anything (candidates for deletion).
- Checked `Web.config` for global settings (`<pages theme="Theme1">`) that apply outside of page markup.

---

## 3. Sitewide dependency (applies to all 185 pages, invisibly)

`Web.config` sets `<pages theme="Theme1">`, so **every page** — regardless of what it includes itself — automatically gets:

- `App_Themes/Theme1/layout.css`
- `App_Themes/Theme1/style3.css`
- `App_Themes/Theme1/styles.css`
- `App_Themes/Theme1/Images/StyleSheet.css`
- `App_Themes/Theme1/Skin1.skin` (server control skin)

This is a single, non-duplicated theme — not a problem, but it's important context: it means visual identity is partly controlled here, outside any page's own `<link>` tags, and any consolidation work must not touch this folder casually.

`Site.master` / `SiteMaster.master` (the two active master pages) load **no external JS/CSS themselves** — no jQuery, no Bootstrap. All vendor-library loading happens at the individual page level, which is *why* the version sprawl below was possible in the first place.

---

## 4. Version matrix — what's actually installed vs. actually used

Status legend: 🟢 actively loaded by ≥1 page · 🟡 loaded but immediately overwritten by a later duplicate on the same page (wasted) · 🔴 present in the repo but loaded by **zero** pages (dead weight) · ⚫ referenced by a page but the **file does not exist** (broken link)

### jQuery — 9 builds found, 4 live in the browser

| Version | File(s) | Status | Used by |
|---|---|---|---|
| 1.4.1 | `Scripts/jquery-1.4.1.js`, `.min.js`, `-vsdoc.js` | 🔴 unused | none |
| 1.8.0 | `Js/jquery-1.8.0.min.js` | 🟢 active | Pattern A — 35 pages (§6) |
| 1.8.2 | `Scripts/highchart/jquery.min.js` | 🔴 unused (commented out in `Default.aspx`) | none |
| 1.9.1 | `Js/jquery-1.9.1.js` | 🟢 active (loaded **last**, so it's the one that actually wins) | Pattern B — 41 pages |
| 1.11.0 | CDN `ajax.googleapis.com/ajax/libs/jquery/1.11.0/jquery.min.js` | 🟢 active, external network dependency | `Mis/frmVigilance.aspx`, `Mis/VigilanceMonitoring.aspx` (loaded *in addition to* local 1.8.0) |
| 1.11.1 | `Scripts/highchart/jquery-1.11.1.min.js` | 🔴 unused (commented out) | none |
| 1.12.4 | `Js/jquery-1.12.4.js` | 🔴 unused | none |
| 3.1.0 | `Js/jquery.min.js` | 🟡 loaded, immediately overwritten | Pattern B — 41 pages (dead weight on every one) |
| 3.2.1 | `vendor/jquery/jquery.js`, `.min.js` | 🔴 unused | none |
| 3.3.1 | `Js/jquery-3.3.1.min.js` | 🟡 loaded first, immediately overwritten | Pattern B — 41 pages (dead weight on every one) |
| 3.4.1 | `Scripts/Scripts/jquery-3.4.1*.js` (4 variants) | 🔴 unused | none |
| 3.4.1 | `js/jquery-3.4.1.min.js` referenced by `Login.aspx` | ⚫ **broken — file does not exist** anywhere in repo | `Login.aspx` |

**Pattern B pages load jQuery 3.3.1, then 3.1.0, then 1.9.1 — in that order — on every single page load.** Only the last `<script>` tag's `jQuery` global survives, so the app is effectively running on **jQuery 1.9.1** (EOL since 2016, several known XSS/CSP-related CVEs) on 41 pages, while paying the download/parse cost of two newer builds for nothing.

### Bootstrap — 6 builds found, 5 still loaded somewhere

| Version | File(s) | Status | Used by |
|---|---|---|---|
| v3.0.0 (CSS) | `css/bootstrap.css` | 🟢 active | Pattern B — 41 pages, `Default.aspx`, `Login.aspx` |
| v3.3.7 (CSS) | `css/bootstrap.min.css` | 🔴 unused standalone; also duplicated **inside** `css/Login1.css` (embedded) | `Login1.css` copy used by `NewLogin.aspx` |
| v3.4.1 (JS) | `Js/bootstrap.js`, `.min.js` | 🟢 active | `Default.aspx` only (most Pattern-B pages load the *CSS* but never load Bootstrap's *JS* at all) |
| v4.0.0 (CSS) | `css/bootstrapnew.css` | 🔴 unused | none |
| v4.0.0-beta.2 | `vendor/bootstrap/**` (CSS+JS+Popper) | 🟢 JS only used | `Login.aspx` loads this JS **while its CSS is still Bootstrap v3.0.0** — a real version mismatch already in production |
| v4.3.1 | `Scripts/Scripts/bootstrap*.js`, `Scripts/Scripts/Vendor/vendor.bundle.base.js` | 🔴 entirely unused | none |
| v5.3.7 (CDN) | `https://cdn.jsdelivr.net/npm/bootstrap@5.3.7/...` | 🟢 active, external network dependency | New "Checker" pages only — `frmIACChecker.aspx`, `frmIACCheckerView.aspx`, `frmComplaintChecker.aspx`, `frmComplaintCheckerView.aspx` |

### jQuery UI — 4 builds found

| Version | File | Status | Used by |
|---|---|---|---|
| 1.8.24 | `Js/jquery-ui.min.js` | 🔴 unused | none |
| 1.11.0 | `Js/jquery-ui.js` | 🟢 active | Pattern B — 41 pages |
| 1.11.1 | `Scripts/highchart/jquery-ui.min.js` | 🔴 unused (commented out) | none |
| 1.12.1 (CSS) | `Js/jquery-ui.css` | 🟢 active | Pattern B — 41 pages |
| 1.12.1 (CSS) | `css/jquery-ui.css` (duplicate copy, root `css/`) | 🔴 unused | none |

### Highcharts — 3 sources

| Version | Location | Status | Used by |
|---|---|---|---|
| 4.0.1 | `Scripts/Highcharts-4.0.1/**` (~24 files: 3d, more, exporting, drilldown, funnel, heatmap, themes…) | 🔴 entirely unused | none |
| 5.0.6 | `Scripts/highchart/*.js` | 🟢 active, but only 4 of the ~9 files in the folder are actually loaded (`highcharts.js`, `data.js`, `encoder.js`, `exporting.js`) | `Default.aspx` |
| "latest" (CDN, unpinned) | `code.highcharts.com/*` | 🟢 active, external, version not pinned — can change/break with no warning | `Dashboard.aspx` |

Also present: NuGet package `DotNet.Highcharts 4.0` (server-side C# chart-builder, packages.config) — separate from the client JS copies above, not part of the version conflict, low priority.

### Font Awesome / icons — 3 versions, plus a broken reference

| Version | File | Status | Used by |
|---|---|---|---|
| 4.6.3 | `css/font-awesome.css` | 🔴 unused | none |
| 4.7.0 | `vendor/font-awesome/css/font-awesome.css`, `.min.css`; also embedded inside `css/Login1.css` | 🟢 embedded copy active | `NewLogin.aspx` (via `Login1.css`) |
| 4.7.0 | `/fonts/font-awesome-4.7.0/css/font-awesome.min.css` referenced by `Login.aspx` | ⚫ **broken — `/fonts/` folder does not exist** in repo | `Login.aspx` |
| bootstrap-icons 1.11.3 (CDN) | jsDelivr | 🟢 active, external | 4 new Checker pages |

### Other vendor libraries confirmed present but **completely unused** (zero references in any `.aspx`/`.master`/`.cs`)

- `Scripts/Scripts/DataTables/**` — 30+ DataTables plugin files (bootstrap/bootstrap4/foundation/jqueryui/semanticui skins, buttons, autoFill, colReorder, fixedColumns, rowGroup, scroller, select…)
- `Scripts/Scripts/Vendor/**` — AdminLTE-style dashboard template JS (`dashboard.js`, `todolist.js`, `off-canvas.js`, `hoverable-collapse.js`, `Chart.js`)
- `Scripts/Scripts/moment.js`, `moment-with-locales.js` (+ `.min.js` variants)
- `Scripts/Scripts/sweetalert.min.js` and `css/sweetalert.css`
- `Scripts/Scripts/jszip.js`, `Scripts/Scripts/pdfmake/**` (Excel/PDF export helpers for DataTables)
- `Scripts/Scripts/popper.js` / `popper-utils.js` / `esm/**` / `umd/**` (Popper standalone builds)
- `Scripts/Scripts/json-serialize.js`, `Scripts/Scripts/aes.js` *(note: this AES helper looks like it's what `NewLogin.aspx` **meant** to load — see §5)*
- `Scripts/Scripts/jquery.dataTables.js` and `vendor/datatables/**`
- `vendor/chart.js/**` (Chart.js, 4 files)
- `vendor/jquery-easing/**`
- `vendor/jquery/**` (jQuery 3.2.1, standalone)
- `css/bootstrapnew.css`, `css/bootstrap.min.css`, `css/custom.css`, `css/PopUp.css`, `css/monthly.css`, `css/popuo-box.css`, `css/style.css`, `css/style3.css`, `css/ssLogin.css`, `css/font-awesome.css`, `css/jquery-ui.css`
- `Js/jquery-1.12.4.js`, `Js/jquery-ui.min.js`, `Js/index.js`, `Js/prefixfree.min.js`
- `Styles/Site.css`

This is roughly **150+ individual files** — likely an entire admin-dashboard template (AdminLTE/SB Admin-style) that was dropped into `Scripts/Scripts` and `vendor/` at some point and never wired into any page.

---

## 5. Broken references (functional bugs, not just version debt)

These aren't style inconsistencies — they are `<script>`/`<link>` tags pointing at files that don't exist, so the browser gets a 404 and that feature silently doesn't run:

| Page | Broken reference | Likely intended file |
|---|---|---|
| `Login.aspx` | `/js/PVC.js` | never existed in repo — looks like leftover from the HTML template this page's `util.css`/`main.css` came from |
| `Login.aspx` | `/js/main.js` | same as above |
| `Login.aspx` | `/js/jquery-3.4.1.min.js` | `Scripts/Scripts/jquery-3.4.1.min.js` exists but at the wrong path |
| `Login.aspx` | `/js/aes.js` | `Scripts/Scripts/aes.js` exists at the wrong path |
| `Login.aspx` | `/fonts/font-awesome-4.7.0/css/font-awesome.min.css` | `vendor/font-awesome/css/font-awesome.min.css` exists at a different path |
| `Login.aspx` | `/fonts/iconic/css/material-design-iconic-font.min.css` | not present anywhere in repo |
| `NewLogin.aspx` | `/EDI/Scripts/aes.js` | `Scripts/Scripts/aes.js` exists at the wrong path |

`Login.aspx` is the page every single user hits first. It is currently loading only ~2 of its 6 non-CSS script references successfully.

---

## 6. Page-wise dependency report

84 of 185 pages carry page-level includes; they fall into four repeating patterns plus a few one-off pages. The remaining 101 pages have no page-level `<link>`/`<script>` tags at all (they get only the sitewide `Theme1` CSS from §3).

### Pattern A — "Legacy ssMain" stack (35 pages)
`../css/ssMain.css` + jQuery **1.8.0** + `JS_CommonFunction.js` + `JS_CommonValidation.js` (a few also add `MaskedEditFix.js` or, redundantly, CDN jQuery 1.11.0)

`frmChangePassword.aspx` · `Mis/frmRRB.aspx` · `Mis/VigilanceMonitoring.aspx`* · `Mis/frmVigilance.aspx`* · `Mis/frmNoc.aspx` · `Mis/frmSRStructure.aspx` · `Mis/frmWBStructure.aspx` · `Master/frmSanction.aspx` · `Admin/BranchMaster.aspx` · `Reports/frmCaseRegister.aspx` · `Admin/frmCircleHead.aspx` · `Search/frmCustomizeReports.aspx` · `Reports/frmComplaintReports.aspx` · `Reports/frmComplaintRpt.aspx` · `Reports/frmIACReports.aspx` · `Reports/frmMiscellaneousReports.aspx` · `Reports/frmMISCReports.aspx` · `Reports/frmORReports.aspx` · `Search/frmFieldWiseSearch.aspx` · `Admin/UserCreation.aspx` · `Master/frmCVOAdvice.aspx` · `Reports/frmRRBReports.aspx` · `Master/frmNatureCase.aspx` · `Reports/frmRTIReports.aspx` · `Master/frmPenaltyProceding.aspx` · `Reports/frmSRReports.aspx` · `Master/frmPenaltyType.aspx` · `Reports/frmVigilanceReports.aspx` · `Reports/frmWBReports.aspx` · `Master/frmRegister.aspx` · `Master/frmScale.aspx` · `Master/frmSourceRef.aspx` · `Master/frmStatus.aspx` · `Upload/frmAccessUpload.aspx` · `Search/frmAuditTrailSearch.aspx`

*(\* also loads redundant CDN jQuery 1.11.0 on top of local 1.8.0)*

**Consolidation impact:** low risk. No Bootstrap involved, one jQuery version, all pages share the exact same include block → safe to update in one pass with a global find/replace of the script paths.

### Pattern B — "Triple-jQuery + Bootstrap 3 + jQuery UI" stack (41 pages)
jQuery **3.3.1** (loaded, discarded) → jQuery **3.1.0** (loaded, discarded) → jQuery **1.9.1** (the one that actually runs) + jQuery UI 1.11.0/1.12.1 CSS + Bootstrap **v3.0.0** CSS (Bootstrap JS only on `Default.aspx`)

`Mis/frmABBFF.aspx` · `Mis/frmIACStructure.aspx` · `Mis/frmComplaintUpdate.aspx` · `Mis/WB.aspx` · `Mis/frmComplaint.aspx` · `Mis/Vigilance.aspx` · `Mis/frmVigilanceUpdate.aspx` · `Mis/PenaltyCharge.aspx` · `Mis/SR.aspx` · `Mis/frmOperationalRef.aspx` · `Mis/NOC.aspx` · `Mis/SanctionForProsecution.aspx` · `Mis/Lodi.aspx` · `Mis/frmMiscStructure.aspx` · `Mis/SanctionDataUpload.aspx` · `Mis/frmIACUpdate.aspx` · `Mis/frmRTI.aspx` · `Mis/RRB.aspx` · `Mis/SanctionForInvestigation.aspx` · `Mis/frmComplaintView.aspx`† · `Reports/ABBFFReport.aspx` · `Upload/frmExcelPFEODetails.aspx` · `Upload/frmExcelUpload.aspx` · `Upload/frmExcelPF.aspx` · `Master/BranchMaster.aspx` · `Master/CircleMaster.aspx` · `Master/EmailMaster.aspx` · `Search/frmEOSearch.aspx` · `Search/frmTableWiseSearch.aspx` · `Search/frmVigilanceStatusSearch.aspx` · `Search/RetirementCases.aspx`‡ · `Reports/LodiReport.aspx` · `Reports/MISCReport.aspx` · `Reports/PenaltyChargeReport.aspx` · `Reports/SanctionForInvestigationReports.aspx` · `Reports/SanctionForProsecutionReports.aspx` · `Reports/VigilanceCaseStatus.aspx` · `Reports/VigilanceMonitoringReports.aspx` · `Master/LodiDisable.aspx`‡ · `Master/ZoneChiefManager.aspx`‡ · `Default.aspx`§

*(† CSS-only variant, no page-level JS · ‡ two-jQuery variant, skips the 1.9.1 file · § also adds Bootstrap 3.4.1 JS and the Highcharts 5.0.6 local build)*

**Consolidation impact:** medium risk, highest payoff. These 41 pages are the main source of wasted downloads (2 extra full jQuery copies per page load) and the biggest chunk of "same pattern, easy to fix once."

### Pattern C — Bootstrap 5 via CDN, new "Maker-Checker" screens (4 pages)
`Mis/frmIACChecker.aspx` · `Mis/frmIACCheckerView.aspx` · `Mis/frmComplaintChecker.aspx` · `Mis/frmComplaintCheckerView.aspx`
— Bootstrap **5.3.7** + Bootstrap Icons 1.11.3, both from `cdn.jsdelivr.net`, no local copy.

**Consolidation impact:** these are the newest pages in the app and visually/behaviorally the most different (Bootstrap 5 dropped the jQuery dependency and changed data-attribute names from Bootstrap 3/4). They cannot be trivially merged into Pattern A/B's Bootstrap 3 stack without a markup rewrite — see §8, Phase 3.

### Pattern D — One-off pages
| Page | What it loads | Notes |
|---|---|---|
| `Login.aspx` | Bootstrap v3.0.0 CSS + Bootstrap v4.0.0-beta.2 JS + Font Awesome 4.7.0 (broken path) + 4 broken script refs | See §5. Highest-traffic page, highest risk. |
| `NewLogin.aspx` | `Login.css`, `Login1.css` (which embeds Bootstrap v3.3.7 and Font Awesome 4.7.0) + 1 broken script ref | Appears to be a newer/alternate login page than `Login.aspx` — worth confirming with the team which one is actually in use before consolidating either. |
| `LoginSSO.aspx` | `normalize.css`, `styles.css` only | No jQuery/Bootstrap at all; simplest page in the app. |
| `Dashboard.aspx` | Highcharts, unpinned "latest" from CDN | Different chart engine version than `Default.aspx`'s local 5.0.6 copy; CDN version can drift without notice. |

### The other 101 pages
No page-level `<script>`/`<link>` tags — they render using only the sitewide `Theme1` CSS (§3) and whatever `AjaxControlToolkit`/`ScriptManager` (via `Site.master`) emits automatically. These pages are **not affected** by the jQuery/Bootstrap consolidation work at all, which is good news for scope: the real work is concentrated in the 84 pages above.

---

## 7. Recommended target stack

Given the constraint "don't change much of the design," the pragmatic target is **not** the newest version of everything — it's the version that's already the majority default, since that's what the existing CSS overrides in `css/custom.css`-style files and inline styles were tuned against:

| Library | Recommended single version | Rationale |
|---|---|---|
| jQuery | **1.9.1** (Pattern B's effective winner) or **1.8.0** (Pattern A) — pick one and standardize both patterns onto it | These are what's *actually* executing today across 76 of the 84 pages; jumping straight to jQuery 3.x risks breaking the many `.live()`/`.browser`/AjaxControlToolkit-era calls this codebase likely still uses. Recommend a follow-up grep for deprecated jQuery APIs before deciding 1.9.1 vs. a newer 1.x. |
| Bootstrap | **v3.0.0/3.3.7 CSS family** (already what 76 of 84 pages visually render against) | Keeps current design pixel-identical. Bootstrap 4/5 change grid classes (`col-xs-*` → `col-*`), so upgrading Bootstrap version *is* a design change — explicitly out of scope per your instruction. |
| jQuery UI | **1.11.0/1.12.1** (already what Pattern B uses) | No change needed, just needs to stop shipping the 3 unused copies. |
| Bootstrap 5 (Checker pages) | **Leave isolated** for now, or backport those 4 pages to Bootstrap 3 in a dedicated follow-up | Forcing Bootstrap 5 pages back to 3, or forcing the other 181 pages up to 5, are both real redesign efforts — flagged separately in §8 Phase 3, not bundled into the "no design change" consolidation. |
| Highcharts | **5.0.6** local copy (already what `Default.aspx` uses) | Pin `Dashboard.aspx` to the same local copy instead of CDN "latest" — removes an external dependency and a silent-break risk. |
| Font Awesome | **4.7.0** | Already the majority version where FA is used at all. |

---

## 8. Engineering effort estimate

Effort assumes: one developer, no automated visual-regression tooling in place (none was found in the repo), manual page-by-page smoke test after each phase, IIS/ASP.NET WebForms constraints (no bundler, so this is done via literal `<script>`/`<link>` tag edits).

### Phase 0 — Safety net (0.5–1 day)
- Take dated screenshots of every distinct page pattern (1 per pattern group, ~8 pages) before touching anything, as your visual baseline.
- Fix the confirmed broken references on `Login.aspx` and `NewLogin.aspx` first (§5) — these are pure bugs, zero design risk, and should not be bundled with the version-consolidation risk below.

### Phase 1 — Delete confirmed dead weight (0.5 day)
- Remove the ~150 unused files identified in §4/§6: `Scripts/Highcharts-4.0.1/`, `Scripts/Scripts/` (DataTables/Vendor/moment/sweetalert/jszip/pdfmake/popper/jquery-3.4.1/bootstrap-4.3.1), `vendor/chart.js`, `vendor/datatables`, `vendor/jquery-easing`, `vendor/jquery`, unused `css/*.css`, unused `Js/*.js`.
- Zero functional risk — nothing references them — but do it as its own commit so it's trivially revertible, and grep once more immediately before deleting in case something loads them dynamically from C# (`ClientScript.RegisterClientScriptInclude`, string-built paths) rather than markup. This audit checked `.cs`/`.ascx` for literal filename matches but a last-mile check is cheap insurance.

### Phase 2 — Collapse Pattern A (35 pages) onto one jQuery build (1–1.5 days)
- All 35 pages already share an identical include block → scripted find/replace across the 35 files, then spot-check ~5 representative pages (one per subfolder: `Mis`, `Reports`, `Master`, `Search`, `Admin`) in-browser.
- Low risk: single library, no Bootstrap involved.

### Phase 3 — Collapse Pattern B (41 pages) from 3 jQuery copies to 1 (2–3 days)
- Remove the two dead jQuery loads (3.3.1, 3.1.0) from all 41 pages, keep 1.9.1 — this alone is a pure performance win with **no visual risk** (the other two were never the active jQuery anyway).
- Standardize the Bootstrap CSS reference (`css/bootstrap.css` v3.0.0) — already consistent across the group, just needs the handful of variant pages (`LodiDisable`, `ZoneChiefManager`, `RetirementCases`, `frmComplaintView`) brought in line.
- Test each of the 41 pages' interactive JS (any DataTables/date-pickers/modals) since removing the discarded jQuery copies is safe, but confirm no page was accidentally depending on the *load order side-effect* (e.g. a plugin that only checks `if (typeof jQuery...)` once at parse time). Budget extra time here for the handful of pages with visible client-side interactivity beyond form postbacks.

### Phase 4 — `Login.aspx` / `NewLogin.aspx` reconciliation (1–2 days, needs a product decision)
- Determine with the team which of `Login.aspx` / `NewLogin.aspx` / `LoginSSO.aspx` is the actual production entry point — three separate login pages with three separate stacks is itself worth resolving before spending effort consolidating all three.
- Fix Bootstrap CSS/JS version mismatch on `Login.aspx` (currently v3.0.0 CSS + v4.0.0-beta.2 JS).

### Phase 5 — Pin `Dashboard.aspx` off the CDN (0.5 day)
- Point it at the local `Scripts/highchart` (5.0.6) copy instead of unpinned `code.highcharts.com`, matching `Default.aspx`. Removes an external runtime dependency for a banking application and a "CDN changes, chart silently renders differently" risk.

### Phase 6 (separate track, real design work — not "no design change") — Bootstrap 5 Checker pages
- The 4 new pages are the only ones on Bootstrap 5 and are architecturally incompatible with the Bootstrap 3 stack the rest of the app uses (different grid class names, no jQuery dependency, different JS component API). Bringing everything to one Bootstrap version means either:
  - (a) backport 4 pages to Bootstrap 3 — smaller effort (~2–3 days, 4 pages), keeps the other 181 pages untouched, **recommended** given your "don't change the design much" constraint, or
  - (b) upgrade 181 pages to Bootstrap 5 — this **is** a redesign (grid system, form controls, and every custom CSS override built against Bootstrap 3 would need re-validation) — estimate weeks, not days, and explicitly out of scope for a "consolidate without changing design" effort.
- Recommend treating this as its own project decision, not part of the consolidation estimate below.

### Total estimate (Phases 0–5, i.e. the "consolidate without changing design" scope)

| Phase | Effort |
|---|---|
| 0 — Safety net + fix broken links | 0.5–1 day |
| 1 — Delete dead weight | 0.5 day |
| 2 — Pattern A jQuery consolidation | 1–1.5 days |
| 3 — Pattern B triple-jQuery cleanup | 2–3 days |
| 4 — Login pages reconciliation | 1–2 days |
| 5 — Dashboard CDN pin | 0.5 day |
| **Total** | **~5.5–8.5 developer-days**, plus a decision from the team on Login page consolidation and whether Phase 6 (Bootstrap 5 pages) is in scope |

This excludes Phase 6, which is a separate, larger, genuinely-design-affecting effort and should be scoped independently once a direction (backport vs. upgrade) is chosen.

---

## 9. Quick-reference: what's safe to delete right now

Zero references found anywhere in `.aspx` / `.master` / `.cs` / `.ascx`:

```
Scripts/Highcharts-4.0.1/               (entire folder — 24 files)
Scripts/Scripts/DataTables/             (entire folder — 30+ files)
Scripts/Scripts/Vendor/                 (entire folder)
Scripts/Scripts/bootstrap*.js(.map)     (v4.3.1 build)
Scripts/Scripts/jquery-3.4.1*.js        (4 variants)
Scripts/Scripts/moment*.js
Scripts/Scripts/sweetalert.min.js
Scripts/Scripts/jszip.js / .min.js
Scripts/Scripts/pdfmake/
Scripts/Scripts/popper*.js, esm/, umd/
Scripts/Scripts/json-serialize.js
Scripts/Scripts/jquery.dataTables.js(.min).js
Scripts/jquery-1.4.1*.js
vendor/chart.js/
vendor/datatables/
vendor/jquery-easing/
vendor/jquery/
css/bootstrapnew.css
css/bootstrap.min.css
css/custom.css
css/PopUp.css
css/monthly.css
css/popuo-box.css
css/style.css
css/style3.css
css/ssLogin.css
css/font-awesome.css
css/jquery-ui.css
css/sweetalert.css
Js/jquery-1.12.4.js
Js/jquery-ui.min.js
Js/index.js
Js/prefixfree.min.js
Styles/Site.css
```

*(Note: `Scripts/Scripts/aes.js` is referenced by name in two broken paths — see §5 — so before deleting it, first decide whether to fix `NewLogin.aspx`/`Login.aspx` to point at it correctly.)*

Before actually deleting anything, re-run a repo-wide search for each filename immediately prior to removal (last-mile safety check for dynamic/string-built references from code-behind), and remove in its own commit separate from any consolidation work so it's trivially revertible.
