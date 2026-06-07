# Example Outputs

This repo now exposes dedicated renderers for the classic-six tranche plus the full modern-core tranche. Everything else stays on grouped fallback renderers.

## Classic-six dedicated paths

- `code_analyzer` - `classic-six.code_analyzer`
  - Example: `build graph edges stayed consistent across the audit pass`
  - Trace context: `smoke=true`, `detail=dedicated smoke renderer`, `analysisFocus=typed interfaces, agent-authored patches, and MCP assumptions`
- `data_processing` - `classic-six.data_processing`
  - Example: `dataset transforms stayed deterministic under seed control`
  - Trace context: `smoke=true`, `detail=dedicated smoke renderer`, `dataWindow=embeddings, semantic chunks, and batch transforms with deterministic ordering`
- `jargon` - `classic-six.jargon`
  - Example: `terminology drift was reduced to a readable glossary entry`
  - Trace context: `smoke=true`, `detail=dedicated smoke renderer`, `languagePolicy=credible 2026 terminology instead of fake-deep phrasing`
- `metrics` - `classic-six.metrics`
  - Example: `latency, throughput, and burn-rate values stayed visible`
  - Trace context: `smoke=true`, `detail=dedicated smoke renderer`, `signalBlend=queue depth, token spend, and GPU occupancy in a single operations lane`
- `network_activity` - `classic-six.network_activity`
  - Example: `request flow stayed readable from client to endpoint`
  - Trace context: `smoke=true`, `detail=dedicated smoke renderer`, `transportMix=RPC, event-stream, and adapter traffic under deterministic retry rules`
- `system_monitoring` - `classic-six.system_monitoring`
  - Example: `collector backpressure stayed visible in the output`
  - Trace context: `smoke=true`, `detail=dedicated smoke renderer`, `telemetryScope=collector pressure, runner health, and policy-denial signals across the stack`

## Modern-core dedicated paths

- `agent_workflows` - `modern-core.agent_workflows`
  - Example: `delegation and handoff steps stayed distinct in the trace`
  - Trace context: `smoke=true`, `detail=dedicated smoke renderer`, `coordinationMode=delegated agent work, approval gates, and cross-repo handoff envelopes`
- `platform_engineering` - `modern-core.platform_engineering`
  - Example: `golden paths, identity federation, queue ownership, and paved-road rollouts stayed explicit`
  - Trace context: `smoke=true`, `detail=dedicated smoke renderer`, `platformSurface=golden paths, identity boundaries, and queue ownership in the shared platform lane`
- `observability_ai_runtime` - `modern-core.observability_ai_runtime`
  - Example: `inference spans, token burn, GPU saturation, and sandbox denials stayed correlated`
  - Trace context: `smoke=true`, `detail=dedicated smoke renderer`, `runtimeSignals=trace spans, token burn, GPU pressure, and policy denials in one runtime lane`
- `delivery_preview_ops` - `modern-core.delivery_preview_ops`
  - Example: `preview deploys, canary health, release flags, and rollback checkpoints stayed coordinated`
  - Trace context: `smoke=true`, `detail=dedicated smoke renderer`, `deliveryGuardrail=preview deploys, canaries, release flags, and rollback checkpoints under seed control`
- `supply_chain_security` - `modern-core.supply_chain_security`
  - Example: `attestations, dependency drift, key rotation, and registry trust signals stayed linked`
  - Trace context: `smoke=true`, `detail=dedicated smoke renderer`, `supplyChainPosture=provenance, attestations, dependency drift, and secret exposure in one security lane`

## Grouped fallback families

All other families remain on grouped renderers for this tranche, including AI governance, security/blockchain, health/protocol, and overlay/quantum lanes.
