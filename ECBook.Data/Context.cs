using ECBook.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECBook.Data
{
    public class Context : DbContext
    {
        public virtual DbSet<Profile> Profile { get; set; }
        public virtual DbSet<Post> Post { get; set; }

        public string DbPath { get; }


        private string ConfigureDbPath()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            return System.IO.Path.Join(path, "ecbook.db");
        }

        public Context()
        {
            DbPath = ConfigureDbPath();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");

        public Context(DbContextOptions<Context> options)
            : base(options)
        {
            DbPath = ConfigureDbPath();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Profile>().HasData(
                new Profile { Id = 1, FirstName = "Jane", LastName = "Doe", JobTitle = "Developer" },
                new Profile { Id = 2, FirstName = "John", LastName = "Smith", JobTitle = "Consultant" },
                new Profile { Id = 3, FirstName = "Maisie", LastName = "Jones", JobTitle = "Project Manager" },
                new Profile { Id = 4, FirstName = "Jack", LastName = "Simpson", JobTitle = "Engagement Officer" },
                new Profile { Id = 5, FirstName = "Sadie", LastName = "Williams", JobTitle = "Finance Director" },
                new Profile { Id = 6, FirstName = "Pete", LastName = "Jackson", JobTitle = "Developer" },
                new Profile { Id = 7, FirstName = "Sinead", LastName = "O'Leary", JobTitle = "Consultant" }
                );

            modelBuilder.Entity<Post>().HasData(
                new Post { Id = 1, Content = exampleText, DateTimePosted = new DateTimeOffset(2020, 11, 1, 10, 0, 0, TimeSpan.Zero), AuthorId = 1 },
                new Post { Id = 2, Content = exampleText, DateTimePosted = new DateTimeOffset(2020, 11, 1, 15, 0, 0, TimeSpan.Zero), AuthorId = 2 }
                );
        }

        private static string exampleText = "Lorem Ipsum is simply dummy text of the printing and typesetting " +
                                            "industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an " +
                                            "unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived " +
                                            "not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.";
    }
}
