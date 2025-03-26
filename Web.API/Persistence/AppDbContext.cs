using API.Domain;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;

namespace API.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    public DbSet<Product> Products { get; set; }
    public DbSet<Log> Logs { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().ToCollection("products");
        modelBuilder.Entity<Log>().ToCollection("logs");

        modelBuilder.Entity<Product>().HasKey(p => p.Id);
        modelBuilder.Entity<Product>().HasData(
            new Product("iPhone 15 Pro", "Apple's latest flagship smartphone with a ProMotion display and improved cameras", 999.99m),
            new Product("Dell XPS 15", "Dell's high-performance laptop with a 4K InfinityEdge display", 1899.99m),
            new Product("Sony WH-1000XM4", "Sony's top-of-the-line wireless noise-canceling headphones", 349.99m),
            new Product("Sony WH-1000XM4", "Sony's top-of-the-line wireless noise-canceling headphones", 349.99m)
            );
        
        modelBuilder.Entity<Log>().HasKey(p => p.Id);
        modelBuilder.Entity<Log>().HasData(
            new Log(requestBody:"test",responseBody:"test",requestDate:DateTimeOffset.Now, responseDate:DateTimeOffset.Now.AddHours(1))
        );
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // optionsBuilder.UseInMemoryDatabase("codewithmuca");
        
        var mongoClient = new MongoClient("mongodb://localhost:27017");
        optionsBuilder.UseMongoDB(mongoClient, "codewithmuca");
        optionsBuilder.EnableSensitiveDataLogging(true);
    }
}