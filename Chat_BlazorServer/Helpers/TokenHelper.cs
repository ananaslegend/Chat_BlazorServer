using Blazored.LocalStorage;
using Chat_BlazorServer.Helpers.Abstractions;

namespace Chat_BlazorServer.Helpers
{
    public class TokenHelper : ITokenHelper
    {
        private readonly ILocalStorageService localStorage;

        public TokenHelper(ILocalStorageService localStorage)
        {
            this.localStorage = localStorage;
        }
        
        public async Task<string> GetTokenAsync()
        {
            var authToken = await localStorage.GetItemAsStringAsync("authToken");

            if (authToken is null)
                return null;

            return authToken.Trim('"');
        }

        public async Task RemoveTokenAsync()
        {
            await localStorage.RemoveItemAsync("authToken");
        }

        public async Task WriteTokenAsync(HttpResponseMessage response)
        {
            var token = await response.Content.ReadAsStringAsync();

            await localStorage.SetItemAsync<string>("authToken", token);
        }
    }
}
