using Chat_BlazorServer.BLL.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat_BlazorServer.BLL.Services.Abstractions
{
    public interface IAuthJwtService : IAuth, IJwtToken
    {

    }
}
