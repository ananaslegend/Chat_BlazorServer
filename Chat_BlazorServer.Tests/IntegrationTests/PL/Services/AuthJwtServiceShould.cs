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

namespace Chat_BlazorServer.Tests.IntegrationTests.PL.Services
{
    public class AuthJwtServiceShould
    {
        private UnitOfWork inMemoryUnitOfWork;
        private Fixture fixture;
        private AuthJwtService sut;
        private ApplicationUser testUser;
        private InMemoryUnitOfWorkFactory inMemoryUnitOfWorkFactory = new();
        private readonly Mock<IConfiguration> config = new();
        private UserManager<ApplicationUser> userManager;
        private UserLoginDTO userLoginDTO = new()
        {
            UserName = "test",
            Password = "testtest123"
        };
        public async void Setup()
        {
            testUser = fixture.Create<ApplicationUser>();

            inMemoryUnitOfWork = await inMemoryUnitOfWorkFactory
                .BuildAsync();

            sut = new AuthJwtService(config.Object, userManager);
        }
        [Fact]
        public async Task Registration_WithSuccessData()
        {
            // Arrange
            Setup();
            // Act 
            var result = sut.Registration(userLoginDTO).Result;
            // Assert
            result.Should().BeTrue();
        }
        [Fact]
        public async Task Auth_WithSuccessData()
        {
            // Arrange
            Setup();

            // Act
            //var result = sut.Auth(new UserLoginDTO()
            //{
            //    UserName = testUser.UserName,
            //    Password = testUser.
            //})
        }
    }
}
