using Chat_BlazorServer.Domain.Enums;
using Chat_BlazorServer.Shared.Components.Abstractions;

namespace Chat_BlazorServer.Shared.Components
{
    public class CreatePublicChatModel : CreateChatModel
    {
        public string ChatName { get; set; }
        public string UserName { get; set; }
        public ChatType Type { get; set; } = ChatType.PublicChat;
    }
}
