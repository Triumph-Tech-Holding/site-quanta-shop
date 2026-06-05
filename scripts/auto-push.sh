#!/usr/bin/env bash
# Auto-push — mantém o GitHub sempre em dia, de graça.
# • Só faz commit/push QUANDO há mudança real (parado = custo zero).
# • NUNCA faz deploy. NUNCA usa --force.
# • Integra o remoto com segurança antes de enviar.
# • Roda uma única instância, em segundo plano junto com o app.
set -uo pipefail

INTERVAL="${AUTO_PUSH_INTERVAL:-600}"   # segundos entre verificações (padrão: 10 min)
BRANCH="$(git rev-parse --abbrev-ref HEAD 2>/dev/null || echo main)"
LOCK="/tmp/auto-push.lock"

if [ -e "$LOCK" ] && kill -0 "$(cat "$LOCK" 2>/dev/null)" 2>/dev/null; then
  echo "[auto-push] já está rodando. Saindo."
  exit 0
fi
echo $$ > "$LOCK"
trap 'rm -f "$LOCK"' EXIT

git config user.name  >/dev/null 2>&1 || git config user.name  "Auto-Push Bot"
git config user.email >/dev/null 2>&1 || git config user.email "bot@triumphtech.local"

# Montar URL autenticada em memória (token nunca gravado em arquivo)
if [ -z "${GITHUB_PAT:-}" ]; then
  echo "[auto-push] ERRO: variável GITHUB_PAT não definida. Configure o secret no Replit."
  exit 1
fi
REPO_SLUG="31maurosergio-byte/site-quanta-shop"
AUTH_URL="https://31maurosergio-byte:${GITHUB_PAT}@github.com/${REPO_SLUG}.git"

echo "[auto-push] vigiando a cada ${INTERVAL}s na branch ${BRANCH}."

while true; do
  if [ -n "$(git status --porcelain 2>/dev/null)" ]; then
    git add -A
    git commit -m "auto: snapshot $(date '+%Y-%m-%d %H:%M:%S')" >/dev/null 2>&1
    if ! git pull --rebase --autostash "$AUTH_URL" "$BRANCH" >/dev/null 2>&1; then
      git rebase --abort >/dev/null 2>&1 || true
      echo "[auto-push] conflito ao integrar o remoto — push adiado. Resolver manualmente."
      sleep "$INTERVAL"; continue
    fi
    if git push "$AUTH_URL" "$BRANCH" >/dev/null 2>&1; then
      echo "[auto-push] enviado para ${BRANCH} em $(date '+%H:%M:%S')."
    else
      echo "[auto-push] falha no push — verifique o GITHUB_PAT e as permissões do repositório."
    fi
  fi
  sleep "$INTERVAL"
done
