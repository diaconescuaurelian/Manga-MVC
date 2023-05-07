using Manga.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Manga.DataAccess.Data
{
    public class ApplicationDbContext :IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Shonen", DisplayOrder = 1 },
                new Category { Id = 2, Name = "Shojo", DisplayOrder = 2 },
                new Category { Id = 3, Name = "Seinin", DisplayOrder = 3 },
                new Category { Id = 4, Name = "Josei", DisplayOrder = 4 },
                new Category { Id = 5, Name = "Kodomomuke", DisplayOrder = 5 }
                );

            modelBuilder.Entity<Product>().HasData(
               new Product 
               { 
                   Id = 1, 
                   Title = "Ajin Demi-Human", 
                   Author = "Gamon Sakurai",
                   VolumeNumber = 1,
                   Price = 67,
                   ISBN = "9781939130846",
                   CategoryId= 3,
                   ImageUrl="",
                   Description = "A bright high schooler has discovered to his horror that death is just a repeatabl event for him—and that humanity has no mercy for a demi-human. To avoid becoming a science experiment for the rest of his interminable life, Kei Nagai must seek out others of his kind. But what would a community of them stand for?",
                   },
               new Product
               {
                   Id = 2,
                   Title = "Ajin Demi-Human",
                   Author = "Gamon Sakurai",
                   VolumeNumber = 2,
                   Price = 67,
                   ISBN = "9781939130853",
                   CategoryId = 3,
                   ImageUrl = "",
                   Description = "A bright high schooler has discovered to his horror that death is just a repeatabl event for him—and that humanity has no mercy for a demi-human. To avoid becoming a science experiment for the rest of his interminable life, Kei Nagai must seek out others of his kind. But what would a community of them stand for?",
               },
               new Product 
               { 
                   Id = 3, 
                   Title = "Attack on Titan", 
                   Author = "Hajime Isayama",
                   VolumeNumber = 1,
                   Price = 60,
                   ISBN = "9781612620244",
                   CategoryId = 1,
                   ImageUrl = "",
                   Description = "Several hundred years ago, humans were nearly exterminated by giants. Giants are typically several stories tall, seem to have no intelligence and who devour human beings.\r\n\r\nA small percentage of humanity survied barricading themselves in a city protected by walls even taller than the biggest of giants.\r\n\r\nFlash forward to the present and the city has not seen a giant in over 100 years - before teenager Elen and his foster sister Mikasa witness something horrific as the city walls are destroyed by a super-giant that appears from nowhere.\r\n\r\n",
               },
               new Product 
               { 
                   Id = 4,
                   Title = "Death Note",
                   Author = "Takeshi Obata",
                   VolumeNumber = 1,
                   Price = 52,
                   ISBN = "9781421501680",
                   CategoryId = 1,
                   ImageUrl = "",
                   Description = "Light tests the boundaries of the Death Note's powers as L and the police begin to close in. Luckily Light's father is the head of the Japanese National Police Agency and leaves vital information about the case lying around the house. With access to his father's files, Light can keep one step ahead of the authorities.\r\n\r\nBut who is the strange man following him, and how can Light guard against enemies whose names he doesn't know?",
               },
               new Product 
               { 
                   Id = 5, 
                   Title = "Vinland Saga",
                   Author = "Makoto Yukimura",
                   VolumeNumber = 1,
                   Price = 52,
                   ISBN = "9781612624204",
                   CategoryId = 3,
                   ImageUrl = "",
                   Description = "As a child, Thorfinn sat at the feet of the great Leif Ericson and thrilled to wild tales of a land far to the west, but his youthful fantasies were shattered by a mercenary raid. Raised by the Vikings who murdered his family, Thorfinn became a terrifying warrior, forever seeking to kill the band's leader, Askeladd, and avenge his father.\r\n\r\nSustaining Thorfinn through his ordeal are his pride in his family and his dreams of a fertile westward land, a land without war or slavery... the land Leif called Vinland.",
               },
               new Product 
               { 
                   Id = 6, 
                   Title = "My Hero Academia",
                   Author = "Kohei Horikoshi",
                   VolumeNumber = 1,
                   Price = 118,
                   ISBN = "9781421582696",
                   CategoryId = 1,
                   ImageUrl = "",
                   Description = "What would the world be like if 80 percent of the population manifested superpowers called “Quirks” at age four? Heroes and villains would be battling it out everywhere! Being a hero would mean learning to use your power, but where would you go to study? The Hero Academy of course! But what would you do if you were one of the 20 percent who were born Quirkless?\r\n\r\nMiddle school student Izuku Midoriya wants to be a hero more than anything, but he hasn’t got an ounce of power in him. With no chance of ever getting into the prestigious U.A. High School for budding heroes, his life is looking more and more like a dead end. Then an encounter with All Might, the greatest hero of them all, gives him a chance to change his destiny…",
               }
               );
        }
    }
}
