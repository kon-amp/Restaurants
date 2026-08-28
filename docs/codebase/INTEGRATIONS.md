# External Integrations

## Core Sections (Required)

### 1) Integration Inventory

| System | Type (API/DB/Queue/etc) | Purpose | Auth model | Criticality | Evidence |
|--------|---------------------------|---------|------------|-------------|----------|
| SQL Server (LocalDB in dev config) | DB | Persistent storage for restaurants, dishes, and Identity tables via EF Core | Connection string (`ConnectionStrings:RestaurantsDb`) | High | Restaurants.API/appsettings.Development.json; Restaurants.Infrastructure/DependencyInjection.cs |
| ASP.NET Core Identity endpoints (`api/identity`) | Auth/identity subsystem | User registration/login/password management endpoints | Identity API endpoints + roles/claims | High | Restaurants.API/Extensions/WebApplicationExtensions.cs; Restaurants.Infrastructure/DependencyInjection.cs |
| JWT bearer package reference | Auth transport | Token bearer authentication capability in API dependencies | [TODO] Exact token validation options not found in reviewed source | Medium | Restaurants.API/Restaurants.API.csproj; Restaurants.API/Extensions/DependencyInjection.cs |
| Swagger/OpenAPI | API documentation integration | API contract visibility and testing UI in development | No auth at integration layer; security scheme documented in Swagger | Medium | Restaurants.API/Extensions/DependencyInjection.cs; Restaurants.API/Extensions/WebApplicationExtensions.cs |

### 2) Data Stores

| Store | Role | Access layer | Key risk | Evidence |
|-------|------|--------------|----------|----------|
| SQL Server via EF Core | Source of truth for domain and identity data | Infrastructure (`RestaurantsDbContext`, repositories) | Sensitive data logging is enabled in DbContext options, which can expose details in logs if used outside development | Restaurants.Infrastructure/DependencyInjection.cs; Restaurants.Infrastructure/Persistence/RestaurantsDbContext.cs |

### 3) Secrets and Credentials Handling

- Credential sources: `appsettings.Development.json` and runtime configuration providers (`GetConnectionString("RestaurantsDb")`).
- Hardcoding checks: Development connection string is committed in `appsettings.Development.json`; no secret-store config file found in repo.
- Rotation or lifecycle notes: [TODO] No secret rotation policy discovered in repository files.

### 4) Reliability and Failure Behavior

- Retry/backoff behavior: None found in reviewed integration code.
- Timeout policy: [TODO] No explicit timeout config located for DB/auth integrations.
- Circuit-breaker or fallback behavior: None found.

### 5) Observability for Integrations

- Logging around external calls: Partial; Serilog request logging and slow-request middleware are configured.
- Metrics/tracing coverage: No metrics/tracing integration config found.
- Missing visibility gaps: No explicit integration-level telemetry around DB retries/failures beyond exception handling.

### 6) Evidence

- Restaurants.API/appsettings.Development.json
- Restaurants.Infrastructure/DependencyInjection.cs
- Restaurants.Infrastructure/Persistence/RestaurantsDbContext.cs
- Restaurants.API/Extensions/WebApplicationExtensions.cs
- Restaurants.API/Extensions/DependencyInjection.cs
- Restaurants.API/Restaurants.API.csproj