---
description: 'xUnit test patterns and conventions for ASP.NET Core'
applyTo: '**/*Tests*/**/*.cs'
---

# .NET Test Instructions

## Test Structure

Use xUnit with `WebApplicationFactory` for integration tests, plain `DbContext` for unit tests.

### Database Strategy

Tests override `ConnectionStrings:DefaultConnection` via `ConfigureAppConfiguration` to point at a test-specific database. No service removal or replacement is needed — both production and test use the SQLite provider.

- **Route/integration tests**: Use a temp file-based SQLite DB (`Data Source={tempPath}`) with a unique path per test instance. Delete the file in `Dispose()`. File-based DBs are needed because `WebApplicationFactory` opens multiple connections (one per request scope), and they must all see the same data.
- **Model/unit tests**: Use `Data Source=:memory:` with `OpenConnection()` to hold the connection open. Works because there's only a single `DbContext` instance.

> The `AddDbContext` call in `Program.cs` must resolve the connection string lazily from `IConfiguration` inside the options lambda — not captured eagerly at startup — so test config overrides take effect.

### Route Test Class Pattern

```csharp
public class TestExampleRoutes : IDisposable
{
    private readonly string _dbPath;
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    // Test data as class constants
    private static readonly Dictionary<string, object>[] TestItems = [...];

    public TestExampleRoutes()
    {
        // Temp file-based SQLite DB — unique per test instance.
        _dbPath = Path.Combine(Path.GetTempPath(), $"TestDb_{Guid.NewGuid()}.db");
        var connectionString = $"Data Source={_dbPath}";

        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                // Use a non-Development environment so appsettings.Development.json is not
                // loaded — this ensures SeedDatabase defaults to false.
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
        db.Items.AddRange(...);
        db.SaveChanges();
    }

    [Fact]
    public async Task GetItems_ReturnsAllItems()
    {
        var response = await _client.GetAsync("/api/items");
        // Assert...
    }

    public void Dispose()
    {
        _client.Dispose();
        _factory.Dispose();
        if (File.Exists(_dbPath))
            File.Delete(_dbPath);
    }
}
```

### Model / Unit Test Class Pattern

For tests that operate directly on a `DbContext` without HTTP (e.g. validation tests), use a simple in-memory SQLite database since there's only a single connection:

```csharp
public class TestModels : IDisposable
{
    private readonly TailspinToysContext _db;

    public TestModels()
    {
        var options = new DbContextOptionsBuilder<TailspinToysContext>()
            .UseSqlite("Data Source=:memory:")
            .Options;
        _db = new TailspinToysContext(options);
        _db.Database.OpenConnection();
        _db.Database.EnsureCreated();
    }

    public void Dispose()
    {
        _db.Dispose();
    }
}
```

### Test Naming

- `MethodName_Scenario_ExpectedResult` pattern
- Use descriptive names that explain the test
