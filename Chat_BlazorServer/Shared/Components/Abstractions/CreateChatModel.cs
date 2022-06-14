using Chat_BlazorServer.Domain.Enums;

namespace Chat_BlazorServer.Shared.Components.Abstractions
{
    public interface CreateChatModel
    {
        public string ChatName { get; set; }
        public ChatType Type { get; set; } 
    }
}
