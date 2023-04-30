using Microsoft.EntityFrameworkCore;
using PublisherDomain;

namespace PublisherData
{
    public class PublisherContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server = localhost; Database = PublisherDatabase; Trusted_Connection = True;");
        }
    }
}