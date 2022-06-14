using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat_BlazorServer.Domain.Models;

namespace Chat_BlazorServer.DataAccess.Abstractions
{
    public interface IMessageRepository : IGenericRepository<Message>
    {
        public Task<ICollection<Message>> GetMessagePack(int chatId, int loaded, int batch);
    }
}
