# Codebase Concerns

## Core Sections (Required)

### 1) Top Risks (Prioritized)

| Severity | Concern | Evidence | Impact | Suggested action |
|----------|---------|----------|--------|------------------|
| high | No automated tests detected in current solution | Restaurants.sln; docs/codebase/.codebase-scan.txt; get_tests result (none found) | Higher regression risk for feature and refactor changes | Add at least one test project (for example Application-level unit tests) and run in CI |
| high | Sensitive data logging enabled in EF Core DbContext options | Restaurants.Infrastructure/DependencyInjection.cs (`EnableSensitiveDataLogging()`) | Potential exposure of sensitive values in logs | Restrict to development-only condition and disable in production |
| medium | Mixed target frameworks across projects (net9 API + net8 libraries) | Restaurants.API/Restaurants.API.csproj; Restaurants.Application/Restaurants.Application.csproj | Upgrade/version drift risk and package compatibility friction | Plan framework alignment strategy and dependency matrix checks |
| medium | JWT package is referenced but concrete JWT bearer option configuration is not visible in reviewed source | Restaurants.API/Restaurants.API.csproj; Restaurants.API/Extensions/DependencyInjection.cs | Auth behavior may depend on defaults/external config, increasing misconfiguration risk | Verify and document explicit token validation parameters |

### 2) Technical Debt

| Debt item | Why it exists | Where | Risk if ignored | Suggested fix |
|-----------|---------------|-------|-----------------|---------------|
| No CI/CD pipeline files detected | Pipeline automation not configured in repository | docs/codebase/.codebase-scan.txt (CI/CD section) | Build/test quality checks may remain manual and inconsistent | Add workflow for restore/build/test and migration checks |
| Legacy/unused middleware file appears alongside current global exception approach | Migration from custom middleware to IExceptionHandler style left older file in tree | Restaurants.API/Middlewares/ErrorHandlingMiddleware.cs; Restaurants.API/Exceptions/GlobalExceptionHandler.cs | Confusion during maintenance about active error path | Clarify and remove/retire unused path if confirmed unused |
| Committed API log files in source tree | Log output folder retained in repo | Restaurants.API/Logs/*.log | Repository noise and possible leakage of runtime data | Ignore/remove runtime logs from source control |

### 3) Security Concerns

| Risk | OWASP category (if applicable) | Evidence | Current mitigation | Gap |
|------|--------------------------------|----------|--------------------|-----|
| Dev connection string stored in repo | A02: Cryptographic Failures / Secrets management (contextual) | Restaurants.API/appsettings.Development.json | Runtime config abstraction via `GetConnectionString` exists | Secrets lifecycle/rotation policy not documented; local dev secret is committed |
| Missing explicit JWT validation configuration in visible source | A07: Identification and Authentication Failures (contextual) | Restaurants.API/Extensions/DependencyInjection.cs | `[Authorize]` attributes and Identity setup are present | Need explicit confirmation of issuer/audience/signing key validation settings |

### 4) Performance and Scaling Concerns

| Concern | Evidence | Current symptom | Scaling risk | Suggested improvement |
|---------|----------|-----------------|-------------|-----------------------|
| `GetAllAsync` loads full restaurants list without paging | Restaurants.Infrastructure/Repositories/RestaurantsRepository.cs | Potential large in-memory result sets | Throughput/memory pressure as table grows | Add pagination/query constraints in use case and repository |
| No retry/timeouts around external dependencies identified | Restaurants.Infrastructure/DependencyInjection.cs; absence of retry policies | Transient failures likely propagate immediately | Lower resiliency under network/database blips | Introduce connection resiliency and explicit timeout settings |
| Slow request logging threshold check uses integer division by seconds | Restaurants.API/Middlewares/RequestTimeLoggingMiddleware.cs | Threshold check may be less precise than direct milliseconds comparison | Observability noise/precision loss | Compare elapsed milliseconds directly with a constant threshold |

### 5) Fragile/High-Churn Areas

| Area | Why fragile | Churn signal | Safe change strategy |
|------|-------------|-------------|----------------------|
| User/identity authorization surface (`IdentityController`, claims principal factory, user role commands) | Rapid recent changes around auth and role management | docs/codebase/.codebase-scan.txt (recent commits include claims/role updates) | Change in small increments; validate authorization paths after each change |
| Startup/DI composition (`WebApplicationExtensions`, `DependencyInjection` in all layers) | Central wiring point affects all requests | Cross-cutting impact by design (startup pipeline + registration) | Keep changes isolated and validate end-to-end build/run immediately |

### 6) `[ASK USER]` Questions

1. [ASK USER] Should this repository standardize all projects on the same target framework version now (for example all net9.0), or keep mixed net8/net9 intentionally?
2. [ASK USER] Do you want `EnableSensitiveDataLogging()` limited to development only as a policy requirement?
3. [ASK USER] Where is the canonical JWT bearer validation setup (issuer/audience/key) expected to live for this repo?
4. [ASK USER] Should the committed `Restaurants.API/Logs/*.log` files be removed and ignored moving forward?

### 7) Evidence

- docs/codebase/.codebase-scan.txt
- Restaurants.sln
- Restaurants.API/Restaurants.API.csproj
- Restaurants.Application/Restaurants.Application.csproj
- Restaurants.Infrastructure/DependencyInjection.cs
- Restaurants.Infrastructure/Repositories/RestaurantsRepository.cs
- Restaurants.API/Middlewares/RequestTimeLoggingMiddleware.cs
- Restaurants.API/appsettings.Development.json
- Restaurants.API/Exceptions/GlobalExceptionHandler.cs