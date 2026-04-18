using System.Globalization;
using Microsoft.EntityFrameworkCore;
using TailspinToys.Api.Models;

namespace TailspinToys.Api.Utils;

public static class SeedDatabase
{
    public static void Seed(TailspinToysContext db)
    {
        if (db.Games.Any())
            return;

        var categories = new Dictionary<string, Category>();
        var publishers = new Dictionary<string, Publisher>();

        var csvPath = Path.Combine(AppContext.BaseDirectory, "Utils", "SeedData", "games.csv");

        // If not found in bin directory, try relative to current working directory
        if (!File.Exists(csvPath))
        {
            csvPath = Path.Combine(Directory.GetCurrentDirectory(), "Utils", "SeedData", "games.csv");
        }

        // Try project directory (for dotnet run)
        if (!File.Exists(csvPath))
        {
            var assemblyDir = Path.GetDirectoryName(typeof(SeedDatabase).Assembly.Location) ?? "";
            csvPath = Path.Combine(assemblyDir, "Utils", "SeedData", "games.csv");
        }

        var random = new Random(42); // Fixed seed for consistency
        var gameCount = 0;

        using var reader = new StreamReader(csvPath);
        // Skip header
        reader.ReadLine();

        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (string.IsNullOrWhiteSpace(line)) continue;

            var fields = ParseCsvLine(line);
            if (fields.Count < 4) continue;

            var title = fields[0];
            var categoryName = fields[1];
            var publisherName = fields[2];
            var description = fields[3];

            // Process category
            if (!categories.ContainsKey(categoryName))
            {
                var category = new Category
                {
                    Name = categoryName,
                    Description = $"Collection of {categoryName} games available for crowdfunding"
                };
                db.Categories.Add(category);
                db.SaveChanges();
                categories[categoryName] = category;
            }

            // Process publisher
            if (!publishers.ContainsKey(publisherName))
            {
                var publisher = new Publisher
                {
                    Name = publisherName,
                    Description = $"{publisherName} is a game publisher seeking funding for exciting new titles"
                };
                db.Publishers.Add(publisher);
                db.SaveChanges();
                publishers[publisherName] = publisher;
            }

            // Generate random star rating between 3.0 and 5.0
            var starRating = Math.Round(random.NextDouble() * 2.0 + 3.0, 1);

            var game = new Game
            {
                Title = title,
                Description = description + " Support this game through our crowdfunding platform!",
                CategoryId = categories[categoryName].Id,
                PublisherId = publishers[publisherName].Id,
                StarRating = starRating
            };
            db.Games.Add(game);
            gameCount++;
        }

        db.SaveChanges();
        Console.WriteLine($"Added {gameCount} games with {categories.Count} categories and {publishers.Count} publishers");
    }

    private static List<string> ParseCsvLine(string line)
    {
        var fields = new List<string>();
        var inQuotes = false;
        var field = "";

        for (int i = 0; i < line.Length; i++)
        {
            var c = line[i];
            if (c == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == ',' && !inQuotes)
            {
                fields.Add(field.Trim());
                field = "";
            }
            else
            {
                field += c;
            }
        }
        fields.Add(field.Trim());

        return fields;
    }
}
