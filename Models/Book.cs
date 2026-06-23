namespace MyBookApi2.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int AuthorId { get; set; }
    public Author Author { get; set; } = null!;
    public decimal Price { get; set; }
    public DateTime PublishedDate { get; set; }
    public string Isbn { get; set; } = string.Empty;
    public ICollection<BookTag> BookTags { get; set; } = new List<BookTag>();
}
