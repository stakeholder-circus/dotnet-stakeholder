# Traceability

Trace every future behavior transfer back to audited Rust lines before implementation.

## Current .NET follow-through

- Dedicated renderers now exist for the classic-six tranche:
  - `code_analyzer`
  - `data_processing`
  - `jargon`
  - `metrics`
  - `network_activity`
  - `system_monitoring`
- Dedicated renderers now exist for the full modern-core tranche:
  - `agent_workflows`
  - `platform_engineering`
  - `observability_ai_runtime`
  - `delivery_preview_ops`
  - `supply_chain_security`
- All other families stay on grouped fallback renderers for this tranche.
- Dedicated family metadata now points back to Rust generator files, Java renderer anchors, and `stakeholder-core/docs/generator-families.md`.
- The smoke tests in `tests/DotnetStakeholder.Tests/SmokeTests.cs` are the current evidence point for dedicated renderer coverage.
