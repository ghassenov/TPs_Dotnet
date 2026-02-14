using Microsoft.EntityFrameworkCore;
using MoviesCrudApp.Data.Interceptors;
using MoviesCrudApp.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;

namespace MoviesCrudApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<MembershipType> MembershipTypes { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.AddInterceptors(new AuditLogInterceptor());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration of Genre-Movie relationship
            modelBuilder.Entity<Movie>()
                .HasOne(m => m.Genre)
                .WithMany(g => g.Movies)
                .HasForeignKey(m => m.GenreId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuration of MembershipType-Customer relationship
            modelBuilder.Entity<Customer>()
                .HasOne(c => c.MembershipType)
                .WithMany(m => m.Customers)
                .HasForeignKey(c => c.MembershipTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed Genres
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

            // Seed MembershipTypes
            modelBuilder.Entity<MembershipType>().HasData(
                new MembershipType { Id = 1, Name = "Bronze", DiscountRate = 5, Description = "Adhésion Bronze" },
                new MembershipType { Id = 2, Name = "Silver", DiscountRate = 10, Description = "Adhésion Silver" },
                new MembershipType { Id = 3, Name = "Gold", DiscountRate = 15, Description = "Adhésion Gold" },
                new MembershipType { Id = 4, Name = "Platinum", DiscountRate = 20, Description = "Adhésion Platinum" }
            );

            // Seed Movies from JSON file
            SeedMoviesFromJson(modelBuilder);
        }

        private void SeedMoviesFromJson(ModelBuilder modelBuilder)
        {
            try
            {
                var jsonPath = Path.Combine(System.AppContext.BaseDirectory, "Data/JsonSeed/Movies.json");
                if (File.Exists(jsonPath))
                {
                    var json = File.ReadAllText(jsonPath);
                    var movies = JsonSerializer.Deserialize<List<Movie>>(json, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                    if (movies != null && movies.Any())
                    {
                        modelBuilder.Entity<Movie>().HasData(movies);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle exception - JSON seeding is optional
                System.Diagnostics.Debug.WriteLine($"Error seeding movies from JSON: {ex.Message}");
            }
        }
    }
}
