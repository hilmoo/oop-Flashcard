using Microsoft.AspNetCore.Components;
using System.Security.Claims;
using flashcard.model;

namespace flashcard.Components.Layout;

public partial class MainLayout : LayoutComponentBase
{
    private readonly UserProfile userProfile = new();

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();

        var user = authState.User;

        if (user.Identity is not null && user.Identity.IsAuthenticated)
        {
            userProfile.Name = user.Identity.Name ?? string.Empty;
            userProfile.Email = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? string.Empty;
            userProfile.ProfilePicture =
                user.Claims.FirstOrDefault(c => c.Type == "urn:google:picture")?.Value ?? string.Empty;
        }
    }
}