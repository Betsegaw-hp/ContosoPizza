using Microsoft.AspNetCore.Authorization;

namespace ContosoPizza.Policies
{
	public class OrderOwnerOrAdminRequirement : IAuthorizationRequirement
	{
		// just a marker for the policy
	}
}