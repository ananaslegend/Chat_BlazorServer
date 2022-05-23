using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat_BlazorServer.DataAccess.Abstractions
{
    public interface IUnitOfWork 
    {
        public IUserRepository Users { get; }
        public IChatRepository Chats { get; }
        public IMessageRepository Messages { get; }
        Task<int> CompleteAsync();
    }
}
