#!/bin/bash
set -e

export DOTNET_CLI_TELEMETRY_OPTOUT=1
export DOTNET_NOLOGO=1
export ASPNETCORE_ENVIRONMENT=Production
export ASPNETCORE_URLS=http://0.0.0.0:8000

echo "[start-prod] === Iniciando Quanta Shop (API .NET + Nuxt) ==="

DB_SERVER="tcp:bigcash.database.windows.net,1433"
DB_NAME="Bigcash"
DB_USER="bigcash"

RAW_SECRET=$(echo "$SQL_CONNECTION_STRING" | sed 's/^[[:space:]]*//;s/[[:space:]]*$//')

if [ -n "$RAW_SECRET" ]; then
  if echo "$RAW_SECRET" | grep -q ";"; then
    export SQL_CONNECTION_STRING=$(echo "$RAW_SECRET" | sed 's/Password \([^;]*\)/Password=\1/g')
    echo "[start-prod] Usando connection string completa do secret."
  else
    export SQL_CONNECTION_STRING="Server=${DB_SERVER};Initial Catalog=${DB_NAME};Persist Security Info=False;User ID=${DB_USER};Password=${RAW_SECRET};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
    echo "[start-prod] Connection string montada a partir da senha."
  fi
else
  echo "[start-prod] WARNING: SQL_CONNECTION_STRING nao configurado — API iniciara sem banco."
fi

API_BINARY="/home/runner/workspace/api/publish/MMN.Api"

if [ ! -f "$API_BINARY" ]; then
  echo "[start-prod] ERRO: Binario publicado nao encontrado em $API_BINARY"
  echo "[start-prod] Execute build-prod.sh para compilar a API antes de iniciar."
  exit 1
fi

echo "[start-prod] Iniciando API via binario publicado..."
"$API_BINARY" &
API_PID=$!
echo "[start-prod] API PID: $API_PID. Aguardando ficar saudavel na porta 8000..."

MAX_WAIT=120
ELAPSED=0
INTERVAL=3

while [ $ELAPSED -lt $MAX_WAIT ]; do
  if curl -sf "http://localhost:8000/api/v2/carousels" -o /dev/null 2>/dev/null; then
    echo "[start-prod] API saudavel apos ${ELAPSED}s."
    break
  fi
  sleep $INTERVAL
  ELAPSED=$((ELAPSED + INTERVAL))
done

if [ $ELAPSED -ge $MAX_WAIT ]; then
  echo "[start-prod] ERRO: API nao ficou saudavel em ${MAX_WAIT}s. Abortando."
  kill $API_PID 2>/dev/null
  exit 1
fi

echo "[start-prod] Iniciando servidor Nuxt na porta 5000..."
exec node /home/runner/workspace/.output/server/index.mjs
