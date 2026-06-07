namespace DotnetStakeholder;

public static class FamilyRegistry
{
    private static readonly GeneratorFamilyDefinition[] Families =
    [
        new() { Id = "code_analyzer", Label = "code_analyzer", Group = FamilyGroup.ClassicSix, Summary = "code review, build graph, SDK drift", RendererKey = "classic-six.code_analyzer", Smoke = true },
        new() { Id = "data_processing", Label = "data_processing", Group = FamilyGroup.ClassicSix, Summary = "fixtures, pipelines, transforms", RendererKey = "classic-six.data_processing", Smoke = true },
        new() { Id = "jargon", Label = "jargon", Group = FamilyGroup.ClassicSix, Summary = "credible domain language", RendererKey = "classic-six.jargon", Smoke = true },
        new() { Id = "metrics", Label = "metrics", Group = FamilyGroup.ClassicSix, Summary = "token cost, burn, queue depth", RendererKey = "classic-six.metrics", Smoke = true },
        new() { Id = "network_activity", Label = "network_activity", Group = FamilyGroup.ClassicSix, Summary = "API, SSE, and transport events", RendererKey = "classic-six.network_activity", Smoke = true },
        new() { Id = "system_monitoring", Label = "system_monitoring", Group = FamilyGroup.ClassicSix, Summary = "health, backpressure, saturation", RendererKey = "classic-six.system_monitoring", Smoke = true },

        new() { Id = "agent_workflows", Label = "agent_workflows", Group = FamilyGroup.ModernCore, Summary = "delegation, retries, approvals", RendererKey = "modern-core.agent_workflows", Smoke = true },
        new() { Id = "platform_engineering", Label = "platform_engineering", Group = FamilyGroup.ModernCore, Summary = "golden paths, identity, queues", RendererKey = "modern-core.platform_engineering", Smoke = true },
        new() { Id = "observability_ai_runtime", Label = "observability_ai_runtime", Group = FamilyGroup.ModernCore, Summary = "tracing, burn rate, GPU pressure", RendererKey = "modern-core.observability_ai_runtime", Smoke = true },
        new() { Id = "delivery_preview_ops", Label = "delivery_preview_ops", Group = FamilyGroup.ModernCore, Summary = "preview deploys, canaries, flags", RendererKey = "modern-core.delivery_preview_ops", Smoke = true },
        new() { Id = "supply_chain_security", Label = "supply_chain_security", Group = FamilyGroup.ModernCore, Summary = "provenance, attestations, secrets", RendererKey = "modern-core.supply_chain_security", Smoke = true },

        new() { Id = "ai_inference_ops", Label = "ai_inference_ops", Group = FamilyGroup.AiGovernance, Summary = "model routing, fallback, cache", RendererKey = "ai-governance.fallback" },
        new() { Id = "knowledge_retrieval", Label = "knowledge_retrieval", Group = FamilyGroup.AiGovernance, Summary = "stale embeddings, recall, citations", RendererKey = "ai-governance.fallback" },
        new() { Id = "evaluation_and_guardrails", Label = "evaluation_and_guardrails", Group = FamilyGroup.AiGovernance, Summary = "eval drift, guardrail failures", RendererKey = "ai-governance.fallback" },
        new() { Id = "aibom_provenance", Label = "aibom_provenance", Group = FamilyGroup.AiGovernance, Summary = "model lineage and AI bills of materials", RendererKey = "ai-governance.fallback" },
        new() { Id = "data_governance_compliance", Label = "data_governance_compliance", Group = FamilyGroup.AiGovernance, Summary = "consent, retention, audit", RendererKey = "ai-governance.fallback" },
        new() { Id = "finops_capacity", Label = "finops_capacity", Group = FamilyGroup.AiGovernance, Summary = "budget, quota, resource burn", RendererKey = "ai-governance.fallback" },

        new() { Id = "identity_and_trust", Label = "identity_and_trust", Group = FamilyGroup.SecurityBlockchain, Summary = "keys, delegation, trust boundaries", RendererKey = "security-blockchain.fallback" },
        new() { Id = "agent_boundary_security", Label = "agent_boundary_security", Group = FamilyGroup.SecurityBlockchain, Summary = "tool, prompt, and auth boundaries", RendererKey = "security-blockchain.fallback" },
        new() { Id = "blockchain_protocol_ops", Label = "blockchain_protocol_ops", Group = FamilyGroup.SecurityBlockchain, Summary = "rollups, validators, account abstraction", RendererKey = "security-blockchain.fallback" },
        new() { Id = "cross_chain_interop", Label = "cross_chain_interop", Group = FamilyGroup.SecurityBlockchain, Summary = "chain abstraction and transfers", RendererKey = "security-blockchain.fallback" },
        new() { Id = "proof_and_sequencer_ops", Label = "proof_and_sequencer_ops", Group = FamilyGroup.SecurityBlockchain, Summary = "proof queues, ordering, MEV", RendererKey = "security-blockchain.fallback" },

        new() { Id = "fhir_profile_generator", Label = "fhir_profile_generator", Group = FamilyGroup.HealthProtocol, Summary = "FHIR resource generation", RendererKey = "health-protocol.fallback" },
        new() { Id = "smart_launch_oauth", Label = "smart_launch_oauth", Group = FamilyGroup.HealthProtocol, Summary = "SMART launch and OAuth context", RendererKey = "health-protocol.fallback" },
        new() { Id = "bulk_fhir_population_ops", Label = "bulk_fhir_population_ops", Group = FamilyGroup.HealthProtocol, Summary = "bulk export and analytics", RendererKey = "health-protocol.fallback" },
        new() { Id = "hl7v2_feed_ops", Label = "hl7v2_feed_ops", Group = FamilyGroup.HealthProtocol, Summary = "ADT/ORU feed handling", RendererKey = "health-protocol.fallback" },
        new() { Id = "clinical_workflow_events", Label = "clinical_workflow_events", Group = FamilyGroup.HealthProtocol, Summary = "hooks, subscriptions, workflow events", RendererKey = "health-protocol.fallback" },
        new() { Id = "dicomweb_imaging_ops", Label = "dicomweb_imaging_ops", Group = FamilyGroup.HealthProtocol, Summary = "QIDO/WADO/STOW imaging flows", RendererKey = "health-protocol.fallback" },
        new() { Id = "openehr_semantic_record_ops", Label = "openehr_semantic_record_ops", Group = FamilyGroup.HealthProtocol, Summary = "archetypes, templates, AQL", RendererKey = "health-protocol.fallback" },
        new() { Id = "device_telemetry_clinical", Label = "device_telemetry_clinical", Group = FamilyGroup.HealthProtocol, Summary = "bedside telemetry and alerts", RendererKey = "health-protocol.fallback" },
        new() { Id = "emr_vendor_adapter", Label = "emr_vendor_adapter", Group = FamilyGroup.HealthProtocol, Summary = "EMR vendor adapter flows", RendererKey = "health-protocol.fallback" },
        new() { Id = "ocpp_chargepoint_ops", Label = "ocpp_chargepoint_ops", Group = FamilyGroup.HealthProtocol, Summary = "OCPP 1.6 and 2.x chargepoint ops", RendererKey = "health-protocol.fallback" },
        new() { Id = "ocpi_roaming_ops", Label = "ocpi_roaming_ops", Group = FamilyGroup.HealthProtocol, Summary = "roaming, sessions, tariffs", RendererKey = "health-protocol.fallback" },
        new() { Id = "mcp_a2a_ops", Label = "mcp_a2a_ops", Group = FamilyGroup.HealthProtocol, Summary = "MCP and A2A tool calls", RendererKey = "health-protocol.fallback" },
        new() { Id = "streaming_bus_ops", Label = "streaming_bus_ops", Group = FamilyGroup.HealthProtocol, Summary = "Kafka, NATS, MQTT, event buses", RendererKey = "health-protocol.fallback" },
        new() { Id = "service_mesh_rpc_ops", Label = "service_mesh_rpc_ops", Group = FamilyGroup.HealthProtocol, Summary = "gRPC and GraphQL federation", RendererKey = "health-protocol.fallback" },
        new() { Id = "edge_client_runtime", Label = "edge_client_runtime", Group = FamilyGroup.HealthProtocol, Summary = "edge UI, hydration, offline sync", RendererKey = "health-protocol.fallback" },
        new() { Id = "embedded_agentic_pipeline", Label = "embedded_agentic_pipeline", Group = FamilyGroup.HealthProtocol, Summary = "deterministic control loops", RendererKey = "health-protocol.fallback" },

        new() { Id = "multilingual_security_packs", Label = "multilingual_security_packs", Group = FamilyGroup.OverlayQuantum, Summary = "localized security/operator tone", RendererKey = "overlay-quantum.fallback" },
        new() { Id = "security_persona_packs", Label = "security_persona_packs", Group = FamilyGroup.OverlayQuantum, Summary = "SOC, CTI, reverse-engineering personas", RendererKey = "overlay-quantum.fallback" },
        new() { Id = "hybrid_runtime_ops", Label = "hybrid_runtime_ops", Group = FamilyGroup.OverlayQuantum, Summary = "quantum jobs, sessions, batches", RendererKey = "overlay-quantum.fallback" },
        new() { Id = "capacity_cost_controller", Label = "capacity_cost_controller", Group = FamilyGroup.OverlayQuantum, Summary = "queues, reservations, spend controls", RendererKey = "overlay-quantum.fallback" },
        new() { Id = "batch_execution_tuner", Label = "batch_execution_tuner", Group = FamilyGroup.OverlayQuantum, Summary = "batch throughput and benchmarks", RendererKey = "overlay-quantum.fallback" },
        new() { Id = "compiler_maintainer", Label = "compiler_maintainer", Group = FamilyGroup.OverlayQuantum, Summary = "transpiler and plugin maintenance", RendererKey = "overlay-quantum.fallback" },
        new() { Id = "interop_adapter_engineer", Label = "interop_adapter_engineer", Group = FamilyGroup.OverlayQuantum, Summary = "OpenQASM and QIR adaptation", RendererKey = "overlay-quantum.fallback" },
        new() { Id = "preflight_capacity_planner", Label = "preflight_capacity_planner", Group = FamilyGroup.OverlayQuantum, Summary = "resource estimation and gating", RendererKey = "overlay-quantum.fallback" },
        new() { Id = "simulator_performance_engineer", Label = "simulator_performance_engineer", Group = FamilyGroup.OverlayQuantum, Summary = "simulators, GPU, local mode", RendererKey = "overlay-quantum.fallback" }
    ];

    private static readonly IReadOnlyDictionary<string, GeneratorFamilyDefinition> ById =
        Families.ToDictionary(family => family.Id, StringComparer.OrdinalIgnoreCase);

    public static IReadOnlyList<GeneratorFamilyDefinition> All => Families;

    public static IReadOnlyList<GeneratorFamilyDefinition> ClassicSix => Families.Where(f => f.Group == FamilyGroup.ClassicSix).ToArray();

    public static IReadOnlyList<GeneratorFamilyDefinition> ModernCore => Families.Where(f => f.Group == FamilyGroup.ModernCore).ToArray();

    public static IReadOnlyList<GeneratorFamilyDefinition> AiGovernance => Families.Where(f => f.Group == FamilyGroup.AiGovernance).ToArray();

    public static IReadOnlyList<GeneratorFamilyDefinition> SecurityBlockchain => Families.Where(f => f.Group == FamilyGroup.SecurityBlockchain).ToArray();

    public static IReadOnlyList<GeneratorFamilyDefinition> HealthProtocol => Families.Where(f => f.Group == FamilyGroup.HealthProtocol).ToArray();

    public static IReadOnlyList<GeneratorFamilyDefinition> OverlayQuantum => Families.Where(f => f.Group == FamilyGroup.OverlayQuantum).ToArray();

    public static bool TryGet(string id, out GeneratorFamilyDefinition definition) => ById.TryGetValue(id, out definition!);

    public static GeneratorFamilyDefinition GetRequired(string id) =>
        TryGet(id, out var definition)
            ? definition
            : throw new CommandLineException($"Unknown family '{id}'.");

    public static string[] DevTypes =>
    [
        "backend",
        "blockchain",
        "data-science",
        "dev-ops",
        "frontend",
        "fullstack",
        "game-development",
        "machine-learning",
        "security",
        "systems-programming"
    ];

    public static string[] JargonLevels => ["low", "normal", "high", "extreme"];

    public static string[] Complexities => ["low", "medium", "high", "extreme"];

    public static string[] OutputFormats => ["text", "json"];
}
