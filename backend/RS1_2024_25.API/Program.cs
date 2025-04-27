
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models;
using RS1_2024_25.API.Data.Models.Modul1_Auth.Services;
using RS1_2024_25.API.Data.Middleware;
using RS1_2024_25.API.Helper;
using RS1_2024_25.API.Helper.Auth;
using RS1_2024_25.API.Services;
using RS1_2024_25.API.SignalRHubs;
using RS1_2024_25.API.Data.Models.Modul1_Auth;

public partial class Program
{
    private static void Main(string[] args)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var builder = WebApplication.CreateBuilder(args);

        // **Dodaj DbContext**
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("db1")));

        // **Dodaj ASP.NET Identity**
        builder.Services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        // **Dodaj IdentityServer**
        builder.Services.AddIdentityServer(options =>
        {
            options.KeyManagement.Enabled = false;
        })
.AddDeveloperSigningCredential()
.AddAspNetIdentity<User>();

        //builder.Services.AddIdentityServer()
        //    .AddDeveloperSigningCredential()
        //    .AddAspNetIdentity<MyAppUser>();
      
        // **Dodaj CORS konfiguraciju**
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
               policy.WithOrigins("http://localhost:4200") // Prilagodi po potrebi
               //policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials(); // Ako koristi� cookies za autentifikaciju
            });
        });


        // **Dodaj servise**
        builder.Services.AddScoped<AuthService>();
        builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        builder.Services.AddTransient<MyAuthService>();
        builder.Services.AddTransient<MyTokenGenerator>();
        builder.Services.AddSignalR();

        // **Dodaj kontrolere i Swagger**
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(x => x.OperationFilter<MyAuthorizationSwaggerHeader>());
        builder.Services.AddHttpContextAccessor();



        // **Dodaj middleware**
        var app = builder.Build();
        app.UseAuthorization();
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseCors("AllowAll");

        //  app.UseCors("AllowAll"); // **Primeni CORS politiku**
        app.UseIdentityServer();
        app.UseAuthentication(); // **Obavezno za ASP.NET Identity**
     

app.UseMiddleware<AuditLogMiddleware>();

        // **Dodaj rute**
        app.MapControllers();
        app.MapHub<MySignalrHub>("/mysignalr-hub-path");
        app.Run();
    }
}
