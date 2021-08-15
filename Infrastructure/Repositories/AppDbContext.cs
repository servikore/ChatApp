using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Id);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();            

            modelBuilder.Entity<User>()
                .HasMany(u => u.Messages)
                .WithOne();

            modelBuilder.Entity<Message>().HasKey(u => u.Id);
            modelBuilder.Entity<Message>()
            .HasOne(m => m.User)
            .WithMany(b => b.Messages);
        }
    }
}
