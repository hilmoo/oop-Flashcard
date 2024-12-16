using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;

namespace flashcard.Components.Pages.Auth
{
	public partial class Logout : ComponentBase
	{
		[CascadingParameter]
		public required HttpContext HttpContext { get; set; }

		[Inject]
		public required NavigationManager Navigation { get; set; }

		protected override async Task OnInitializedAsync()
		{
			if (!HttpContext.User.Identity!.IsAuthenticated)
			{
				Console.WriteLine("User is not authenticated");
				Navigation.NavigateTo("/");
				return;
			}

			var authProperties = new AuthenticationProperties
			{
				RedirectUri = "/google/callback",
			};
			var result = TypedResults.SignOut(authProperties, ["Cookies"]);
			await result.ExecuteAsync(HttpContext);
		}
	}
}
