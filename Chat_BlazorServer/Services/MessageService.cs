using Chat_BlazorServer.DataAccess.Abstractions;
using Chat_BlazorServer.Domain.Models;
using Chat_BlazorServer.Shared.Components;

namespace Chat_BlazorServer.Services
{
    public class MessageService
    {
        private readonly IUnitOfWork dbUnit;
        public MessageService(IUnitOfWork dbUnit)
        {
            this.dbUnit = dbUnit;
        }
        private MessageItem MessageToMessageItem(Message msg)
        {
            MessageItem messageItem = new()
            {
                Id = msg.Id,
                AuthorName = msg.Author.UserName,
                ChatName = msg.Chat.Name,
                Data = msg.Data,
                Date = msg.Date
            };

            if(msg.Reply is not null)
            {
                messageItem.ReplyId = msg.Reply.Id;
                messageItem.ReplyData = msg.Reply.Data;
                messageItem.AuthorName = msg.Author.UserName;
            }

            return  messageItem;
        }

        public async Task<MessageItem> AddNewMessageAsync(CreateMessage createMessage)
        {
            Message msg = new()
            {
                Author = dbUnit.Users.FindUser(createMessage.SenderName).Result,
                Chat = dbUnit.Chats.Get(createMessage.ChatId).Result,
                Data = createMessage.MessageText,
                Date = DateTime.Now,
            };
            if(createMessage.ReplyId != null && createMessage.ReplyId != 0)
            {
                msg.Reply = dbUnit.Messages.Get(createMessage.ReplyId.Value).Result;
            }
            dbUnit.Messages.Add(msg);
            await dbUnit.CompleteAsync();

            MessageItem messageItem = new()
            {
                Id = msg.Id,
                AuthorName = msg.Author.UserName,
                ChatName = msg.Chat.Name,
                Data = msg.Data,
                Date = msg.Date
            };
            if(msg.Reply != null)
            {
                messageItem.ReplyId = msg.Reply.Id;
                messageItem.ReplyData = msg.Reply.Data;
                messageItem.ReplyAuthorName = msg.Reply.Author.UserName;
            }

            return messageItem;
        }

        public async Task<IEnumerable<MessageItem>> GetMessagePack(int chatId, int loaded, int batch)
        {
            var messagePack = await dbUnit.Messages.GetMessagePack(chatId, loaded, batch);
            List<MessageItem> messageItemList = new();

            foreach (var item in messagePack)
            {
                messageItemList.Add(MessageToMessageItem(item));
            }

            return messageItemList;
        }
    }
}
