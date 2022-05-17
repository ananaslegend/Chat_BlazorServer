using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat_BlazorServer.DataAccess.Abstractions
{
    internal interface IUnitOfWork 
    {
        Task<int> CompleteAsync();
    }
}
