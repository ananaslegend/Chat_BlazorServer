using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat_BlazorServer.Data.Context;
using Chat_BlazorServer.DataAccess.Abstractions;
using Chat_BlazorServer.DataAccess.Repositories;

namespace Chat_BlazorServer.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _context;
        public IUserRepository Users { get; private set; }
        public IChatRepository Chats { get; private set; }
        public IMessageRepository Messages { get; private set; }
        public UnitOfWork(ApplicationContext context)
        {
            _context = context;
            Users = new UserRepository(_context);
            Chats = new ChatRepository(_context);
            Messages = new MessageRepository(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
