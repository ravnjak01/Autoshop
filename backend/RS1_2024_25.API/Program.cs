
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
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public partial class Program
{
    private static async Task Main(string[] args)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var builder = WebApplication.CreateBuilder(args);

        // **Dodaj DbContext**
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
       options.UseSqlServer(
           config.GetConnectionString("db1"),
           sqlOptions => sqlOptions.EnableRetryOnFailure(
               maxRetryCount: 5,
               maxRetryDelay: TimeSpan.FromSeconds(10),
               errorNumbersToAdd: null
           )
       )


   );


        
        builder.Services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();


        var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!);

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];
                    var path = context.HttpContext.Request.Path;

                   
                    if (!string.IsNullOrEmpty(accessToken) &&
                        path.StartsWithSegments("/myHub"))
                    {
                        context.Token = accessToken;
                    }
                    return Task.CompletedTask;
                }
            };
        });




        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = false;

        });


        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

          
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Unesite vaš JWT token"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] {}
        }
    });
            c.OperationFilter<MyAuthorizationSwaggerHeader>();
        });



        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontendAndSwagger", policy =>
            {
                policy.WithOrigins("http://localhost:4200", "https://localhost:7001")
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials();
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
        builder.Services.AddHttpContextAccessor();




        // **Dodaj middleware**


        var app =builder.Build();
     

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            await SeedRolesAsync(services);
        }

        app.UseRouting();
        app.UseCors("AllowFrontendAndSwagger");
        app.UseAuthentication();   
        app.UseAuthorization();


        app.Use(async (context, next) =>
        {
            Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");
            await next();
        });
      
        app.UseMiddleware<AuditLogMiddleware>();
        app.UseStaticFiles();
      
        // **Dodaj rute**
        app.MapControllers();
        app.MapHub<MySignalrHub>("/mysignalr-hub-path");

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API v1");
            c.RoutePrefix = "swagger"; 
        });
        app.Run();
    }
    public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string[] roleNames = { "Admin", "Customer", "Manager" };
        string roleToDelete = "Dean";

        var deanRole = await roleManager.FindByNameAsync(roleToDelete);
        if (deanRole != null)
        {
            
            await roleManager.DeleteAsync(deanRole);
        }

        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
}
