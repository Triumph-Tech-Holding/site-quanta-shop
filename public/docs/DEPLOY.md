# DEPLOY.md — Guia de Deploy · Quanta Shop

> **Versão:** 1.0.0 | **Atualizado:** Mai 2026
> Guia oficial para publicar a plataforma Quanta Shop no Replit e ambientes de produção.

---

## 1. Visão Geral da Infraestrutura

```
┌─────────────────────────────────────────┐
│           REPLIT (Autoscale)            │
│                                         │
│  ┌──────────────┐  ┌─────────────────┐  │
│  │  Nuxt 3 SSR  │  │  .NET 8 API     │  │
│  │  porta 5000  │  │  porta 8000     │  │
│  └──────┬───────┘  └────────┬────────┘  │
│         │ proxy Nitro        │           │
│         └────────┬───────────┘           │
└──────────────────┼────────────────────── ┘
                   │
          ┌────────▼────────┐
          │  API Produção   │
          │  api.quantashop │
          │    .com.br      │
          └────────┬────────┘
                   │
          ┌────────▼────────┐
          │  SQL Server     │
          │  Azure (nuvem)  │
          └─────────────────┘
```

---

## 2. Pré-requisitos

### 2.1 Variáveis de Ambiente (Secrets Replit)

| Variável | Ambiente | Descrição |
|----------|----------|-----------|
| `NUXT_JWT_SECRET` | Produção | Secret para assinar/verificar JWT |
| `NUXT_API_BASE_URL` | Produção | `https://api.quantashop.com.br` |
| `NUXT_USE_LOCAL_API` | Produção | `false` |
| `NUXT_PUBLIC_GOOGLE_CLIENT_ID` | Ambos | Client ID do Google OAuth |
| `ALLOWED_ORIGINS` | Produção | Lista de domínios CORS permitidos |
| `JWT_ISSUER` | Produção | Issuer do JWT (deve coincidir com a API) |
| `JWT_AUDIENCE` | Produção | Audience do JWT (deve coincidir com a API) |

> ⚠️ **Nunca** colocar secrets em código ou arquivos `.env` commitados.
> Gerencie tudo pelos **Secrets** do Replit (painel lateral → 🔒 Secrets).

### 2.2 Dependências

```bash
# Node.js >= 18
node --version   # deve retornar v18.x ou superior

# pnpm ou npm
npm --version    # >= 9.x

# Instalar dependências
npm install
```

---

## 3. Ambientes

### 3.1 Desenvolvimento (Replit Dev Server)

```bash
# Inicia o dev server Nuxt na porta 5000
npm run dev

# A API .NET 8 em dev (opcional — app funciona sem ela via fallback)
bash api/start-api.sh
```

**Comportamento em dev:**
- API: tenta `localhost:8000` → fallback para `api.quantashop.com.br` (somente leitura recomendada)
- Blog CRUD: escreve em `localStorage` apenas (nunca contamina produção)
- Hot Module Replacement (HMR) ativo

### 3.2 Build de Produção

```bash
# Compilação Nuxt para produção (SSR)
npm run build

# Verificar o output
ls -la .output/
```

**O que o build gera:**
- `.output/server/` — servidor Nitro (Node.js)
- `.output/public/` — assets estáticos

### 3.3 Iniciar em Produção

```bash
# Inicia o servidor SSR compilado
node .output/server/index.mjs

# Ou via script do projeto
bash start-prod.sh
```

---

## 4. Deploy no Replit (Publicação)

### 4.1 Configuração do Replit

O arquivo `.replit` define o workflow de publicação:

```toml
[deployment]
deploymentTarget = "autoscale"
build = ["sh", "-c", "npm run build"]
run = ["sh", "-c", "node .output/server/index.mjs"]
```

### 4.2 Passo a Passo para Publicar

1. **Garantir que o app está funcionando em dev** — nenhum erro no console
2. **Verificar secrets** — todas as variáveis de produção configuradas
3. **Rodar o build** localmente para confirmar:
   ```bash
   npm run build
   ```
4. **Clicar em "Deploy"** no Replit (botão azul no topo direito)
5. **Aguardar o build** — Replit executa `npm run build` automaticamente
6. **Verificar Health Check** — Replit faz ping em `/` após o deploy
7. **Testar em produção** — acessar `https://quantashop.com.br`

### 4.3 Domínio Customizado

- Domínio primário: `https://quantashop.com.br`
- Configurado no Replit: Settings → Custom Domain
- TLS gerenciado automaticamente pelo Replit (Let's Encrypt)

---

## 5. Checklist de Deploy

Antes de cada publicação, verificar:

- [ ] `npm run build` sem erros de TypeScript ou Vue
- [ ] Nenhuma variável hardcoded de ambiente (usar `useRuntimeConfig()`)
- [ ] Secrets de produção atualizados no Replit
- [ ] CHANGELOG.md atualizado com as mudanças desta versão
- [ ] features.json com status correto das features
- [ ] Nenhuma rota nova sem middleware de autenticação
- [ ] LGPD: nenhum dado sensível em logs do frontend
- [ ] Teste de smoke em: `/`, `/agencia/login`, `/agencia/painel/admin`

---

## 6. Rollback

Se houver problema após o deploy:

1. **Via Replit Checkpoint:** clique no ícone de histórico no Replit e reverta para o checkpoint anterior
2. **Via git:** o Replit mantém histórico de commits automáticos
3. **Redeployar versão anterior:** Replit mantém os últimos deployments na aba Deployments

---

## 7. Monitoramento Pós-Deploy

### 7.1 Logs de Produção

```bash
# Logs do servidor Nitro (via Replit Deployments → Logs)
# Filtrar por nível:
ERROR   # problemas críticos
WARN    # degradação de serviço
INFO    # operação normal
```

### 7.2 Endpoints de Health

| Endpoint | Descrição |
|----------|-----------|
| `GET /` | Home — deve retornar 200 |
| `GET /agencia/login` | Login — deve retornar 200 |
| `GET /api-proxy/v2/partners/get-online-partners` | API parceiros — pode fazer fallback |

### 7.3 Erros Comuns

| Erro | Causa | Solução |
|------|-------|---------|
| `500 Internal Server Error` | Secret faltando em produção | Verificar Secrets no Replit |
| `401 Unauthorized` | JWT_ISSUER/AUDIENCE mismatch | Sincronizar com configuração da API .NET |
| `CORS Error` | `ALLOWED_ORIGINS` incompleto | Adicionar domínio à lista |
| `API timeout` | API .NET indisponível | Verificar status de `api.quantashop.com.br` |
| Build falha TypeScript | Tipo incorreto | Corrigir antes de pushar |

---

## 8. CI/CD (Futuro)

> Status: **Planejado** — ainda não implementado.

Fluxo desejado:
```
commit → GitHub Actions → npm run build → dotnet test → deploy automático
```

Por enquanto, o deploy é manual via painel do Replit.

---

## 9. Segurança no Deploy

- **HTTPS obrigatório** — Replit força redirect HTTP → HTTPS
- **Headers de segurança** — configurar em `nuxt.config.ts`:
  ```typescript
  routeRules: {
    '/**': {
      headers: {
        'X-Frame-Options': 'DENY',
        'X-Content-Type-Options': 'nosniff',
        'Referrer-Policy': 'strict-origin-when-cross-origin'
      }
    }
  }
  ```
- **Rate limiting** — middleware `GlobalLimiter` ativo nos endpoints de auth (10 req/min/IP)
- **CORS** — apenas domínios Quanta + Replit Dev Domain permitidos

---

*Atualizar este guia sempre que o processo de deploy mudar ou novas variáveis de ambiente forem adicionadas.*
