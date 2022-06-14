using Chat_BlazorServer.Domain.Enums;

namespace Chat_BlazorServer.Shared.Components
{
    public class ChatDisplayModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ChatType Type { get; set; }
    }
}
