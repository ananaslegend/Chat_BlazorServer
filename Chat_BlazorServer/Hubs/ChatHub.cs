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
        private readonly MessageService messageService;

        public ChatHub(MessageService messageService)
        {
            this.messageService = messageService;
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
        public async Task RemoveMessage(MessageItem message)
        {
            try
            {
                await messageService.Remove(message.Id);
                await Clients.Group(message.ChatId.ToString())
                                .SendAsync("ReceiveDeleteMessage", message.Id);
            }
            catch
            {
                // Nothin happens
            }
        }
        public async Task UpdateMessage(MessageItem message)
        {
            await messageService.Update(message);

            await Clients.Group(message.ChatId.ToString())
                .SendAsync("ReceiveUpdateMessage", message);
        }
    }
}
