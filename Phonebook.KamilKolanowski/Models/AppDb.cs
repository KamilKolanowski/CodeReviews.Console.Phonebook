using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Phonebook.KamilKolanowski.Models;

public class AppDb
{
    internal class AppDbContext : DbContext
    {
        internal DbSet<Contact> Contacts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("TCSA");
            base.OnModelCreating(modelBuilder);
        }
    }
}
