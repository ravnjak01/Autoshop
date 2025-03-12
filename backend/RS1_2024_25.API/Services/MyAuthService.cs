using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models.Modul1_Auth;
using RS1_2024_25.API.Helper;

namespace RS1_2024_25.API.Services
{
    public class MyAuthService(ApplicationDbContext applicationDbContext, IHttpContextAccessor httpContextAccessor, MyTokenGenerator myTokenGenerator)
    {

        // Generisanje novog tokena za korisnika
        public async Task<MyAuthenticationToken> GenerateSaveAuthToken(MyAppUser user, CancellationToken cancellationToken = default)
        {
            string randomToken = myTokenGenerator.Generate(10);

            var authToken = new MyAuthenticationToken
            {
                IpAddress = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? string.Empty,
                Value = randomToken,
                MyAppUser = user,
                RecordedAt = DateTime.Now,
            };

            applicationDbContext.Add(authToken);
            await applicationDbContext.SaveChangesAsync(cancellationToken);

            return authToken;
        }

        // Uklanjanje tokena iz baze podataka
        //public async Task<bool> RevokeAuthToken(string tokenValue, CancellationToken cancellationToken = default)
        //{
        //    var authToken = await applicationDbContext.MyAuthenticationTokensAll
        //        .FirstOrDefaultAsync(t => t.Value == tokenValue, cancellationToken);

        //    if (authToken == null)
        //        return false;

        //    applicationDbContext.Remove(authToken);
        //    await applicationDbContext.SaveChangesAsync(cancellationToken);

        //    return true;
        //}

        //// Dohvatanje informacija o autentifikaciji korisnika
        //public MyAuthInfo GetAuthInfoFromTokenString(string? authToken)
        //{
        //    if (string.IsNullOrEmpty(authToken))
        //    {
        //        return GetAuthInfoFromTokenModel(null);
        //    }

        //    MyAuthenticationToken? myAuthToken = applicationDbContext.MyAuthenticationTokensAll
        //        .IgnoreQueryFilters()
        //        .SingleOrDefault(x => x.Value == authToken);

        //    return GetAuthInfoFromTokenModel(myAuthToken);
        //}


        // Dohvatanje informacija o autentifikaciji korisnika
        //public MyAuthInfo GetAuthInfoFromRequest()
        //{
        //    string? authToken = httpContextAccessor.HttpContext?.Request.Headers["my-auth-token"];
        //    return GetAuthInfoFromTokenString(authToken);
        //}

        public MyAuthInfo GetAuthInfoFromTokenModel(MyAuthenticationToken? myAuthToken)
        {
            if (myAuthToken == null)
            {
                return new MyAuthInfo
                {
                    IsAdmin = false,
                    IsDean = false,
                    IsLoggedIn = false,
                };
            }

            return new MyAuthInfo
            {
                UserId = myAuthToken.MyAppUserId,
                Email = myAuthToken.MyAppUser!.Email,
                FirstName = myAuthToken.MyAppUser.FirstName,
                LastName = myAuthToken.MyAppUser.LastName,
                IsAdmin = myAuthToken.MyAppUser.IsAdmin,
                IsDean = myAuthToken.MyAppUser.IsDean,
                IsLoggedIn = true,
            };
        }
    }

    // DTO to hold authentication information
    public class MyAuthInfo
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsDean { get; set; }
        public bool IsStudent { get; set; }
        public bool IsProfessor { get; set; }
        public bool IsLoggedIn { get; set; }
        public string SlikaPath {  get; set; }
    }
}
