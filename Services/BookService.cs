using MyBookApi2.DTOs;
using MyBookApi2.Mappers;
using MyBookApi2.Models;

namespace MyBookApi2.Services;

public class BookService : IBookService
{
    // In-memory store
    private static readonly List<Book> _books = new()
    {
        new Book { Id = 1, Title = "Clean Code", Author = "Robert C. Martin", Price = 34.99m, PublishedDate = new DateTime(2008, 8, 1) },
        new Book { Id = 2, Title = "The Pragmatic Programmer", Author = "Andrew Hunt", Price = 49.99m, PublishedDate = new DateTime(1999, 10, 20) },
        new Book { Id = 3, Title = "Refactoring", Author = "Martin Fowler", Price = 44.99m, PublishedDate = new DateTime(2018, 11, 20) }
    };

    private static int _nextId = 4;

    public Task<IEnumerable<BookResponseDTO>> GetAllAsync(string? author, int page, int pageSize)
    {
        IEnumerable<Book> query = _books;

        // Filter: case-insensitive partial match on author
        if (!string.IsNullOrWhiteSpace(author))
        {
            query = query.Where(b =>
                b.Author.Contains(author, StringComparison.OrdinalIgnoreCase));
        }

        // Paginate: Skip/Take
        var results = query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(BookMapper.ToResponse);

        return Task.FromResult(results);
    }

    public Task<BookResponseDTO?> GetByIdAsync(int id)
    {
        var book = _books.FirstOrDefault(b => b.Id == id);
        return Task.FromResult(book is null ? null : BookMapper.ToResponse(book));
    }

    public Task<BookResponseDTO> CreateAsync(BookCreateDTO dto)
    {
        var entity = BookMapper.ToEntity(dto);
        entity.Id = _nextId++;
        _books.Add(entity);

        return Task.FromResult(BookMapper.ToResponse(entity));
    }

    public Task<BookResponseDTO?> UpdateAsync(int id, BookUpdateDTO dto)
    {
        var existing = _books.FirstOrDefault(b => b.Id == id);
        if (existing is null) return Task.FromResult<BookResponseDTO?>(null);

        BookMapper.ApplyUpdate(dto, existing);
        return Task.FromResult<BookResponseDTO?>(BookMapper.ToResponse(existing));
    }

    public Task<bool> DeleteAsync(int id)
    {
        var existing = _books.FirstOrDefault(b => b.Id == id);
        if (existing is null) return Task.FromResult(false);

        _books.Remove(existing);
        return Task.FromResult(true);
    }
}
