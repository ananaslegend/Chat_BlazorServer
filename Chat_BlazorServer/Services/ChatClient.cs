using Chat_BlazorServer.Domain.DTOs;
using Chat_BlazorServer.Domain.Enums;
using Chat_BlazorServer.Helpers.Abstractions;
using Chat_BlazorServer.Shared.Components;
using Chat_BlazorServer.Shared.Components.Abstractions;
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

        public async Task<HttpResponseMessage> GetResponseAllUserChatsAsync(NameModel model)
        {
            var token = await tokenHelper.GetTokenAsync();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(model);
            var payload = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("chats/all_user_chats/", payload);
            //return await response.Content.ReadFromJsonAsync<List<ChatDisplay>>();
            return response;
        }

        public async Task<IEnumerable<ChatDisplayModel>> GetChatsByName(NameModel chatName)
        {
            var token = await tokenHelper.GetTokenAsync();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(chatName);
            var payload = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("chats/find_chats/", payload);

            return await response.Content.ReadFromJsonAsync<IEnumerable<ChatDisplayModel>>();
        }

        public async Task CreateChat(CreateChatModel model, ChatType type)
        {
            var token = await tokenHelper.GetTokenAsync();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(model);
            var payload = new StringContent(json, Encoding.UTF8, "application/json");

            if(type == ChatType.PrivateChat)
            {
                var response = await client.PostAsync("chats/create_private_chat", payload);
            }
                
            if(type == ChatType.PublicChat)
            {
                var response = await client.PostAsync("chats/create_public_chat", payload);
            }  
        }

        public async Task<bool> JoinToChat(int chatId, string userName)
        {
            var token = await tokenHelper.GetTokenAsync();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(new JoinChatModel()
            {   
                ChatId = chatId,
                UserName = userName
            });
            var payload = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("chats/join_to_chat/", payload);

            return response.IsSuccessStatusCode ? true : false;
        }
    }
}
