using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Components;

namespace flashcard.Components.Pages.Auth
{
	public partial class Login : ComponentBase
	{
		[CascadingParameter]
		public required HttpContext HttpContext { get; set; }


		protected override async void OnInitialized()
		{
			var authProperties = new AuthenticationProperties
			{
				RedirectUri = "/google/callback",
			};
			var result = TypedResults.Challenge(authProperties, [GoogleDefaults.AuthenticationScheme]);
			await result.ExecuteAsync(HttpContext);
		}
	}
}
