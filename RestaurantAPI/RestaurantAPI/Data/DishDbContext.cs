using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Models;

namespace RestaurantAPI.Data
{
    public class DishDbContext : DbContext
    {
        public DishDbContext(DbContextOptions options) : base(options)
        {

        }

        public virtual DbSet<Dish> Dishes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dish>().Property(o => o.Price).HasColumnType("decimal(15,2)");
        }

    }
}
