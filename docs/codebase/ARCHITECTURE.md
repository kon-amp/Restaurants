# Architecture

## Core Sections (Required)

### 1) Architectural Style

- Primary style: Layered Clean Architecture with CQRS use cases in the Application layer.
- Why this classification: Solution splits into API/Application/Infrastructure/Domain projects and registers MediatR handlers + validation pipeline.
- Primary constraints:
  - API should act as composition/transport layer (`Program.cs`, extension-based startup).
  - Application handlers depend on repository abstractions.
  - Infrastructure owns EF Core + Identity details and implements Application abstractions.

### 2) System Flow

```text
HTTP request -> API controller -> MediatR pipeline + validators -> command/query handler -> repository abstraction -> Infrastructure repository/DbContext -> SQL Server -> HTTP response
```

Flow details (evidence-backed):
1. `Program.Main` builds the web app and configures services/pipeline.
2. Controller action receives request and calls `mediator.Send(...)`.
3. `ValidationBehavior<TRequest,TResponse>` executes FluentValidation validators and throws domain `ValidationException` on failure.
4. Handler performs use-case logic and calls `IRestaurantsRepository`/`IDishesRepository` abstractions.
5. Infrastructure repository executes EF Core operations via `RestaurantsDbContext` and `SaveChangesAsync()`.
6. Global exception handler maps domain exceptions to ProblemDetails responses.

### 3) Layer/Module Responsibilities

| Layer or module | Owns | Must not own | Evidence |
|-----------------|------|--------------|----------|
| Restaurants.API | Endpoint routing, auth attributes, middleware pipeline | Persistence internals and domain data rules | Restaurants.API/Controllers/RestaurantsController.cs; Restaurants.API/Extensions/WebApplicationExtensions.cs |
| Restaurants.Application | Use-case handlers, pipeline validation, DTO mapping, abstractions | Direct `DbContext` usage and HTTP response shaping | Restaurants.Application/Behavior/ValidationBehavior.cs; Restaurants.Application/Abstractions/Repositories/IRestaurantsRepository.cs |
| Restaurants.Infrastructure | EF Core DbContext, SQL Server access, Identity adapters and claims | Controller/request orchestration | Restaurants.Infrastructure/Persistence/RestaurantsDbContext.cs; Restaurants.Infrastructure/Repositories/RestaurantsRepository.cs |
| Restaurants.Domain | Entity model and domain exception contracts | Framework/infrastructure references | Restaurants.Domain/Entities/Restaurant.cs; Restaurants.Domain/Exceptions/ValidationException.cs |

### 4) Reused Patterns

| Pattern | Where found | Why it exists |
|---------|-------------|---------------|
| Repository | `IRestaurantsRepository` + `RestaurantsRepository`, `IDishesRepository` + `DishesRepository` | Isolate Application use cases from EF Core implementation details |
| CQRS (MediatR) | `CreateRestaurantCommand`/`CreateRestaurantCommandHandler`, query handlers in Application | Keep request handling use-case focused and testable by contract |
| Pipeline behavior | `ValidationBehavior<TRequest,TResponse>` | Centralized command/query validation before handler execution |
| Composition root extensions | API `ConfigureServices` and DI extension methods | Keep startup wiring modular and consistent |

### 5) Known Architectural Risks

- Mixed target frameworks (API net9.0; other projects net8.0) increase upgrade/cohesion risk if package/runtime changes diverge.
- `AddAuthentication()` is present in API DI, but no `AddJwtBearer(...)` configuration is visible in source reviewed, which may indicate incomplete or externally configured JWT setup.

### 6) Evidence

- Restaurants.API/Program.cs
- Restaurants.API/Controllers/RestaurantsController.cs
- Restaurants.Application/Behavior/ValidationBehavior.cs
- Restaurants.Application/Restaurants/Commands/CreateRestaurant/CreateRestaurantCommandHandler.cs
- Restaurants.Infrastructure/Repositories/RestaurantsRepository.cs
- Restaurants.Infrastructure/Persistence/RestaurantsDbContext.cs
- Restaurants.API/Exceptions/GlobalExceptionHandler.cs