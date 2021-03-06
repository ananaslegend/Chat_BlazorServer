using Chat_BlazorServer.Data.Context;
using Chat_BlazorServer.DataAccess.Abstractions;
using Chat_BlazorServer.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat_BlazorServer.DataAccess.Repositories
{
    public class UserRepository : GenericRepository<ApplicationUser>, IUserRepository
    {
        public UserRepository(ApplicationContext context) : base(context)
        {

        }

        public async Task<ApplicationUser> FindUser(string userName)
        {
            var user = await ApplicationContext
                .Users
                .Where(u => u.UserName == userName)
                .FirstOrDefaultAsync();

            return user;
        }

        //downcast from generic 
        public ApplicationContext ApplicationContext
        {
            get { return _context as ApplicationContext; }
        }
    }
}
