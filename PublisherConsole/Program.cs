// See https://aka.ms/new-console-template for more information

using Microsoft.EntityFrameworkCore;
using PublisherData;
using PublisherDomain;

using (var context = new PublisherContext())
{
    context.Database.EnsureCreated();
}

DeleteAuthors();
GetAuthors();
AddAuthor();
GetAuthors();
AddAuthorsAndBooks();
GetAuthors();

void AddAuthorsAndBooks()
{
    using (var context = new PublisherContext())
    {
        var author = new Author { FirstName = "Sowndarrajan", LastName = "Jayapal" };
        var book1 = new Book { Title = "ABC", PublishDate = new DateTime(2023, 04, 02), BasePrice = 100 };
        var book2 = new Book { Title = "XYZ", PublishDate = new DateTime(2023, 02, 01), BasePrice = 200 };

        author.Books = new List<Book> { book1, book2 };
        context.Authors.Add(author);
        context.SaveChanges();
    }
}

void GetAuthors()
{
    using (var context = new PublisherContext())
    {
        var authors = context.Authors.Include(author => author.Books).ToList();

        foreach (var author in authors)
        {
            Console.WriteLine(author.FirstName + " " + author.LastName);

            foreach (var authorBook in author.Books)
            {
                Console.WriteLine("\t" + authorBook.Title + " " + authorBook.PublishDate.ToString("d") + " " + authorBook.BasePrice.ToString("C"));
            }
        }
    }
}

void AddAuthor()
{
    var author = new Author { FirstName = "Aathithaya", LastName = "Karikalan" };
    using (var context = new PublisherContext())
    {
        context.Authors.Add(author);
        context.SaveChanges();
    }
}

void DeleteAuthors()
{
    using (var context = new PublisherContext())
    {
        foreach (var author in context.Authors)
        {
            context.Authors.Remove(author);
        }
        context.SaveChanges();
    }
}