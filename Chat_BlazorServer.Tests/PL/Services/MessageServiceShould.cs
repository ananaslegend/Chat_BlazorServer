using Chat_BlazorServer.DataAccess.Abstractions;
using Chat_BlazorServer.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat_BlazorServer.Tests.PL.Services
{
    public class MessageServiceShould
    {
        private readonly Mock<IUnitOfWork> _mock = new();
        public MessageServiceShould()
        {
            MessageService sut = new(_mock.Object);
        }

        [Fact]
        public void AddNewMessage()
        {
            
        }
    }
}
