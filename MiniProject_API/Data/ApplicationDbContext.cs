using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MiniProject_API.Models;

namespace MiniProject_API.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Action" },
                new Category { Id = 2, Name = "SciFi" },
                new Category { Id = 3, Name = "History" },
                new Category { Id = 4, Name = "Fantasy" },
                new Category { Id = 5, Name = "Mystery" }
                );
            modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Title = "The War of Worlds", Description = "A SciFi classic", ISBN = "978-1-234567-01-1", Author = "H.G. Wells", Price = 299.99, CategoryId = 2 },
            new Product { Id = 2, Title = "The Lost Symbol", Description = "A thrilling mystery novel", ISBN = "978-1-234567-02-2", Author = "Dan Brown", Price = 199.99, CategoryId = 5 },
            new Product { Id = 3, Title = "A Brief History of Time", Description = "Understanding the universe", ISBN = "978-1-234567-03-3", Author = "Stephen Hawking", Price = 349.99, CategoryId = 3 },
            new Product { Id = 4, Title = "Dune", Description = "An epic SciFi saga", ISBN = "978-1-234567-04-4", Author = "Frank Herbert", Price = 399.99, CategoryId = 2 },
            new Product { Id = 5, Title = "Harry Potter and the Sorcerer's Stone", Description = "A young wizard's journey", ISBN = "978-1-234567-05-5", Author = "J.K. Rowling", Price = 299.99, CategoryId = 4 },
            new Product { Id = 6, Title = "The Hobbit", Description = "A fantasy adventure", ISBN = "978-1-234567-06-6", Author = "J.R.R. Tolkien", Price = 279.99, CategoryId = 4 },
            new Product { Id = 7, Title = "The Da Vinci Code", Description = "A historical mystery", ISBN = "978-1-234567-07-7", Author = "Dan Brown", Price = 219.99, CategoryId = 5 },
            new Product { Id = 8, Title = "Ender's Game", Description = "A SciFi military story", ISBN = "978-1-234567-08-8", Author = "Orson Scott Card", Price = 259.99, CategoryId = 2 },
            new Product { Id = 9, Title = "The Art of War", Description = "A military strategy book", ISBN = "978-1-234567-09-9", Author = "Sun Tzu", Price = 149.99, CategoryId = 3 },
            new Product { Id = 10, Title = "1984", Description = "A dystopian classic", ISBN = "978-1-234567-10-0", Author = "George Orwell", Price = 189.99, CategoryId = 3 }
        );
        }
    }
}
