// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ParagonTestApplication.Data
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using ParagonTestApplication.Models.DataModels;

    /// <summary>
    /// Main Db Context.
    /// </summary>
    public class MainDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainDbContext"/> class.
        /// </summary>
        /// <param name="options">DbContextOptions.</param>
        public MainDbContext(DbContextOptions<MainDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets webinars set.
        /// </summary>
        public DbSet<Webinar> Webinars { get; set; }

        /// <summary>
        /// Gets or sets series set.
        /// </summary>
        public DbSet<Series> Series { get; set; }

        /// <summary>
        /// SaveChanges.
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">acceptAllChangesOnSuccess.</param>
        /// <returns>Save changes.</returns>
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        /// <summary>
        /// SaveChanges (async).
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">acceptAllChangesOnSuccess.</param>
        /// <param name="cancellationToken">cancellationToken.</param>
        /// <returns>Save changes.</returns>
        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            this.OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        /// <summary>
        /// OnModelCreating.
        /// </summary>
        /// <param name="modelBuilder">ModelBuilder.</param>
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

        private void OnBeforeSaving()
        {
            foreach (var entry in this.ChangeTracker.Entries())
            {
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
}