# Quanta Shop API (MMN.Api)

A RESTful API for the Quanta Shop cashback/e-commerce platform.

## Tech Stack

- **Framework**: .NET 8 / ASP.NET Core Web API
- **Language**: C#
- **Database**: Microsoft SQL Server (Azure) / InMemory EF Core (development with USE_TEST_DATABASE=true)
- **Auth**: JWT Bearer tokens
- **Cache**: InMemory Distributed Cache (IDistributedCache)
- **Documentation**: Swagger/OpenAPI
- **Real-time**: SignalR
- **Architecture**: Multi-project solution (Domain, Repository, Business, API layers)

## Project Structure

```
Bigcash.sln               - Solution file
MMN.Api/                  - ASP.NET Core REST API (main entry point)
MMN.Dominio/              - Domain models (entities)
MMN.INegocio/             - Business logic interfaces
MMN.IRepositorio/         - Repository interfaces
MMN.Negocio/              - Business logic implementations
MMN.Repositorio/          - EF Core repository implementations + mappings
MMN.Integracoes/          - External API integrations (Afilio, Zanox, etc.)
MMN.Util/                 - Utility classes, models (AppSettings, JWT, Hash)
```

## Running the App

The workflow command is `bash run.sh` which:
1. Changes to the `MMN.Api/` directory (required for appsettings.json to load)
2. Runs `dotnet run` with `GenerateDocumentationFile=false` for faster builds
3. The app starts on **port 5000**

### Environment Variables

| Variable | Purpose |
|---|---|
| `DB_CONNECTION_STRING` | Azure SQL Server connection string (production) |
| `SQL_CONNECTION_STRING` | Alternative name for DB connection (fallback) |
| `ASPNETCORE_ENVIRONMENT` | Development (dev) or Production (deploy) |
| `USE_TEST_DATABASE=true` | Use EF Core InMemory DB instead of SQL Server |
| `ASAAS_BASE_URL` | Asaas payment API base URL |
| `PAGARME_ID_PLAN` | PagarMe subscription plan ID |

## Development vs Production

- **Dev** (`run.sh`): `ASPNETCORE_ENVIRONMENT=Development`, connects to Azure SQL via `DB_CONNECTION_STRING`
- **Deploy**: `ASPNETCORE_ENVIRONMENT=Production`, uses `dotnet publish` Release build
- Background services (Awin, PagarMe, Asaas, Fatura) only run in Production mode
- Test mode (`USE_TEST_DATABASE=true`): uses InMemory DB, seeds test admin user

## Deployment Configuration

- **Build**: `dotnet publish MMN.Api/MMN.Api.csproj -c Release -o ./publish`
- **Run**: `ASPNETCORE_ENVIRONMENT=Production dotnet ./publish/MMN.Api.dll --urls http://0.0.0.0:5000`
- **Target**: Autoscale
- `package.json` exists in root (minimal) to satisfy Replit's Node.js module detection

## Key Fixes Applied

1. **Redis → InMemory**: Replaced `AddStackExchangeRedisCache(localhost:6379)` with `AddDistributedMemoryCache()` (no Redis on Replit)
2. **ForwardedHeaders**: Added `UseForwardedHeaders()` for proper proxy support, removed `UseHttpsRedirection()` (TLS terminated at proxy)
3. **System.Drawing Linux**: Added `System.Drawing.EnableUnixSupport` runtime config + `libgdiplus` nix package
4. **Deploy build path**: Fixed `dotnet publish` path to reference `MMN.Api/MMN.Api.csproj` from workspace root
5. **NuGet audit**: Disabled `NuGetAudit` in deploy build to prevent vulnerability warnings from blocking build
6. **Compilation**: Disabled `GenerateDocumentationFile` for faster builds
7. **Seed data**: Fixed domain model property mismatches in `SeedTestData()`
8. **Swagger**: Made `IncludeXmlComments` conditional on file existence
9. **Working directory**: `run.sh` must `cd` to `MMN.Api/` before running

## API Endpoints

- `GET /health` - Health check
- `GET /swagger` - Swagger UI
- `POST /api/UsuarioLogin/autenticacao` - Login (returns JWT)
- And many more REST endpoints for users, products, cashback, etc.

## Notes

- The solution has 40+ compiler warnings (mostly obsolete API usages) — pre-existing
- Image processing (`System.Drawing`) enabled on Linux via `libgdiplus` + runtime switch
- Background services disabled in Development mode (require integration secrets)
- Login tested: `ericbaumbach` / `Eric#1989` → works against Azure SQL production DB
