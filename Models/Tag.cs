namespace MyBookApi2.Models;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<BookTag> BookTags { get; set; } = new List<BookTag>();
}
