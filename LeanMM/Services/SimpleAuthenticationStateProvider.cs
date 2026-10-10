using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace LeanMM.Services
{
    public class SimpleAuthenticationStateProvider : AuthenticationStateProvider
    {
        private ClaimsPrincipal _currentUser = new ClaimsPrincipal(new ClaimsIdentity());

        public string? UserName => _currentUser?.Identity?.Name;

        public void Login(string username)
        {
            var identity = new ClaimsIdentity(new[] 
            {
                new Claim(ClaimTypes.Name, username)
            }, "DummyAuth");

            _currentUser = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
        }

        public void Logout()
        {
            _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(new AuthenticationState(_currentUser));
        }
    }
}
