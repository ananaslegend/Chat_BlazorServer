using Chat_BlazorServer.Domain.DTOs;
using RestSharp;

namespace Chat_BlazorServer.Services.Abstractions
{
    public interface IChatClient
    {
        public string BaseUrl { get; }
        public string AuthToken { get; }

        public Task<RestResponse> GetAsync(string path);
        public Task<T> GetAsync<T>(string path);
        public Task<string> PostLoginAsync(string path, UserLoginDTO user);
    }
}
