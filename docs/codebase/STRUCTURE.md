# Codebase Structure

## Core Sections (Required)

### 1) Top-Level Map

| Path | Purpose | Evidence |
|------|---------|----------|
| .github/ | Repository instructions, workflows, and skills assets | docs/codebase/.codebase-scan.txt (directory tree) |
| docs/codebase/ | Generated onboarding/codebase knowledge docs | docs/codebase/.codebase-scan.txt |
| Restaurants.API/ | Presentation layer: controllers, middleware, API host bootstrapping | Restaurants.API/Program.cs; Restaurants.API/Extensions/WebApplicationExtensions.cs |
| Restaurants.Application/ | Use-case layer (CQRS handlers, validators, DTOs, abstractions) | Restaurants.Application/DependencyInjection.cs; docs/codebase/.codebase-scan.txt |
| Restaurants.Infrastructure/ | Persistence, repositories, Identity integrations, seed data | Restaurants.Infrastructure/DependencyInjection.cs; Restaurants.Infrastructure/Persistence/RestaurantsDbContext.cs |
| Restaurants.Domain/ | Domain entities, constants, domain exceptions | Restaurants.Domain/Entities/Restaurant.cs; Restaurants.Domain/Exceptions/NotFoundException.cs |
| README.md | Primary onboarding and architecture intent document | README.md |
| Restaurants.sln | Solution composition and project relationships | Restaurants.sln |

### 2) Entry Points

- Main runtime entry: Restaurants.API/Program.cs
- Secondary entry points (worker/cli/jobs): None found.
- How entry is selected (script/config): `Main` builds `WebApplicationBuilder`, then runs `ConfigureServices().ConfigurePipeline()` and `RunAsync()` in API.

### 3) Module Boundaries

| Boundary | What belongs here | What must not be here |
|----------|-------------------|------------------------|
| Restaurants.API | HTTP endpoints, middleware, OpenAPI, auth/exception pipeline wiring | Domain/business rules and direct DbContext data access |
| Restaurants.Application | MediatR commands/queries/handlers, validation behavior, DTO mapping, interfaces | EF Core concrete persistence or ASP.NET host concerns |
| Restaurants.Infrastructure | DbContext, repository implementations, Identity and claims integration | Controller logic and API transport concerns |
| Restaurants.Domain | Entities, constants, domain exception types | Dependencies on Application/Infrastructure/API |

### 4) Naming and Organization Rules

- File naming pattern: PascalCase C# files (for example `CreateRestaurantCommandHandler.cs`, `RestaurantsDbContext.cs`).
- Directory organization pattern: Hybrid layer-first (project boundaries) plus feature folders inside Application (`Restaurants`, `Dishes`, `User`).
- Import aliasing or path conventions: Namespace-based imports with relative project references; no aliasing config file found.

### 5) Evidence

- docs/codebase/.codebase-scan.txt
- Restaurants.sln
- Restaurants.API/Program.cs
- Restaurants.API/Extensions/WebApplicationExtensions.cs
- Restaurants.Application/DependencyInjection.cs
- Restaurants.Infrastructure/DependencyInjection.cs
- Restaurants.Domain/Entities/Restaurant.cs