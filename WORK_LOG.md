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

