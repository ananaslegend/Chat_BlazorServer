using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat_BlazorServer.Domain.Abstractions;
using Chat_BlazorServer.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Chat_BlazorServer.Domain.Models
{
    public class Chat : IChat
    {
        public int Id { get; set; }
        public ChatType Type { get; set; }
        public string? Name { get; set; }
        public ICollection<ApplicationUser> ChatUsers { get; set; }
        public ICollection<Message>? Messages { get; set; }
    }
}
