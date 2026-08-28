# Technology Stack

## Core Sections (Required)

### 1) Runtime Summary

| Area | Value | Evidence |
|------|-------|----------|
| Primary language | C# | docs/codebase/.codebase-scan.txt (Files by language) |
| Runtime + version | .NET 9.0 (API), .NET 8.0 (Application/Infrastructure/Domain) | Restaurants.API/Restaurants.API.csproj; Restaurants.Application/Restaurants.Application.csproj; Restaurants.Infrastructure/Restaurants.Infrastructure.csproj; Restaurants.Domain/Restaurants.Domain.csproj |
| Package manager | NuGet (PackageReference in SDK-style .csproj files) | Restaurants.API/Restaurants.API.csproj |
| Module/build system | .NET SDK-style projects + Visual Studio solution | Restaurants.sln; Restaurants.API/Restaurants.API.csproj |

### 2) Production Frameworks and Dependencies

| Dependency | Version | Role in system | Evidence |
|------------|---------|----------------|----------|
| Microsoft.NET.Sdk.Web | SDK (implicit) | ASP.NET Core Web API host | Restaurants.API/Restaurants.API.csproj |
| MediatR | 12.4.1 | CQRS request/handler dispatch | Restaurants.Application/Restaurants.Application.csproj |
| AutoMapper | 15.0.1 | Command/DTO/entity mapping | Restaurants.Application/Restaurants.Application.csproj |
| FluentValidation.DependencyInjectionExtensions | 12.0.0 | Validator registration for pipeline validation | Restaurants.Application/Restaurants.Application.csproj |
| Microsoft.EntityFrameworkCore.SqlServer | 9.0.8 | SQL Server provider for persistence | Restaurants.Infrastructure/Restaurants.Infrastructure.csproj |
| Microsoft.AspNetCore.Identity.EntityFrameworkCore | 8.0.22 | Identity persistence and auth user store integration | Restaurants.Infrastructure/Restaurants.Infrastructure.csproj |
| Microsoft.AspNetCore.Authentication.JwtBearer | 9.0.13 | JWT bearer auth package referenced by API layer | Restaurants.API/Restaurants.API.csproj |
| Serilog.AspNetCore | 9.0.0 | HTTP/app logging integration | Restaurants.API/Restaurants.API.csproj |
| Swashbuckle.AspNetCore.SwaggerGen | 9.0.6 | OpenAPI generation | Restaurants.API/Restaurants.API.csproj |

### 3) Development Toolchain

| Tool | Purpose | Evidence |
|------|---------|----------|
| Microsoft.EntityFrameworkCore.Tools | EF Core migrations/tooling | Restaurants.API/Restaurants.API.csproj; Restaurants.Infrastructure/Restaurants.Infrastructure.csproj |
| Visual Studio solution configurations (Debug/Release) | Build configurations | Restaurants.sln |
| Python scan script (skill tooling) | Repository analysis artifact generation | .github/skills/acquire-codebase-knowledge/scripts/scan.py; docs/codebase/.codebase-scan.txt |

### 4) Key Commands

```bash
dotnet restore Restaurants.sln
dotnet build Restaurants.sln
dotnet run --project Restaurants.API
[TODO] dotnet test (no test projects currently in solution)
```

### 5) Environment and Config

- Config sources: Restaurants.API/appsettings.json, Restaurants.API/appsettings.Development.json
- Required env vars: ConnectionStrings__RestaurantsDb, [TODO] JWT-related secret/key variable names
- Deployment/runtime constraints: SQL Server connection is required via `ConnectionStrings:RestaurantsDb`; API project targets net9.0 while class libraries target net8.0.

### 6) Evidence

- Restaurants.sln
- Restaurants.API/Restaurants.API.csproj
- Restaurants.Application/Restaurants.Application.csproj
- Restaurants.Infrastructure/Restaurants.Infrastructure.csproj
- Restaurants.Domain/Restaurants.Domain.csproj
- Restaurants.API/appsettings.Development.json
- docs/codebase/.codebase-scan.txt