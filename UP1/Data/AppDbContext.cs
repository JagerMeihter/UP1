using System.Data.Entity;
using UP1.Models;

namespace UP1.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("ReadWriteDbConnection")
        {
            // Автоматическое создание и обновление БД при запуске
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AppDbContext, Configuration>());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<BookGenre> BookGenres { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<UserBookList> UserBookLists { get; set; }
        public DbSet<ReadingStatus> ReadingStatuses { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<ComplaintType> ComplaintTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookGenre>()
                .HasKey(bg => new { bg.BookId, bg.GenreId });

            base.OnModelCreating(modelBuilder);
        }
    }
}