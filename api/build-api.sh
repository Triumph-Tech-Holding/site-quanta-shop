#!/bin/bash
set -e
export DOTNET_CLI_TELEMETRY_OPTOUT=1
export DOTNET_NOLOGO=1

echo "[api] Restoring packages..."
cd /home/runner/workspace/api
dotnet restore MMN.Api/MMN.Api.csproj -p:NuGetAudit=false --nologo

echo "[api] Building and publishing..."
dotnet publish MMN.Api/MMN.Api.csproj \
  -c Release \
  -o ./publish \
  -p:GenerateDocumentationFile=false \
  -p:NuGetAudit=false \
  --nologo

echo "[api] Build complete. Run with: dotnet publish/MMN.Api.dll --urls http://0.0.0.0:5001"
