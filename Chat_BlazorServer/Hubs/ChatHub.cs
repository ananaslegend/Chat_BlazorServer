using Chat_BlazorServer.DataAccess;
using Chat_BlazorServer.DataAccess.Abstractions;
using Chat_BlazorServer.Domain.Models;
using Chat_BlazorServer.Services;
using Chat_BlazorServer.Shared.Components;
using Microsoft.AspNetCore.SignalR;

namespace Chat_BlazorServer.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IUnitOfWork dbUnit;
        private readonly MessageService messageService;

        public ChatHub(IUnitOfWork dbUnit, MessageService messageService)
        {
            this.dbUnit = dbUnit;
            this.messageService = messageService;
        }
        
        public async Task SendTestMessage()
        {
            await Clients.Caller.SendAsync("ReceiveTestMessage", "hi from Server");
        }
        public async Task JoinRoom(int chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, Convert.ToString(chatId));
        }
        public async Task SendMessage(CreateMessage createMessage)
        {
            var newMessageItem = messageService.AddNewMessageAsync(createMessage).Result;
            await Clients.Group(createMessage.ChatId.ToString())
                         .SendAsync("AddMessage", newMessageItem);
        }
        public async Task GetMassagePack(int chatId, int loaded, int batch)
        {
            var pack = messageService.GetMessagePack(chatId, loaded, batch).Result;
            await Clients.Caller.SendAsync("AddMessagePack", pack);
        }
    }

    //public class ChatHub : Hub
    //{
    //    private readonly UnitOfWork dbUnit;

    //    public ChatHub(UnitOfWork dbUnit)
    //    {
    //        this.dbUnit = dbUnit;

    //    }
    //    public async Task JoinRoom(int chatId)
    //    {
    //        await Groups.AddToGroupAsync(Context.ConnectionId, Convert.ToString(chatId));
    //    }
    //    public async Task LeaveRoom(string chatGuid)
    //    {
    //        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatGuid);
    //    }
    //    public async Task GetMassagePack(int chatId, int loaded, int batch)
    //    {
    //        var pack = dbUnit.Messages.GetMessagePack(chatId, loaded, batch);
    //        await Clients.Caller.SendAsync("AddMessagePack", pack);
    //    }
    //    public async Task AddMessage(CreateMessage createMessage)
    //    {
    //        Message msg = new()
    //        {
    //            Author = dbUnit.Users.FindUser(createMessage.SenderName),
    //            Chat = dbUnit.Chats.Get(createMessage.ChatId),
    //            Data = createMessage.MessageText,
    //            Date = DateTime.Now,
    //            Reply = null
    //        };
    //        dbUnit.Messages.Add(msg);
    //        await Clients.Group(Convert.ToString(createMessage.ChatId)).SendAsync("AddMessage", msg);
    //    }
    //}
}
