using MyBookApi2.DTOs;
using MyBookApi2.Models;

namespace MyBookApi2.Mappers;

public static class BookMapper
{
    public static Book ToEntity(BookCreateDTO dto)
    {
        return new Book
        {
            Title = dto.Title,
            Price = dto.Price,
            PublishedDate = dto.PublishedDate ?? DateTime.UtcNow
        };
    }

    public static void ApplyUpdate(BookUpdateDTO dto, Book entity)
    {
        entity.Title = dto.Title;
        entity.Price = dto.Price;
        entity.PublishedDate = dto.PublishedDate ?? entity.PublishedDate;
    }

    public static BookResponseDTO ToResponse(Book entity)
    {
        return new BookResponseDTO
        {
            Id = entity.Id,
            Title = entity.Title,
            Author = entity.Author?.Name ?? string.Empty,
            Price = entity.Price,
            PublishedDate = entity.PublishedDate
        };
    }
}
