using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Chat_BlazorServer.Domain.DTOs;
using Chat_BlazorServer.Domain.Models;
using Chat_BlazorServer.DataAccess.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Chat_BlazorServer.BLL.Services;
using Chat_BlazorServer.BLL.Services.Abstractions;

namespace Chat_BlazorServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAuthJwtService _authService;
    
        public LoginController(IAuthJwtService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userData)
        {
            try
            {
                var user = await _authService.Auth(userData);
                var token = _authService.GenerateJwtToken(user);
                return Ok(token);
            }
            catch
            {
                return NotFound("User not found");
            }
        }

        [HttpPost("registration")]
        public async Task<IActionResult> Registration([FromBody] UserLoginDTO userData)
        {
            return await _authService.Registration(userData) ? Ok() : BadRequest();   
        }
    }
}
