using Microsoft.EntityFrameworkCore;
using MoviesCrudApp.Models;

namespace MoviesCrudApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration de la relation Genre-Film
            modelBuilder.Entity<Movie>()
                .HasOne(m => m.Genre)
                .WithMany(g => g.Movies)
                .HasForeignKey(m => m.GenreId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed des Genres
            modelBuilder.Entity<Genre>().HasData(
                new Genre { Id = 1, Name = "Action", Description = "Films d'action et d'aventure" },
                new Genre { Id = 2, Name = "Comédie", Description = "Films comiques et humoristiques" },
                new Genre { Id = 3, Name = "Drame", Description = "Films dramatiques et émotionnels" },
                new Genre { Id = 4, Name = "Science-Fiction", Description = "Films de science-fiction" },
                new Genre { Id = 5, Name = "Horreur", Description = "Films d'horreur et thriller" },
                new Genre { Id = 6, Name = "Romance", Description = "Films romantiques" },
                new Genre { Id = 7, Name = "Animation", Description = "Films d'animation" },
                new Genre { Id = 8, Name = "Documentaire", Description = "Documentaires" }
            );

            // Seed des Films
            modelBuilder.Entity<Movie>().HasData(
                new Movie
                {
                    Id = 1,
                    Title = "The Shawshank Redemption",
                    Description = "Deux hommes emprisonnés créent un lien fort au fil des années.",
                    ReleaseDate = new DateTime(1994, 9, 23),
                    GenreId = 3,
                    Rating = 9.3m,
                    Duration = 142
                },
                new Movie
                {
                    Id = 2,
                    Title = "The Godfather",
                    Description = "Le patriarche vieillissant d'une dynastie criminelle transfère le contrôle à son fils réticent.",
                    ReleaseDate = new DateTime(1972, 3, 24),
                    GenreId = 3,
                    Rating = 9.2m,
                    Duration = 175
                },
                new Movie
                {
                    Id = 3,
                    Title = "The Dark Knight",
                    Description = "Batman affronte le Joker dans une bataille pour l'âme de Gotham City.",
                    ReleaseDate = new DateTime(2008, 7, 18),
                    GenreId = 1,
                    Rating = 9.0m,
                    Duration = 152
                },
                new Movie
                {
                    Id = 4,
                    Title = "Pulp Fiction",
                    Description = "Les vies de deux tueurs à gages, d'un boxeur et d'un gangster s'entremêlent.",
                    ReleaseDate = new DateTime(1994, 10, 14),
                    GenreId = 1,
                    Rating = 8.9m,
                    Duration = 154
                }
            );
        }
    }
}
