using Chat_BlazorServer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat_BlazorServer.BLL.Abstractions
{
    public interface IJwtToken
    {
        public string GenerateJwtToken(ApplicationUser user);
    }
}
