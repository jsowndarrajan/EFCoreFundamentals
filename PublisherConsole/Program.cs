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
QueryAuthorsByName();
QueryAuthorsByLastName("Karikalan");

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
        PrintAuthors(authors);
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

void QueryAuthorsByName()
{
    using (var context = new PublisherContext())
    {
        //It will produce a non-parameterized SQL query
        var authors = context.Authors.Where(author => author.FirstName == "Sowndarrajan").ToList();
        PrintAuthors(authors);
    }
}

void QueryAuthorsByLastName(string lastName)
{
    using (var context = new PublisherContext())
    {
        //It will produce a parameterized SQL query
        var authors = context.Authors.Where(author => author.LastName == lastName).ToList();
        PrintAuthors(authors);
    }
}

void PrintAuthors(IEnumerable<Author> list)
{
    foreach (var author in list)
    {
        Console.WriteLine(author.FirstName + " " + author.LastName);

        foreach (var authorBook in author.Books)
        {
            Console.WriteLine("\t" + authorBook.Title + " " + authorBook.PublishDate.ToString("d") + " " +
                              authorBook.BasePrice.ToString("C"));
        }
    }
}