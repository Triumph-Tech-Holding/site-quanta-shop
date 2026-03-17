#!/bin/bash
set -e
export ASPNETCORE_ENVIRONMENT=Production
export DOTNET_CLI_TELEMETRY_OPTOUT=1
export DOTNET_NOLOGO=1

echo "[start.sh] Starting MMN.Api in Production mode..."
cd ./publish
exec dotnet MMN.Api.dll --urls http://0.0.0.0:5000
