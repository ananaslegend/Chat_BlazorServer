using RestSharp;
using Blazored.LocalStorage;
using RestSharp.Authenticators;
using Microsoft.AspNetCore.Mvc;
using Chat_BlazorServer.Services.Abstractions;
using Chat_BlazorServer.Helpers;
using Chat_BlazorServer.Domain.DTOs;
using Chat_BlazorServer.Helpers.Abstractions;

namespace Chat_BlazorServer.Services
{
    public class ChatClient : IChatClient
    {
        private readonly ILocalStorageService localStorage;
        private readonly IConfiguration conf;
        private readonly IAuthHelper authHelper;
        private readonly RestClient _client;
        private readonly string _authToken;

        public string BaseUrl { get; set; }
        public string AuthToken { get; set; }

        public ChatClient(ILocalStorageService localStorage, IConfiguration conf, IAuthHelper authHelper)
        { 
            this.localStorage = localStorage;
            this.conf = conf;
            this.authHelper = authHelper;
            BaseUrl = conf["Url:BaseUrl"];

            var options = new RestClientOptions(conf["Url:BaseUrl"]);
            _client = new RestClient(conf["Url:BaseUrl"]);
        }

        public async Task<RestResponse> GetAsync(string path)
        {
            AuthToken = await authHelper.GetToken();

            _client.Authenticator = new JwtAuthenticator(AuthToken);

            var request = new RestRequest(path);

            var response = await _client.GetAsync(request);

            return response;
        }

        public async Task<T> GetAsync<T>(string path)
        {
            AuthToken = await authHelper.GetToken();

            _client.Authenticator = new JwtAuthenticator(AuthToken);

            var request = new RestRequest(path);

            var response = await _client.GetAsync<T>(request);

            return response;
        }

        public async Task<string> PostLoginAsync(string path, UserLoginDTO user)
        {           
            var request = new RestRequest(path);
            request.AddJsonBody(user);

            var response = await _client.PostAsync<string>(request);
            
            return response;
        }
    }
}
