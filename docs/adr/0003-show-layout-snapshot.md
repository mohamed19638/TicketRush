# ADR-0003: Freeze Venue Layout for Each Show
- Status: Accepted
- Date: 2026-10-31
- Decision owner: Learner; AI is advisory
- Related task: D0-04

## Context
Venues have reusable, editable sections and seats. A show has its own inventory, and later venue edits must not silently change the seats offered for an existing show. The project plan requires this policy to be decided before V0 is complete.

## Decision drivers
Stable inventory identity, predictable organizer edits, preserving the meaning of existing orders and tickets, and keeping the initial implementation understandable.

## Options
1. **Read the current Venue layout for every Show** — fewer copied records, but venue edits can unexpectedly add, remove or rename seats in existing shows.
2. **Freeze a layout snapshot when a Show is created** — existing shows keep the seat layout they were created with; later venue edits apply only to future shows. Requires storing show-specific inventory/layout data.

## Decision
Choose option 2. When a Show is created, its venue seat layout is copied into show-specific inventory. Existing shows keep their snapshot when the Venue layout changes. The snapshot records the seat identity and display data needed to preserve the Show's layout; exact database columns and constraints are defined with the D0-05 inventory types and D0-06 mappings.

## Consequences
- Existing Show inventory is stable even if an organizer edits the reusable Venue layout.
- New Shows use the latest Venue layout at their creation time.
- Snapshot rows consume additional database storage and need an explicit generation/validation flow.
- Venue ownership and authorization still apply; a snapshot is not authorization evidence.

## Failure modes and rollback
- If snapshot generation is partial, do not expose the Show inventory as complete; generation must be made atomic in its persistence task.
- Do not update old Show snapshots implicitly when Venue seats change. Any exceptional correction to an existing Show requires an explicit, audited workflow that preserves sold/held inventory semantics.
- If copied layout data causes excessive write or storage cost, measure it and propose a superseding ADR with a migration and compatibility plan.

## Revisit when
Measured storage/write costs or a concrete organizer workflow requires updating a Show's layout after creation.
