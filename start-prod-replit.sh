#!/bin/bash
set -e

export NUXT_USE_LOCAL_API=false

echo "[start-prod-replit] Iniciando servidor Nuxt (API remota Azure)"
echo "[start-prod-replit] Porta 5000, HOST 0.0.0.0"

exec node /home/runner/workspace/.output/server/index.mjs
