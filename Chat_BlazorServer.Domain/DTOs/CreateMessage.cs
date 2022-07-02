using System.ComponentModel.DataAnnotations;

namespace Chat_BlazorServer.Domain.DTOs
{
    public class CreateMessage
    {
        public int ChatId { get; set; }
        public string MessageText { get; set; }
        public string? SenderName { get; set; }
        public int? ReplyId { get; set; }
    }
}
