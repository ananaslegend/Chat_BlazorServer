using RestSharp;

namespace Chat_BlazorServer.Services.Abstractions
{
    public interface IRestService
    {
        public string BaseUrl { get; }
        public string AuthToken { get; }

        public Task<RestResponse> GetAsync(string path);
    }
}
