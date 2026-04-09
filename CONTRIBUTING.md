# Contributing

Follow the shared contract in `core/` and do not implement parity behavior without traceability rows.

## Working commands
- `dotnet format dotnet-stakeholder.sln --verify-no-changes`
- `dotnet build dotnet-stakeholder.sln`
- `dotnet test dotnet-stakeholder.sln`
- `docker build -t dotnet-stakeholder .`
- `docker run --rm dotnet-stakeholder --list-values`
