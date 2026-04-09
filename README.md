> [!IMPORTANT]
> This repository is part of a Codex-assisted rewrite experiment. All changes are manually reviewed, a human remains in the loop, and missing behavior is tracked explicitly rather than hidden. The project exists for fun, research, language learning, AI agent workflow/planning, interop experiments, and code review testing.
# dotnet-stakeholder

`dotnet-stakeholder` is now the widened classic-six follower for the stakeholder rewrite.

## Commands
- `dotnet run --project src/DotnetStakeholder/DotnetStakeholder.csproj -- --list-values`
- `dotnet run --project src/DotnetStakeholder/DotnetStakeholder.csproj -- --dev-type backend --complexity medium`
- `dotnet test tests/DotnetStakeholder.Tests/DotnetStakeholder.Tests.csproj`
- `dotnet build src/DotnetStakeholder/DotnetStakeholder.csproj`
- `docker build -t dotnet-stakeholder .`
- `docker run --rm dotnet-stakeholder --list-values`

## Notes
- Classic-six now has dedicated renderers for `code_analyzer`, `data_processing`, `jargon`, `metrics`, `network_activity`, and `system_monitoring`.
- `agent_workflows` remains dedicated in the modern-core lane.
- Experimental provider flags are parsed and fail fast until that runtime lands.
- The static CLI and normalized JSON contract are shared with the web and future follower ports.
