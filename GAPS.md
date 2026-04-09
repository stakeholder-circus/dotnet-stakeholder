> [!NOTE]
> Missing or deferred behavior must fail fast and be tracked explicitly. No placeholder behavior should mask absent parity work.

# Gaps

- Contract surface exists.
- Runtime foundation now covers typed config/value enums, seeded scheduling, registry dispatch, normalized JSON, and smoke evidence for `code_analyzer` and `agent_workflows`.
- Experimental provider runtime is intentionally not implemented yet; `--experimental-provider` and related flags fail fast with an explicit message.
