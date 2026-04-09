# Docker

The repo ships a single-stage build/test Dockerfile with a runtime image entrypoint.

## Commands
- `docker build -t dotnet-stakeholder .`
- `docker run --rm dotnet-stakeholder --list-values`

## CI intent
- Build and test in the SDK image.
- Smoke the published runtime image by invoking `--list-values`.
