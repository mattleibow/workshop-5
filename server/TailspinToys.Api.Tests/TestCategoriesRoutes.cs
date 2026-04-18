// Integration tests for the categories API routes.
// Verifies that GET /api/categories returns the correct category data.

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

public class TestCategoriesRoutes : IDisposable
{
    private readonly string _dbPath;
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    private static readonly string[] TestCategoryNames = ["Strategy", "Card Game", "Puzzle"];

    private const string CategoriesApiPath = "/api/categories";

    public TestCategoriesRoutes()
    {
        _dbPath = Path.Combine(Path.GetTempPath(), $"TestDb_{Guid.NewGuid()}.db");
        var connectionString = $"Data Source={_dbPath}";

        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");

                builder.ConfigureAppConfiguration((context, config) =>
                {
                    config.AddInMemoryCollection(new Dictionary<string, string?>
                    {
                        ["ConnectionStrings:DefaultConnection"] = connectionString
                    });
                });
            });

        _client = _factory.CreateClient();

        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TailspinToysContext>();
        SeedTestData(db);
    }

    private void SeedTestData(TailspinToysContext db)
    {
        db.Categories.AddRange(TestCategoryNames.Select(name => new Category { Name = name }));
        db.SaveChanges();
    }

    [Fact]
    public async Task GetCategories_ReturnsAllCategories()
    {
        // Act
        var response = await _client.GetAsync(CategoriesApiPath);
        var data = await response.Content.ReadFromJsonAsync<List<Dictionary<string, object>>>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(data);
        Assert.Equal(TestCategoryNames.Length, data.Count);

        var returnedNames = data.Select(c => c["name"]?.ToString()).OrderBy(n => n).ToList();
        var expectedNames = TestCategoryNames.OrderBy(n => n).ToList();
        Assert.Equal(expectedNames, returnedNames);
    }

    [Fact]
    public async Task GetCategories_ReturnsIdAndName()
    {
        // Act
        var response = await _client.GetAsync(CategoriesApiPath);
        var data = await response.Content.ReadFromJsonAsync<List<Dictionary<string, object>>>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(data);
        Assert.NotEmpty(data);

        var category = data[0];
        Assert.True(category.ContainsKey("id"), "Response should contain 'id'");
        Assert.True(category.ContainsKey("name"), "Response should contain 'name'");
        Assert.Equal(2, category.Count);

        var id = Assert.IsType<JsonElement>(category["id"]);
        Assert.True(id.GetInt32() > 0);
    }

    [Fact]
    public async Task GetCategories_EmptyDatabase_ReturnsEmptyList()
    {
        // Clear all categories
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TailspinToysContext>();
        db.Categories.RemoveRange(db.Categories);
        db.SaveChanges();

        // Act
        var response = await _client.GetAsync(CategoriesApiPath);
        var data = await response.Content.ReadFromJsonAsync<List<object>>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(data);
        Assert.Empty(data);
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
