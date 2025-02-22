using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoPizza.Models
{
	public enum OrderStatus
	{
		Pending,    // Order placed but not yet processed
		Processing, // being prepared
		Completed,  // Delivered
		Cancelled   // User(for thier own order) or admin cancelled it
	}

	public class Order
	{
		[Key]
		public int Id { get; set; }

		[ForeignKey("User")]
		public int UserId { get; set; }

		public DateTime Timestamp { get; set; } = DateTime.UtcNow;

		public OrderStatus Status { get; set; } = OrderStatus.Pending;

		[Required]
		[Column(TypeName = "decimal(10,2)")]
		public decimal TotalPrice { get; set; } = 0;

		// nav
		public virtual User User { get; set; } = null!;

		// One order can have many order items
		public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

		public void UpdateTotalPrice()
		{
			// I should call this when the order updated
			TotalPrice = OrderItems.Sum(item => item.UnitPrice * item.Quantity);
		}

	}
}