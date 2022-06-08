using Chat_BlazorServer.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat_BlazorServer.Domain.DTOs
{
    public class ChatDisplay
    {
        public string Name { get; set; }
        public ChatType Type { get; set; }
    }
}
