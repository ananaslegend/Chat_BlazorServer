using Chat_BlazorServer.Data.Context;
using Chat_BlazorServer.DataAccess;
using Chat_BlazorServer.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat_BlazorServer.Tests.IntegrationTests.PL.Services
{
    public class InMemoryUnitOfWorkFactory
    {
        private UnitOfWork inMemoryUnitOfWork;
        public InMemoryUnitOfWorkFactory()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "MessageServiceIntegrationTestDb")
                .Options;
            var context = new ApplicationContext(options);
            inMemoryUnitOfWork = new UnitOfWork(context);
        }

        public async Task<UnitOfWork> BuildAsync()
        {
            await inMemoryUnitOfWork.CompleteAsync();

            return inMemoryUnitOfWork;
        }

        public InMemoryUnitOfWorkFactory WithMessage(Message message)
        {
            inMemoryUnitOfWork.Messages.Add(message);

            return this;
        }

        public InMemoryUnitOfWorkFactory WithUser(ApplicationUser user)
        {
            inMemoryUnitOfWork.Users.Add(user);

            return this;
        }

        public InMemoryUnitOfWorkFactory WithChat(Chat chat)
        {
            inMemoryUnitOfWork.Chats.Add(chat);

            return this;
        }

    }
}
