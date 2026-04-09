# Example outputs

This repo now exposes dedicated renderers for the classic-six widening tranche plus `agent_workflows`. Everything else stays on grouped fallback renderers.

## Classic-six dedicated paths

- `code_analyzer` - `classic-six.code_analyzer`
  - Example: `build graph edges stayed consistent across the audit pass`
  - Trace context: `smoke=true`, `detail=dedicated smoke renderer`
- `data_processing` - `classic-six.data_processing`
  - Example: `dataset transforms stayed deterministic under seed control`
  - Trace context: `smoke=true`, `detail=dedicated smoke renderer`
- `jargon` - `classic-six.jargon`
  - Example: `terminology drift was reduced to a readable glossary entry`
  - Trace context: `smoke=true`, `detail=dedicated smoke renderer`
- `metrics` - `classic-six.metrics`
  - Example: `latency, throughput, and burn-rate values stayed visible`
  - Trace context: `smoke=true`, `detail=dedicated smoke renderer`
- `network_activity` - `classic-six.network_activity`
  - Example: `request flow stayed readable from client to endpoint`
  - Trace context: `smoke=true`, `detail=dedicated smoke renderer`
- `system_monitoring` - `classic-six.system_monitoring`
  - Example: `collector backpressure stayed visible in the output`
  - Trace context: `smoke=true`, `detail=dedicated smoke renderer`

## Dedicated modern-core path

- `agent_workflows` - `modern-core.agent_workflows`
  - Example: `delegation and handoff steps stayed distinct in the trace`
  - Trace context: `smoke=true`, `detail=dedicated smoke renderer`

## Grouped fallback families

All other families remain on grouped renderers for this tranche, including the remaining modern-core, AI governance, security/blockchain, health/protocol, and overlay/quantum lanes.
