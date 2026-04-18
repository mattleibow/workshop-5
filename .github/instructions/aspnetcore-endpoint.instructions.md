---
description: 'ASP.NET Core Minimal API endpoint patterns and conventions'
applyTo: 'server/**/*.cs'
---

# ASP.NET Core Minimal API Endpoint Instructions

## Endpoint Pattern

Use Minimal API route groups for organizing endpoints by resource.

### Route Group Structure

```csharp
public static class GamesRoutes
{
    public static void MapGamesRoutes(this WebApplication app)
    {
        var group = app.MapGroup("/api/games");

        group.MapGet("/", async (TailspinToysContext db) =>
        {
            var games = await db.Games
                .Include(g => g.Publisher)
                .Include(g => g.Category)
                .ToListAsync();

            return Results.Ok(games.Select(g => g.ToDict()));
        });

        group.MapGet("/{id:int}", async (int id, TailspinToysContext db) =>
        {
            var game = await db.Games
                .Include(g => g.Publisher)
                .Include(g => g.Category)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (game is null)
                return Results.NotFound(new { error = "Game not found" });

            return Results.Ok(game.ToDict());
        });
    }
}
```

### Response Patterns

```csharp
// Success (200)
return Results.Ok(data);

// Not Found (404)
return Results.NotFound(new { error = "Resource not found" });

// Bad Request (400)
return Results.BadRequest(new { error = "Invalid input" });
```

### Registration

Register routes in Program.cs:
```csharp
app.MapGamesRoutes();
```

### Conventions

- Use `async` for all database operations
- Include related entities with `.Include()`
- Use `ToDict()` methods on models for JSON serialization
- Return appropriate HTTP status codes
- Use route constraints (e.g., `{id:int}`) for type safety

## Required Testing

- All endpoints need integration tests per [dotnet-tests.instructions.md](./dotnet-tests.instructions.md)
- Run: [scripts/run-server-tests.sh](../../scripts/run-server-tests.sh) (or [scripts/run-server-tests.ps1](../../scripts/run-server-tests.ps1) on Windows)
- All tests must pass before commit

## Registration & References

- Register routes in [server/TailspinToys.Api/Program.cs](../../server/TailspinToys.Api/Program.cs)
- Example: [server/TailspinToys.Api/Routes/GamesRoutes.cs](../../server/TailspinToys.Api/Routes/GamesRoutes.cs)
- Tests: [server/TailspinToys.Api.Tests/TestGamesRoutes.cs](../../server/TailspinToys.Api.Tests/TestGamesRoutes.cs)
