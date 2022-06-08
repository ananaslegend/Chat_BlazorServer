using Chat_BlazorServer.Data.Context;
using Chat_BlazorServer.DataAccess.Abstractions;
using Chat_BlazorServer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat_BlazorServer.DataAccess.Repositories
{
    public class ChatRepository : GenericRepository<Chat>, IChatRepository
    {
        public ChatRepository(ApplicationContext context) : base(context)
        {
        }

        //downcast from generic 
        public ApplicationContext ApplicationContext
        {
            get { return _context as ApplicationContext; }
        }

        public ICollection<Chat> GetChatsByName(string chatName)
        {
            return (ICollection<Chat>)ApplicationContext.Chats.Where(c => c.Name == chatName);
        }

        //public void AddUserToChat(string chatId, string userId)
        //{
        //    ApplicationContext.Chats.FirstOrDefault().ChatUsers.Add();
        //}
    }
}
