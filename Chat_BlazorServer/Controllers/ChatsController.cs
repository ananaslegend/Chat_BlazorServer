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
        [HttpGet("all_user_chats/{Id}")]
        public async Task<IActionResult> GetAllUserChats([FromRoute] string Id)
        {
            ICollection<Chat> list = userManager.Users.FirstOrDefault(user => user.Id == Id).UserChats;

            ICollection<ChatDisplay> userChats = new List<ChatDisplay>();
            foreach (Chat item in list)
            {
                userChats.Add(
                    new ChatDisplay()
                    {
                        Name = item.Name,
                        Type = item.Type
                    });
            }

            return Ok(userChats);
        }

        [HttpGet("find_chats/{chatname}")]
        public async Task<IActionResult> FindChats([FromRoute] string chatname)
        {
            var list = dbUnit.Chats.GetChatsByName(chatname);

            ICollection<ChatNameIdDto> chats = new List<ChatNameIdDto>();

            foreach (Chat item in list)
            {
                chats.Add(
                    new ChatNameIdDto()
                    {
                        Name = item.Name,
                        Id = item.Id
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

        [HttpGet("join_to_chat/{userId}/{chatId}")]
        public async Task<IActionResult> JoinToChat([FromRoute] string chatId, [FromRoute] string userId)
        {
            

            return Ok();
        }
    }
}
