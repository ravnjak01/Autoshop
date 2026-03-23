namespace RS1_2024_25.API.Endpoints.DataSeedEndpoints;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models.Modul2_Basic;
using RS1_2024_25.API.Data.Models.ShoppingCart;
using RS1_2024_25.API.Helper.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Category = Data.Models.ShoppingCart.Category;

[Authorize(Roles = "Admin")]
[Route("data-seed")]
public class DataSeedGenerateEndpoint(ApplicationDbContext db, IWebHostEnvironment env)
    : MyEndpointBaseAsync
    .WithoutRequest
    .WithResult<string>
{
    [HttpPost]
    public override async Task<string> HandleAsync(CancellationToken cancellationToken = default)
    {
        if (!env.IsDevelopment())
        {
            throw new UnauthorizedAccessException("Data seeding is allowed only in Development environment.");
        }

        if (!await db.BlogPosts.AnyAsync(cancellationToken))
        {
            var blogs = new List<BlogPost>
            {
                new BlogPost()
                {
                    Title = "I dalje najviše uvozimo dizele, zašto ih bh. vozači toliko vole?",
                    Content = " Golf Dvojka je jedan od najvećih krivaca zašto bosanskohercegovački vozači najviše vjeruju polovnim dizelašima...",
                    Image = await System.IO.File.ReadAllBytesAsync(
                        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "blogs", "golf2.webp")),
                    PublishedDate = DateTime.UtcNow,
                    IsPublished = false,
                    Active = true
                },
                new BlogPost()
                {
                    Title = "Kako je propao Mercedesov taksi program? Radnički karakter više ne ide uz premium imidž",
                    Content = "Podsjećamo da su fabrička Mercedesova taksi vozila...",
                    Image = await System.IO.File.ReadAllBytesAsync(
                        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "blogs", "mercedes-taxi.jpg")),
                    PublishedDate = DateTime.UtcNow,
                    IsPublished = true,
                    Active = true
                },
                new BlogPost()
                {
                    Title = "Audi, BMW, Ford, Volkswagen: Prolazna kriza ili početak kraja njemačkog carstva",
                    Content = " Stub njemačke, ali i evropske ekonomije je pred kolapsom...",
                    PublishedDate = DateTime.UtcNow,
                    IsPublished = false,
                    Active = false
                }
            };

            await db.AddRangeAsync(blogs, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);
        }

        if (!await db.BlogComments.AnyAsync(cancellationToken))
        {
            var postovi = await db.BlogPosts.ToListAsync(cancellationToken);

            if (postovi.Any())
            {
                var prviId = postovi[0].Id;
                db.BlogComments.Add(new BlogComment { BlogPostId = prviId, Content = "Odličan post!", CreatedAt = DateTime.UtcNow });
                db.BlogComments.Add(new BlogComment { BlogPostId = prviId, Content = "Zanimljiv tekst, hvala!", CreatedAt = DateTime.UtcNow });

                if (postovi.Count > 1)
                {
                    var drugiId = postovi[1].Id;
                    db.BlogComments.Add(new BlogComment { BlogPostId = drugiId, Content = "Slažem se sa prethodnim komentarima.", CreatedAt = DateTime.UtcNow });
                }

                await db.SaveChangesAsync(cancellationToken);
            }
        }

        if (!await db.BlogRatings.AnyAsync(cancellationToken))
        {
            var postovi = await db.BlogPosts.ToListAsync(cancellationToken);

            if (postovi.Any())
            {
                var prviId = postovi[0].Id;
                db.BlogRatings.Add(new BlogRating { BlogPostId = prviId, Rating = 5, CreatedAt = DateTime.UtcNow });
                db.BlogRatings.Add(new BlogRating { BlogPostId = prviId, Rating = 4, CreatedAt = DateTime.UtcNow });

                if (postovi.Count > 1)
                {
                    var drugiId = postovi[1].Id;
                    db.BlogRatings.Add(new BlogRating { BlogPostId = drugiId, Rating = 3, CreatedAt = DateTime.UtcNow });
                }

                await db.SaveChangesAsync(cancellationToken);
            }
        }

        if (!await db.Categories.AnyAsync(cancellationToken))
        {
            var categories = new List<Category>
            {
                new Category { Name = "Tires", Code = "CAT001" },
                new Category { Name = "Batteris", Code = "CAT002" },
                new Category { Name = "Equipment/Cosmetics", Code = "CAT003" },
                new Category { Name = "Car Floor Mats", Code = "CAT004" },
                new Category { Name = "Trunk Mats", Code = "CAT005" },
                new Category { Name = "Alloy Wheels", Code = "CAT006" },
                new Category { Name = "Oil and Fluids", Code = "CAT007" },
                new Category { Name = "Suspension System", Code = "CAT008" },
                new Category { Name = "Braking System", Code = "CAT009" },
                new Category { Name = "Ignition System", Code = "CAT010" },
                new Category { Name = "Transmission", Code = "CAT011" },
                new Category { Name = "Filters", Code = "CAT012" },
                new Category { Name = "Engine Parts", Code = "CAT013" }
            };

            await db.Categories.AddRangeAsync(categories, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);
        }

        if (!await db.Products.AnyAsync(cancellationToken))
        {
            var sveKat = await db.Categories.ToListAsync(cancellationToken);

            var katAkumulatori = sveKat.FirstOrDefault(c => c.Code == "CAT002");
            var katGume = sveKat.FirstOrDefault(c => c.Code == "CAT001");
            var katOvjes = sveKat.FirstOrDefault(c => c.Code == "CAT008");
            var katPaljenje = sveKat.FirstOrDefault(c => c.Code == "CAT010");
            var katKocnice = sveKat.FirstOrDefault(c => c.Code == "CAT009");
            var katTransmisija = sveKat.FirstOrDefault(c => c.Code == "CAT011");
            var katFilteri = sveKat.FirstOrDefault(c => c.Code == "CAT012");
            var katDijeloviMotora = sveKat.FirstOrDefault(c => c.Code == "CAT013");
            var katOprema = sveKat.FirstOrDefault(c => c.Code == "CAT003");
            var katUljeTekucine = sveKat.FirstOrDefault(c => c.Code == "CAT007");

            await db.Products.AddRangeAsync( new List<Product> {
                new Product { Name = "Car battery Exide Premium 77Ah", Code = "ACC-EX77", SKU = "EXIDE-77-P", Description = "High qualitiy battery capacitiy 77Ah i 760A starting current.", Price = 185.50m, StockQuantity = 15, Category = katAkumulatori, Brend = "Exide", ImageUrl = "/images/products/car_battery.jpg", Active = true, CreatedAt = DateTime.UtcNow },
                new Product { Name = "Castrol Edge 5W-30", Code = "COL-121", SKU = "OLE-5-P", Description = "Great synthetic motor oil.", Price = 50.00m, StockQuantity = 20, Category = katUljeTekucine, Brend = "Castrol", ImageUrl = "/images/products/castrol_oil.jpg", Active = true, CreatedAt = DateTime.UtcNow },
                new Product { Name = "Michelin Pilot Sport 5", Code = "TRE-I22", SKU = "TIRE-77-A", Description = "Tires of supberb performances.", Price = 250.00m, StockQuantity = 18, Category = katGume, Brend = "Michelin", ImageUrl = "/images/products/michelin_tire.jpg", Active = true, CreatedAt = DateTime.UtcNow },
                new Product { Name = "Bilstein B6 Amortizer", Code = "SUS-B6-01", SKU = "BIL-B6-VAF", Description = "High performance gas shock absorbers.", Price = 120.00m, StockQuantity = 8, Category = katOvjes, Brend = "Bilstein", ImageUrl = "/images/products/shock_absorbers.webp", Active = true, CreatedAt = DateTime.UtcNow },
                new Product { Name = "NGK Iridium IX Svjećice", Code = "IGN-NGK-IX", SKU = "NGK-7092-B", Description = "Top quality iridium spark plugs.", Price = 15.50m, StockQuantity = 100, Category = katPaljenje, Brend = "NGK", ImageUrl = "/images/products/ngk.jpg", Active = true, CreatedAt = DateTime.UtcNow },
                new Product { Name = "Brembo Max Brake Discs", Code = "BRK-BM-05", SKU = "BRE-DISK-V1", Description = "High qualitiy ventilating discs.", Price = 145.00m, StockQuantity = 12, Category = katKocnice, Brend = "Brembo", ImageUrl = "/images/products/brembo_disk.jpg", Active = true, CreatedAt = DateTime.UtcNow },
                new Product { Name = "ZF Gearbox Service Kit", Code = "GRB-ZF-8HP", SKU = "ZF-GA8HP-KIT", Description = "Original set for automatic transmission service.", Price = 350.00m, StockQuantity = 5, Category = katTransmisija, Brend = "ZF", ImageUrl = "/images/products/gearbox.webp", Active = true, CreatedAt = DateTime.UtcNow },
                new Product { Name = "Air Filter Mann-Filter", Code = "FLT-AIR-001", SKU = "MANN-C30005", Description = "Great air filter.", Price = 25.50m, StockQuantity = 45, Category = katFilteri, Brend = "Mann-Filter", ImageUrl = "/images/products/filter_zraka.jpg", Active = true, CreatedAt = DateTime.UtcNow },
                new Product { Name = "Elring Head Gasket", Code = "ENG-GASK-001", SKU = "ELR-735.450", Description = "High quality head gasket.", Price = 85.00m, StockQuantity = 12, Category = katDijeloviMotora, Brend = "Elring", ImageUrl = "/images/products/head_gasket.webp", Active = true, CreatedAt = DateTime.UtcNow },
                new Product { Name = "Sonax Ceramic Spray", Code = "KOZ-SNX-001", SKU = "SONAX-257-400", Description = "Ceramic coating in sprey.", Price = 35.00m, StockQuantity = 25, Category = katOprema, Brend = "Sonax", ImageUrl = "/images/products/sonax_sprej.jpg", Active = true, CreatedAt = DateTime.UtcNow },
                new Product { Name = "AMC Cylinder Head", Code = "ENG-CH-908", SKU = "AMC-908711", Description = "New aluminium cylinder head.", Price = 1250.00m, StockQuantity = 2, Category = katDijeloviMotora, Brend = "AMC", ImageUrl = "/images/products/cylinder_head.webp", Active = true, CreatedAt = DateTime.UtcNow },
                new Product { Name = "Antifreeze Febi G12++", Code = "LIQ-ANT-012", SKU = "FEBI-37400-5L", Description = "Cooling concentrate.", Price = 45.00m, StockQuantity = 30, Category = katUljeTekucine, Brend = "Febi Bilstein", ImageUrl = "/images/products/antifreeze.webp", Active = true, CreatedAt = DateTime.UtcNow }

                },cancellationToken);

            await db.SaveChangesAsync(cancellationToken);
        }

        if (!await db.Discounts.AnyAsync(cancellationToken))
        {
            var proljetnaAkcija = new Discount
            {
                Name = "Proljetna Akcija",
                DiscountPercentage = 10,
                StartDate = new DateTime(2026, 4, 1),
                EndDate = new DateTime(2026, 6, 1)
            };

            db.Discounts.Add(proljetnaAkcija);
            await db.SaveChangesAsync(cancellationToken);

            if (!await db.DiscountProducts.AnyAsync(cancellationToken))
            {
                var proizvodi = await db.Products.Take(2).ToListAsync(cancellationToken);
                if (proizvodi.Count >= 2)
                {
                    db.DiscountProducts.AddRange(
                        new DiscountProduct { Discount = proljetnaAkcija, Product = proizvodi[0] },
                        new DiscountProduct { Discount = proljetnaAkcija, Product = proizvodi[1] }
                    );
                }
            }

            if (!await db.DiscountCategories.AnyAsync(cancellationToken))
            {
                var kategorije = await db.Categories.Take(2).ToListAsync(cancellationToken);
                if (kategorije.Count >= 2)
                {
                    db.DiscountCategories.AddRange(
                        new DiscountCategory { Discount = proljetnaAkcija, Category = kategorije[0] },
                        new DiscountCategory { Discount = proljetnaAkcija, Category = kategorije[1] }
                    );
                }
            }

            if (!await db.DiscountCodes.AnyAsync(cancellationToken))
            {
                db.DiscountCodes.AddRange(
                    new DiscountCode { Discount = proljetnaAkcija, Code = "PROLJECE5", ValidFrom = DateTime.UtcNow, ValidTo = DateTime.UtcNow.AddDays(3) },
                    new DiscountCode { Discount = proljetnaAkcija, Code = "AUTO10", ValidFrom = DateTime.UtcNow, ValidTo = DateTime.UtcNow.AddDays(5) }
                );
            }

            await db.SaveChangesAsync(cancellationToken);
        }

        return "Data generation completed successfully.";
    }
}