# Coding Conventions

## Core Sections (Required)

### 1) Naming Rules

| Item | Rule | Example | Evidence |
|------|------|---------|----------|
| Files | PascalCase per type/use-case | `CreateRestaurantCommandHandler.cs` | Restaurants.Application/Restaurants/Commands/CreateRestaurant/CreateRestaurantCommandHandler.cs |
| Functions/methods | PascalCase methods, async methods return `Task`/`Task<T>` | `Handle`, `ConfigurePipeline`, `CreateRestaurant` | Restaurants.Application/Restaurants/Commands/CreateRestaurant/CreateRestaurantCommandHandler.cs; Restaurants.API/Extensions/WebApplicationExtensions.cs |
| Types/interfaces | Types in PascalCase; interfaces prefixed with `I` | `RestaurantsDbContext`, `IRestaurantsRepository` | Restaurants.Infrastructure/Persistence/RestaurantsDbContext.cs; Restaurants.Application/Abstractions/Repositories/IRestaurantsRepository.cs |
| Constants/env vars | C# constants/properties in PascalCase; env var convention uses double underscore for nested keys | `UserRoles.Owner`, `ConnectionStrings__RestaurantsDb` | Restaurants.Domain/Constants/UserRoles.cs; README.md |

### 2) Formatting and Linting

- Formatter: [TODO] No explicit formatter config found (`.editorconfig` not found in workspace scan).
- Linter: [TODO] No explicit analyzer/linter config identified in repository root.
- Most relevant enforced rules: Nullable enabled, implicit usings enabled, API model-state auto-validation suppressed in favor of pipeline validation.
- Run commands: `dotnet build Restaurants.sln`.

### 3) Import and Module Conventions

- Import grouping/order: Standard `using` directives at file top; namespaces align to folder/project structure.
- Alias vs relative import policy: Namespace imports plus project references; no alias map/config found.
- Public exports/barrel policy: Not applicable in C# project layout; types are exposed through namespaces and project references.

### 4) Error and Logging Conventions

- Error strategy by layer: Application/Domain throw typed domain exceptions (`NotFoundException`, `ValidationException`); API maps them centrally via `GlobalExceptionHandler`.
- Logging style and required context fields: Structured logging via Serilog and `ILogger<T>` with contextual properties (for example `{Verb}`, `{Path}`, `{Time}` in request-time middleware).
- Sensitive-data redaction rules: [TODO] No explicit redaction policy/config discovered in reviewed files.

### 5) Testing Conventions

- Test file naming/location rule: [TODO] No test project or test file pattern found.
- Mocking strategy norm: [TODO]
- Coverage expectation: [TODO]

### 6) Evidence

- Restaurants.API/Extensions/DependencyInjection.cs
- Restaurants.API/Exceptions/GlobalExceptionHandler.cs
- Restaurants.API/Middlewares/RequestTimeLoggingMiddleware.cs
- Restaurants.Application/Behavior/ValidationBehavior.cs
- Restaurants.Application/Restaurants/Commands/CreateRestaurant/CreateRestaurantCommandHandler.cs
- Restaurants.Infrastructure/Persistence/RestaurantsDbContext.cs
- docs/codebase/.codebase-scan.txt