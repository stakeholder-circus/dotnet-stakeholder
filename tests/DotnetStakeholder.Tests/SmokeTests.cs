using System.Text.Json;
using Xunit;

namespace DotnetStakeholder.Tests;

public class SmokeTests
{
    public static TheoryData<string, string> ClassicSixSmokeCases => new()
    {
        { "code_analyzer", "classic-six.code_analyzer" },
        { "data_processing", "classic-six.data_processing" },
        { "jargon", "classic-six.jargon" },
        { "metrics", "classic-six.metrics" },
        { "network_activity", "classic-six.network_activity" },
        { "system_monitoring", "classic-six.system_monitoring" }
    };

    [Fact]
    public void ListValuesIncludesFull2026FamilyRegistry()
    {
        var payload = RunAndParseListValues();

        Assert.Contains(payload.GetProperty("generatorFamilies").EnumerateArray(), item => item.GetProperty("id").GetString() == "code_analyzer");
        Assert.Contains(payload.GetProperty("generatorFamilies").EnumerateArray(), item => item.GetProperty("id").GetString() == "agent_workflows");
        Assert.True(payload.GetProperty("generatorFamilies").GetArrayLength() >= 30);
    }

    [Theory]
    [MemberData(nameof(ClassicSixSmokeCases))]
    public void ClassicSixFamiliesUseDedicatedRenderers(string familyId, string rendererKey)
    {
        var session = RunAndParseSession(
            "--dev-type", "backend",
            "--complexity", "medium",
            "--seed", $"{familyId}-smoke",
            "--focus-family", familyId,
            "--output-format", "json");

        Assert.Contains(session.GetProperty("selectedFamilies").EnumerateArray(), item => item.GetString() == familyId);

        var events = session.GetProperty("events").EnumerateArray().ToArray();
        var activity = Assert.Single(events.Where(eventItem => eventItem.GetProperty("eventType").GetString() == "generator.activity" && eventItem.GetProperty("context").GetProperty("family").GetString() == familyId));
        Assert.Equal(rendererKey, activity.GetProperty("context").GetProperty("renderer").GetString());
        Assert.True(activity.GetProperty("context").GetProperty("smoke").GetBoolean());
        Assert.Equal("dedicated smoke renderer", activity.GetProperty("context").GetProperty("detail").GetString());
    }

    [Fact]
    public void AgentWorkflowsSmokeUsesDedicatedRenderer()
    {
        var session = RunAndParseSession(
            "--dev-type", "fullstack",
            "--complexity", "medium",
            "--seed", "agent-workflows-smoke",
            "--focus-family", "agent_workflows",
            "--output-format", "json");

        Assert.Contains(session.GetProperty("selectedFamilies").EnumerateArray(), item => item.GetString() == "agent_workflows");

        var events = session.GetProperty("events").EnumerateArray().ToArray();
        var activity = Assert.Single(events.Where(eventItem => eventItem.GetProperty("eventType").GetString() == "generator.activity" && eventItem.GetProperty("context").GetProperty("family").GetString() == "agent_workflows"));
        Assert.Contains("delegation", activity.GetProperty("message").GetString()!, StringComparison.OrdinalIgnoreCase);
        Assert.Equal("modern-core.agent_workflows", activity.GetProperty("context").GetProperty("renderer").GetString());
    }

    [Fact]
    public void ExperimentalFlagsFailFastWithExplicitMessage()
    {
        var result = RunAndCaptureError("--experimental-provider", "openai-compatible");

        Assert.Equal(2, result.exitCode);
        Assert.Contains("experimental-provider is not implemented yet in dotnet-stakeholder", result.stderr, StringComparison.OrdinalIgnoreCase);
    }

    private static JsonElement RunAndParseListValues()
    {
        var output = RunAndCaptureStdout("--list-values");
        return JsonSerializer.Deserialize<JsonElement>(output, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    private static JsonElement RunAndParseSession(params string[] args)
    {
        var output = RunAndCaptureStdout(args);
        return JsonSerializer.Deserialize<JsonElement>(output, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    private static string RunAndCaptureStdout(params string[] args)
    {
        var originalOut = Console.Out;
        var originalErr = Console.Error;
        using var stdout = new StringWriter();
        using var stderr = new StringWriter();
        Console.SetOut(stdout);
        Console.SetError(stderr);
        try
        {
            var exitCode = DotnetStakeholderApp.Run(args);
            Assert.Equal(0, exitCode);
            return stdout.ToString();
        }
        finally
        {
            Console.SetOut(originalOut);
            Console.SetError(originalErr);
        }
    }

    private static (int exitCode, string stderr) RunAndCaptureError(params string[] args)
    {
        var originalOut = Console.Out;
        var originalErr = Console.Error;
        using var stdout = new StringWriter();
        using var stderr = new StringWriter();
        Console.SetOut(stdout);
        Console.SetError(stderr);
        try
        {
            var exitCode = DotnetStakeholderApp.Run(args);
            return (exitCode, stderr.ToString());
        }
        finally
        {
            Console.SetOut(originalOut);
            Console.SetError(originalErr);
        }
    }
}
