using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat_BlazorServer.Data.Context;
using Chat_BlazorServer.DataAccess.Abstractions;

namespace Chat_BlazorServer.DataAccess
{   //! Мені дуже сподобалась ваша реалізаця UnitOfWork, але не можу знайти щось схоже, а точно не пам'ятаю як було...
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _context;
        public UnitOfWork(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        //public void GetRepository()
        //{
        //}
    }
}
