using Chat_BlazorServer.Domain.Enums;
using Chat_BlazorServer.Shared.Components.Abstractions;

namespace Chat_BlazorServer.Shared.Components
{
    public class CreatePrivateChatModel : CreateChatModel
    {
        public string ChatName { get; set; }
        public string UserName { get; set; }
        public string CompanionName { get; set; }
        public ChatType Type { get; set; } = ChatType.PrivateChat;
    }
}
