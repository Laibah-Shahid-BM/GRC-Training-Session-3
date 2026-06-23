using Microsoft.EntityFrameworkCore;
using MyBookApi2.Data;
using MyBookApi2.DTOs;
using MyBookApi2.Mappers;
using MyBookApi2.Models;

namespace MyBookApi2.Services;

public class BookService : IBookService
{
    private readonly AppDbContext _context;

    public BookService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BookResponseDTO>> GetAllAsync(string? author, int page, int pageSize)
    {
        var query = _context.Books
            .AsNoTracking()
            .Include(b => b.Author)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(author))
        {
            query = query.Where(b =>
                b.Author.Name.Contains(author));
        }

        var results = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(b => BookMapper.ToResponse(b))
            .ToListAsync();

        return results;
    }

    public async Task<BookResponseDTO?> GetByIdAsync(int id)
    {
        var book = await _context.Books
            .AsNoTracking()
            .Include(b => b.Author)
            .FirstOrDefaultAsync(b => b.Id == id);

        return book is null ? null : BookMapper.ToResponse(book);
    }

    public async Task<BookResponseDTO> CreateAsync(BookCreateDTO dto)
    {
        var author = await _context.Authors.FirstOrDefaultAsync(a => a.Name == dto.Author)
                     ?? new Author { Name = dto.Author, Bio = string.Empty };

        var entity = BookMapper.ToEntity(dto);
        entity.Author = author;

        _context.Books.Add(entity);
        await _context.SaveChangesAsync();

        await _context.Entry(entity).Reference(b => b.Author).LoadAsync();
        return BookMapper.ToResponse(entity);
    }

    public async Task<BookResponseDTO?> UpdateAsync(int id, BookUpdateDTO dto)
    {
        var existing = await _context.Books
            .Include(b => b.Author)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (existing is null) return null;

        var author = await _context.Authors.FirstOrDefaultAsync(a => a.Name == dto.Author)
                     ?? new Author { Name = dto.Author, Bio = string.Empty };

        BookMapper.ApplyUpdate(dto, existing);
        existing.Author = author;

        await _context.SaveChangesAsync();
        return BookMapper.ToResponse(existing);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _context.Books.FindAsync(id);
        if (existing is null) return false;

        _context.Books.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
