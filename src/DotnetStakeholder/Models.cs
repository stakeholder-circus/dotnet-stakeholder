using System.Text.Json.Serialization;

namespace DotnetStakeholder;

public enum DevType
{
    Backend,
    Blockchain,
    DataScience,
    DevOps,
    Frontend,
    Fullstack,
    GameDevelopment,
    MachineLearning,
    Security,
    SystemsProgramming
}

public enum JargonLevel
{
    Low,
    Normal,
    High,
    Extreme
}

public enum Complexity
{
    Low,
    Medium,
    High,
    Extreme
}

public enum OutputFormat
{
    Text,
    Json
}

public enum FamilyGroup
{
    ClassicSix,
    ModernCore,
    AiGovernance,
    SecurityBlockchain,
    HealthProtocol,
    OverlayQuantum
}

public enum ExperimentalAdapterMode
{
    Api,
    Consumer
}

public sealed record SessionConfig
{
    public DevType DevType { get; init; } = DevType.Fullstack;

    public Complexity Complexity { get; init; } = Complexity.Medium;

    public JargonLevel Jargon { get; init; } = JargonLevel.Normal;

    public OutputFormat OutputFormat { get; init; } = OutputFormat.Text;

    public string Seed { get; init; } = "stakeholder-2026";

    public string Project { get; init; } = "stakeholder";

    public string Framework { get; init; } = "dotnet-stakeholder";

    public string? FocusFamily { get; init; }

    public bool Alerts { get; init; }

    public bool Team { get; init; } = true;

    public bool Minimal { get; init; }

    public bool Trace { get; init; } = true;
}

public sealed record ExperimentalConfig
{
    public string? Provider { get; init; }

    public string? Model { get; init; }

    public string? Profile { get; init; }

    public string? Prompt { get; init; }

    public ExperimentalAdapterMode AdapterMode { get; init; } = ExperimentalAdapterMode.Api;

    public bool HasAnyFlag { get; init; }
}

public sealed record GeneratorFamilyDefinition
{
    public required string Id { get; init; }

    public required string Label { get; init; }

    public required FamilyGroup Group { get; init; }

    public required string Summary { get; init; }

    public required string RendererKey { get; init; }

    public bool Smoke { get; init; }
}

public sealed record RendererOutput
{
    public required string Message { get; init; }

    public required string Terminal { get; init; }

    public Dictionary<string, object?> Context { get; init; } = new();
}

public sealed record SessionEvent
{
    public required string EventType { get; init; }

    public required int Sequence { get; init; }

    public required string Message { get; init; }

    public required string Timestamp { get; init; }

    public Dictionary<string, object?> Context { get; init; } = new();

    public Dictionary<string, object?>? Provenance { get; init; }

    public string? Terminal { get; init; }
}

public sealed record SessionResult
{
    public required string SessionId { get; init; }

    public required string Mode { get; init; }

    public required SessionConfig Config { get; init; }

    public required IReadOnlyList<string> SelectedFamilies { get; init; }

    public required IReadOnlyList<SessionEvent> Events { get; init; }
}

public sealed record ListValuesPayload
{
    public required string[] DevTypes { get; init; }

    public required string[] JargonLevels { get; init; }

    public required string[] Complexities { get; init; }

    public required string[] OutputFormats { get; init; }

    public required IReadOnlyList<object> GeneratorFamilies { get; init; }

    public required string[] ExperimentalProviders { get; init; }

    public required string[] ExperimentalAdapterModes { get; init; }
}

public sealed record ParseResult
{
    public required bool ShowHelp { get; init; }

    public required bool ListValues { get; init; }

    public required SessionConfig SessionConfig { get; init; }

    public required ExperimentalConfig ExperimentalConfig { get; init; }
}

public sealed class CommandLineException : Exception
{
    public CommandLineException(string message) : base(message)
    {
    }
}

public sealed class ExperimentalProviderNotImplementedException : Exception
{
    public ExperimentalProviderNotImplementedException(string message) : base(message)
    {
    }
}
