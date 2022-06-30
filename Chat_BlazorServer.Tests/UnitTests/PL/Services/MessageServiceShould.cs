using AutoFixture;
using Chat_BlazorServer.DataAccess.Abstractions;
using Chat_BlazorServer.Domain.Models;
using Chat_BlazorServer.Services;
using Chat_BlazorServer.Shared.Components;
using FluentAssertions;
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
        private readonly Mock<IUnitOfWork> mockUnitOfWork = new();
        private Fixture fixture = new Fixture();
        private MessageService sut;
        public MessageServiceShould()
        {
            sut = new(mockUnitOfWork.Object);
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }
        [Fact]
        public async void AddNewMessage_WithSuccessData()
        {
            // Arrange
            CreateMessage messageToCreate = fixture.Create<CreateMessage>();

            mockUnitOfWork.Setup(o => o.Users.FindUser(It.IsAny<string>()).Result)
                .Returns(() =>
                {
                    var user = fixture.Create<ApplicationUser>();
                    user.UserName = messageToCreate.SenderName;

                    return user;
                });
            mockUnitOfWork.Setup(o => o.Chats.Get(It.IsAny<int>()).Result)
                .Returns(() =>
                {
                    var chat = fixture.Create<Chat>();
                    chat.Id = messageToCreate.ChatId;
                    return chat;
                });
            mockUnitOfWork.Setup(o => o.Messages.GetMessageById(It.IsAny<int>()).Result)
                .Returns(() =>
                {
                    var replyMessage = fixture.Create<Message>();
                    replyMessage.Id = messageToCreate.ReplyId.Value;
                    return replyMessage;
                });

            // Act 
            var newMessage = await sut.AddNewMessageAsync(messageToCreate);

            // Assert
            newMessage.Should().BeOfType<MessageItem>();
            newMessage.AuthorName.Should().Be(messageToCreate.SenderName);
            newMessage.ChatId.Should().Be(messageToCreate.ChatId);
            newMessage.ReplyId.Should().Be(messageToCreate.ReplyId.Value);

            mockUnitOfWork.Verify(unit => unit.Messages.Add(It.IsAny<Message>()));
            mockUnitOfWork.Verify(unit => unit.CompleteAsync());
        }
        [Fact]
        public async void AddNewMessage_WithSuccessDataWithoutReply()
        {
            // Arrange
            CreateMessage messageToCreate = fixture.Create<CreateMessage>();
            messageToCreate.ReplyId = null;

            mockUnitOfWork.Setup(o => o.Messages.Add(It.IsAny<Message>()));
            mockUnitOfWork.Setup(o => o.Users.FindUser(It.IsAny<string>()).Result)
                .Returns(() =>
                {
                    var user = fixture.Create<ApplicationUser>();
                    user.UserName = messageToCreate.SenderName;

                    return user;
                });
            mockUnitOfWork.Setup(o => o.Chats.Get(It.IsAny<int>()).Result)
                .Returns(() =>
                {
                    var chat = fixture.Create<Chat>();
                    chat.Id = messageToCreate.ChatId;
                    return chat;
                });

            // Act 
            var newMessage = await sut.AddNewMessageAsync(messageToCreate);

            // Assert
            newMessage.Should().BeOfType<MessageItem>();

            mockUnitOfWork.Verify(unit => unit.Messages.Add(It.IsAny<Message>()));
            mockUnitOfWork.Verify(unit => unit.CompleteAsync());
        }
        [Fact]
        public async void AddNewMessage_WithNonExistentAuthor()
        {
            // Arrange
            CreateMessage messageToCreate = fixture.Create<CreateMessage>();

            mockUnitOfWork.Setup(o => o.Chats.Get(It.IsAny<int>()).Result).Returns(fixture.Create<Chat>());
            mockUnitOfWork.Setup(o => o.Messages.GetMessageById(It.IsAny<int>()).Result).Returns(fixture.Create<Message>());

            // Act and Assert
            await Assert.ThrowsAsync<NullReferenceException>(async () => await sut.AddNewMessageAsync(messageToCreate));
        }
        [Fact]
        public async void AddNewMessage_WithNonExistentChatId()
        {
            // Arrange
            CreateMessage messageToCreate = fixture.Create<CreateMessage>();

            mockUnitOfWork.Setup(o => o.Users.FindUser(It.IsAny<string>()).Result).Returns(fixture.Create<ApplicationUser>());
            mockUnitOfWork.Setup(o => o.Messages.GetMessageById(It.IsAny<int>()).Result).Returns(fixture.Create<Message>());

            // Act and Assert
            await Assert.ThrowsAsync<NullReferenceException>(async () => await sut.AddNewMessageAsync(messageToCreate));
        }
        [Fact]
        public async void AddNewMessage_WithNonExistentReplyId()
        {
            // Arrange
            CreateMessage messageToCreate = fixture.Create<CreateMessage>();

            mockUnitOfWork.Setup(o => o.Chats.Get(It.IsAny<int>()).Result).Returns(fixture.Create<Chat>());
            mockUnitOfWork.Setup(o => o.Users.FindUser(It.IsAny<string>()).Result).Returns(fixture.Create<ApplicationUser>());

            // Act and Assert
            await Assert.ThrowsAsync<NullReferenceException>(async () => await sut.AddNewMessageAsync(messageToCreate));
        }
        [Fact]
        public async void Remove_WithSuccessData()
        {
            // Arrange 
            CreateMessage messageToCreate = fixture.Create<CreateMessage>();
            mockUnitOfWork.Setup(o => o.Messages.GetMessageById(It.IsAny<int>()).Result)
                          .Returns(fixture.Create<Message>());

            mockUnitOfWork.Setup(o => o.Messages.Remove(It.IsAny<Message>()));
            var msgIdToRem = fixture.Create<int>();

            // Act 
            await sut.Remove(msgIdToRem);

            // Assert
            mockUnitOfWork.Verify(unit => unit.Messages.Remove(It.IsAny<Message>()));
            mockUnitOfWork.Verify(unit => unit.CompleteAsync());
        }
        [Fact]
        public async void Remove_WithNonExistentMessageId()
        {

            // Arrange 
            CreateMessage messageToCreate = fixture.Create<CreateMessage>();
            mockUnitOfWork.Setup(o => o.Messages.GetMessageById(It.IsAny<int>()).Result)
                          .Throws(new InvalidOperationException());

            mockUnitOfWork.Setup(o => o.Messages.Remove(It.IsAny<Message>()));

            // Act and Assert
            await Assert.ThrowsAsync<AggregateException>(async () => await sut.Remove(fixture.Create<int>()));
        }
        [Fact]
        public async void Update_WithSuccessData()
        {
            // Arrange 
            CreateMessage messageToCreate = fixture.Create<CreateMessage>();
            mockUnitOfWork.Setup(o => o.Messages.GetMessageById(It.IsAny<int>()).Result)
                          .Returns(fixture.Create<Message>());

            mockUnitOfWork.Setup(o => o.Messages.UpdateMessageData(It.IsAny<Message>(), It.IsAny<string>()));

            // Act 
            await sut.Update(fixture.Create<MessageItem>());

            // Assert
            mockUnitOfWork.Verify(unit => unit.Messages.UpdateMessageData(It.IsAny<Message>(), It.IsAny<string>()));
            mockUnitOfWork.Verify(unit => unit.CompleteAsync());
        }
        [Fact]
        public async void Update_WithNonExistentMessageId()
        {

            // Arrange 
            CreateMessage messageToCreate = fixture.Create<CreateMessage>();
            mockUnitOfWork.Setup(o => o.Messages.GetMessageById(It.IsAny<int>()).Result)
                          .Throws(new InvalidOperationException());

            mockUnitOfWork.Setup(o => o.Messages.UpdateMessageData(It.IsAny<Message>(), It.IsAny<string>()));

            // Act and Assert
            await Assert.ThrowsAsync<AggregateException>(async () => await sut.Update(fixture.Create<MessageItem>()));
        }
        [Theory]
        [InlineData(0, 0, 20)]
        public async void GetMessagePack_WithSuccessData(int chatId, int loaded, int batch)
        {
            // Arrange 
            mockUnitOfWork.Setup(o => o.Messages.GetMessagePack(chatId, loaded, batch).Result)
                .Returns(() =>
                {
                    var pack = fixture.CreateMany<Message>(batch).ToList();
                    return pack;
                });

            // Act 
            var result = await sut.GetMessagePack(chatId, loaded, batch);

            // Assert
            result.Should().NotBeNull();
            result.Count.Should().BeLessThanOrEqualTo(batch);
            result.Should().BeAssignableTo<ICollection<MessageItem>>();

            mockUnitOfWork.Verify(unit => unit.Messages.GetMessagePack(chatId, loaded, batch));
        }
    }
}
