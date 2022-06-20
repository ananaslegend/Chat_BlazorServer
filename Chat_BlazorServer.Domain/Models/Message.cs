using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace Chat_BlazorServer.Domain.Models
{
    public class Message
    {
        public int Id { get; set; }
        public ApplicationUser Author { get; set; }
        public Chat Chat { get; set; }
        public DateTime Date { get; set; }
        public Message? Reply { get; set; }
        public string Data { get; set; }
    }
}
