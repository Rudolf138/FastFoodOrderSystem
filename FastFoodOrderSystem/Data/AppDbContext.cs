using Microsoft.EntityFrameworkCore;
using System;

namespace FastFoodOrderSystem.Data
{
    public class RecentOrder
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class Setting
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class AppDbContext : DbContext
    {
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<RecentOrder> RecentOrders { get; set; }
        public DbSet<Setting> Settings { get; set; }

        private readonly string _dbPath;

        public AppDbContext()
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var dir = System.IO.Path.Combine(folder, "PrimePlate");
            if (!System.IO.Directory.Exists(dir)) System.IO.Directory.CreateDirectory(dir);
            _dbPath = System.IO.Path.Combine(dir, "primeplate.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={_dbPath}");
        }
    }
}
