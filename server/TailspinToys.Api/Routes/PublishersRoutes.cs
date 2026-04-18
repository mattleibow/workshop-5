// Routes for the publishers API resource.
// Provides endpoints to retrieve publisher information.

using Microsoft.EntityFrameworkCore;

namespace TailspinToys.Api.Routes;

/// <summary>Extension methods to register publisher API routes.</summary>
public static class PublishersRoutes
{
    /// <summary>Maps all publisher-related routes onto the application.</summary>
    /// <param name="app">The <see cref="WebApplication"/> to register routes on.</param>
    public static void MapPublishersRoutes(this WebApplication app)
    {
        var group = app.MapGroup("/api/publishers");

        group.MapGet("/", async (TailspinToysContext db) =>
        {
            var publishers = await db.Publishers
                .OrderBy(p => p.Id)
                .Select(p => new { p.Id, p.Name })
                .ToListAsync();

            return Results.Ok(publishers);
        });
    }
}
