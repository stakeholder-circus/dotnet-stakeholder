using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DotnetStakeholder;

public static class SessionJson
{
    public static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = true
    };
}

public static class DotnetStakeholderJson
{
    public static string SerializeListValues(ListValuesPayload payload) => JsonSerializer.Serialize(payload, SessionJson.Options);

    public static string SerializeSession(SessionResult session) => JsonSerializer.Serialize(session, SessionJson.Options);

    public static string FormatTextSession(SessionResult session)
    {
        var builder = new StringBuilder();
        builder.AppendLine(AnsiHeader(session));
        foreach (var eventItem in session.Events)
        {
            builder.AppendLine(eventItem.Terminal ?? $"{eventItem.Sequence:000} {eventItem.Message}");
        }

        return builder.ToString();
    }

    private static string AnsiHeader(SessionResult session)
    {
        var reset = "\u001b[0m";
        var muted = "\u001b[2m";
        return $"{muted}{session.SessionId}{reset} \u001b[38;5;81m{session.Mode}{reset} {muted}{session.Config.DevType}{reset}";
    }

    public static ListValuesPayload BuildListValues()
    {
        return new ListValuesPayload
        {
            DevTypes = FamilyRegistry.DevTypes,
            JargonLevels = FamilyRegistry.JargonLevels,
            Complexities = FamilyRegistry.Complexities,
            OutputFormats = FamilyRegistry.OutputFormats,
            GeneratorFamilies = FamilyRegistry.All.Select(family => new
            {
                id = family.Id,
                label = family.Label,
                group = family.Group.ToString(),
                summary = family.Summary,
                renderer = family.RendererKey,
                smoke = family.Smoke
            }).Cast<object>().ToArray(),
            ExperimentalProviders = ["openai-compatible", "anthropic", "consumer-session"],
            ExperimentalAdapterModes = ["api", "consumer"]
        };
    }
}
