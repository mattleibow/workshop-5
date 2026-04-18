using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TailspinToys.Api;
using TailspinToys.Api.Models;

namespace TailspinToys.Api.Tests;

public class TestGamesRoutes : IDisposable
{
    private readonly string _dbPath;
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    // Test data
    private static readonly Dictionary<string, object>[] TestPublishers =
    [
        new() { ["name"] = "DevGames Inc" },
        new() { ["name"] = "Scrum Masters" }
    ];

    private static readonly Dictionary<string, object>[] TestCategories =
    [
        new() { ["name"] = "Strategy" },
        new() { ["name"] = "Card Game" }
    ];

    private static readonly Dictionary<string, object>[] TestGames =
    [
        new()
        {
            ["title"] = "Pipeline Panic",
            ["description"] = "Build your DevOps pipeline before chaos ensues",
            ["publisher_index"] = 0,
            ["category_index"] = 0,
            ["star_rating"] = 4.5
        },
        new()
        {
            ["title"] = "Agile Adventures",
            ["description"] = "Navigate your team through sprints and releases",
            ["publisher_index"] = 1,
            ["category_index"] = 1,
            ["star_rating"] = 4.2
        }
    ];

    private const string GamesApiPath = "/api/games";

    public TestGamesRoutes()
    {
        // Use a temp file-based SQLite DB so all connections naturally share the same data.
        _dbPath = Path.Combine(Path.GetTempPath(), $"TestDb_{Guid.NewGuid()}.db");
        var connectionString = $"Data Source={_dbPath}";

        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                // Use a non-Development environment so appsettings.Development.json (which sets
                // SeedDatabase=true) is not loaded — the default of false applies instead.
                builder.UseEnvironment("Testing");

                // Override the connection string — no service removal needed
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    config.AddInMemoryCollection(new Dictionary<string, string?>
                    {
                        ["ConnectionStrings:DefaultConnection"] = connectionString
                    });
                });
            });

        _client = _factory.CreateClient();

        // Seed test data
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TailspinToysContext>();
        SeedTestData(db);
    }

    private void SeedTestData(TailspinToysContext db)
    {
        var publishers = TestPublishers.Select(p =>
            new Publisher { Name = (string)p["name"] }).ToList();
        db.Publishers.AddRange(publishers);

        var categories = TestCategories.Select(c =>
            new Category { Name = (string)c["name"] }).ToList();
        db.Categories.AddRange(categories);
        db.SaveChanges();

        foreach (var gameData in TestGames)
        {
            var publisherIndex = (int)gameData["publisher_index"];
            var categoryIndex = (int)gameData["category_index"];
            var game = new Game
            {
                Title = (string)gameData["title"],
                Description = (string)gameData["description"],
                StarRating = (double)gameData["star_rating"],
                Publisher = publishers[publisherIndex],
                Category = categories[categoryIndex]
            };
            db.Games.Add(game);
        }
        db.SaveChanges();
    }

    [Fact]
    public async Task GetGames_ReturnsAllGames()
    {
        // Act
        var response = await _client.GetAsync(GamesApiPath);
        var data = await response.Content.ReadFromJsonAsync<List<Dictionary<string, object>>>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(data);
        Assert.Equal(TestGames.Length, data.Count);

        for (var i = 0; i < data.Count; i++)
        {
            var gameData = data[i];
            var expectedGame = TestGames[i];
            var expectedPublisher = TestPublishers[(int)expectedGame["publisher_index"]];
            var expectedCategory = TestCategories[(int)expectedGame["category_index"]];

            Assert.Equal(expectedGame["title"].ToString(), gameData["title"]?.ToString());

            var publisher = Assert.IsType<JsonElement>(gameData["publisher"]);
            Assert.Equal(expectedPublisher["name"].ToString(), publisher.GetProperty("name").GetString());

            var category = Assert.IsType<JsonElement>(gameData["category"]);
            Assert.Equal(expectedCategory["name"].ToString(), category.GetProperty("name").GetString());

            var starRating = Assert.IsType<JsonElement>(gameData["starRating"]);
            Assert.Equal((double)expectedGame["star_rating"], starRating.GetDouble());
        }
    }

    [Fact]
    public async Task GetGames_ReturnsCorrectStructure()
    {
        // Act
        var response = await _client.GetAsync(GamesApiPath);
        var data = await response.Content.ReadFromJsonAsync<List<Dictionary<string, object>>>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(data);
        Assert.NotEmpty(data);

        var requiredFields = new[] { "id", "title", "description", "publisher", "category", "starRating" };
        foreach (var field in requiredFields)
        {
            Assert.True(data[0].ContainsKey(field), $"Missing field: {field}");
        }
    }

    [Fact]
    public async Task GetGameById_ReturnsGame()
    {
        // Get the first game's ID and title from the list
        var listResponse = await _client.GetAsync(GamesApiPath);
        var games = await listResponse.Content.ReadFromJsonAsync<List<Dictionary<string, object>>>();
        Assert.NotNull(games);
        var gameId = games[0]["id"];
        var expectedTitle = games[0]["title"]?.ToString();

        // Act
        var response = await _client.GetAsync($"{GamesApiPath}/{gameId}");
        var data = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(data);
        Assert.Equal(expectedTitle, data["title"]?.ToString());

        var publisher = Assert.IsType<JsonElement>(data["publisher"]);
        Assert.Equal(TestPublishers[0]["name"].ToString(), publisher.GetProperty("name").GetString());

        var category = Assert.IsType<JsonElement>(data["category"]);
        Assert.Equal(TestCategories[0]["name"].ToString(), category.GetProperty("name").GetString());

        var starRating = Assert.IsType<JsonElement>(data["starRating"]);
        Assert.Equal((double)TestGames[0]["star_rating"], starRating.GetDouble());
    }

    [Fact]
    public async Task GetGameById_NotFound_Returns404()
    {
        // Act
        var response = await _client.GetAsync($"{GamesApiPath}/999");
        var data = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.NotNull(data);
        Assert.Equal("Game not found", data["error"]?.ToString());
    }

    [Fact]
    public async Task GetGames_EmptyDatabase_ReturnsEmptyList()
    {
        // Clear all games
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TailspinToysContext>();
        db.Games.RemoveRange(db.Games);
        db.SaveChanges();

        // Act
        var response = await _client.GetAsync(GamesApiPath);
        var data = await response.Content.ReadFromJsonAsync<List<object>>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(data);
        Assert.Empty(data);
    }

    [Fact]
    public async Task GetGameByInvalidId_Returns404()
    {
        // Act - invalid-id should not match the {id:int} route
        var response = await _client.GetAsync($"{GamesApiPath}/invalid-id");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    public void Dispose()
    {
        _client.Dispose();
        _factory.Dispose();
        try
        {
            if (File.Exists(_dbPath))
                File.Delete(_dbPath);
        }
        catch (IOException)
        {
            // Best-effort cleanup; ignore if the file is locked or already deleted
        }
    }
}
