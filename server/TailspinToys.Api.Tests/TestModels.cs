using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using TailspinToys.Api.Models;

namespace TailspinToys.Api.Tests;

public class TestModels : IDisposable
{
    private readonly TailspinToysContext _db;

    // Test data
    private static readonly Dictionary<string, string> ValidPublisher = new()
    {
        ["name"] = "Test Publisher",
        ["description"] = "A great publisher for testing"
    };

    private static readonly Dictionary<string, string> ValidCategory = new()
    {
        ["name"] = "Strategy",
        ["description"] = "Strategic games for testing"
    };

    private static readonly Dictionary<string, object> ValidGame = new()
    {
        ["title"] = "Test Game",
        ["description"] = "An exciting test game with lots of features",
        ["star_rating"] = 4.5
    };

    public TestModels()
    {
        var options = new DbContextOptionsBuilder<TailspinToysContext>()
            .UseSqlite("Data Source=:memory:")
            .Options;
        _db = new TailspinToysContext(options);
        // Keep the connection open for the lifetime of the test — required for SQLite in-memory
        // databases so the schema and data persist across multiple operations on this DbContext.
        _db.Database.OpenConnection();
        _db.Database.EnsureCreated();
    }

    [Fact]
    public void GameTitle_TooShort_ThrowsValidationError()
    {
        // Create required publisher and category
        var publisher = new Publisher { Name = ValidPublisher["name"], Description = ValidPublisher["description"] };
        var category = new Category { Name = ValidCategory["name"], Description = ValidCategory["description"] };
        _db.Publishers.Add(publisher);
        _db.Categories.Add(category);
        _db.SaveChanges();

        // Attempt to create game with title that's too short
        var game = new Game
        {
            Title = "X",
            Description = (string)ValidGame["description"],
            Publisher = publisher,
            Category = category,
            StarRating = 4.0
        };
        _db.Games.Add(game);

        var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
        var context = new System.ComponentModel.DataAnnotations.ValidationContext(game);
        var isValid = System.ComponentModel.DataAnnotations.Validator.TryValidateObject(game, context, validationResults, true);

        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.ErrorMessage!.Contains("Game title must be at least 2 characters"));

        var exception = Assert.Throws<ValidationException>(() => _db.SaveChanges());
        Assert.Contains("Game title must be at least 2 characters", exception.Message);
    }

    [Fact]
    public void GameDescription_TooShort_ThrowsValidationError()
    {
        var publisher = new Publisher { Name = ValidPublisher["name"], Description = ValidPublisher["description"] };
        var category = new Category { Name = ValidCategory["name"], Description = ValidCategory["description"] };
        _db.Publishers.Add(publisher);
        _db.Categories.Add(category);
        _db.SaveChanges();

        var game = new Game
        {
            Title = (string)ValidGame["title"],
            Description = "Too short",
            Publisher = publisher,
            Category = category,
            StarRating = 4.0
        };
        _db.Games.Add(game);

        var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
        var context = new System.ComponentModel.DataAnnotations.ValidationContext(game);
        var isValid = System.ComponentModel.DataAnnotations.Validator.TryValidateObject(game, context, validationResults, true);

        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.ErrorMessage!.Contains("Description must be at least 10 characters"));

        var exception = Assert.Throws<ValidationException>(() => _db.SaveChanges());
        Assert.Contains("Description must be at least 10 characters", exception.Message);
    }

    [Fact]
    public void ValidGame_CreatesSuccessfully()
    {
        var publisher = new Publisher { Name = ValidPublisher["name"], Description = ValidPublisher["description"] };
        var category = new Category { Name = ValidCategory["name"], Description = ValidCategory["description"] };
        _db.Publishers.Add(publisher);
        _db.Categories.Add(category);
        _db.SaveChanges();

        var game = new Game
        {
            Title = (string)ValidGame["title"],
            Description = (string)ValidGame["description"],
            Publisher = publisher,
            Category = category,
            StarRating = (double)ValidGame["star_rating"]
        };
        _db.Games.Add(game);
        _db.SaveChanges();

        Assert.NotEqual(0, game.Id);
        Assert.Equal(ValidGame["title"], game.Title);
        Assert.Equal(ValidGame["description"], game.Description);
        Assert.Equal((double)ValidGame["star_rating"], game.StarRating);
    }

    [Fact]
    public void PublisherName_TooShort_ThrowsValidationError()
    {
        var publisher = new Publisher { Name = "X", Description = ValidPublisher["description"] };

        var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
        var context = new System.ComponentModel.DataAnnotations.ValidationContext(publisher);
        var isValid = System.ComponentModel.DataAnnotations.Validator.TryValidateObject(publisher, context, validationResults, true);

        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.ErrorMessage!.Contains("Publisher name must be at least 2 characters"));

        _db.Publishers.Add(publisher);
        var exception = Assert.Throws<ValidationException>(() => _db.SaveChanges());
        Assert.Contains("Publisher name must be at least 2 characters", exception.Message);
    }

    [Fact]
    public void CategoryName_TooShort_ThrowsValidationError()
    {
        var category = new Category { Name = "X", Description = ValidCategory["description"] };

        var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
        var context = new System.ComponentModel.DataAnnotations.ValidationContext(category);
        var isValid = System.ComponentModel.DataAnnotations.Validator.TryValidateObject(category, context, validationResults, true);

        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.ErrorMessage!.Contains("Category name must be at least 2 characters"));

        _db.Categories.Add(category);
        var exception = Assert.Throws<ValidationException>(() => _db.SaveChanges());
        Assert.Contains("Category name must be at least 2 characters", exception.Message);
    }

    [Fact]
    public void Description_NullAllowed()
    {
        var publisher = new Publisher { Name = ValidPublisher["name"], Description = null };
        _db.Publishers.Add(publisher);
        _db.SaveChanges();

        Assert.NotEqual(0, publisher.Id);
        Assert.Null(publisher.Description);
    }

    public void Dispose()
    {
        _db.Dispose();
    }
}
