using AutoFixture;
using Chat_BlazorServer.BLL.Services;
using Chat_BlazorServer.DataAccess;
using Chat_BlazorServer.Domain.DTOs;
using Chat_BlazorServer.Domain.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat_BlazorServer.Tests.UnitTests.PL.Services
{
    public class AuthJwtServiceShould
    {
        private Mock<UnitOfWork> mockUnitOfWork;
        private Fixture fixture;
        private AuthJwtService sut;
        private Mock<IConfiguration> mockConfig = new();
        private Mock<UserManager<ApplicationUser>> mockUserManager;
        private UserLoginDTO userLoginDTO = new()
        {
            UserName = "test",
            Password = "testtest123"
        };
        public AuthJwtServiceShould()
        {
            // UserManager mocking 
            var store = new Mock<IUserStore<ApplicationUser>>();
            mockUserManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            mockUserManager.Object.UserValidators.Add(new UserValidator<ApplicationUser>());
            mockUserManager.Object.PasswordValidators.Add(new PasswordValidator<ApplicationUser>());

            sut = new AuthJwtService(mockConfig.Object, mockUserManager.Object);

        }
        // Не певен, що тут є що тестувати, оскільки клас невеликий, 
        // в більшості випадків використовуються перевірені бібліотеки,  
        // крім того мокання для окремих методів є заскладним або неможливим 
    }
}
