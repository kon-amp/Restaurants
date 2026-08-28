# Copilot Instructions — Restaurants API

These instructions give AI coding agents (GitHub Copilot, Copilot Chat, Copilot
coding agent) the context needed to work productively in this repository. Follow
them for every change unless the user explicitly overrides them.

## Project summary

ASP.NET Core REST API (target framework **net9.0**) for managing **Restaurants**
and their **Dishes**, with ASP.NET Core Identity (JWT Bearer) for auth. Stack:
EF Core (SQL Server), MediatR (CQRS), AutoMapper, FluentValidation, Serilog,
Swagger/Swashbuckle.

## Solution / Clean Architecture layout

Four projects, dependencies point **inward** toward `Restaurants.Domain`:

- `Restaurants.API` — controllers, middleware, filters, Swagger, JWT auth, DI
  composition root (`Program.cs`, `Extensions/WebApplicationExtensions.cs`,
  `Extensions/DependencyInjection.cs`). References `Restaurants.Infrastructure`
  **only** at the composition root to wire DI — never add business logic here
  that depends directly on Infrastructure types.
- `Restaurants.Application` — use cases via MediatR CQRS: `Restaurants/Commands/*`,
  `Restaurants/Queries/*`, `Restaurants/Dtos`, `Behavior/ValidationBehavior`,
  `Abstractions/Repositories`, `Abstractions/User`.
- `Restaurants.Infrastructure` — EF Core (`Persistence/RestaurantsDbContext`),
  `Repositories`, `Identity`, `Seeders`, `Authorization`.
- `Restaurants.Domain` — `Entities` (`Restaurant`, `Dish`, `Address`),
  `Constants` (`UserRoles`), `Exceptions` (`NotFoundException`, `ValidationException`).
  No outward dependencies — keep it that way.

When adding a feature, respect this direction: Domain has no dependencies;
Application depends only on Domain; Infrastructure depends on Application +
Domain; API depends on Application (and Infrastructure only for DI wiring).

## Project hierarchy

```
Restaurants/
├── Restaurants.sln
├── README.md
├── .github/
│   ├── copilot-instructions.md
│   └── workflows/
├── Restaurants.API/                     # Presentation layer
│   ├── Controllers/
│   ├── Extensions/                      # DependencyInjection.cs, WebApplicationExtensions.cs
│   ├── Filters/
│   ├── Middlewares/                     # RequestTimeLoggingMiddleware
│   ├── Exceptions/                      # GlobalExceptionHandler
│   ├── Properties/
│   ├── Logs/                            # Serilog rolling file output
│   ├── Program.cs
│   ├── appsettings.json
│   └── appsettings.Development.json
├── Restaurants.Application/              # Use cases (CQRS via MediatR)
│   ├── Abstractions/
│   │   ├── Repositories/                 # IRestaurantsRepository, IDishesRepository
│   │   └── User/                         # IUserContext, etc.
│   ├── Behavior/                         # ValidationBehavior<TRequest, TResponse>
│   ├── Restaurants/
│   │   ├── Commands/
│   │   │   ├── CreateRestaurant/ (+ Validators/)
│   │   │   ├── UpdateRestaurant/ (+ Validators/)
│   │   │   └── DeleteRestaurant/
│   │   ├── Queries/
│   │   │   ├── GetAllRestaurants/
│   │   │   └── GetRestaurantById/
│   │   └── Dtos/
│   ├── Dishes/
│   │   ├── Commands/
│   │   │   ├── CreateDish/ (+ Validators/)
│   │   │   └── DeleteDishes/
│   │   ├── Queries/
│   │   │   ├── GetDishByIdForRestaurant/
│   │   │   └── GetDishesForRestaurant/
│   │   └── Dtos/
│   └── User/
│       └── Commands/
│           ├── AssignUserRole/ (+ Validators/)
│           ├── UnnasignUserRole/ (+ Validators/)
│           └── UpdateUserDetails/
├── Restaurants.Infrastructure/            # EF Core, Identity, repositories
│   ├── Persistence/                       # RestaurantsDbContext
│   ├── Repositories/                       # RestaurantsRepository, DishesRepository
│   ├── Identity/
│   ├── Authorization/
│   ├── Seeders/                            # RestaurantSeeder
│   └── Migrations/
└── Restaurants.Domain/                    # Innermost layer, no outward dependencies
    ├── Entities/                           # Restaurant, Dish, Address
    ├── Constants/                          # UserRoles
    └── Exceptions/                         # NotFoundException, ValidationException
```

`bin/`, `obj/`, and `.vs/` are build/IDE artifacts — never edit or commit
changes to their contents. New features under `Restaurants` or `Dishes`
follow the same `Commands/<UseCase>/Validators` and `Queries/<UseCase>` shape
shown above; new top-level feature areas should get their own folder as a
sibling of `Restaurants/` and `Dishes/` under `Restaurants.Application` (e.g.
`User/`).

## CQRS / feature conventions (mandatory for new use cases)

Place new use cases under `Restaurants.Application/Restaurants/{Commands|Queries}/<UseCase>/`
with:
- The request: `<UseCase>Command` / `<UseCase>Query` implementing `IRequest<TResponse>`.
- `<UseCase>CommandHandler` / `<UseCase>QueryHandler` implementing
  `IRequestHandler<TRequest, TResponse>`.
- For commands, a `Validators/<UseCase>CommandValidator` using FluentValidation
  (`AbstractValidator<TCommand>`) — validators are auto-registered from the
  assembly and run automatically via the `ValidationBehavior<,>` MediatR pipeline
  before the handler executes. Do not manually call validators in handlers.
- Map Command/Entity/DTO via AutoMapper profiles (auto-scanned from the
  Application assembly) rather than manual mapping code.
- Return DTOs from `Restaurants.Application/Restaurants/Dtos` — never expose
  Domain entities from handlers/controllers.

Controllers must stay thin: delegate to `mediator.Send(...)`, no business logic.
Default to `[Authorize]`; use `[AllowAnonymous]` to opt out, or
`[Authorize(Roles = UserRoles.Owner)]` (etc.) to restrict further.

## Error handling

Throw `Restaurants.Domain.Exceptions.NotFoundException` / `ValidationException`
from Application/Infrastructure code — do not return raw HTTP status codes from
handlers. `GlobalExceptionHandler` (registered via `AddExceptionHandler` +
`AddProblemDetails`) maps these centrally:

| Exception | Status |
| --- | --- |
| `NotFoundException` | 404 (ProblemDetails with `resourceType`/`resourceId`) |
| `ValidationException` | 400 (ValidationProblemDetails, per-field errors) |
| other | 500 |

`[ApiController]` automatic model-state validation is intentionally disabled
(`SuppressModelStateInvalidFilter = true`) so all validation flows through the
MediatR `ValidationBehavior` — do not re-enable it or add ad-hoc validation in
controllers.

## Data access

- All EF Core access goes through `RestaurantsDbContext` and the repository
  abstractions (`IRestaurantsRepository`, `IDishesRepository` in
  `Restaurants.Application.Abstractions.Repositories`, implemented in
  `Restaurants.Infrastructure/Repositories`). Handlers depend on the
  abstractions, never on `DbContext` directly.
- When changing entities in `Restaurants.Domain/Entities`, add a corresponding
  EF Core migration:
  `dotnet ef migrations add <Name> --project Restaurants.Infrastructure --startup-project Restaurants.API`.
- Connection string key: `ConnectionStrings:RestaurantsDb`. Never hard-code
  connection strings or secrets in source — use User Secrets locally
  (`dotnet user-secrets --project Restaurants.API set ...`) or environment
  variables (e.g. `ConnectionStrings__RestaurantsDb`) elsewhere.

## Identity & security

- Auth is ASP.NET Core Identity (`ApplicationUser`, `IdentityRole`) exposed via
  the minimal API at `api/identity`, plus JWT Bearer authentication for the
  regular API surface.
- Never commit JWT signing keys, connection strings, or other secrets to
  `appsettings*.json`.
- Respect role-based authorization already in place (e.g. `UserRoles.Owner`
  required to create a restaurant) — don't loosen `[Authorize]` attributes
  without an explicit request.

## Logging

- Use **Serilog** (already configured via `UseSerilog`); do not introduce a
  different logging framework.
- `RequestTimeLoggingMiddleware` logs requests over 4 seconds — keep this in
  the pipeline when touching `WebApplicationExtensions.ConfigurePipeline`.

## Build, run, and verify

```powershell
dotnet restore Restaurants.sln
dotnet build Restaurants.sln
dotnet run --project Restaurants.API
```

- Swagger UI is available at `/swagger` in Development.
- After changing entities/DbContext, apply migrations before running:
  `dotnet ef database update --project Restaurants.Infrastructure --startup-project Restaurants.API`.
- There is no dedicated test project in the solution today; if you add one,
  place it as a sibling project (e.g. `Restaurants.Application.Tests`) and run
  it with `dotnet test`. Do not introduce a new test framework/tooling without
  being asked.
- Always build (`dotnet build`) after making changes to verify they compile
  before considering a task done.

## General guidance for AI changes in this repo

- Keep changes scoped to the layer they belong in; don't leak EF Core/Identity
  types into `Restaurants.Application` or `Restaurants.Domain`.
- Prefer extending existing patterns (CQRS handler + validator + AutoMapper
  profile) over introducing new architectural styles.
- Match existing C# conventions: nullable reference types enabled
  (`<Nullable>enable</Nullable>`), implicit usings enabled, async/await for I/O,
  `record`/`class` DTOs as already used in `Dtos`.
- Update `README.md` if you change architecture, DI composition, or conventions
  described in it — it is the primary onboarding document and should stay in
  sync with the code.
