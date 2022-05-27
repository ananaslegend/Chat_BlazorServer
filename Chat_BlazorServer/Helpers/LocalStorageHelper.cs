using Blazored.LocalStorage;

namespace Chat_BlazorServer.Helpers
{
    public class LocalStorageHelper
    {
        private readonly ILocalStorageService localStorage;

        public LocalStorageHelper(ILocalStorageService localStorage)
        {
            this.localStorage = localStorage;
        }

        public async Task<string> GetToken()
        {
            return await localStorage.GetItemAsStringAsync("authToken");
        }
    }
}
