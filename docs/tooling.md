# Tooling

## Standard commands
- `dotnet format dotnet-stakeholder.sln --verify-no-changes`
- `dotnet build dotnet-stakeholder.sln`
- `dotnet test dotnet-stakeholder.sln`
- `docker build -t dotnet-stakeholder .`
- `docker run --rm dotnet-stakeholder --list-values`

## Notes
- `Directory.Build.props` enables warning-as-error behavior and latest analysis for the scaffold.
- `dotnet format` is the formatter/linter gate for C# sources in this repo.
