#!/bin/bash
set -e
export DOTNET_CLI_TELEMETRY_OPTOUT=1
export DOTNET_NOLOGO=1

echo "[build.sh] Publishing MMN.Api in Release mode..."
dotnet publish MMN.Api/MMN.Api.csproj \
    -c Release \
    -o ./publish \
    -p:GenerateDocumentationFile=false \
    -p:NuGetAudit=false

echo "[build.sh] Build completed successfully."
