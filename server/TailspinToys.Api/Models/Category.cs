using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TailspinToys.Api.Models;

public class Category
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MinLength(2, ErrorMessage = "Category name must be at least 2 characters")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    [JsonIgnore]
    public List<Game> Games { get; set; } = [];

    public object ToDict()
    {
        return new
        {
            Id,
            Name,
            Description,
            GameCount = Games?.Count ?? 0
        };
    }
}
