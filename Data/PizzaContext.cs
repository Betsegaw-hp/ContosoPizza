using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Models
{
	public class PizzaDbContext(DbContextOptions<PizzaDbContext> options) : DbContext(options)
	{
		public DbSet<Pizza> Pizzas { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderItem> OrderItems { get; set; }
		public DbSet<PizzaSize> PizzaSizes { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite("Data Source=pizza.db")
					.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Order>()
				.HasOne(o => o.User)
				.WithMany()
				.HasForeignKey(o => o.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<OrderItem>()
				.HasOne(oi => oi.PizzaSize)
				.WithMany()
				.HasForeignKey(oi => oi.PizzaSizeId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}

}