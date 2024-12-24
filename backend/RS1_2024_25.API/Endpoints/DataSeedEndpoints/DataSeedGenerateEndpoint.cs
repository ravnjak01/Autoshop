namespace RS1_2024_25.API.Endpoints.DataSeedEndpoints;

using Microsoft.AspNetCore.Mvc;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models.Modul1_Auth;
using RS1_2024_25.API.Helper.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

[Route("data-seed")]
public class DataSeedGenerateEndpoint(ApplicationDbContext db)
    : MyEndpointBaseAsync
    .WithoutRequest
    .WithResult<string>
{
    [HttpPost]
    public override async Task<string> HandleAsync(CancellationToken cancellationToken = default)
    {
        //if (db.MyAppUsersAll.Any())
        //{
        //    throw new Exception("Podaci su vec generisani");
        //}

        //// Kreiranje korisnika s ulogama
        //var users = new List<MyAppUser>
        //{
        //    new MyAppUser
        //    {
        //        Email = "admin",
        //        FirstName = "Admin",
        //        LastName = "One",
        //        IsAdmin = true,
        //        IsDean = false
        //    },
        //    new MyAppUser
        //    {
        //        Email = "manager",
        //        FirstName = "Manager",
        //        LastName = "One",
        //        IsAdmin = false,
        //        IsDean = true
        //    },
        //    new MyAppUser
        //    {
        //        Email = "user1",
        //        FirstName = "User",
        //        LastName = "One",
        //        IsAdmin = false,
        //        IsDean = false
        //    },
        //    new MyAppUser
        //    {
        //        Email = "user2",
        //        FirstName = "User",
        //        LastName = "Two",
        //        IsAdmin = false,
        //        IsDean = false
        //    }
        //};

        //foreach (var x in users)
        //{
        //    x.SetPassword("test");
        //}

        

        return "Data generation completed successfully.";
    }
}
