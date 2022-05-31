namespace Chat_BlazorServer.Helpers.Abstractions
{
    public interface IAuthHelper
    {
        public Task<string> GetToken();
    }
}
