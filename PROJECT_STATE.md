# Project State
- Last updated: 2026-10-31
- Current stage: V0 — Walking skeleton
- Current task: D0-03 — ADR-0001/0002 (proposed drafts ready for learner review)
- Branch / commit: master / docs(adr): propose architecture and data conventions (learner review pending)
- Last accepted stage tag: none

## Working state
- Build: Passed with .NET SDK 10.0.403; zero warnings and errors.
- Unit tests: Passed; one template smoke test in each unit-test project.
- Integration tests: Project discovery passed; current tests are template smoke tests, not database integration tests.
- Compose: Not created; deferred to D0-07.
- Current demo path: None; no business endpoints or UI exist yet.

## Completed tasks
- [x] D0-01 — Repo conventions, project state/log and templates.
- [x] D0-02 — Solution and project skeleton; project reference directions verified.
- [x] D0-02A — Day-one CI and public-repository hygiene; GitHub checks and failure exercise verified.
- [x] DOC-01 — Phase and task roadmap index; 102 backlog rows counted.

## Active blockers
- None.

## Open decisions
- Decision needed: Accept or revise proposed ADR-0001/0002 decisions before D0-04.
- Recommended defaults: Modular monolith; UUID IDs; UTC `timestamptz`; integer minor units; string-backed enum storage.
- Dependent task: D0-04.
- Test framework: xUnit is present as the initial test-project template choice; revisit if project ADRs require a different standard.

## Known defects / limitations
- Behavior: API and Worker are empty host scaffolds; no TicketRush business behavior exists.
- Reproduction: Not applicable.
- Risk: Template tests prove test discovery only, not application correctness or PostgreSQL semantics.

## Feature flags
| Flag | Default | Purpose | Rollback |
|---|---|---|---|

## Latest experiments
| ID | Variant | Result | Report |
|---|---|---|---|

## Current task gate (review pending)
- ID: D0-03 — ADR-0001/0002.
- Proposed artifacts: `docs/adr/0001-modular-monolith-and-dependency-boundaries.md`, `docs/adr/0002-ids-time-money-and-enum-storage.md`.
- Gate: Learner reviews alternatives and accepts or revises both decisions; no domain/application code is in scope.

