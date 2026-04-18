// Integration tests for the publishers API routes.
// Verifies that GET /api/publishers returns the correct publisher data.

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

public class TestPublishersRoutes : IDisposable
{
    private readonly string _dbPath;
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    private static readonly string[] TestPublisherNames = ["DevGames Inc", "Scrum Masters", "Agile Arcade"];

    private const string PublishersApiPath = "/api/publishers";

    public TestPublishersRoutes()
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
        db.Publishers.AddRange(TestPublisherNames.Select(name => new Publisher { Name = name }));
        db.SaveChanges();
    }

    [Fact]
    public async Task GetPublishers_ReturnsAllPublishers()
    {
        // Act
        var response = await _client.GetAsync(PublishersApiPath);
        var data = await response.Content.ReadFromJsonAsync<List<Dictionary<string, object>>>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(data);
        Assert.Equal(TestPublisherNames.Length, data.Count);

        var returnedNames = data.Select(p => p["name"]?.ToString()).OrderBy(n => n).ToList();
        var expectedNames = TestPublisherNames.OrderBy(n => n).ToList();
        Assert.Equal(expectedNames, returnedNames);
    }

    [Fact]
    public async Task GetPublishers_ReturnsIdAndName()
    {
        // Act
        var response = await _client.GetAsync(PublishersApiPath);
        var data = await response.Content.ReadFromJsonAsync<List<Dictionary<string, object>>>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(data);
        Assert.NotEmpty(data);

        var publisher = data[0];
        Assert.True(publisher.ContainsKey("id"), "Response should contain 'id'");
        Assert.True(publisher.ContainsKey("name"), "Response should contain 'name'");
        Assert.Equal(2, publisher.Count);

        var id = Assert.IsType<JsonElement>(publisher["id"]);
        Assert.True(id.GetInt32() > 0);
    }

    [Fact]
    public async Task GetPublishers_EmptyDatabase_ReturnsEmptyList()
    {
        // Clear all publishers
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TailspinToysContext>();
        db.Publishers.RemoveRange(db.Publishers);
        db.SaveChanges();

        // Act
        var response = await _client.GetAsync(PublishersApiPath);
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
