FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore dotnet-stakeholder.sln
RUN dotnet test dotnet-stakeholder.sln
RUN dotnet publish src/DotnetStakeholder/DotnetStakeholder.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:10.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "DotnetStakeholder.dll"]
