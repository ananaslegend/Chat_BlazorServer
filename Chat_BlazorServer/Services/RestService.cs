using RestSharp;
using Blazored.LocalStorage;
using RestSharp.Authenticators;
using Microsoft.AspNetCore.Mvc;
using Chat_BlazorServer.Services.Abstractions;

namespace Chat_BlazorServer.Services
{
    public class RestService : IRestService
    {
        private readonly ILocalStorageService localStorage;
        private readonly IConfiguration conf;

        public string BaseUrl { get; set; }
        public string AuthToken { get; private set; }

        public RestService(ILocalStorageService localStorage, IConfiguration conf)
        { 
            this.localStorage = localStorage;
            this.conf = conf;

            BaseUrl = conf["Url:BaseUrl"];
        }

        public async Task<RestResponse> GetAsync(string path)
        {
            AuthToken = await localStorage.GetItemAsStringAsync("authToken");

            var client = new RestClient(BaseUrl + path)
            {
                Authenticator = new HttpBasicAuthenticator("Bearer", AuthToken)
            };

            var request = new RestRequest();

            var response = await client.GetAsync(request);

            return response;
            
        }
    }
}
