using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ParagonTestApplication.Models.DataModels;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ParagonTestApplication.Data
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
        {
        }

        public DbSet<Webinar> Webinars { get; set; }
        public DbSet<Series> Series { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Webinar>().ToTable("Webinar").HasKey(x => x.Id);
            modelBuilder.Entity<Series>().ToTable("Series").HasKey(x => x.Id);

            modelBuilder.Entity<Webinar>()
                .HasIndex(x => x.Name)
                .IsUnique();

            modelBuilder.Entity<Series>()
                .HasIndex(x => x.Name)
                .IsUnique();

            modelBuilder.Entity<Webinar>()
                .HasQueryFilter(post => EF.Property<bool>(post, "IsDeleted") == false);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            foreach (var entry in ChangeTracker.Entries())
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues["IsDeleted"] = false;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.CurrentValues["IsDeleted"] = true;
                        break;
                }
        }
    }
}