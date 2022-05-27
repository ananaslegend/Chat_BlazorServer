using Blazored.LocalStorage;
using Chat_BlazorServer.BLL.Services;
using Chat_BlazorServer.Controllers;
using Chat_BlazorServer.Data;
using Chat_BlazorServer.Data.Context;
using Chat_BlazorServer.DataAccess;
using Chat_BlazorServer.DataAccess.Abstractions;
using Chat_BlazorServer.Domain.Models;
using Chat_BlazorServer.Helpers;
using Chat_BlazorServer.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.Extensions.DependencyInjection;
using Chat_BlazorServer.Services.Abstractions;

namespace Chat_BlazorServer.Configuration
{
    public static class AppConfiguration
    {
        public static WebApplicationBuilder AddServices(WebApplicationBuilder builder)
        {
            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();

            builder.Services.AddControllers();
            //todo remove this \/ \/ \/
            builder.Services.AddSingleton<WeatherForecastService>();

            builder.Services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("Chat_BlazorServer"));
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        //ValidateIssuer = true,
                        //ValidateAudience = true,
                        //ValidateLifetime = true,
                        ValidIssuer = builder.Configuration["JWT:Issuer"],
                        ValidAudience = builder.Configuration["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
                    };
                });

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>( opt => 
                {
                    opt.Password.RequireDigit = true;
                    opt.Password.RequiredLength = 5;
                    opt.Password.RequireUppercase = false;
                    opt.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<ApplicationContext>();

            builder.Services.AddScoped<AuthJwtService>();
            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

            //Blazor things 
            builder.Services.AddHttpClient();
            builder.Services.AddBlazoredLocalStorage();

            builder.Services.AddTransient<IRestService, RestService>();

            return builder;
        }
    }
}
