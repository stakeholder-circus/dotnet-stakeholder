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
    private readonly string[] _phrases;

    protected DedicatedSmokeRenderer(string rendererKey, string accent, params string[] phrases)
    {
        _rendererKey = rendererKey;
        _accent = accent;
        _phrases = phrases;
    }

    public string RendererKey => _rendererKey;

    public RendererOutput Render(GeneratorFamilyDefinition family, SessionConfig config, SeededRandom rng, int ordinal)
    {
        var message = _phrases[rng.Next(_phrases.Length)];
        var context = BaseContext(family, config, ordinal);
        context["smoke"] = true;
        context["detail"] = "dedicated smoke renderer";
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
        "delegation and handoff steps stayed distinct in the trace",
        "delegation, approval gates, and retry paths remained visible",
        "delegation workflows stayed explicit without collapsing detail",
        "delegation planning remained readable from prompt to result")
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
