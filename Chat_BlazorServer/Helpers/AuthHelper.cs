using Blazored.LocalStorage;
using Chat_BlazorServer.Helpers.Abstractions;

namespace Chat_BlazorServer.Helpers
{
    public class AuthHelper : IAuthHelper
    {
        private readonly ILocalStorageService localStorage;

        public AuthHelper(ILocalStorageService localStorage)
        {
            this.localStorage = localStorage;
        }
        
        public async Task<string> GetToken()
        {
            var authToken = await localStorage.GetItemAsStringAsync("authToken");

            if (authToken is null)
                return null;

            return authToken.Trim('"');
        }
    }
}
