using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat_BlazorServer.Domain.Enums;

namespace Chat_BlazorServer.Domain.Abstractions
{
    interface IChat
    {
        public int Id { get; set; }
        public ChatType Type { get; set; }
    }
}
