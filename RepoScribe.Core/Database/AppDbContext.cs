using Microsoft.EntityFrameworkCore;
using RepoScribe.Core.Database.Entities;

namespace RepoScribe.Core.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<ContentItemEntity> ContentItems { get; set; }
        public DbSet<CodeContentItemEntity> CodeContentItems { get; set; }
        public DbSet<ImageContentItemEntity> ImageContentItems { get; set; }
        public DbSet<PdfContentItemEntity> PdfContentItems { get; set; }
        public DbSet<RepositoryContentItemEntity> RepositoryContentItems { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Fluent API configurations
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure inheritance
            modelBuilder.Entity<ContentItemEntity>()
                .HasDiscriminator<string>("ContentType")
                .HasValue<ContentItemEntity>("ContentItem")
                .HasValue<CodeContentItemEntity>("CodeContentItem")
                .HasValue<ImageContentItemEntity>("ImageContentItem")
                .HasValue<PdfContentItemEntity>("PdfContentItem")
                .HasValue<RepositoryContentItemEntity>("RepositoryContentItem");

            // Additional configurations can be added here
        }
    }
}
