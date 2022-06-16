using Chat_BlazorServer.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Chat_BlazorServer.Pages;
using Chat_BlazorServer.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Chat_BlazorServer.Domain.Models;
using Chat_BlazorServer.DataAccess.Abstractions;
using Chat_BlazorServer.DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Chat_BlazorServer.Configuration;
using Chat_BlazorServer.Hubs;

var builder = AppConfiguration
                .AddServicesAsync(WebApplication
                .CreateBuilder(args));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapBlazorHub();
    endpoints.MapHub<ChatHub>("/chatHub");
    endpoints.MapFallbackToPage("/_Host");
    endpoints.MapControllers();
});

app.Run();
