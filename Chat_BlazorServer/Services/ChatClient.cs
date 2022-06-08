using Chat_BlazorServer.Domain.DTOs;
using Chat_BlazorServer.Helpers.Abstractions;
using Chat_BlazorServer.Shared.Components;
using Newtonsoft.Json;
using System.Collections;
using System.Net.Http.Headers;
using System.Text;

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

        public async Task<ICollection> GetAllUserChatsAsync(string path)
        {
            var token = await tokenHelper.GetTokenAsync();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("all_user_chats");

            return await response.Content.ReadFromJsonAsync<List<ChatDisplay>>();
        }

        public async Task<ICollection<ChatNameIdDto>> GetChatsByName(string chatName)
        {
            var token = await tokenHelper.GetTokenAsync();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"find_chats/{chatName}");

            return await response.Content.ReadFromJsonAsync<List<ChatNameIdDto>>();
        }

        public async Task CreatePrivateChat(CreatePrivateChatModel model)
        {
            var token = await tokenHelper.GetTokenAsync();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(model);
            var payload = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("chats/create_private_chat", payload);
        }
    }
}
