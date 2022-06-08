using Chat_BlazorServer.Domain.Enums;

namespace Chat_BlazorServer.Shared.Components
{
    public class CreatePrivateChatModel
    {
        public string ChatName { get; set; }
        public string UserName { get; set; }
        public string CompanionName { get; set; }
        public ChatType Type { get; set; } = ChatType.PrivateChat;
    }
}
