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

    public static TheoryData<string, string, string, string, string> ModernCoreDedicatedCases => new()
    {
        { "agent_workflows", "modern-core.agent_workflows", "coordinationMode", "delegated agent work, approval gates, and cross-repo handoff envelopes", "src/main/java/com/stakeholder/generators/AgentWorkflowsRenderer.java" },
        { "platform_engineering", "modern-core.platform_engineering", "platformSurface", "golden paths, identity boundaries, and queue ownership in the shared platform lane", "src/main/java/com/stakeholder/generators/PlatformEngineeringRenderer.java" },
        { "observability_ai_runtime", "modern-core.observability_ai_runtime", "runtimeSignals", "trace spans, token burn, GPU pressure, and policy denials in one runtime lane", "src/main/java/com/stakeholder/generators/ObservabilityAIRuntimeRenderer.java" },
        { "delivery_preview_ops", "modern-core.delivery_preview_ops", "deliveryGuardrail", "preview deploys, canaries, release flags, and rollback checkpoints under seed control", "src/main/java/com/stakeholder/generators/DeliveryPreviewOpsRenderer.java" },
        { "supply_chain_security", "modern-core.supply_chain_security", "supplyChainPosture", "provenance, attestations, dependency drift, and secret exposure in one security lane", "src/main/java/com/stakeholder/generators/SupplyChainSecurityRenderer.java" }
    };

    [Fact]
    public void ListValuesIncludesFull2026FamilyRegistry()
    {
        var payload = RunAndParseListValues();

        Assert.Contains(payload.GetProperty("generatorFamilies").EnumerateArray(), item => item.GetProperty("id").GetString() == "code_analyzer");
        Assert.Contains(payload.GetProperty("generatorFamilies").EnumerateArray(), item => item.GetProperty("id").GetString() == "agent_workflows");
        Assert.True(payload.GetProperty("generatorFamilies").GetArrayLength() >= 30);

        foreach (var (familyId, rendererKey) in new[]
        {
            ("platform_engineering", "modern-core.platform_engineering"),
            ("observability_ai_runtime", "modern-core.observability_ai_runtime"),
            ("delivery_preview_ops", "modern-core.delivery_preview_ops"),
            ("supply_chain_security", "modern-core.supply_chain_security")
        })
        {
            var family = payload.GetProperty("generatorFamilies").EnumerateArray().Single(item => item.GetProperty("id").GetString() == familyId);
            Assert.Equal(rendererKey, family.GetProperty("rendererKey").GetString());
            Assert.True(family.GetProperty("smoke").GetBoolean());
        }
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
        Assert.Contains("Java, Rust, and stakeholder-core", activity.GetProperty("message").GetString()!, StringComparison.Ordinal);
        Assert.Equal("modern-core.agent_workflows", activity.GetProperty("context").GetProperty("renderer").GetString());
    }

    [Theory]
    [MemberData(nameof(ModernCoreDedicatedCases))]
    public void ModernCoreFamiliesUseDedicatedRenderersWithTraceabilityMetadata(
        string familyId,
        string rendererKey,
        string focusKey,
        string focusValue,
        string javaPath)
    {
        var session = RunAndParseSession(
            "--dev-type", "dev-ops",
            "--complexity", "high",
            "--seed", $"{familyId}-depth",
            "--focus-family", familyId,
            "--output-format", "json");

        Assert.Contains(session.GetProperty("selectedFamilies").EnumerateArray(), item => item.GetString() == familyId);

        var events = session.GetProperty("events").EnumerateArray().ToArray();
        var activity = Assert.Single(events.Where(eventItem => eventItem.GetProperty("eventType").GetString() == "generator.activity" && eventItem.GetProperty("context").GetProperty("family").GetString() == familyId));
        Assert.Equal(rendererKey, activity.GetProperty("context").GetProperty("renderer").GetString());
        Assert.True(activity.GetProperty("context").GetProperty("smoke").GetBoolean());
        Assert.Equal("dedicated smoke renderer", activity.GetProperty("context").GetProperty("detail").GetString());
        Assert.Equal(focusKey, activity.GetProperty("context").GetProperty("familyFocusKey").GetString());
        Assert.Equal(focusValue, activity.GetProperty("context").GetProperty(focusKey).GetString());
        Assert.Equal("rust-stakeholder", activity.GetProperty("context").GetProperty("traceabilitySourceRepo").GetString());
        Assert.Equal("java-stakeholder", activity.GetProperty("context").GetProperty("traceabilityJavaRepo").GetString());
        Assert.Equal(javaPath, activity.GetProperty("context").GetProperty("traceabilityJavaPath").GetString());
        Assert.Equal("stakeholder-core", activity.GetProperty("context").GetProperty("traceabilityContractRepo").GetString());
        Assert.Equal("depth", activity.GetProperty("context").GetProperty("traceabilityParityClass").GetString());
        Assert.Contains("Java, Rust, and stakeholder-core", activity.GetProperty("message").GetString()!, StringComparison.Ordinal);
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
