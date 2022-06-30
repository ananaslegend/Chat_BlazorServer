using AutoFixture;
using Chat_BlazorServer.Data.Context;
using Chat_BlazorServer.DataAccess;
using Chat_BlazorServer.Domain.Models;
using Chat_BlazorServer.Services;
using Chat_BlazorServer.Shared.Components;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat_BlazorServer.Tests.IntegrationTests.PL.Services
{
    public class MessageServiceShould
    {
        private UnitOfWork inMemoryUnitOfWork;
        private Fixture fixture;
        private MessageService sut;
        private ApplicationUser testUser;
        private Chat testChat;
        private Message testMessage;
        private InMemoryUnitOfWorkFactory inMemoryUnitOfWorkFactory = new();

        public MessageServiceShould()
        {            
            fixture = new Fixture();
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior()); 
        }
        private async void Setup()
        {
            testUser = fixture.Create<ApplicationUser>();
            testChat = fixture.Create<Chat>();
            testChat.ChatUsers.Add(testUser);
            testMessage = fixture.Create<Message>();
            testMessage.Chat = testChat;
            testMessage.Author = testUser;
            testMessage.Reply = null;

            inMemoryUnitOfWork = await inMemoryUnitOfWorkFactory
                .WithUser(testUser)
                .WithChat(testChat)
                .WithMessage(testMessage)
                .BuildAsync();

            sut = new MessageService(inMemoryUnitOfWork);
        }
        private MessageItem MessageToMessageItem(Message msg)
        {
            MessageItem messageItem = new()
            {
                Id = msg.Id,
                AuthorName = msg.Author.UserName,
                ChatName = msg.Chat.Name,
                ChatId = msg.Chat.Id,
                Data = msg.Data,
                Date = msg.Date
            };

            if (msg.Reply != null)
            {
                messageItem.ReplyId = msg.Reply.Id;
                messageItem.ReplyData = msg.Reply.Data;
                messageItem.ReplyAuthorName = msg.Reply.Author.UserName;
            }

            return messageItem;
        }
        [Fact]
        public async void AddNewMessage_WithSuccessDataWithReply()
        {
            // Arrange
            Setup();

            CreateMessage messageToCreate = fixture.Create<CreateMessage>();
            messageToCreate.SenderName = testUser.UserName;
            messageToCreate.ChatId = testChat.Id;
            messageToCreate.ReplyId = testMessage.Id;

            // Act 
            var newMessage = await sut.AddNewMessageAsync(messageToCreate);

            // Assert
            newMessage.Should().BeOfType<MessageItem>();
            newMessage.AuthorName.Should().Be(messageToCreate.SenderName);
            newMessage.ChatId.Should().Be(messageToCreate.ChatId);
            newMessage.ReplyId.Should().Be(messageToCreate.ReplyId.Value);
        }
        [Fact]
        public async void AddNewMessage_WithSuccessDataWithoutReply()
        {
            // Arrange
            Setup();

            CreateMessage messageToCreate = fixture.Create<CreateMessage>();
            messageToCreate.SenderName = testUser.UserName;
            messageToCreate.ChatId = testChat.Id;
            messageToCreate.ReplyId = 0;

            // Act 
            var newMessage = await sut.AddNewMessageAsync(messageToCreate);

            // Assert
            newMessage.Should().BeOfType<MessageItem>();
            newMessage.AuthorName.Should().Be(messageToCreate.SenderName);
            newMessage.ChatId.Should().Be(messageToCreate.ChatId);
        }
        [Fact]
        public async void AddNewMessage_WithNonExistentAuthor()
        {
            // Arrange
            Setup();

            CreateMessage messageToCreate = fixture.Create<CreateMessage>();
            messageToCreate.SenderName = "NonExistentUser";
            messageToCreate.ChatId = testChat.Id;
            messageToCreate.ReplyId = 0;

            // Act 
            Func<Task> result = async () => await sut.AddNewMessageAsync(messageToCreate);

            // Assert
            await result.Should().ThrowAsync<Exception>();
        }
        [Fact]
        public async void AddNewMessage_WithNonExistentChat()
        {
            // Arrange
            Setup();

            CreateMessage messageToCreate = fixture.Create<CreateMessage>();
            messageToCreate.SenderName = testUser.UserName;
            messageToCreate.ChatId = -1;
            messageToCreate.ReplyId = 0;

            // Act 
            Func<Task> result = async () => await sut.AddNewMessageAsync(messageToCreate);

            // Assert
            await result.Should().ThrowAsync<Exception>();
        }
        [Fact]
        public async void RemoveMessage_WithSuccessData()
        {
            // Arrange
            Setup();
            
            // Act 
            Func<Task> result = async () => await sut.Remove(testMessage.Id);

            // Assert (if Exeption doesnt throw test should pass)
            await result.Should().NotThrowAsync();
        }
        [Fact]
        public async void RemoveMessage_WithNonExistentMessageId()
        {
            // Arrange
            Setup();

            // Act 
            Func<Task> result = async () => await sut.Remove(-1);

            // Assert (if Exeption doesnt throw test should pass)
            await result.Should().ThrowAsync<Exception>();
        }
        [Fact]
        public async void UpdateMessage_WithSuccessData()
        {
            // Arrange
            Setup();
            testMessage.Data = "test_update";

            // Act 
            Func<Task> result = async () => await sut.Update(MessageToMessageItem(testMessage));

            // Assert 
            await result.Should().NotThrowAsync();
            Assert.Equal(testMessage.Data, inMemoryUnitOfWork.Messages.Get(testMessage.Id).Result.Data);
        }
        [Fact]
        public async void UpdateMessage_WithNonExistentMessageId()
        {
            // Arrange
            Setup();
            testMessage.Data = "test_update";
            testMessage.Id = -1;

            // Act 
            Func<Task> result = async () => await sut.Update(MessageToMessageItem(testMessage));

            // Assert 
            await result.Should().ThrowAsync<Exception>();
        }
        [Theory]
        [InlineData(1, 0, 20)]
        [InlineData(1, 20, 30)]
        [InlineData(1, 40, 20)]
        public async void GetMessagePack_WithSuccessData(int chatId, int loaded, int batch)
        {
            Setup();
            // Arrange
            for(var i = 1; i <= 50; i++)
            {
                var msg = fixture.Create<Message>();
                msg.Chat = testChat;
                msg.Author = testUser;
                msg.Data = i.ToString();
                inMemoryUnitOfWork.Messages.Add(msg);
                await inMemoryUnitOfWork.CompleteAsync();
            }

            // Act 
            var result = await sut.GetMessagePack(chatId, loaded, batch);

            // Assert 
            result.Should().NotBeNull();
            result.Count.Should().BeLessThanOrEqualTo(batch);
            result.Should().BeAssignableTo<ICollection<MessageItem>>();
        }
        [Theory]
        [InlineData(-1, 0, 20)]
        public async void GetMessagePack_WithNonExistentMessageId(int chatId, int loaded, int batch)
        {
            Setup();
            // Arrange
            for (var i = 1; i <= 50; i++)
            {
                var msg = fixture.Create<Message>();
                msg.Chat = testChat;
                msg.Author = testUser;
                msg.Data = i.ToString();
                inMemoryUnitOfWork.Messages.Add(msg);
                await inMemoryUnitOfWork.CompleteAsync();
            }

            // Act 
            var result = await sut.GetMessagePack(chatId, loaded, batch);

            // Assert 
            result.Should().BeNullOrEmpty();
        }
    }
}