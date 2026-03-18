#!/bin/bash
set -e
export DOTNET_CLI_TELEMETRY_OPTOUT=1
export DOTNET_NOLOGO=1

echo "[build-prod] === Build unificado: API .NET + Nuxt ==="

echo "[build-prod] Publicando MMN.Api em Release..."
cd /home/runner/workspace/api
bash build.sh
cd /home/runner/workspace

echo "[build-prod] Build da API concluido. Publicado em: api/publish/"

echo "[build-prod] Build do Nuxt..."
npm run build:nuxt

echo "[build-prod] === Build concluido com sucesso ==="
