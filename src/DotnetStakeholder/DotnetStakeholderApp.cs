using System.Text.Json;

namespace DotnetStakeholder;

public static class DotnetStakeholderApp
{
    private static readonly SessionScheduler Scheduler = new();
    private static readonly RendererRegistry Renderers = new();

    public static int Run(string[] args)
    {
        try
        {
            var parse = Parse(args);
            if (parse.ShowHelp)
            {
                PrintHelp();
                return 0;
            }

            if (parse.ExperimentalConfig.HasAnyFlag)
            {
                throw new ExperimentalProviderNotImplementedException(BuildExperimentalProviderMessage(parse.ExperimentalConfig));
            }

            if (parse.ListValues)
            {
                Console.WriteLine(DotnetStakeholderJson.SerializeListValues(DotnetStakeholderJson.BuildListValues()));
                return 0;
            }

            var session = RunSession(parse.SessionConfig);
            if (parse.SessionConfig.OutputFormat == OutputFormat.Json)
            {
                Console.WriteLine(DotnetStakeholderJson.SerializeSession(session));
            }
            else
            {
                Console.WriteLine(DotnetStakeholderJson.FormatTextSession(session));
            }

            return 0;
        }
        catch (ExperimentalProviderNotImplementedException exception)
        {
            Console.Error.WriteLine(exception.Message);
            return 2;
        }
        catch (CommandLineException exception)
        {
            Console.Error.WriteLine(exception.Message);
            return 2;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception.Message);
            return 2;
        }
    }

    public static SessionResult RunSession(SessionConfig config)
    {
        var rng = new SeededRandom(BuildSeed(config));
        var selectedFamilies = Scheduler.SelectFamilies(config);
        var events = new List<SessionEvent>();
        var startedAt = DateTimeOffset.UtcNow;
        var sessionId = $"dotnet-{Math.Abs(BuildSeed(config).GetHashCode()):x}";

        AddEvent("session.start", $"session started for {config.DevType}", new Dictionary<string, object?>
        {
            ["sessionId"] = sessionId,
            ["devType"] = config.DevType.ToString(),
            ["complexity"] = config.Complexity.ToString(),
            ["jargon"] = config.Jargon.ToString()
        }, "baseline", events, startedAt, rng);

        AddEvent("session.plan", $"selected {selectedFamilies.Count} generator lanes", new Dictionary<string, object?>
        {
            ["families"] = selectedFamilies.Select(family => family.Id).ToArray(),
            ["devType"] = config.DevType.ToString()
        }, "baseline", events, startedAt, rng);

        var ordinal = 0;
        foreach (var family in selectedFamilies)
        {
            ordinal++;
            var renderer = Renderers.Get(family.Id);
            var output = renderer.Render(family, config, rng, ordinal);
            var context = new Dictionary<string, object?>
            {
                ["family"] = family.Id,
                ["familyLabel"] = family.Label,
                ["group"] = family.Group.ToString(),
                ["renderer"] = renderer.RendererKey,
                ["devType"] = config.DevType.ToString(),
                ["smoke"] = family.Smoke,
                ["seed"] = config.Seed
            };

            foreach (var pair in output.Context)
            {
                context[pair.Key] = pair.Value;
            }

            AddEvent(
                "generator.activity",
                output.Message,
                context,
                renderer.RendererKey,
                events,
                startedAt,
                rng,
                output.Terminal);

            if (config.Trace)
            {
                AddEvent(
                    "generator.trace",
                    $"{family.Id} trace row",
                    new Dictionary<string, object?>
                    {
                        ["family"] = family.Id,
                        ["group"] = family.Group.ToString(),
                        ["renderer"] = renderer.RendererKey,
                        ["trace"] = true
                    },
                    renderer.RendererKey,
                    events,
                    startedAt,
                    rng,
                    output.Terminal);
            }
        }

        AddEvent("session.end", "deterministic session complete", new Dictionary<string, object?>
        {
            ["status"] = "ok",
            ["durationEvents"] = events.Count
        }, "baseline", events, startedAt, rng);

        return new SessionResult
        {
            SessionId = sessionId,
            Mode = "static",
            Config = config,
            SelectedFamilies = selectedFamilies.Select(family => family.Id).ToArray(),
            Events = events
        };
    }

    private static ParseResult Parse(string[] args)
    {
        var argsList = args.ToList();
        var showHelp = argsList.Contains("--help");
        var listValues = argsList.Contains("--list-values");

        if (TryGetAnyExperimentalFlag(argsList, out var experimental))
        {
            experimental = experimental with { HasAnyFlag = true };
            return new ParseResult
            {
                ShowHelp = showHelp,
                ListValues = listValues,
                SessionConfig = ParseSessionConfig(argsList),
                ExperimentalConfig = experimental
            };
        }

        return new ParseResult
        {
            ShowHelp = showHelp,
            ListValues = listValues,
            SessionConfig = ParseSessionConfig(argsList),
            ExperimentalConfig = new ExperimentalConfig { HasAnyFlag = false }
        };
    }

    private static SessionConfig ParseSessionConfig(IReadOnlyList<string> args)
    {
        return new SessionConfig
        {
            DevType = ParseEnum(args, "--dev-type", DevType.Fullstack),
            Complexity = ParseEnum(args, "--complexity", Complexity.Medium),
            Jargon = ParseEnum(args, "--jargon", JargonLevel.Normal),
            OutputFormat = ParseEnum(args, "--output-format", OutputFormat.Text),
            Seed = GetValue(args, "--seed") ?? "stakeholder-2026",
            Project = GetValue(args, "--project") ?? "stakeholder",
            Framework = GetValue(args, "--framework") ?? "dotnet-stakeholder",
            FocusFamily = GetValue(args, "--focus-family"),
            Alerts = HasFlag(args, "--alerts"),
            Team = HasFlag(args, "--team"),
            Minimal = HasFlag(args, "--minimal"),
            Trace = HasFlag(args, "--trace") || !HasFlag(args, "--minimal")
        };
    }

    private static bool TryGetAnyExperimentalFlag(IReadOnlyList<string> args, out ExperimentalConfig experimental)
    {
        var provider = GetValue(args, "--experimental-provider");
        var model = GetValue(args, "--experimental-model");
        var profile = GetValue(args, "--experimental-profile");
        var prompt = GetValue(args, "--experimental-prompt");
        var adapterModeText = GetValue(args, "--experimental-adapter-mode");
        var any = provider is not null || model is not null || profile is not null || prompt is not null || adapterModeText is not null || HasFlag(args, "--experimental-provider") || HasFlag(args, "--experimental-model") || HasFlag(args, "--experimental-profile") || HasFlag(args, "--experimental-prompt") || HasFlag(args, "--experimental-adapter-mode");

        experimental = new ExperimentalConfig
        {
            Provider = provider,
            Model = model,
            Profile = profile,
            Prompt = prompt,
            AdapterMode = ParseEnum(args, "--experimental-adapter-mode", ExperimentalAdapterMode.Api),
            HasAnyFlag = any
        };

        return any;
    }

    private static string BuildExperimentalProviderMessage(ExperimentalConfig experimental)
    {
        var provider = experimental.Provider ?? "experimental-provider";
        var details = string.Join(", ",
            new[]
            {
                experimental.Provider is not null ? $"provider={experimental.Provider}" : null,
                experimental.Model is not null ? $"model={experimental.Model}" : null,
                experimental.Profile is not null ? $"profile={experimental.Profile}" : null,
                experimental.Prompt is not null ? $"prompt={experimental.Prompt}" : null,
                $"adapter-mode={experimental.AdapterMode.ToString().ToLowerInvariant()}"
            }.Where(value => value is not null)!);
        return $"experimental-provider is not implemented yet in dotnet-stakeholder. Requested {provider}{(details.Length > 0 ? $" ({details})" : string.Empty)}.";
    }

    private static void PrintHelp()
    {
        Console.WriteLine("Usage: dotnet-stakeholder [options]");
        Console.WriteLine("  --list-values");
        Console.WriteLine("  --dev-type <backend|blockchain|data-science|dev-ops|frontend|fullstack|game-development|machine-learning|security|systems-programming>");
        Console.WriteLine("  --complexity <low|medium|high|extreme>");
        Console.WriteLine("  --jargon <low|normal|high|extreme>");
        Console.WriteLine("  --output-format <text|json>");
        Console.WriteLine("  --seed <value>");
        Console.WriteLine("  --focus-family <family-id>");
        Console.WriteLine("  --alerts");
        Console.WriteLine("  --team");
        Console.WriteLine("  --minimal");
        Console.WriteLine("  --trace");
        Console.WriteLine("  experimental provider flags are parsed but fail fast");
    }

    private static void AddEvent(
        string eventType,
        string message,
        Dictionary<string, object?> context,
        string renderer,
        ICollection<SessionEvent> events,
        DateTimeOffset startedAt,
        SeededRandom rng,
        string? terminal = null)
    {
        var sequence = events.Count + 1;
        var provenance = new Dictionary<string, object?>
        {
            ["provider"] = "local",
            ["model"] = "deterministic",
            ["adapterMode"] = "api",
            ["promptVersion"] = "baseline",
            ["cache"] = "n/a",
            ["personalizationProfile"] = "baseline",
            ["renderer"] = renderer
        };

        var eventItem = new SessionEvent
        {
            EventType = eventType,
            Sequence = sequence,
            Message = message,
            Timestamp = startedAt.AddSeconds(sequence).UtcDateTime.ToString("O"),
            Context = context,
            Provenance = provenance,
            Terminal = terminal ?? FormatAnsiLine(sequence, message, context, renderer, rng)
        };
        events.Add(eventItem);
    }

    private static string BuildSeed(SessionConfig config) =>
        string.Join(':', config.Seed, config.DevType, config.Complexity, config.Jargon, config.FocusFamily ?? string.Empty, config.Project, config.Framework, config.Alerts, config.Team);

    private static string FormatAnsiLine(int sequence, string message, IReadOnlyDictionary<string, object?> context, string renderer, SeededRandom rng)
    {
        var accent = renderer.Contains("code_analyzer", StringComparison.OrdinalIgnoreCase) ? "\u001b[38;5;81m"
            : renderer.Contains("agent_workflows", StringComparison.OrdinalIgnoreCase) ? "\u001b[38;5;213m"
            : renderer.Contains("health", StringComparison.OrdinalIgnoreCase) ? "\u001b[38;5;82m"
            : renderer.Contains("security", StringComparison.OrdinalIgnoreCase) ? "\u001b[38;5;214m"
            : "\u001b[38;5;177m";
        var reset = "\u001b[0m";
        var muted = "\u001b[2m";
        var detail = context.TryGetValue("family", out var family) ? $"{muted}{family}{reset}" : $"{muted}{renderer}{reset}";
        var suffix = rng.Next(2) == 0 ? $"{muted}trace{reset}" : $"{muted}registry{reset}";
        return $"{muted}{sequence:000}{reset} {accent}{message}{reset} {detail} {suffix}";
    }

    private static bool HasFlag(IReadOnlyList<string> args, string name)
    {
        for (var index = 0; index < args.Count; index++)
        {
            var arg = args[index];
            if (arg.Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (arg.StartsWith(name + "=", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    private static string? GetValue(IReadOnlyList<string> args, string name)
    {
        for (var index = 0; index < args.Count; index++)
        {
            var arg = args[index];
            if (arg.Equals(name, StringComparison.OrdinalIgnoreCase) && index + 1 < args.Count)
            {
                return args[index + 1];
            }

            if (arg.StartsWith(name + "=", StringComparison.OrdinalIgnoreCase))
            {
                return arg[(name.Length + 1)..];
            }
        }

        return null;
    }

    private static TEnum ParseEnum<TEnum>(IReadOnlyList<string> args, string name, TEnum fallback)
        where TEnum : struct, Enum
    {
        var value = GetValue(args, name);
        if (string.IsNullOrWhiteSpace(value))
        {
            return fallback;
        }

        if (Enum.TryParse<TEnum>(NormalizeToken(value), true, out var parsed))
        {
            return parsed;
        }

        throw new CommandLineException($"Invalid value '{value}' for {name}.");
    }

    private static string NormalizeToken(string value) =>
        value.Replace("-", string.Empty, StringComparison.Ordinal).Replace("_", string.Empty, StringComparison.Ordinal);
}
