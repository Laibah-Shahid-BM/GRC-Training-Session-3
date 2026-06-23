using Microsoft.EntityFrameworkCore;
using MyBookApi2.Models;

namespace MyBookApi2.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Book> Books => Set<Book>();
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<BookTag> BookTags => Set<BookTag>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookTag>()
            .HasKey(bt => new { bt.BookId, bt.TagId });

        modelBuilder.Entity<Book>()
            .Property(b => b.Price)
            .HasColumnType("decimal(18,2)");

        // --- Seed Authors ---
        modelBuilder.Entity<Author>().HasData(
            new Author { Id = 1, Name = "Robert C. Martin", Bio = "Software engineer known as Uncle Bob, author of Clean Code" },
            new Author { Id = 2, Name = "Martin Fowler",    Bio = "Software engineer, author and speaker on software design" },
            new Author { Id = 3, Name = "Andrew Hunt",      Bio = "Programmer and co-author of The Pragmatic Programmer" }
        );

        // --- Seed Tags ---
        modelBuilder.Entity<Tag>().HasData(
            new Tag { Id = 1, Name = "Programming" },
            new Tag { Id = 2, Name = "Software Design" },
            new Tag { Id = 3, Name = "Refactoring" },
            new Tag { Id = 4, Name = "Best Practices" }
        );

        // --- Seed Books (8) ---
        modelBuilder.Entity<Book>().HasData(
            new Book { Id = 1, Title = "Clean Code",                                       AuthorId = 1, Price = 34.99m, PublishedDate = new DateTime(2008, 8,  1) },
            new Book { Id = 2, Title = "Clean Architecture",                               AuthorId = 1, Price = 39.99m, PublishedDate = new DateTime(2017, 9, 20) },
            new Book { Id = 3, Title = "The Clean Coder",                                  AuthorId = 1, Price = 29.99m, PublishedDate = new DateTime(2011, 5, 13) },
            new Book { Id = 4, Title = "Refactoring",                                      AuthorId = 2, Price = 44.99m, PublishedDate = new DateTime(2018, 11,20) },
            new Book { Id = 5, Title = "Patterns of Enterprise Application Architecture",  AuthorId = 2, Price = 49.99m, PublishedDate = new DateTime(2002, 11, 5) },
            new Book { Id = 6, Title = "Analysis Patterns",                                AuthorId = 2, Price = 39.99m, PublishedDate = new DateTime(1996, 10,25) },
            new Book { Id = 7, Title = "The Pragmatic Programmer",                         AuthorId = 3, Price = 49.99m, PublishedDate = new DateTime(1999, 10,20) },
            new Book { Id = 8, Title = "Pragmatic Unit Testing in C# with NUnit",          AuthorId = 3, Price = 34.99m, PublishedDate = new DateTime(2007, 7, 17) }
        );

        // --- Seed BookTags (6 links) ---
        modelBuilder.Entity<BookTag>().HasData(
            new BookTag { BookId = 1, TagId = 1 },
            new BookTag { BookId = 1, TagId = 4 },  
            new BookTag { BookId = 2, TagId = 2 },  
            new BookTag { BookId = 4, TagId = 3 },
            new BookTag { BookId = 4, TagId = 2 }, 
            new BookTag { BookId = 7, TagId = 1 }   
        );
    }
}
