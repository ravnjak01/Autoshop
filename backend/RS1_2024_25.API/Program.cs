
// using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using RS1_2024_25.API.Data;
//using RS1_2024_25.API.Data.Models;
//using RS1_2024_25.API.Data.Models.Modul1_Auth.Services;
//using RS1_2024_25.API.Helper;
//using RS1_2024_25.API.Helper.Auth;
//using RS1_2024_25.API.Services;
//using RS1_2024_25.API.SignalRHubs;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


//internal class Program
//{
//    private static void Main(string[] args)
//    {
//        var config = new ConfigurationBuilder()
//.AddJsonFile("appsettings.json", false)
//.Build();

//        var builder = WebApplication.CreateBuilder(args);

//        // Add services to the container.

//        builder.Services.AddDbContext<ApplicationDbContext>(options =>
//            options.UseSqlServer(config.GetConnectionString("db1")));



//        builder.Services.AddControllers();
//        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//        builder.Services.AddEndpointsApiExplorer();
//        builder.Services.AddSwaggerGen(x => x.OperationFilter<MyAuthorizationSwaggerHeader>());
//        builder.Services.AddHttpContextAccessor();

//        //dodajte vaše servise
//        builder.Services.AddTransient<MyAuthService>();
//        builder.Services.AddTransient<MyTokenGenerator>();
//        builder.Services.AddSignalR();
//        builder.Services.AddScoped<AuthService>();
//        builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

//        builder.Services.AddIdentity<User, IdentityRole>()
//            .AddEntityFrameworkStores<ApplicationDbContext>()
//            .AddDefaultTokenProviders();// Veže Identity Server za ASP.NET Identity
//        builder.Services.AddIdentityServer()
//           .AddDeveloperSigningCredential() // Koristi se za razvoj, u produkciji koristi pravi klju?
//           //.AddInMemoryClients(Config.Clients)
//           //.AddInMemoryApiScopes(Config.ApiScopes)
//           //.AddInMemoryApiResources(Config.ApiResources)
//           //.AddInMemoryIdentityResources(Config.IdentityResources)
//           .AddAspNetIdentity<User>();
//        //dodano ispod ovo AddCors
//        builder.Services.AddCors(options =>
//        {
//            options.AddPolicy("AllowAll",
//                policy =>
//                {
//                    policy.WithOrigins("http://localhost:7000");
//                    policy.AllowCredentials()
//                          .AllowAnyMethod()
//                          .AllowAnyHeader();
//                });
//        });
//        var app = builder.Build();

//        // Configure the HTTP request pipeline.
//        app.UseSwagger();
//        app.UseSwaggerUI();

//        //app.UseCors(
//        //    options => options
//        //        .SetIsOriginAllowed(x => _ = true)
//        //        .AllowAnyMethod()
//        //        .AllowAnyHeader()
//        //        .AllowCredentials()
//        //); //This needs to set everything allowed

//        app.UseCors("AllowAll");//dodano
//        app.UseIdentityServer();
//        app.UseAuthorization();

//        app.MapControllers();
//        app.MapHub<MySignalrHub>("/mysginalr-hub-path");

//        app.Run();
//    }
//}
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
        builder.Services.AddIdentityServer()
            .AddDeveloperSigningCredential()
            .AddAspNetIdentity<User>();

        // **Dodaj CORS konfiguraciju**
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.WithOrigins("http://localhost:7000") // Prilagodi po potrebi
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials(); // Ako koristiš cookies za autentifikaciju
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

        var app = builder.Build();

        // **Dodaj middleware**
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseCors("AllowAll"); // **Primeni CORS politiku**
        app.UseIdentityServer();
        app.UseAuthentication(); // **Obavezno za ASP.NET Identity**
        app.UseAuthorization();

app.UseMiddleware<AuditLogMiddleware>();

        // **Dodaj rute**
        app.MapControllers();
        app.MapHub<MySignalrHub>("/mysignalr-hub-path");
        app.Run();
    }
}
