# AGENTS.md — TicketRush

## Read first
- `docs/TicketRush_Full_Project_Plan.md`
- `PROJECT_STATE.md`
- `WORK_LOG.md`
- Current approved task / GitHub Issue

## Product and stack
- TicketRush is a sandbox ticket booking platform, not a real payment service.
- Use .NET 10 LTS, ASP.NET Core, EF Core and PostgreSQL. Introduce Redis/RabbitMQ/SignalR only in their planned stages.
- PostgreSQL is the source of truth for inventory, holds, orders, payments and tickets.
- No microservices/new dependencies without an approved task and ADR.
- UI is plain HTML/CSS/JavaScript; do not turn it into a frontend project.

## Operating rules
1. Work on exactly one approved task.
2. Do not implement future stages, stretch features or adjacent refactors.
3. Before code, explain the concept in Egyptian Arabic, why it exists, failure modes and acceptance criteria.
4. State files to touch and transaction/security boundaries first.
5. Report only tests/commands actually run.
6. Explain the diff file-by-file, including assumptions and failure modes.
7. Update `PROJECT_STATE.md` and `WORK_LOG.md` after each task.
8. Stop and wait for user approval before the next task.

## Architecture
- Domain has no EF Core/ASP.NET/Infrastructure/Redis/RabbitMQ dependency.
- Application depends on Domain only.
- Infrastructure implements ports/adapters for Application/Domain.
- API controllers stay thin; business rules belong in Domain/Application.
- Worker consumers reuse application use cases, not duplicated rules.
- Never trust client UserId, role, price, total or inventory state.
- Use explicit request DTOs, not tracked entities as request models.

## Correctness
- Enforce invariants in PostgreSQL wherever possible and transactionalize related effects.
- Never decide inventory sellability from cache, SignalR, Redis TTL, a client value or prior SELECT alone.
- Checkout/payment require idempotency and race tests.
- Treat payment timeout as Unknown unless simulator proves decline.
- Outbox/inbox must handle duplicate messages; assume at-least-once delivery.
- ACK only after durable consumer work commits.
- Expiry worker is cleanup; checkout checks expiry independently.

## Quality and security
- Add a test that fails if the critical guard is removed.
- Use real disposable PostgreSQL/Redis/RabbitMQ containers for their semantics; don't claim DB-lock correctness from an in-memory fake.
- Bound inputs, quantity, page sizes and payloads.
- Store UTC instants and integer minor units for money.
- Never log password, raw JWT/refresh token, secret or card data.
- Return safe Problem Details, never stack trace/SQL internals.
- Never use real payments, real card data or actual email delivery.

## Language
- Explanations/docs: Egyptian Arabic, technical terms remain English.
- Code, identifiers, SQL, config, comments, commit messages and Mermaid labels: English only.
- Keep comments sparse and useful; no Arabic in any code block/comment.

## Required response for each task
1. Concept explanation.
2. Scope/files to touch.
3. Acceptance criteria and verification commands.
4. Diff summary and critical lines.
5. Actual test results.
6. Unproven behavior/failure modes.
7. Project state/log update and proposed commit.
8. Stop and wait for user approval.
