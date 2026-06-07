using System.Text;

namespace DotnetStakeholder;

public interface IFamilyRenderer
{
    string RendererKey { get; }

    RendererOutput Render(GeneratorFamilyDefinition family, SessionConfig config, SeededRandom rng, int ordinal);
}

public abstract class DedicatedSmokeRenderer : IFamilyRenderer
{
    private readonly string _rendererKey;
    private readonly string _accent;
    private readonly string _focusKey;
    private readonly string _focusValue;
    private readonly string _rustPath;
    private readonly string _javaPath;
    private readonly string[] _phrases;

    protected DedicatedSmokeRenderer(
        string rendererKey,
        string accent,
        string focusKey,
        string focusValue,
        string rustPath,
        string javaPath,
        params string[] phrases)
    {
        _rendererKey = rendererKey;
        _accent = accent;
        _focusKey = focusKey;
        _focusValue = focusValue;
        _rustPath = rustPath;
        _javaPath = javaPath;
        _phrases = phrases;
    }

    public string RendererKey => _rendererKey;

    public RendererOutput Render(GeneratorFamilyDefinition family, SessionConfig config, SeededRandom rng, int ordinal)
    {
        var message = $"{_phrases[rng.Next(_phrases.Length)]} Traceability is anchored to Java, Rust, and stakeholder-core.";
        var context = BaseContext(family, config, ordinal);
        context["smoke"] = true;
        context["detail"] = "dedicated smoke renderer";
        context["familyFocusKey"] = _focusKey;
        context[_focusKey] = _focusValue;
        context["traceabilitySourceRepo"] = "rust-stakeholder";
        context["traceabilitySourcePath"] = _rustPath;
        context["traceabilityJavaRepo"] = "java-stakeholder";
        context["traceabilityJavaPath"] = _javaPath;
        context["traceabilityContractRepo"] = "stakeholder-core";
        context["traceabilityContractPath"] = "docs/generator-families.md";
        context["traceabilityParityClass"] = "depth";
        return new RendererOutput
        {
            Message = message,
            Terminal = AnsiLine($"{family.Label}: {message}", _accent, ordinal, config.Trace, family.Group),
            Context = context
        };
    }

    private Dictionary<string, object?> BaseContext(GeneratorFamilyDefinition family, SessionConfig config, int ordinal) =>
        new()
        {
            ["family"] = family.Id,
            ["group"] = family.Group.ToString(),
            ["renderer"] = _rendererKey,
            ["devType"] = config.DevType.ToString(),
            ["trace"] = config.Trace,
            ["alerts"] = config.Alerts,
            ["team"] = config.Team,
            ["ordinal"] = ordinal
        };

    private static string AnsiLine(string message, string accent, int ordinal, bool trace, FamilyGroup group)
    {
        var reset = "\u001b[0m";
        var muted = "\u001b[2m";
        var seq = $"{ordinal:000}";
        var trailer = trace ? $"{muted}trace{reset}" : $"{muted}{group}{reset}";
        return $"{muted}{seq}{reset} {accent}{message}{reset} {trailer}";
    }
}

public sealed class CodeAnalyzerRenderer : DedicatedSmokeRenderer
{
    public CodeAnalyzerRenderer() : base(
        "classic-six.code_analyzer",
        "\u001b[38;5;81m",
        "analysisFocus",
        "typed interfaces, agent-authored patches, and MCP assumptions",
        "src/generators/code_analyzer.rs",
        "src/main/java/com/stakeholder/generators/CodeAnalyzerRenderer.java",
        "build graph edges stayed consistent across the audit pass",
        "SDK drift was reduced to an explicit traceable mismatch",
        "renderer output now points back to traceability rows",
        "review findings stayed anchored to the source contract")
    {
    }
}

public sealed class DataProcessingRenderer : DedicatedSmokeRenderer
{
    public DataProcessingRenderer() : base(
        "classic-six.data_processing",
        "\u001b[38;5;45m",
        "dataWindow",
        "embeddings, semantic chunks, and batch transforms with deterministic ordering",
        "src/generators/data_processing.rs",
        "src/main/java/com/stakeholder/generators/DataProcessingRenderer.java",
        "dataset transforms stayed deterministic under seed control",
        "feature extraction remained traceable back to the input rows",
        "stream ingestion stayed consistent across the replay path",
        "data pipeline handoffs stayed explicit in the trace")
    {
    }
}

public sealed class JargonRenderer : DedicatedSmokeRenderer
{
    public JargonRenderer() : base(
        "classic-six.jargon",
        "\u001b[38;5;141m",
        "languagePolicy",
        "credible 2026 terminology instead of fake-deep phrasing",
        "src/generators/jargon.rs",
        "src/main/java/com/stakeholder/generators/JargonRenderer.java",
        "terminology drift was reduced to a readable glossary entry",
        "overheated wording stayed anchored to an explicit contract",
        "domain language remained precise across the renderer lane",
        "the jargon profile stayed usable without losing meaning")
    {
    }
}

public sealed class MetricsRenderer : DedicatedSmokeRenderer
{
    public MetricsRenderer() : base(
        "classic-six.metrics",
        "\u001b[38;5;87m",
        "signalBlend",
        "queue depth, token spend, and GPU occupancy in a single operations lane",
        "src/generators/metrics.rs",
        "src/main/java/com/stakeholder/generators/MetricsRenderer.java",
        "latency, throughput, and burn-rate values stayed visible",
        "SLO context stayed attached to the generated line",
        "the metrics lane stayed readable under seed control",
        "the session summary kept its operational counters explicit")
    {
    }
}

public sealed class NetworkActivityRenderer : DedicatedSmokeRenderer
{
    public NetworkActivityRenderer() : base(
        "classic-six.network_activity",
        "\u001b[38;5;214m",
        "transportMix",
        "RPC, event-stream, and adapter traffic under deterministic retry rules",
        "src/generators/network_activity.rs",
        "src/main/java/com/stakeholder/generators/NetworkActivityRenderer.java",
        "request flow stayed readable from client to endpoint",
        "transport edges stayed explicit in the generated trace",
        "event stream activity stayed deterministic under seed control",
        "network hops remained visible without losing the contract")
    {
    }
}

public sealed class SystemMonitoringRenderer : DedicatedSmokeRenderer
{
    public SystemMonitoringRenderer() : base(
        "classic-six.system_monitoring",
        "\u001b[38;5;82m",
        "telemetryScope",
        "collector pressure, runner health, and policy-denial signals across the stack",
        "src/generators/system_monitoring.rs",
        "src/main/java/com/stakeholder/generators/SystemMonitoringRenderer.java",
        "collector backpressure stayed visible in the output",
        "health checks remained attached to the terminal line",
        "signal freshness stayed readable under seed control",
        "monitoring noise stayed bounded by the renderer contract")
    {
    }
}

public sealed class AgentWorkflowsRenderer : DedicatedSmokeRenderer
{
    public AgentWorkflowsRenderer() : base(
        "modern-core.agent_workflows",
        "\u001b[38;5;213m",
        "coordinationMode",
        "delegated agent work, approval gates, and cross-repo handoff envelopes",
        "src/generators/agent_workflows.rs",
        "src/main/java/com/stakeholder/generators/AgentWorkflowsRenderer.java",
        "delegation and handoff steps stayed distinct in the trace",
        "delegation, approval gates, and retry paths remained visible",
        "delegation workflows stayed explicit without collapsing detail",
        "delegation planning remained readable from prompt to result")
    {
    }
}

public sealed class PlatformEngineeringRenderer : DedicatedSmokeRenderer
{
    public PlatformEngineeringRenderer() : base(
        "modern-core.platform_engineering",
        "\u001b[38;5;117m",
        "platformSurface",
        "golden paths, identity boundaries, and queue ownership in the shared platform lane",
        "src/generators/platform_engineering.rs",
        "src/main/java/com/stakeholder/generators/PlatformEngineeringRenderer.java",
        "golden paths, identity federation, queue ownership, and paved-road rollouts stayed explicit",
        "platform contracts, tenancy edges, and queue ownership stayed aligned across the control plane",
        "identity boundaries and paved-road handoffs stayed readable under seed control",
        "the shared platform lane kept ownership, queues, and defaults visible")
    {
    }
}

public sealed class ObservabilityAIRuntimeRenderer : DedicatedSmokeRenderer
{
    public ObservabilityAIRuntimeRenderer() : base(
        "modern-core.observability_ai_runtime",
        "\u001b[38;5;111m",
        "runtimeSignals",
        "trace spans, token burn, GPU pressure, and policy denials in one runtime lane",
        "src/generators/observability_ai_runtime.rs",
        "src/main/java/com/stakeholder/generators/ObservabilityAIRuntimeRenderer.java",
        "inference spans, token burn, GPU saturation, and sandbox denials stayed correlated",
        "runtime traces, GPU pressure, and policy denials stayed visible in one lane",
        "AI runtime telemetry stayed explicit from trace spans to token burn",
        "the runtime lane kept saturation, burn, and denials attached to the same trace")
    {
    }
}

public sealed class DeliveryPreviewOpsRenderer : DedicatedSmokeRenderer
{
    public DeliveryPreviewOpsRenderer() : base(
        "modern-core.delivery_preview_ops",
        "\u001b[38;5;221m",
        "deliveryGuardrail",
        "preview deploys, canaries, release flags, and rollback checkpoints under seed control",
        "src/generators/delivery_preview_ops.rs",
        "src/main/java/com/stakeholder/generators/DeliveryPreviewOpsRenderer.java",
        "preview deploys, canary health, release flags, and rollback checkpoints stayed coordinated",
        "preview environments and rollback cues stayed visible before promotion",
        "the delivery lane kept canaries, flags, and rollback gates readable",
        "release previews stayed explicit without hiding the rollback path")
    {
    }
}

public sealed class SupplyChainSecurityRenderer : DedicatedSmokeRenderer
{
    public SupplyChainSecurityRenderer() : base(
        "modern-core.supply_chain_security",
        "\u001b[38;5;203m",
        "supplyChainPosture",
        "provenance, attestations, dependency drift, and secret exposure in one security lane",
        "src/generators/supply_chain_security.rs",
        "src/main/java/com/stakeholder/generators/SupplyChainSecurityRenderer.java",
        "attestations, dependency drift, key rotation, and registry trust signals stayed linked",
        "provenance, signing, and dependency drift stayed visible in one security lane",
        "the supply-chain gate kept attestations and secret exposure explicit",
        "dependency trust signals stayed readable without hiding provenance gaps")
    {
    }
}

public sealed class GroupedFamilyRenderer : IFamilyRenderer
{
    private readonly string _rendererKey;
    private readonly string _accent;
    private readonly string[] _phrases;

    public GroupedFamilyRenderer(string rendererKey, string accent, params string[] phrases)
    {
        _rendererKey = rendererKey;
        _accent = accent;
        _phrases = phrases;
    }

    public string RendererKey => _rendererKey;

    public RendererOutput Render(GeneratorFamilyDefinition family, SessionConfig config, SeededRandom rng, int ordinal)
    {
        var message = _phrases[rng.Next(_phrases.Length)];
        var context = new Dictionary<string, object?>
        {
            ["family"] = family.Id,
            ["group"] = family.Group.ToString(),
            ["renderer"] = _rendererKey,
            ["devType"] = config.DevType.ToString(),
            ["trace"] = config.Trace,
            ["alerts"] = config.Alerts,
            ["team"] = config.Team,
            ["ordinal"] = ordinal,
            ["summary"] = family.Summary
        };

        return new RendererOutput
        {
            Message = message,
            Terminal = AnsiLine($"{family.Label}: {message}", _accent, ordinal, config.Trace, family.Group),
            Context = context
        };
    }

    private static string AnsiLine(string message, string accent, int ordinal, bool trace, FamilyGroup group)
    {
        var reset = "\u001b[0m";
        var muted = "\u001b[2m";
        var seq = $"{ordinal:000}";
        var trailer = trace ? $"{muted}trace{reset}" : $"{muted}{group}{reset}";
        return $"{muted}{seq}{reset} {accent}{message}{reset} {trailer}";
    }
}

public sealed class RendererRegistry
{
    private readonly Dictionary<string, IFamilyRenderer> _byFamilyId;

    public RendererRegistry()
    {
        var classic = new GroupedFamilyRenderer(
            "classic-six.fallback",
            "\u001b[38;5;81m",
            "fixture streams stayed normalized and repeatable",
            "schema transforms remained deterministic under seed control",
            "the canonical event contract stayed aligned");

        var modern = new GroupedFamilyRenderer(
            "modern-core.fallback",
            "\u001b[38;5;213m",
            "platform and delivery signals stayed visible",
            "operational detail remained deterministic under seed control",
            "the modern runtime lane stayed readable");

        var ai = new GroupedFamilyRenderer(
            "ai-governance.fallback",
            "\u001b[38;5;87m",
            "AI governance state stayed readable in the session",
            "the model and retrieval lane remained explicit",
            "governance checks stayed attached to the output");

        var security = new GroupedFamilyRenderer(
            "security-blockchain.fallback",
            "\u001b[38;5;214m",
            "security and blockchain behavior stayed concrete",
            "trust-boundary state remained readable",
            "protocol edges stayed explicit in the trace");

        var health = new GroupedFamilyRenderer(
            "health-protocol.fallback",
            "\u001b[38;5;82m",
            "health and protocol details stayed visible",
            "payload shape and transport stayed deterministic",
            "the operational lane remained readable");

        var overlay = new GroupedFamilyRenderer(
            "overlay-quantum.fallback",
            "\u001b[38;5;177m",
            "overlay and quantum runtime details stayed explicit",
            "the session remained practical and bounded",
            "control flow stayed readable across the lane");

        _byFamilyId = new Dictionary<string, IFamilyRenderer>(StringComparer.OrdinalIgnoreCase);
        foreach (var family in FamilyRegistry.All)
        {
            _byFamilyId[family.Id] = family.Id switch
            {
                "code_analyzer" => new CodeAnalyzerRenderer(),
                "data_processing" => new DataProcessingRenderer(),
                "jargon" => new JargonRenderer(),
                "metrics" => new MetricsRenderer(),
                "network_activity" => new NetworkActivityRenderer(),
                "system_monitoring" => new SystemMonitoringRenderer(),
                "agent_workflows" => new AgentWorkflowsRenderer(),
                "platform_engineering" => new PlatformEngineeringRenderer(),
                "observability_ai_runtime" => new ObservabilityAIRuntimeRenderer(),
                "delivery_preview_ops" => new DeliveryPreviewOpsRenderer(),
                "supply_chain_security" => new SupplyChainSecurityRenderer(),
                _ when family.Group == FamilyGroup.ClassicSix => classic,
                _ when family.Group == FamilyGroup.ModernCore => modern,
                _ when family.Group == FamilyGroup.AiGovernance => ai,
                _ when family.Group == FamilyGroup.SecurityBlockchain => security,
                _ when family.Group == FamilyGroup.HealthProtocol => health,
                _ when family.Group == FamilyGroup.OverlayQuantum => overlay,
                _ => classic
            };
        }
    }

    public IFamilyRenderer Get(string familyId) =>
        _byFamilyId.TryGetValue(familyId, out var renderer)
            ? renderer
            : throw new CommandLineException($"No renderer registered for '{familyId}'.");
}
