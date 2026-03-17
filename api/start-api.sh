#!/bin/bash
export ASPNETCORE_ENVIRONMENT=Development
export DOTNET_CLI_TELEMETRY_OPTOUT=1
export DOTNET_NOLOGO=1
export ASPNETCORE_URLS=http://0.0.0.0:8000
export USE_TEST_DATABASE=${USE_TEST_DATABASE:-false}

echo "[api] Starting MMN.Api on port 8000 (this may take a moment to compile)..."

cd /home/runner/workspace/api/MMN.Api
exec dotnet run --no-launch-profile -p:NuGetAudit=false 2>&1
