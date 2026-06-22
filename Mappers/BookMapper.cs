using MyBookApi2.DTOs;
using MyBookApi2.Models;

namespace MyBookApi2.Mappers;

public static class BookMapper
{
    // POST: DTO → new Entity (Id left at 0 for the in-memory store to assign)
    public static Book ToEntity(BookCreateDTO dto)
    {
        return new Book
        {
            Title = dto.Title,
            Author = dto.Author,
            Price = dto.Price,
            PublishedDate = dto.PublishedDate ?? DateTime.UtcNow
        };
    }

    // PUT: mutate existing entity with update DTO (preserves Id)
    public static void ApplyUpdate(BookUpdateDTO dto, Book entity)
    {
        entity.Title = dto.Title;
        entity.Author = dto.Author;
        entity.Price = dto.Price;
        entity.PublishedDate = dto.PublishedDate ?? entity.PublishedDate;
    }

    // GET: Entity → response DTO (no secrets or internal fields leak out)
    public static BookResponseDTO ToResponse(Book entity)
    {
        return new BookResponseDTO
        {
            Id = entity.Id,
            Title = entity.Title,
            Author = entity.Author,
            Price = entity.Price,
            PublishedDate = entity.PublishedDate
        };
    }
}
