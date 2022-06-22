using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat_BlazorServer.Domain.Models;

namespace Chat_BlazorServer.DataAccess.Abstractions
{
    public interface IChatRepository : IGenericRepository<Chat>
    {
        public IEnumerable<Chat> GetChatsByName(string chatName);
        public void AddUserToChat(int chatId, ApplicationUser user);
        public IEnumerable<Chat> GetAllUserChats(ApplicationUser user);
    }
}
