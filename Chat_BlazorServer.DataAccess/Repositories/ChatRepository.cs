using Chat_BlazorServer.Data.Context;
using Chat_BlazorServer.DataAccess.Abstractions;
using Chat_BlazorServer.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

        public IEnumerable<Chat> GetChatsByName(string chatName)
        {
            return ApplicationContext.Chats.Where(c => c.Name == chatName).AsEnumerable<Chat>();
        }

        public IEnumerable<Chat> GetAllUserChats(ApplicationUser user)
        {
            return ApplicationContext.Chats.Where(u => u.ChatUsers.Contains(user));
        }

        public void AddUserToChat(int chatId, ApplicationUser user)
        {
            var chat = ApplicationContext.Chats
                .Include(u => u.ChatUsers)
                .FirstOrDefault(c => c.Id == chatId);
            if (chat is null)
            {
                throw new Exception("User not found");
            }

            if(!chat.ChatUsers.Contains(user))
            {
                chat.ChatUsers.Add(user);
            }
        }
    }
}
