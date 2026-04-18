// Routes for the games API resource.
// Provides endpoints to list, filter, and retrieve individual game records.

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TailspinToys.Api;

namespace TailspinToys.Api.Routes;

/// <summary>Extension methods to register game API routes.</summary>
public static class GamesRoutes
{
    /// <summary>Maps all game-related routes onto the application.</summary>
    /// <param name="app">The <see cref="WebApplication"/> to register routes on.</param>
    public static void MapGamesRoutes(this WebApplication app)
    {
        var group = app.MapGroup("/api/games");

        group.MapGet("/", async (int? categoryId, int? publisherId, TailspinToysContext db) =>
        {
            var query = db.Games
                .Include(g => g.Publisher)
                .Include(g => g.Category)
                .AsQueryable();

            if (categoryId.HasValue)
                query = query.Where(g => g.CategoryId == categoryId.Value);

            if (publisherId.HasValue)
                query = query.Where(g => g.PublisherId == publisherId.Value);

            var games = await query.OrderBy(g => g.Id).ToListAsync();

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
