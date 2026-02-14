using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IdentityApp.Models;

namespace IdentityApp.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Produit> Produits { get; set; }
    public DbSet<PanierParUser> PanierParUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure PanierParUser relationships
        builder.Entity<PanierParUser>()
            .HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<PanierParUser>()
            .HasOne(p => p.Produit)
            .WithMany()
            .HasForeignKey(p => p.ProduitId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
