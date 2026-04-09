using System.Text;

namespace DotnetStakeholder;

public sealed class SeededRandom
{
    private ulong _state;

    public SeededRandom(string seed)
    {
        _state = HashSeed(seed);
    }

    public int Next(int maxExclusive)
    {
        if (maxExclusive <= 0)
        {
            return 0;
        }

        _state = unchecked(1664525UL * _state + 1013904223UL);
        return (int)((_state >> 1) % (ulong)maxExclusive);
    }

    public T Pick<T>(IReadOnlyList<T> values)
    {
        if (values.Count == 0)
        {
            throw new InvalidOperationException("Cannot pick from an empty list.");
        }

        return values[Next(values.Count)];
    }

    public IReadOnlyList<T> Shuffle<T>(IEnumerable<T> values)
    {
        var copy = values.ToList();
        for (var index = copy.Count - 1; index > 0; index--)
        {
            var swap = Next(index + 1);
            (copy[index], copy[swap]) = (copy[swap], copy[index]);
        }

        return copy;
    }

    private static ulong HashSeed(string seed)
    {
        const ulong offset = 1469598103934665603;
        const ulong prime = 1099511628211;
        var state = offset;
        foreach (var character in Encoding.UTF8.GetBytes(seed ?? string.Empty))
        {
            state ^= character;
            state *= prime;
        }

        return state;
    }
}

public sealed class SessionScheduler
{
    private static readonly string[] AlertCandidates =
    [
        "system_monitoring",
        "observability_ai_runtime",
        "supply_chain_security",
        "agent_boundary_security",
        "device_telemetry_clinical",
        "ocpp_chargepoint_ops"
    ];

    private static readonly string[] TeamCandidates =
    [
        "agent_workflows",
        "delivery_preview_ops",
        "platform_engineering",
        "mcp_a2a_ops"
    ];

    private static readonly string[] ProviderCandidates =
    [
        "ai_inference_ops",
        "evaluation_and_guardrails",
        "aibom_provenance",
        "knowledge_retrieval"
    ];

    private static readonly string[] HighRiskCandidates =
    [
        "supply_chain_security",
        "observability_ai_runtime",
        "agent_boundary_security",
        "identity_and_trust"
    ];

    private static readonly string[] DevTypeBiasBackend = ["agent_workflows", "platform_engineering", "observability_ai_runtime", "supply_chain_security", "delivery_preview_ops"];
    private static readonly string[] DevTypeBiasBlockchain = ["blockchain_protocol_ops", "cross_chain_interop", "proof_and_sequencer_ops", "identity_and_trust", "supply_chain_security"];
    private static readonly string[] DevTypeBiasDataScience = ["knowledge_retrieval", "ai_inference_ops", "evaluation_and_guardrails", "aibom_provenance", "observability_ai_runtime"];
    private static readonly string[] DevTypeBiasDevOps = ["platform_engineering", "delivery_preview_ops", "observability_ai_runtime", "supply_chain_security", "finops_capacity"];
    private static readonly string[] DevTypeBiasFrontend = ["edge_client_runtime", "delivery_preview_ops", "agent_workflows", "observability_ai_runtime", "platform_engineering"];
    private static readonly string[] DevTypeBiasFullstack = ["platform_engineering", "delivery_preview_ops", "agent_workflows", "knowledge_retrieval", "observability_ai_runtime"];
    private static readonly string[] DevTypeBiasGame = ["edge_client_runtime", "observability_ai_runtime", "delivery_preview_ops", "platform_engineering", "simulator_performance_engineer"];
    private static readonly string[] DevTypeBiasMachineLearning = ["ai_inference_ops", "knowledge_retrieval", "evaluation_and_guardrails", "aibom_provenance", "observability_ai_runtime"];
    private static readonly string[] DevTypeBiasSecurity = ["supply_chain_security", "agent_boundary_security", "identity_and_trust", "aibom_provenance", "multilingual_security_packs"];
    private static readonly string[] DevTypeBiasSystems = ["observability_ai_runtime", "embedded_agentic_pipeline", "identity_and_trust", "platform_engineering", "service_mesh_rpc_ops"];

    public IReadOnlyList<GeneratorFamilyDefinition> SelectFamilies(SessionConfig config)
    {
        var rng = new SeededRandom(BuildSeed(config));
        var selected = new List<GeneratorFamilyDefinition>();
        var used = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var desiredCount = GetDesiredCount(config.Complexity);

        AddIfPresent(config.FocusFamily, selected, used);

        var classicPick = rng.Pick(FamilyRegistry.ClassicSix);
        Add(classicPick, selected, used);

        if (config.FocusFamily is not null && selected.Count < desiredCount && !FamilyRegistry.GetRequired(config.FocusFamily).Group.Equals(FamilyGroup.ClassicSix))
        {
            AddIfPresent(config.FocusFamily, selected, used);
        }

        if (config.Complexity is Complexity.Medium or Complexity.High or Complexity.Extreme)
        {
            Add(rng.Pick(FamilyRegistry.ModernCore), selected, used);
        }

        if (config.Complexity is Complexity.High or Complexity.Extreme)
        {
            Add(rng.Pick(FamilyRegistry.SecurityBlockchain), selected, used);
        }

        if (config.Alerts)
        {
            AddFirstAvailable(AlertCandidates, rng, selected, used);
        }

        if (config.Team)
        {
            AddFirstAvailable(TeamCandidates, rng, selected, used);
        }

        if (LooksLikeProviderScenario(config))
        {
            AddFirstAvailable(ProviderCandidates, rng, selected, used);
        }

        AddFirstAvailable(BiasFor(config.DevType), rng, selected, used);

        AddFirstAvailable(FamilyRegistry.AiGovernance.Select(f => f.Id), rng, selected, used);
        AddFirstAvailable(FamilyRegistry.HealthProtocol.Select(f => f.Id), rng, selected, used);
        AddFirstAvailable(FamilyRegistry.OverlayQuantum.Select(f => f.Id), rng, selected, used);

        foreach (var family in FamilyRegistry.All)
        {
            if (selected.Count >= desiredCount)
            {
                break;
            }

            Add(family, selected, used);
        }

        return selected.Take(desiredCount).ToArray();
    }

    private static bool LooksLikeProviderScenario(SessionConfig config)
    {
        var subject = string.Join(' ', new[] { config.Project, config.Framework, config.FocusFamily }.Where(value => !string.IsNullOrWhiteSpace(value)));
        return subject.Contains("openai", StringComparison.OrdinalIgnoreCase)
            || subject.Contains("anthropic", StringComparison.OrdinalIgnoreCase)
            || subject.Contains("claude", StringComparison.OrdinalIgnoreCase)
            || subject.Contains("llm", StringComparison.OrdinalIgnoreCase)
            || subject.Contains("provider", StringComparison.OrdinalIgnoreCase);
    }

    private static string[] BiasFor(DevType devType) => devType switch
    {
        DevType.Backend => DevTypeBiasBackend,
        DevType.Blockchain => DevTypeBiasBlockchain,
        DevType.DataScience => DevTypeBiasDataScience,
        DevType.DevOps => DevTypeBiasDevOps,
        DevType.Frontend => DevTypeBiasFrontend,
        DevType.Fullstack => DevTypeBiasFullstack,
        DevType.GameDevelopment => DevTypeBiasGame,
        DevType.MachineLearning => DevTypeBiasMachineLearning,
        DevType.Security => DevTypeBiasSecurity,
        DevType.SystemsProgramming => DevTypeBiasSystems,
        _ => DevTypeBiasFullstack
    };

    private static int GetDesiredCount(Complexity complexity) => complexity switch
    {
        Complexity.Low => 1,
        Complexity.Medium => 2,
        Complexity.High => 3,
        Complexity.Extreme => 4,
        _ => 2
    };

    private static string BuildSeed(SessionConfig config) =>
        string.Join(':', config.Seed, config.DevType, config.Complexity, config.Jargon, config.FocusFamily ?? string.Empty, config.Project, config.Framework, config.Alerts, config.Team);

    private static void AddIfPresent(string? id, ICollection<GeneratorFamilyDefinition> selected, ISet<string> used)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return;
        }

        if (FamilyRegistry.TryGet(id, out var family))
        {
            Add(family, selected, used);
        }
    }

    private static void AddFirstAvailable(IEnumerable<string> ids, SeededRandom rng, ICollection<GeneratorFamilyDefinition> selected, ISet<string> used)
    {
        var pool = ids
            .Select(id => FamilyRegistry.TryGet(id, out var family) ? family : null)
            .Where(family => family is not null && !used.Contains(family.Id))
            .Select(family => family!)
            .ToArray();

        if (pool.Length == 0)
        {
            return;
        }

        Add(rng.Pick(pool), selected, used);
    }

    private static void Add(GeneratorFamilyDefinition family, ICollection<GeneratorFamilyDefinition> selected, ISet<string> used)
    {
        if (used.Add(family.Id))
        {
            selected.Add(family);
        }
    }
}
