using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Chat_BlazorServer.Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Chat> UserChats { get; set; }
    }
}
