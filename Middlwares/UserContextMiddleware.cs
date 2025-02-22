using System.Security.Claims;

namespace ContosoPizza.Middlewares
{
	public class UserContextMiddleware
	{
		private readonly RequestDelegate _next;

		public UserContextMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			if (context.User.Identity is { IsAuthenticated: true })
			{
				var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
				context.Items["UserId"] = userId;
			}

			await _next(context);
		}
	}
}