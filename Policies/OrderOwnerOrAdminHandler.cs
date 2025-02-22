using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace ContosoPizza.Policies
{
	public class OrderOwnerOrAdminHandler : AuthorizationHandler<OrderOwnerOrAdminRequirement, int>
	{
		// The resource here is the Order's UserId.
		protected override Task HandleRequirementAsync(
			AuthorizationHandlerContext context,
			OrderOwnerOrAdminRequirement requirement,
			int orderOwnerId)
		{

			if (context.User.IsInRole("Admin"))
			{
				context.Succeed(requirement);
				return Task.CompletedTask;
			}

			// Otherwise, check if the user id == owner ID.
			var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out int userId))
			{
				if (userId == orderOwnerId)
				{
					context.Succeed(requirement);
				}
			}

			return Task.CompletedTask;
		}
	}

}