using System.ComponentModel.DataAnnotations;

namespace MyBookApi2.DTOs;

public class BookUpdateDTO
{
    [Required(ErrorMessage = "Title is required.")]
    [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Author is required.")]
    [MaxLength(100, ErrorMessage = "Author cannot exceed 100 characters.")]
    public string Author { get; set; } = string.Empty;

    [Range(0.01, 9999.99, ErrorMessage = "Price must be between 0.01 and 9999.99.")]
    public decimal Price { get; set; }

    public DateTime? PublishedDate { get; set; }
}
