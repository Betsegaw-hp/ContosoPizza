using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ContosoPizza.Models;
using ContosoPizza.Models.Configurations;
using Microsoft.Extensions.Options;
using ContosoPizza.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.OpenApi.Extensions;

namespace ContosoPizza.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
	private readonly JwtConfigs _jwtConfigs;
	private readonly PizzaDbContext _context;

	public AuthController(IOptions<JwtConfigs> jwtConfigs, PizzaDbContext context)
	{
		_jwtConfigs = jwtConfigs.Value;
		_context = context;
	}

	[HttpPost("signup")]
	public async Task<IActionResult> SignUp([FromBody] LoginModel login)
	{
		var existingUser = await _context.Users
			.FirstOrDefaultAsync(u => u.Username == login.Username);

		if (existingUser != null)
		{
			return BadRequest("Username already exists.");
		}

		// Hash the password
		var passwordHash = HashPassword(login.Password);

		var adminCount = await _context.Users.CountAsync(u => u.Role == RoleOpt.Admin);
		var newUser = new User
		{
			Username = login.Username,
			PasswordHash = passwordHash,
			Role = adminCount > 0 ? RoleOpt.User : RoleOpt.Admin
		};

		_context.Users.Add(newUser);
		await _context.SaveChangesAsync();

		return Ok(new { message = "User successfully registered." });
	}

	[HttpPost("login")]
	public async Task<IActionResult> Login([FromBody] LoginModel login)
	{
		var user = await _context.Users
				.FirstOrDefaultAsync(u => u.Username == login.Username);
		string secretKey = _jwtConfigs?.SecretKey ?? throw new ArgumentNullException("JWT secret key is not Foud!");


		if (user == null || !VerifyPassword(login.Password, user.PasswordHash))
		{
			return Unauthorized("Invalid credentials.");
		}
		var userRole = user.Role.GetDisplayName();
		var claims = new[]
		{
			new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
			new Claim(ClaimTypes.Role, userRole)
		};

		var key = new SymmetricSecurityKey(Encoding.UTF32.GetBytes(secretKey));
		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		var token = new JwtSecurityToken(
			issuer: _jwtConfigs.ValidIssuer,
			audience: _jwtConfigs.ValidAudience,
			claims: claims,
			expires: DateTime.Now.AddMinutes(30),
			signingCredentials: creds);

		var Response = new AuthResponseDto
		{
			Token = new JwtSecurityTokenHandler().WriteToken(token),
			User = new UserDto { Username = login.Username, Role = userRole }
		};

		return Ok(Response);
	}

	[HttpPost("logout")]
	public IActionResult LogOut()
	{
		// TODO: Revoke tokens from server using Blacklisting
		return Ok(new { message = "Logged out successfully." });
	}

	private static string HashPassword(string password)
	{
		byte[] salt = new byte[16];
		using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
		{
			rng.GetBytes(salt);
		}

		// Hash the password using PBKDF2
		byte[] hash = KeyDerivation.Pbkdf2(
			password: password,
			salt: salt,
			prf: KeyDerivationPrf.HMACSHA256,
			iterationCount: 10000,
			numBytesRequested: 256 / 8);

		// Combine salt and hash (store as base64)
		return Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
	}

	private static bool VerifyPassword(string enteredPassword, string storedPasswordHash)
	{
		// Split the stored hash to get salt and password hash
		var parts = storedPasswordHash.Split(':');
		if (parts.Length != 2) return false;

		byte[] salt = Convert.FromBase64String(parts[0]);
		byte[] storedHash = Convert.FromBase64String(parts[1]);

		// Hash the entered password with the stored salt
		byte[] enteredHash = KeyDerivation.Pbkdf2(
			password: enteredPassword,
			salt: salt,
			prf: KeyDerivationPrf.HMACSHA256,
			iterationCount: 10000,
			numBytesRequested: 256 / 8);

		// Compare both hashes
		return storedHash.SequenceEqual(enteredHash);
	}

}
