using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Core.Constans;
using WpfApp.Core.Models;

namespace WpfApp.Core.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<AtomicHabitModel> AtomicHabits { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AtomicHabitModel>().ToTable("AtomicHabits");
        }
    }

    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            // connection string taka sama jak w Bootstrapper.cs
            optionsBuilder.UseSqlite($"Data Source={DbConnectionString.connectionString}");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
