# dotnet-stakeholder AGENTS

1. Use only .NET CLI; no Visual Studio project types.
2. Required commands:
   - `dotnet build`
   - `dotnet test`
3. Target .NET and C# versions stable for cross-platform CLI.
4. Wire `core/` submodule and load shared spec/fixtures.
5. Keep deterministic hooks: `--seed`, `--output-format`.
6. Match Rust semantics from Rust source; document any intentional divergence in `docs/divergences.md`.
