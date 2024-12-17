using Microsoft.AspNetCore.Components;
using System.Security.Claims;

namespace flashcard.Components.Pages.Auth
{
    public partial class Profile : ComponentBase
    {
        private bool isAuthenticated = false;
        private string userName = string.Empty;
        private string userEmail = string.Empty;
        private string photoProfile = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity?.IsAuthenticated == true)
            {
                isAuthenticated = true;

                userName = user.FindFirst(ClaimTypes.Name)?.Value
                           ?? "No name";

                userEmail = user.FindFirst(ClaimTypes.Email)?.Value
                            ?? "No email";

                photoProfile = user.FindFirst("urn:google:picture")?.Value
                               ?? "err";

                // Debug information
                Console.WriteLine($"User is authenticated. Name: {userName}, Email: {userEmail}");
            }
        }
    }
}