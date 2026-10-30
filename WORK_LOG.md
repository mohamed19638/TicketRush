# Work Log

## 2026-10-31 — D0-01 — Establish project working agreements
### Goal
Create the repository operating contract and task templates before application code.

### What I learned
The repository was empty apart from Git metadata. The full plan is the source of truth; this task is documentation-only.

### Changes made
- Files: `AGENTS.md`, `PROJECT_STATE.md`, `WORK_LOG.md`, `.github/ISSUE_TEMPLATE/task.md`, `.github/pull_request_template.md`, `docs/TicketRush_Full_Project_Plan.md`.
- Decisions: No application code or D0-02 work is included.

### Verification
- Command: SHA256 source/copy comparison, recursive file inventory, application-code file count, and Git status review.
- Result: Passed. Plan copy hash matches the source (2,183 lines); exactly six task files exist; no C#, project, SQL, or Docker application files were created.
- Environment/seed: Local working tree; no application environment.

### Break-it exercise
- Guard changed: Not applicable to this documentation-only task.
- Expected failure: Not applicable.
- Observed result: Not applicable.

### Questions/limitations
- No build or application tests apply because no application code exists yet.

### Commit
- `docs: establish project working agreements`

### Next task (not started)
- D0-02 — Solution and project skeleton.

## 2026-10-31 — D0-02 — Scaffold solution and project boundaries
### Goal
Create the .NET 10 solution and the planned application, host, and test project skeletons with dependency directions enforced by project references.

### What I learned
The installed SDK is 10.0.403 with .NET 10.0.14 runtime. Domain must remain independent; adding a Domain-to-Infrastructure reference creates a project dependency cycle.

### Changes made
- Files: `TicketRush.sln`; projects under `src/` for Domain, Application, Infrastructure, Api, and Worker; projects under `tests/` for Testing, Domain.UnitTests, Application.UnitTests, Api.IntegrationTests, and Infrastructure.IntegrationTests.
- Decisions: Used the SDK xUnit template for the initial test projects. No business entities, endpoints, persistence, migrations, Docker Compose, or CI were added.

### Verification
- Command: `dotnet build TicketRush.sln`; `dotnet test TicketRush.sln --no-build`; `dotnet sln TicketRush.sln list`; inspect all project references; temporarily add the forbidden Domain-to-Infrastructure reference, build, remove it, and rebuild.
- Result: Build passed with 0 warnings/errors. All four test projects discovered and passed 1 template smoke test each. The forbidden reference caused MSB4006 circular dependency; it was removed, Domain has no project references, and the final build passed.
- Environment/seed: Windows x64; .NET SDK 10.0.403, runtime 10.0.14; no database or containers used.

### Break-it exercise
- Guard changed: Temporarily added Domain -> Infrastructure project reference.
- Expected failure: Circular dependency violates the architecture direction.
- Observed result: Build failed with MSB4006; reference removed and build passed again.

### Questions/limitations
- Current test contents are SDK template smoke tests only; no business behavior or database semantics have been tested.
- Day-one CI/public-repository hygiene is D0-02A and remains unstarted.

### Commit
- `chore(solution): scaffold project structure`

### Next task (not started)
- D0-02A — Day-one CI and public-repository hygiene.

## 2026-10-31 — D0-02A — Day-one CI and public-repository hygiene
### Goal
Add day-one GitHub Actions checks for .NET restore/build/format/tests, exclude local and generated files, provide safe local configuration guidance, and scan for secrets.

### What I learned
The first GitHub run exposed that the Gitleaks Action builds an invalid scan range when the previous commit is the repository root. Scanning the checked-out full Git history with the official CLI container handles the initial push correctly.

### Changes made
- Files: `.github/workflows/ci.yml`, `.gitignore`, `.env.example`, `PROJECT_STATE.md`, `WORK_LOG.md`.
- Decisions: CI uses .NET 10 and scans full repository history with pinned Gitleaks CLI v8.30.1. `.env.example` contains only local placeholders and is explicitly not ignored. No app secrets or personal data were added. The repository is public at https://github.com/mohamed19638/TicketRush.

### Verification
- Commands: `dotnet restore TicketRush.sln`; `dotnet format TicketRush.sln --verify-no-changes --no-restore`; `dotnet build TicketRush.sln --configuration Release --no-restore`; `dotnet test TicketRush.sln --configuration Release --no-build --no-restore`; Gitleaks v8.30.1 directory and Git-history scans in Docker; `git check-ignore`; GitHub Actions checks on `master` and on the temporary break-it branch.
- Result: All normal .NET commands exited 0. Build had 0 warnings and errors; all four current test projects passed one SDK template smoke test each. Gitleaks found no leaks locally or in the successful remote history scan. GitHub Actions run #2 on `9eb03aa` succeeded end-to-end (restore, format, build, tests, full-history secret scan). The `.gitignore` checks passed: generated/user-specific paths and `.env` are ignored; `.env.example` remains trackable.
- Break-it result: On temporary `codex/ci-fail-check`, format and build passed, local tests exited 1 as expected, and GitHub Actions run #3 failed at the `Test` step with exit code 1. The temporary branch was deleted locally and remotely; `master` remains clean.

### Break-it exercise
- Guard changed: Replaced a template test body temporarily with `Assert.Equal(1, 2)` on `codex/ci-fail-check`.
- Expected failure: An intentionally failing test should make the GitHub Actions check fail and prevent a green status.
- Observed result: Confirmed both locally and in GitHub Actions run #3; the workflow status was Failure at `dotnet test`. This verifies the CI check goes red; branch protection requiring that check has not been configured.

### Questions/limitations
- GitHub run #1 failed in the Gitleaks Action because its initial-push range referenced the nonexistent parent of the root commit; it did not report any secret. Replaced the wrapper with pinned Gitleaks CLI v8.30.1; run #2 passed.

### Commit
- Commits: `65a6750 ci: add day-one build and secret scan`; `9eb03aa fix(ci): scan full history on initial push`.

### Next task (not started)
- D0-03 — ADR-0001/0002.

### Remote CI follow-up
- First GitHub run: Restore, format verification, build and test steps completed; Gitleaks Action failed because it formed an invalid `root^..HEAD` range on the first push, where the root commit has no parent.
- Fix: Replaced the Action wrapper with the pinned official Gitleaks CLI container scanning the checked-out full Git history. Kept output redacted and reduced workflow token permissions to repository contents read-only.
- Verification: GitHub Actions run #2 passed; final `master` run #4 passed; run #3 rejected the intentional failing test. Break-it branch was removed after verification.

## 2026-10-31 — D0-03 — Architecture and data-representation ADRs
### Goal
Record V0 architecture boundaries and the initial ID/time/money/enum persistence conventions, with alternatives and trade-offs for learner review.

### What I learned
PostgreSQL `timestamptz` stores a UTC instant, not the venue's timezone name. Npgsql maps UTC `DateTime` values to `timestamptz` and rejects non-UTC values for that mapping. EF Core supports explicit enum-to-string value conversion.

### Changes made
- Files: `docs/adr/0001-modular-monolith-and-dependency-boundaries.md`, `docs/adr/0002-ids-time-money-and-enum-storage.md`, `PROJECT_STATE.md`, `WORK_LOG.md`.
- Decisions accepted by learner: Modular monolith; UUID/Guid IDs; UTC DateTime plus `timestamptz` and separate Venue timezone; integer minor units; stable string enum storage with database constraints.
- No application code, EF mappings, entities or migrations were added in D0-03.

### Verification
- Commands: `git diff --check`; manual review against plan sections A3, B1-B3, B10 and F5; verify each ADR contains status, context, options, decision, consequences, failure modes, revisit criteria and references.
- Result: Passed. `git diff --check` was clean; both ADRs contain the required sections and references. Learner selected the plan-aligned options via the decision prompts; ADR statuses updated to Accepted. No application or migration files changed.

### Break-it exercise
- Guard changed: Not applicable to documentation-only decisions.
- Expected failure: Not applicable.
- Observed result: Not applicable.

### Questions/limitations
- Accepted conventions are initial defaults and may be superseded only through a later reviewed ADR and a data-migration plan where stored representations change.

### Commit
- `docs(adr): propose architecture and data conventions`; updated to Accepted after learner selection.

### Next task (not started)
- D0-04 — Identity/venue/catalog Domain types; approved and not started.

## 2026-10-31 — DOC-01 — Add phase and task roadmap index
### Goal
Create a concise Arabic guide to all phases and backlog tasks for learner orientation.

### What I learned
The plan contains 102 task rows across 14 phases when D0-02A is counted, while its summary line says 95. Excluding D0-02A still yields 101 rows. The source plan was left unchanged.

### Changes made
- Files: `docs/TicketRush_Roadmap.md`, `PROJECT_STATE.md`, `WORK_LOG.md`.
- Decisions: Kept canonical task IDs and titles; added short Arabic descriptions and stage-gate summaries.

### Verification
- Command: Parse task rows from the full plan; compare phase counts and task IDs to the roadmap; `git diff --check`.
- Result: Passed. Source backlog and roadmap each contain 102 task IDs; all IDs are unique and matched with no missing or extra rows. Phase counts match the roadmap headers. `git diff --check` passed.
- Environment/seed: Documentation-only; no application commands.

### Break-it exercise
- Guard changed: Not applicable to this documentation task.
- Expected failure: Not applicable.
- Observed result: Not applicable.

### Questions/limitations
- The 95-versus-102 discrepancy remains in the source plan and should be reconciled before backlog freeze.

### Commit
- `docs(roadmap): add phase and task index`

### Next task (not started)
- D0-02A — Day-one CI and public-repository hygiene.

