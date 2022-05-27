using Blazored.LocalStorage;

namespace Chat_BlazorServer.Helpers
{
    public class AuthHelper
    {
        private readonly HttpClient httpClient;
        private readonly ILocalStorageService localStorage;

        public AuthHelper(HttpClient httpClient, ILocalStorageService localStorage)
        {
            this.httpClient = httpClient;
            this.localStorage = localStorage;
        }

        public async Task SetAuthHeader()
        {
            var token = await this.localStorage.GetItemAsync<string>("authToken");

            this.httpClient.DefaultRequestHeaders.Authorization
                = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
    }
}
