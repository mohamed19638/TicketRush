## What changed and why?

## Related issue
Closes #

## Design / behavior
- API or message contract changes:
- Schema/migration:
- Transaction and concurrency:
- Authorization/data scope:

## Verification
- [ ] Build/format/analyzers pass
- [ ] Unit tests pass
- [ ] Relevant integration tests pass against real containers
- [ ] Concurrency/idempotency tests added where needed
- [ ] Invariant checker clean
- [ ] Experiment attached if performance is claimed
- [ ] README/API docs/PROJECT_STATE updated

## Risk and rollback
- Risk:
- Feature flag / rollback:
- Migration expand/contract plan:

## Self-review / AI review
- [ ] No business SQL/external I/O hidden in controller
- [ ] Transactions reviewed
- [ ] Races/retries/duplicate behavior reviewed
- [ ] Authorization/IDOR reviewed
- [ ] Secrets/PII are not logged
- [ ] Errors expose no internals
- [ ] Metrics avoid unbounded cardinality
