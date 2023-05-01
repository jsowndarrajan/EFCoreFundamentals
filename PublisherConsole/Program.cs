// See https://aka.ms/new-console-template for more information

using Microsoft.EntityFrameworkCore;
using PublisherData;
using PublisherDomain;

var context = new PublisherContext();
context.Database.EnsureCreated();

DeleteAuthors();
GetAuthors();
AddAuthor();
GetAuthors();
AddAuthorsAndBooks();
QueryAuthorsByName();
QueryAuthorsByLastName("Karikalan");
FilterAuthorsByPartialText();
FindAuthorsById(1);
AddSomeMoreAuthors();
SkipAndTakeAuthors();

void SkipAndTakeAuthors()
{
    var pageSize = 2;
    for (var offset = 0; offset < 5; offset++)
    {
        var authors = context.Authors.Skip(pageSize * offset).Take(pageSize).ToList();
        Console.WriteLine($"Page #{offset + 1}");
        PrintAuthors(authors);
    }
}

void AddSomeMoreAuthors()
{
    for (var i = 1; i <= 5; i++)
    {
        context.Authors.Add(new Author { FirstName = "Author", LastName = i.ToString() });
    }

    context.SaveChanges();
}

void FindAuthorsById(int authorId)
{
    var author = context.Authors.Find(authorId);
    PrintAuthor(author);
}

void FilterAuthorsByPartialText()
{
    var authors = context.Authors.Where(author => author.FirstName.Contains("So"));
    PrintAuthors(authors);

    var authors1 = context.Authors.Where(author => author.FirstName.StartsWith("So"));
    PrintAuthors(authors1);

    var authors2 = context.Authors.Where(author => EF.Functions.Like(author.FirstName, "So%"));
    PrintAuthors(authors2);
}

void AddAuthorsAndBooks()
{
    var author = new Author { FirstName = "Sowndarrajan", LastName = "Jayapal" };
    var book1 = new Book { Title = "ABC", PublishDate = new DateTime(2023, 04, 02), BasePrice = 100 };
    var book2 = new Book { Title = "XYZ", PublishDate = new DateTime(2023, 02, 01), BasePrice = 200 };

    author.Books = new List<Book> { book1, book2 };
    context.Authors.Add(author);
    context.SaveChanges();
}

void GetAuthors()
{
    var authors = context.Authors.Include(author => author.Books).ToList();
    PrintAuthors(authors);
}

void AddAuthor()
{
    var author = new Author { FirstName = "Aathithaya", LastName = "Karikalan" };
    context.Authors.Add(author);
    context.SaveChanges();
}

void DeleteAuthors()
{
    foreach (var author in context.Authors)
    {
        context.Authors.Remove(author);
    }
    context.SaveChanges();
}

void QueryAuthorsByName()
{
    //It will produce a non-parameterized SQL query
    var authors = context.Authors.Where(author => author.FirstName == "Sowndarrajan").ToList();
    PrintAuthors(authors);
}

void QueryAuthorsByLastName(string lastName)
{
    //It will produce a parameterized SQL query
    var authors = context.Authors.Where(author => author.LastName == lastName).ToList();
    PrintAuthors(authors);
}

void PrintAuthors(IEnumerable<Author> list)
{
    foreach (var author in list)
    {
        PrintAuthor(author);
    }
}

void PrintAuthor(Author author)
{
    if (author is null)
    {
        Console.WriteLine("Unknown author");
        return;
    }

    Console.WriteLine(author.FirstName + " " + author.LastName);

    foreach (var authorBook in author.Books)
    {
        Console.WriteLine("\t" + authorBook.Title + " " + authorBook.PublishDate.ToString("d") + " " +
                          authorBook.BasePrice.ToString("C"));
    }
}