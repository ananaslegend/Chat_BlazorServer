using Chat_BlazorServer.Helpers.Abstractions;
using System.Net.Http.Headers;

namespace Chat_BlazorServer.Services
{
    public class ChatClient
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ITokenHelper tokenHelper;
        private readonly HttpClient client;

        public ChatClient(IHttpClientFactory httpClientFactory, ITokenHelper tokenHelper)
        {
            this.httpClientFactory = httpClientFactory;
            this.tokenHelper = tokenHelper;

            client = httpClientFactory.CreateClient("BaseClient");
        }

        public async Task<HttpResponseMessage> GetAuthAsync(string path)
        {
            var token = await tokenHelper.GetTokenAsync();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            return await client.GetAsync(path);
        }
    }
}
