using System.ComponentModel.DataAnnotations;

namespace Chat_BlazorServer.Shared.Components
{
    public class CreateMessage
    {
        public int ChatId { get; set; }
        [Required]
        public string MessageText { get; set; }
        public string? SenderName { get; set; }
    }
}
