using Microsoft.EntityFrameworkCore;
using shortenerApi.Entities;
using shortenerApi.Services;

namespace shortenerApi.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<ShortenedUrl> ShortenedUrls { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<ShortenedUrl>().HasIndex(u => u.Id).IsUnique();
                modelBuilder.Entity<ShortenedUrl>().Property(u => u.Code).HasMaxLength(UrlShortenerService.NumberOfChartsShortlink);
            }


}
