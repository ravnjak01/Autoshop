
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


        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
            options.LoginPath = "/api/auth/login"; // frontend šalje POST
            options.AccessDeniedPath = "/api/auth/access-denied";
            options.SlidingExpiration = true;
        });

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
            options.AddPolicy("AllowFrontend", policy =>
            {
               policy.WithOrigins("http://localhost:4200") // Prilagodi po potrebi
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
        builder.Services.AddAuthorization();
        builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
        builder.Services.AddScoped<IEmailService, EmailService>();
        // **Dodaj kontrolere i Swagger**
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(x => x.OperationFilter<MyAuthorizationSwaggerHeader>());
        builder.Services.AddHttpContextAccessor();

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // trajanje sesije
            options.LoginPath = "/api/auth/login"; // gdje preusmjeriti ako nije autorizovan
            options.AccessDeniedPath = "/api/auth/access-denied";
            options.SlidingExpiration = true; // produži trajanje ako korisnik ostaje aktivan
        });



        // **Dodaj middleware**



        var app =builder.Build();


        app.UseCors("AllowFrontend"); // **Primeni CORS politiku**

        app.UseRouting(); // **Obavezno za ASP.NET Core**
        app.UseAuthentication(); // **Obavezno za ASP.NET Identity**


        app.UseAuthorization(); 
        app.UseIdentityServer(); // **Obavezno za IdentityServer**
        app.Use(async (context, next) =>
        {
            Console.WriteLine($"🧪 Request: {context.Request.Method} {context.Request.Path}");
            await next();
        });
      
        app.UseMiddleware<AuditLogMiddleware>();
     
        // **Dodaj rute**
        app.MapControllers();
        app.MapHub<MySignalrHub>("/mysignalr-hub-path");

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API v1");
            c.RoutePrefix = "swagger"; // This line changes the Swagger UI path
        });
        app.Run();
    }
}
