#!/bin/bash
export ASPNETCORE_ENVIRONMENT=Development
export DOTNET_CLI_TELEMETRY_OPTOUT=1
export DOTNET_NOLOGO=1

cd /home/runner/workspace/MMN.Api

echo "[run.sh] Starting MMN.Api (building if needed)..."
exec dotnet run \
    --project MMN.Api.csproj \
    --no-launch-profile \
    -p:GenerateDocumentationFile=false \
    -p:NoWarn=NU1701%3BNU1902%3BNU1903%3BRAZORSDK1006
