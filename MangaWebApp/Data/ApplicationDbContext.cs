using MangaWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MangaWebApp.Data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Shonen", DisplayOrder = 1 },
                new Category { Id = 2, Name = "Shojo", DisplayOrder = 2 },
                new Category { Id = 3, Name = "Seinin", DisplayOrder = 3 },
                new Category { Id = 4, Name = "Josei", DisplayOrder = 4 },
                new Category { Id = 5, Name = "Kodomomuke", DisplayOrder = 5 }
                );
        }
    }
}
