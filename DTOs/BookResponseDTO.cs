namespace MyBookApi2.DTOs;

public record BookResponseDTO
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Author { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public DateTime? PublishedDate { get; init; }
}
