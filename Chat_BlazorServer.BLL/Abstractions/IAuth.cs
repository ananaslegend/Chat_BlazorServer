using Chat_BlazorServer.Domain.DTOs;
using Chat_BlazorServer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat_BlazorServer.BLL.Abstractions
{
    public interface IAuth
    {
        public Task<ApplicationUser> Auth(UserLoginDTO userData);
        public Task<bool> Register(UserLoginDTO userData);
    }
}
