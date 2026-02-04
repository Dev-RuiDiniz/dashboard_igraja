using System.Net.Http.Json;
using IgrejaSocial.Domain.Models;

namespace IgrejaSocial.Web.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserInfoResponse?> LoginAsync(LoginRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<UserInfoResponse>();
        }

        public async Task LogoutAsync()
        {
            await _httpClient.PostAsync("api/auth/logout", null);
        }

        public async Task<UserInfoResponse?> GetCurrentUserAsync()
        {
            var response = await _httpClient.GetAsync("api/auth/me");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<UserInfoResponse>();
        }
    }
}
