using System.ComponentModel.DataAnnotations;


namespace ContosoPizza.Models
{
	public enum RoleOpt
	{
		Admin,
		User
	}
	public class User
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string Username { get; set; } = string.Empty;

		[Required]
		public string PasswordHash { get; set; } = string.Empty; // Store hashed password
		public RoleOpt Role { get; set; } = RoleOpt.User; // Default role
	}
}
