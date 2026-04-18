using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using TailspinToys.Api.Models;

namespace TailspinToys.Api;

public class TailspinToysContext : DbContext
{
    public TailspinToysContext(DbContextOptions<TailspinToysContext> options)
        : base(options)
    {
    }

    public DbSet<Game> Games => Set<Game>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Publisher> Publishers => Set<Publisher>();

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        ValidatePendingEntities();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        ValidatePendingEntities();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>(entity =>
        {
            entity.ToTable("games");
            entity.HasOne(g => g.Category)
                .WithMany(c => c.Games)
                .HasForeignKey(g => g.CategoryId);
            entity.HasOne(g => g.Publisher)
                .WithMany(p => p.Games)
                .HasForeignKey(g => g.PublisherId);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("categories");
            entity.HasIndex(c => c.Name).IsUnique();
        });

        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.ToTable("publishers");
            entity.HasIndex(p => p.Name).IsUnique();
        });
    }

    private void ValidatePendingEntities()
    {
        foreach (var entry in ChangeTracker.Entries()
                     .Where(e => e.State is EntityState.Added or EntityState.Modified))
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(entry.Entity);

            if (!Validator.TryValidateObject(entry.Entity, validationContext, validationResults, validateAllProperties: true))
            {
                var firstError = validationResults.FirstOrDefault()?.ErrorMessage ?? "Validation failed.";
                throw new ValidationException(firstError);
            }
        }
    }
}
