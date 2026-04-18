using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TailspinToys.Api.Models;

public class Game
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MinLength(2, ErrorMessage = "Game title must be at least 2 characters")]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MinLength(10, ErrorMessage = "Description must be at least 10 characters")]
    public string Description { get; set; } = string.Empty;

    [Column("star_rating")]
    public double? StarRating { get; set; }

    [ForeignKey("Category")]
    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    [ForeignKey("Publisher")]
    public int PublisherId { get; set; }
    public Publisher? Publisher { get; set; }

    public object ToDict()
    {
        return new
        {
            Id,
            Title,
            Description,
            Publisher = Publisher != null ? new { Publisher.Id, Publisher.Name } : null,
            Category = Category != null ? new { Category.Id, Category.Name } : null,
            StarRating
        };
    }
}
