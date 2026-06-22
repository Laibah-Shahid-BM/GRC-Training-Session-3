using Microsoft.AspNetCore.Mvc;
using MyBookApi2.DTOs;
using MyBookApi2.Services;

namespace MyBookApi2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    // DI: framework injects the Scoped IBookService — no 'new BookService()' anywhere
    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    // GET api/books?author=martin&page=1&pageSize=10
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? author,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var books = await _bookService.GetAllAsync(author, page, pageSize);
        return Ok(books);
    }

    // GET api/books/3
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var book = await _bookService.GetByIdAsync(id);
        if (book is null) return NotFound();  // auto ProblemDetails in .NET 8

        return Ok(book);
    }

    // POST api/books  → 201 Created + Location header
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BookCreateDTO dto)
    {
        // [ApiController] triggers automatic 400 if ModelState is invalid,
        // so if we reach this line, dto already passed all DataAnnotations.
        var created = await _bookService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT api/books/3
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] BookUpdateDTO dto)
    {
        var updated = await _bookService.UpdateAsync(id, dto);
        if (updated is null) return NotFound();

        return Ok(updated);
    }

    // DELETE api/books/3  → 204 No Content
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _bookService.DeleteAsync(id);
        if (!deleted) return NotFound();

        return NoContent();
    }
}
