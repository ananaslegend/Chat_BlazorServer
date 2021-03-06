using Blazored.LocalStorage;
using Chat_BlazorServer.BLL.Services;
using Chat_BlazorServer.Data;
using Chat_BlazorServer.Data.Context;
using Chat_BlazorServer.DataAccess;
using Chat_BlazorServer.DataAccess.Abstractions;
using Chat_BlazorServer.Domain.Models;
using Chat_BlazorServer.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Chat_BlazorServer.BLL.Services.Abstractions;
using Chat_BlazorServer.Helpers.Abstractions;
using Chat_BlazorServer.Services;
using Microsoft.AspNetCore.ResponseCompression;
using Chat_BlazorServer.Hubs;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Azure.Identity;

namespace Chat_BlazorServer.Configuration
{
    public static class AppConfiguration
    {
        public static WebApplicationBuilder AddServicesAsync(WebApplicationBuilder builder)
        {
            var keyVaultEndpoint = new Uri(builder.Configuration["AzureKeyVault"]);
            builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());

            // Add services to the container.
            builder.Services.AddRazorPages();

            builder.Services.AddServerSideBlazor();

            builder.Services.AddControllers()
                            .AddJsonOptions(x =>
                                x.JsonSerializerOptions.ReferenceHandler 
                                = ReferenceHandler.IgnoreCycles);
            //todo remove this \/ \/ \/
            builder.Services.AddSingleton<WeatherForecastService>();

            builder.Services.AddDbContext<ApplicationContext>(opt =>
            {
            opt.UseSqlServer(builder.Configuration["DefaultDbConnection"],
                b => b.MigrationsAssembly("Chat_BlazorServer"));
            });

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(opt =>
                {
                    opt.Password.RequireDigit = true;
                    opt.Password.RequiredLength = 5;
                    opt.Password.RequireUppercase = false;
                    opt.Password.RequireNonAlphanumeric = false;
                })
                            .AddEntityFrameworkStores<ApplicationContext>();

            builder.Services.AddScoped<IAuthJwtService, AuthJwtService>();
            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

            builder.Services.AddSignalR(o =>
            {
                o.EnableDetailedErrors = true;
            });

            builder.Services.AddTransient<MessageService>();

            builder.Services.AddAuthorization()
                            .AddAuthentication(opt =>
                            {
                                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                            })
                            .AddJwtBearer(opt =>
                            {
                                opt.TokenValidationParameters = new TokenValidationParameters
                                {
                                    ValidateIssuer = true,
                                    ValidateAudience = true,
                                    ValidateLifetime = true,
                                    ValidIssuer = builder.Configuration["JWT-Issuer"],
                                    ValidAudience = builder.Configuration["JWT-Audience"],
                                    IssuerSigningKey = new SymmetricSecurityKey(
                                    Encoding.UTF8.GetBytes(builder.Configuration["JWT-Key"]))
                                };
                            });

            builder.Services.AddResponseCompression(opt =>
                {
                    opt.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" });
                });

            //Client things 
            builder.Services.AddBlazoredLocalStorage();

            builder.Services.AddScoped<ITokenHelper, TokenHelper>();

            builder.Services.AddHttpClient("BaseClient", client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["Url:BaseUrl"]);
            });

            builder.Services.AddScoped<ChatClient>();

            return builder;
        }
    }
}
