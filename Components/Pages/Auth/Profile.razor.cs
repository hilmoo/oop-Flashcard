using Microsoft.AspNetCore.Components;

namespace flashcard.Components.Pages.Auth
{
	public partial class Profile : ComponentBase
	{
		private bool isAuthenticated = false;
		private string userName = string.Empty;
		private string userEmail = string.Empty;

		protected override async Task OnInitializedAsync()
		{
			var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
			var user = authState.User;

			if (user.Identity?.IsAuthenticated == true)
			{
				isAuthenticated = true;

				userName = user.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value
						?? "No name";

				userEmail = user.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value
						?? "No email";

				// Debug information
				Console.WriteLine($"User is authenticated. Name: {userName}, Email: {userEmail}");
				foreach (var claim in user.Claims)
				{
					Console.WriteLine($"Claim Type: {claim.Type}, Value: {claim.Value}");
				}
			}
		}
	}
}
