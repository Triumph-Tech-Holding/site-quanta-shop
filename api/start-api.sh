#!/bin/bash
export ASPNETCORE_ENVIRONMENT=Development
export DOTNET_CLI_TELEMETRY_OPTOUT=1
export DOTNET_NOLOGO=1
export ASPNETCORE_URLS=http://0.0.0.0:8000
export USE_TEST_DATABASE=${USE_TEST_DATABASE:-false}

# Fix: Replit secrets form sometimes drops the "=" sign from "Password=VALUE"
# resulting in "Password VALUE" instead of "Password=VALUE"
if [ -n "$SQL_CONNECTION_STRING" ]; then
  export SQL_CONNECTION_STRING=$(echo "$SQL_CONNECTION_STRING" | sed 's/Password \([^;]*\)/Password=\1/g')
fi

echo "[api] Starting MMN.Api on port 8000 (this may take a moment to compile)..."

cd /home/runner/workspace/api/MMN.Api
exec dotnet run --no-launch-profile -p:NuGetAudit=false 2>&1
