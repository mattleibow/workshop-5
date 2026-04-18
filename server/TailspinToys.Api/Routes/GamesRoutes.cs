using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TailspinToys.Api;

namespace TailspinToys.Api.Routes;

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
                .OrderBy(g => g.Id)
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
