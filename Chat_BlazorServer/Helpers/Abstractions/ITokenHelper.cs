namespace Chat_BlazorServer.Helpers.Abstractions
{
    public interface ITokenHelper
    {
        public Task<string> GetTokenAsync();
        public Task RemoveTokenAsync();
        public Task WriteTokenAsync(HttpResponseMessage response);
    }
}
