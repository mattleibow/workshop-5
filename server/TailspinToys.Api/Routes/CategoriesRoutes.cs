// Routes for the categories API resource.
// Provides endpoints to retrieve category information used for filtering games.

using Microsoft.EntityFrameworkCore;

namespace TailspinToys.Api.Routes;

/// <summary>Extension methods to register category API routes.</summary>
public static class CategoriesRoutes
{
    /// <summary>Maps all category-related routes onto the application.</summary>
    /// <param name="app">The <see cref="WebApplication"/> to register routes on.</param>
    public static void MapCategoriesRoutes(this WebApplication app)
    {
        var group = app.MapGroup("/api/categories");

        group.MapGet("/", async (TailspinToysContext db) =>
        {
            var categories = await db.Categories
                .OrderBy(c => c.Id)
                .Select(c => new { c.Id, c.Name })
                .ToListAsync();

            return Results.Ok(categories);
        });
    }
}
