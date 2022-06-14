using Chat_BlazorServer.DataAccess.Abstractions;
using Chat_BlazorServer.Domain.DTOs;
using Chat_BlazorServer.Domain.Models;
using Chat_BlazorServer.Shared.Components;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Chat_BlazorServer.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUnitOfWork dbUnit;

        public ChatsController(UserManager<ApplicationUser> userManager, IUnitOfWork dbUnit)
        {
            this.userManager = userManager;
            this.dbUnit = dbUnit;
        }
        [HttpPost("all_user_chats/")]
        public async Task<IActionResult> GetAllUserChats([FromBody] NameModel userName)
        {
            //ICollection<Chat> list = userManager.Users.FirstOrDefault(user => user.UserName == userName.Title).UserChats;
            var user = userManager.FindByNameAsync(userName.Title).Result;

            IEnumerable<Chat> list = dbUnit.Chats.GetAllUserChats(user);

            ICollection<ChatDisplayModel> userChats = new List<ChatDisplayModel>();
            foreach (Chat item in list)
            {
                userChats.Add(
                    new ChatDisplayModel()
                    {
                        Name = item.Name,
                        Type = item.Type
                    });
            }

            return Ok(userChats);
        }

        [HttpPost("find_chats/")]
        public async Task<IActionResult> FindChats([FromBody] NameModel chatname)
        {
            var list = dbUnit.Chats.GetChatsByName(chatname.Title);

            ICollection<ChatDisplayModel> chats = new List<ChatDisplayModel>();

            foreach (Chat item in list)
            {
                chats.Add(
                    new ChatDisplayModel()
                    {
                        Name = item.Name,
                        Id = item.Id,
                        Type = item.Type
                    });
            }

            return Ok(chats);
        }

        [HttpPost("create_private_chat")]
        public async Task<IActionResult> CreatePrivateChat([FromBody] CreatePrivateChatModel chatModel)
        {
            Chat newChat = new()
            {
                Name = chatModel.ChatName,
                Type = chatModel.Type,
            };
            var user = userManager.Users.FirstOrDefault(x => x.UserName == chatModel.UserName);
            var companion = userManager.Users.FirstOrDefault(x => x.UserName == chatModel.CompanionName);

            newChat.ChatUsers.Add(user);
            newChat.ChatUsers.Add(companion);

            dbUnit.Chats.Add(newChat);

            await dbUnit.CompleteAsync();

            return Ok();
        }

        [HttpPost("create_public_chat")]
        public async Task<IActionResult> CreatePublicChat([FromBody] CreatePublicChatModel chatModel)
        {
            Chat newChat = new()
            {
                Name = chatModel.ChatName,
                Type = chatModel.Type,
            };
            var user = userManager.Users.FirstOrDefault(x => x.UserName == chatModel.UserName);

            newChat.ChatUsers.Add(user);

            dbUnit.Chats.Add(newChat);

            await dbUnit.CompleteAsync();

            return Ok();
        }

        [HttpPost("join_to_chat/{chatId}/{userName}")]
        public async Task<IActionResult> JoinToChat([FromRoute] string chatId, [FromRoute] string userName)
        {
            var user = userManager.FindByNameAsync(userName).Result;

            dbUnit.Chats.AddUserToChat(chatId, user);
            await dbUnit.CompleteAsync();

            return Ok();
        }
    }
}
