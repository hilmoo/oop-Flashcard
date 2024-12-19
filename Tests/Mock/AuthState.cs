using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace flashcard.Tests.Mock;

public class AuthState
{
    public class MockAuthenticationStateProvider(AuthenticationState authState) : AuthenticationStateProvider
    {
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(authState);
        }
    }

    public static void SetupAuthenticationState(IServiceCollection services, string userName = "Test1",
        string userEmail = "tes1@mail.com")
    {
        var authState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity([
            new Claim(ClaimTypes.Name, userName),
            new Claim(ClaimTypes.Email, userEmail)
        ], "mock")));

        var authStateProvider = new AuthState.MockAuthenticationStateProvider(authState);
        services.AddSingleton<AuthenticationStateProvider>(authStateProvider);
    }
}