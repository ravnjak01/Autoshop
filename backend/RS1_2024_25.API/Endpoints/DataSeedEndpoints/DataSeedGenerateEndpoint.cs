namespace RS1_2024_25.API.Endpoints.DataSeedEndpoints;

using Microsoft.AspNetCore.Mvc;
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

[Route("data-seed")]
public class DataSeedGenerateEndpoint(ApplicationDbContext db)
    : MyEndpointBaseAsync
    .WithoutRequest
    .WithResult<string>
{
    [HttpPost]
    public override async Task<string> HandleAsync(CancellationToken cancellationToken = default)
    {
        if (!db.BlogPosts.Any())
        {
            // Kreiranje blogs
            var blogs = new List<BlogPost>
            {
                new BlogPost()
                {
                    Title = "I dalje najviše uvozimo dizele, zašto ih bh. vozači toliko vole?",
                    Content = " Golf Dvojka je jedan od najvećih krivaca zašto bosanskohercegovački vozači najviše vjeruju polovnim dizelašima. Taj trend nikad nije sišao s kursa, čak i u nepogodno vrijeme za njihovu nabavku, ali i izuzetno skupo održavanje.\r\n\r\n\r\nKada kupuju polovni auto, bh. vozači u 80-90 posto slučajeva po \"defaultu\" ne misle ni o čemu drugo, nego o dizelu. Pretpostavimo da je druga odluka da to bude Volkswagen.\r\n\r\nVolkswagen Golf II je omasovio pristup dizelima kao svetoj kravi, zbog čega i danas mnogi od njih donose prvo odluku da kupovina polovnog vozila mora biti dizel. \r\n\r\nProsječan bh. kupac voli se uklopiti u budžet od 10-12 hiljada KM. Aktuelni euro 5 standard omogućava nabavku dizela za te novce, ali računajte da su to već 15 godina stari automobili. To potvrđuje i statistika prvi put registrovanih uvezenih automobila.",
                    PublishedDate = DateTime.Now,
                    IsPublished = false,
                    Active = true,
                },
                new BlogPost()
                {
                    Title = "Kako je propao Mercedesov taksi program? Radnički karakter više ne ide uz premium imidž",
                    Content = "Podsjećamo da su fabrička Mercedesova taksi vozila sa ugrađenim taksimetrom i brojnim specifičnim prepravkama, koje su imale za cilj da ojačaju dijelove izložene ubrzanom habanju u uslovima taksi prevoza, sa proizvodne trake u Stuttgartu prvi put sišla 1896. godine. Bila je to pionirska ideja koju niko nije uspio da nadmaši do današnjih dana.\r\n\r\nMercedes taksi postao je obilježje prestiža, te najbolje moguće usluge u ovoj automobilskoj uslužnoj djelatnosti, ali i veoma dobra pokretna reklama za njemački brend. Zahvaljujući veoma kvalitetnim limuzinama proizvedenim između sedamdesetih i devedesetih godina prošlog vijeka Mercedes je dobio epitet nepoderivog i neuništivog, što se bilo dragocjeno nasljedstvo, ali i značajan kredit za modele iz 21. vijeka koji se nisu proslavili po ovom pitanju.",
                    PublishedDate = DateTime.Now,
                    IsPublished = true,
                    Active = true
                },
                new BlogPost()
                {
                    Title = "Audi, BMW, Ford, Volkswagen: Prolazna kriza ili početak kraja njemačkog carstva ",
                    Content = " Stub njemačke, ali i evropske ekonomije je pred kolapsom, jer njemačka autoindustrija prolazi kroz turbulentna vremena. BMW, Audi, Ford i Volkswagen, dugogodišnji stubovi sektora, suočavaju se s naglim padom profita i velikim izazovima koji ugrožavaju njihovu budućnost.\r\n\r\nVeličinu krize kroz koju prolazi industrija u Njemačkoj najbolje otkrivaju brojke, krize, štrajkovi i protesti posljednih mjeseci. Njemačko carstvo, čini se, nikada prije nije bilo ovako u paketu uzdrmano.\r\n\r\nBMW je zabilježio pad profita za 84 posto u trećem kvartalu 2024. godine. Ne zaostaje ni Audi, koji se bori s mnogo brutalnijim padom profita od 91 posto. Što se tiče Volkswagena, on se priprema za pokretanje drastičnog plana uštede od više od 10 milijardi eura i uz prijetnju štrajkovima. Slično je i kod Forda. Naime, evropska filijala američkog giganta planira otpustiti 4.000 radnika, uglavnom u Njemačkoj zbog drastičnog pada potražnje za električnim vozilima.",
                    PublishedDate = DateTime.Now,
                    IsPublished = false,
                    Active = false
                }
            };

            db.AddRange(blogs);
            db.SaveChanges();
        }

        if (!db.BlogComments.Any())
        {
            // Dohvatamo sve postojeće Blog postove iz baze
            var postovi = db.BlogPosts.ToList();

            if (postovi.Any())
            {
                // Uzimamo ID prvog posta (umjesto 1)
                var prviId = postovi[0].Id;

                db.BlogComments.Add(new BlogComment { BlogPostId = prviId, Content = "Odličan post!", CreatedAt = DateTime.Now });
                db.BlogComments.Add(new BlogComment { BlogPostId = prviId, Content = "Zanimljiv tekst, hvala!", CreatedAt = DateTime.Now });

                // Ako imamo barem dva posta, dodajemo komentar i na drugi (umjesto 2)
                if (postovi.Count > 1)
                {
                    var drugiId = postovi[1].Id;
                    db.BlogComments.Add(new BlogComment { BlogPostId = drugiId, Content = "Slažem se sa prethodnim komentarima.", CreatedAt = DateTime.Now });
                }

                db.SaveChanges();
            }
        }
        if (!db.BlogRatings.Any())
        {
            var postovi = db.BlogPosts.ToList();

            if (postovi.Any())
            {
                var prviId = postovi[0].Id;

                db.BlogRatings.Add(new BlogRating { BlogPostId = prviId, Rating = 5, CreatedAt = DateTime.Now });
                db.BlogRatings.Add(new BlogRating { BlogPostId = prviId, Rating = 4, CreatedAt = DateTime.Now });

                if (postovi.Count > 1)
                {
                    var drugiId = postovi[1].Id;
                    db.BlogRatings.Add(new BlogRating { BlogPostId = drugiId, Rating = 3, CreatedAt = DateTime.Now });
                }

                db.SaveChanges();
            }
        }

        if (!db.Categories.Any())
        {
            var categories = new List<Category>
    {
        new Category { Name = "Gume", Code = "CAT001" },
        new Category { Name = "Akumulatori", Code = "CAT002" },
        new Category { Name = "Oprema/Kozmetika", Code = "CAT003" },
        new Category { Name = "Autopatosnice", Code = "CAT004" },
        new Category { Name = "Podmetači za gepek", Code = "CAT005" },
        new Category { Name = "Alu felge", Code = "CAT006" },
        new Category { Name = "Ulje i tekućine", Code = "CAT007" },
        new Category { Name = "Sistem ovjesa", Code = "CAT008" },
        new Category { Name = "Kočioni sistem", Code = "CAT009" },
        new Category { Name = "Sistem paljenja", Code = "CAT010" },
        new Category { Name = "Transmisija", Code = "CAT011" },
        new Category { Name = "Filteri", Code = "CAT012" },
        new Category { Name = "Dijelovi motora", Code = "CAT013" }
    };

            db.Categories.AddRange(categories);
            db.SaveChanges();
        }

        if (!db.Products.Any())
        {

            var sveKat = db.Categories.ToList();

            // 2. Umjesto ID-a, koristi First ili Find da nađeš CIJELI OBJEKAT
            var katAkumulatori = sveKat.FirstOrDefault(c => c.Code == "CAT002");
            var katUlje = sveKat.FirstOrDefault(c => c.Code == "CAT007");
            var katGume = sveKat.FirstOrDefault(c => c.Code == "CAT001");
            var katOvjes = sveKat.FirstOrDefault(c => c.Code == "CAT008");
            var katPaljenje = sveKat.FirstOrDefault(c => c.Code == "CAT010");
            var katKocnice = sveKat.FirstOrDefault(c => c.Code == "CAT009");
            var katTransmisija = sveKat.FirstOrDefault(c => c.Code == "CAT011");
            var katFilteri = sveKat.FirstOrDefault(c => c.Code == "CAT012");
            var katDijeloviMotora = sveKat.FirstOrDefault(c => c.Code == "CAT013");
            var katOprema = sveKat.FirstOrDefault(c => c.Code == "CAT003");
            var katUljeTekucine = sveKat.FirstOrDefault(c => c.Code == "CAT007");
            var katPatosnice = sveKat.FirstOrDefault(c => c.Code == "CAT004");
            var katPodmetaci = sveKat.FirstOrDefault(c => c.Code == "CAT005");
            var katFelge = sveKat.FirstOrDefault(c => c.Code == "CAT006");


            db.Products.AddRange(
                new Product
                {
                    Name = "Akumulator Exide Premium 77Ah",
                    Code = "ACC-EX77",
                    SKU = "EXIDE-77-P",
                    Description = "Visokokvalitetni akumulator snage 77Ah i 760A startne struje.",
                    Price = 185.50m,
                    StockQuantity = 15,
                    Category = katAkumulatori, 
                    Brend = "Exide",
                    ImageUrl = "/images/products/car_battery.jpg",
                    Active = true,
                    CreatedAt = DateTime.Now
                },
                new Product
                {
                    Name = "Castrol Edge 5W-30",
                    Code = "COL-121",
                    SKU = "OLE-5-P",
                    Description = "Vrhunsko sintetičko motorno ulje.",
                    Price = 50.00m,
                    StockQuantity = 20,
                    Category = katUljeTekucine, 
                    Brend = "Castrol",
                    ImageUrl = "/images/products/castrol_oil.jpg",
                    Active = true,
                    CreatedAt = DateTime.Now
                },
                new Product
                {
                    Name = "Michelin Pilot Sport 5",
                    Code = "TRE-I22",
                    SKU = "TIRE-77-A",
                    Description = "Gume vrhunskih performansi.",
                    Price = 250.00m,
                    StockQuantity = 18,
                    Category= katGume,
                    Brend = "Michelin",
                    ImageUrl = "/images/products/michelin_tire.jpg",
                    Active = true,
                    CreatedAt = DateTime.Now
                },
                new Product
                {
                    Name = "Bilstein B6 Amortizer",
                    Code = "SUS-B6-01",
                    SKU = "BIL-B6-VAF",
                    Description = "Plinski amortizeri visokih performansi.",
                    Price = 120.00m,
                    StockQuantity = 8,
                    Category = katOvjes,
                    Brend = "Bilstein",
                    ImageUrl = "/images/products/shock_absorbers.webp",
                    Active = true,
                    CreatedAt = DateTime.Now
                },
                new Product
                {
                    Name = "NGK Iridium IX Svjećice",
                    Code = "IGN-NGK-IX",
                    SKU = "NGK-7092-B",
                    Description = "Iridijumske svjećice vrhunske kvalitete.",
                    Price = 15.50m,
                    StockQuantity = 100,
                    Category = katPaljenje,
                    Brend = "NGK",
                    ImageUrl = "/images/products/ngk.jpg",
                    Active = true,
                    CreatedAt = DateTime.Now
                },
                new Product
                {
                    Name = "Brembo Max Kočioni Diskovi",
                    Code = "BRK-BM-05",
                    SKU = "BRE-DISK-V1",
                    Description = "Visokokvalitetni ventilirajući diskovi.",
                    Price = 145.00m,
                    StockQuantity = 12,
                    Category = katKocnice,
                    Brend = "Brembo",
                    ImageUrl = "/images/products/brembo_disk.jpg",
                    Active = true,
                    CreatedAt = DateTime.Now
                },
                new Product
                {
                    Name = "ZF Set za servis mjenjača",
                    Code = "GRB-ZF-8HP",
                    SKU = "ZF-GA8HP-KIT",
                    Description = "Originalni set za servis automatskog mjenjača.",
                    Price = 350.00m,
                    StockQuantity = 5,
                    Category = katTransmisija,
                    Brend = "ZF",
                    ImageUrl = "/images/products/gearbox.webp",
                    Active = true,
                    CreatedAt = DateTime.Now
                },
                new Product
                {
                    Name = "Filter zraka Mann-Filter",
                    Code = "FLT-AIR-001",
                    SKU = "MANN-C30005",
                    Description = "Vrhunski filter zraka.",
                    Price = 25.50m,
                    StockQuantity = 45,
                    Category = katFilteri,
                    Brend = "Mann-Filter",
                    ImageUrl = "/images/products/filter_zraka.jpg",
                    Active = true,
                    CreatedAt = DateTime.Now
                },
                new Product
                {
                    Name = "Brtva glave motora Elring",
                    Code = "ENG-GASK-001",
                    SKU = "ELR-735.450",
                    Description = "Visokokvalitetna brtva glave motora.",
                    Price = 85.00m,
                    StockQuantity = 12,
                    Category = katDijeloviMotora,
                    Brend = "Elring",
                    ImageUrl = "/images/products/head_gasket.webp",
                    Active = true,
                    CreatedAt = DateTime.Now
                },
                new Product
                {
                    Name = "Sonax Ceramic Spray",
                    Code = "KOZ-SNX-001",
                    SKU = "SONAX-257-400",
                    Description = "Keramički premaz u spreju.",
                    Price = 35.00m,
                    StockQuantity = 25,
                    Category = katOprema,
                    Brend = "Sonax",
                    ImageUrl = "/images/products/sonax_sprej.jpg",
                    Active = true,
                    CreatedAt = DateTime.Now
                },
                new Product
                {
                    Name = "AMC Glava motora",
                    Code = "ENG-CH-908",
                    SKU = "AMC-908711",
                    Description = "Nova aluminijska glava motora.",
                    Price = 1250.00m,
                    StockQuantity = 2,
                    Category = katDijeloviMotora,
                    Brend = "AMC",
                    ImageUrl = "/images/products/cylinder_head.webp",
                    Active = true,
                    CreatedAt = DateTime.Now
                },
                new Product
                {
                    Name = "Antifriz Febi G12++",
                    Code = "LIQ-ANT-012",
                    SKU = "FEBI-37400-5L",
                    Description = "Rashladni koncentrat.",
                    Price = 45.00m,
                    StockQuantity = 30,
                    Category = katUljeTekucine,
                    Brend = "Febi Bilstein",
                    ImageUrl = "/images/products/antifreeze.webp",
                    Active = true,
                    CreatedAt = DateTime.Now
                }
            );
            db.SaveChanges();
        }

        if (!db.Discounts.Any())
        {


            db.Discounts.Add(new Discount
            {
                Name = "Proljetna Akcija",
                DiscountPercentage = 10,
                StartDate = new DateTime(2025, 4, 1),
                EndDate = new DateTime(2025, 6, 1)
            });
            db.SaveChanges();
        }

        if (!db.DiscountProducts.Any())
        {
            db.DiscountProducts.AddRange(
                new DiscountProduct { DiscountId = 1, ProductId = 1 }, 
                new DiscountProduct { DiscountId = 1, ProductId = 2 }  
            );
            db.SaveChanges();
        }

        if (!db.DiscountCategories.Any())
        {

            db.DiscountCategories.AddRange(
                new DiscountCategory { DiscountId = 1, CategoryId = 1 },
                new DiscountCategory { DiscountId = 1, CategoryId = 2 }
                );
            db.SaveChanges();
        }

        if (!db.DiscountCodes.Any())
        {
            db.DiscountCodes.AddRange(
                new DiscountCode { DiscountId = 1, Code = "PROLJECE5" },
                new DiscountCode { DiscountId = 1, Code = "AUTO10" }
                );
            db.SaveChanges();
        }

        return "Data generation completed successfully.";
    }
}
