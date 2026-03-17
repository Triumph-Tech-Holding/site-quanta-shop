#!/bin/bash
export ASPNETCORE_ENVIRONMENT=Development
export DOTNET_CLI_TELEMETRY_OPTOUT=1
export DOTNET_NOLOGO=1
export ASPNETCORE_URLS=http://0.0.0.0:8000
export USE_TEST_DATABASE=${USE_TEST_DATABASE:-false}

# Build the SQL connection string from the secret.
# The secret (SQL_CONNECTION_STRING) can be either:
#   a) The full ADO.NET connection string (contains semicolons)
#   b) Just the password (simpler to paste in Replit's secrets form)
#
# If it's just the password, we build the full string automatically.
# The server, database, and user are fixed for this project.

DB_SERVER="tcp:bigcash.database.windows.net,1433"
DB_NAME="Bigcash"
DB_USER="bigcash"

RAW_SECRET=$(echo "$SQL_CONNECTION_STRING" | sed 's/^[[:space:]]*//;s/[[:space:]]*$//')

if [ -n "$RAW_SECRET" ]; then
  if echo "$RAW_SECRET" | grep -q ";"; then
    # Full connection string provided — fix missing "=" after Password if needed
    export SQL_CONNECTION_STRING=$(echo "$RAW_SECRET" | sed 's/Password \([^;]*\)/Password=\1/g')
    echo "[api] Using full connection string from secret."
  else
    # Just the password provided — build the full connection string
    export SQL_CONNECTION_STRING="Server=${DB_SERVER};Initial Catalog=${DB_NAME};Persist Security Info=False;User ID=${DB_USER};Password=${RAW_SECRET};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
    echo "[api] Built connection string from password secret."
  fi
else
  echo "[api] WARNING: SQL_CONNECTION_STRING is not set — running with in-memory database."
fi

echo "[api] Starting MMN.Api on port 8000 (this may take a moment to compile)..."

cd /home/runner/workspace/api/MMN.Api
exec dotnet run --no-launch-profile -p:NuGetAudit=false 2>&1
