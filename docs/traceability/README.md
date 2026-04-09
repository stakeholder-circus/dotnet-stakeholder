# Traceability

Trace every future behavior transfer back to audited Rust lines before implementation.

## Current .NET follow-through

- Dedicated renderers now exist for the classic-six widening tranche:
  - `code_analyzer`
  - `data_processing`
  - `jargon`
  - `metrics`
  - `network_activity`
  - `system_monitoring`
- `agent_workflows` remains dedicated in the modern-core lane.
- All other families stay on grouped fallback renderers for this tranche.
- The smoke tests in `tests/DotnetStakeholder.Tests/SmokeTests.cs` are the current evidence point for dedicated renderer coverage.
