# Project State
- Last updated: 2026-10-31
- Current stage: V0 — Walking skeleton
- Current task: D0-05 — Inventory/hold/order/payment/ticket Domain types (not started; wait for approval)
- Branch / commit: master / D0-04 pending commit
- Last accepted stage tag: none

## Working state
- Build: D0-04 solution build passed with .NET SDK 10.0.403; zero warnings and errors.
- Unit tests: D0-04 solution test run passed (15 total; 12 Domain behavior tests and 3 template smoke tests).
- Integration tests: Project discovery passed; current tests are template smoke tests, not database integration tests.
- Compose: Not created; deferred to D0-07.
- Current demo path: None; no business endpoints or UI exist yet.

## Completed tasks
- [x] D0-01 — Repo conventions, project state/log and templates.
- [x] D0-02 — Solution and project skeleton; project reference directions verified.
- [x] D0-02A — Day-one CI and public-repository hygiene; GitHub checks and failure exercise verified.
- [x] D0-03 — ADR-0001/0002; learner accepted the architecture and data conventions.
- [x] D0-04 — Identity, venue and catalog Domain types; focused invariant tests passed.
- [x] DOC-01 — Phase and task roadmap index; 102 backlog rows counted.

## Active blockers
- None.

## Open decisions
- D0-03 architecture and ID/time/money/enum decisions are accepted.
- Venue layout snapshot policy is open. Resolve before designing show inventory: should existing shows keep a frozen layout when a venue changes, or reflect the current venue layout?
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

## Next single task (not started)
- ID: D0-05 — Inventory/hold/order/payment/ticket Domain types.
- Acceptance criteria: legal states and transition behavior captured and tested.
- Scope: inventory, hold, order, payment and ticket Domain types and focused tests only. Resolve the open venue-layout snapshot decision before choosing how show inventory relates to venue seats.

