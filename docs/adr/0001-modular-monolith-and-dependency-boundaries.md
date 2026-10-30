# ADR-0001: Modular Monolith and Dependency Boundaries
- Status: Proposed
- Date: 2026-10-31
- Decision owner: Learner; AI is advisory
- Related task/experiment: D0-03

## Context
TicketRush is an early-stage ticket-booking learning project. Booking, inventory, orders and payments need clear ownership and reliable PostgreSQL transactions. The repository already has separate Domain, Application, Infrastructure, API and Worker projects. The project plan calls for a modular monolith, not microservices.

## Decision drivers
Correctness of transactional booking flows, understandable module boundaries, low local-development and operations cost, testability, and the ability to measure before adding distributed infrastructure.

## Options
1. **Microservices from the start** — independent deployments and scaling; adds network failure modes, distributed transactions, service discovery and deployment work before independent scaling needs exist.
2. **One unstructured application** — fewest projects initially; business rules, persistence and transport can become tangled, making tests and ownership unclear.
3. **Modular monolith with layered projects** — one solution and PostgreSQL source of truth, explicit business modules, and separate API/Worker hosts; requires discipline to preserve project and module boundaries.

## Decision
Propose option 3.

- Keep one business codebase and PostgreSQL database as the authoritative store.
- Keep `TicketRush.Api` and `TicketRush.Worker` as separate executable hosts because HTTP and background jobs have different lifecycles. They share Application and Domain behavior and use Infrastructure adapters.
- Preserve these project references: Domain depends only on BCL/domain-only packages; Application depends on Domain; Infrastructure depends on Application and Domain; API and Worker compose Application with Infrastructure.
- Organize business code into explicit modules inside the existing projects. Modules own their DTOs. Do not add a `TicketRush.Contracts` project or a broad `Common` bucket without a demonstrated need.
- Add Redis, RabbitMQ, SignalR and other distributed components only in their planned tasks and behind the corresponding decision/experiment.

## Consequences
- Benefits: Core booking changes can use ordinary PostgreSQL transactions; one local environment is easier to run; API and Worker can reuse the same use cases; project references make some boundary violations visible at build time.
- Costs/added complexity: Modules still share a process and database, so developers must avoid cross-module shortcuts and keep transaction ownership explicit.
- Operational implications: API and Worker can be deployed as separate processes, but this does not make them separate microservices. No independent service database or message broker is required for the initial slice.

## Failure modes and rollback
- If modules directly reach into each other's persistence or copy rules, restore boundaries with module-owned Application use cases and tests; keep controllers and consumers as adapters.
- If the layered projects make a small change harder to understand, simplify within the existing boundaries rather than introducing more layers or mediator/framework packages.
- If a later module has measured independent scaling, deployment, security or ownership needs, propose a separate extraction ADR. Do not split the current system as a speculative optimization.

## Revisit when
Measured deployment/scale constraints, team ownership boundaries, or repeated cross-module transaction coupling show that this shape is limiting the project.
