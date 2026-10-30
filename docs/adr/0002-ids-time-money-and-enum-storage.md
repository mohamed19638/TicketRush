# ADR-0002: IDs, Time, Money and Enum Storage
- Status: Accepted
- Date: 2026-10-31
- Decision owner: Learner; AI is advisory
- Related task/experiment: D0-03

## Context
The project plan requires UUIDs for externally addressed identifiers unless justified otherwise, UTC instants, integer minor units for money, an explicit currency allowlist, and a deliberate enum-persistence choice. These rules must be consistent before entities and database mappings are added.

## Decision drivers
Correctness across machines and time zones, safe monetary arithmetic, understandable stored values, simple EF Core/Npgsql mappings, explicit database constraints, and avoiding new packages without a demonstrated benefit.

## Options
1. **Integer IDs** — compact and naturally ordered; expose sequential identifiers and require a migration strategy if identifiers are later distributed across systems.
2. **UUID IDs** — larger and less naturally ordered, but suitable for external references without relying on a central sequence. UUIDs still do not provide authorization.
3. **Local timestamps or floating-point/decimal currency amounts** — familiar in some APIs; creates timezone ambiguity or rounding/scale questions that the plan explicitly seeks to prevent.
4. **Integer enum ordinals** — compact; stored meanings can silently change if enum numbers are reordered or reused.
5. **String enum values or PostgreSQL native enums** — readable and stable when managed deliberately; strings require validation, while native enum schema changes add migration/provider-specific work.

## Decision
Adopt the following initial conventions.

### IDs
- Use PostgreSQL `uuid` columns and .NET `Guid` for externally addressed entity IDs. Generate IDs in the application with `Guid.NewGuid()` unless an approved later decision changes the generator.
- Treat every ID as an identifier, never as proof of ownership or authorization. Every query/action still applies role and resource-ownership checks.
- Accept the UUID index-locality trade-off for the initial workload; consider ordered UUIDs only after measuring a relevant insert/index bottleneck.

### Time
- Model event instants as UTC `DateTime` values (`Kind=Utc`) and persist them as PostgreSQL `timestamp with time zone` (`timestamptz`). API timestamps use ISO 8601 UTC (`Z`). Do not store local machine time or an implicit local `DateTime` as an instant.
- Store a venue timezone identifier separately for display and interpreting venue-local schedules; `timestamptz` preserves the instant, not the original timezone name.
- Use the BCL UTC `DateTime` mapping initially instead of adding NodaTime. Npgsql maps UTC `DateTime` to `timestamptz`; it rejects non-UTC `DateTime` values for that column. Revisit a stronger time library if conversion mistakes or richer calendar rules become material.

### Money and currency
- Store prices and totals as signed 64-bit integer minor units (`long` in .NET, `bigint` in PostgreSQL); reject negative values where the business rule requires nonnegative money. Never use `float` or `double` for money.
- Store currency explicitly using a three-letter code and begin with the plan's allowlist (`EGP`, `USD`, `EUR`). A show/order uses one currency; do not perform FX conversion.
- Keep fee/percentage rounding policy explicit in the task that introduces fees; this ADR does not invent a rounding rule. Check supported business maxima before defining database constraints.

### Enum persistence
- Persist status/type enums as stable string values rather than numeric ordinals. Configure EF Core conversions explicitly and add database validation constraints with the relevant schema task.
- Treat persisted strings as data contracts: renaming/removing a value requires an explicit data migration. Do not rely on enum declaration order.
- Prefer strings over PostgreSQL native enum types initially to keep schema evolution straightforward; revisit if measured storage/query needs justify provider-specific enum management.

## Consequences
- Benefits: IDs are consistent between .NET and PostgreSQL; instants have one unambiguous representation; money arithmetic stays exact in minor units; persisted states are readable during SQL inspection and incident review.
- Costs/added complexity: UUIDs use more index space than integer IDs; UTC kind must be maintained; integer money requires explicit currency/scale; string states take more storage and need DB constraints plus migration discipline.
- Operational implications: Local display converts UTC instants using the venue's timezone. Currency allowlist and persisted state values must be reviewed when added or changed. Npgsql's timestamp mapping and EF Core enum conversion behavior are documented by their official references below.

## Failure modes and rollback
- Reject or normalize non-UTC instants at application boundaries before persistence; never silently reinterpret a local time as UTC.
- Guard money ranges and calculate in checked integer arithmetic. Define and test fee rounding before implementing fees.
- Validate enum values in application/domain logic and in database constraints. When a value changes, deploy a migration that accounts for existing rows before removing the old representation.
- If a future measured workload justifies ordered IDs, NodaTime, PostgreSQL native enums or another representation, write a migration/compatibility plan in a superseding ADR before changing stored data.

## Revisit when
Measured index pressure, multi-currency requirements, richer venue-local calendar rules, or operational experience shows the conventions create meaningful cost.

## References
- [Npgsql date and time handling](https://www.npgsql.org/doc/types/datetime.html)
- [EF Core value conversions](https://learn.microsoft.com/en-us/ef/core/modeling/value-conversions)
