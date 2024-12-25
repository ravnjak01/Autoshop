namespace RS1_2024_25.API.Endpoints.DataSeedEndpoints;

using Microsoft.AspNetCore.Mvc;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models.Modul1_Auth;
using RS1_2024_25.API.Data.Models.Modul2_Basic;
using RS1_2024_25.API.Helper.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                },
                new BlogPost()
                {
                    Title = "Kako je propao Mercedesov taksi program? Radnički karakter više ne ide uz premium imidž",
                    Content = "Podsjećamo da su fabrička Mercedesova taksi vozila sa ugrađenim taksimetrom i brojnim specifičnim prepravkama, koje su imale za cilj da ojačaju dijelove izložene ubrzanom habanju u uslovima taksi prevoza, sa proizvodne trake u Stuttgartu prvi put sišla 1896. godine. Bila je to pionirska ideja koju niko nije uspio da nadmaši do današnjih dana.\r\n\r\nMercedes taksi postao je obilježje prestiža, te najbolje moguće usluge u ovoj automobilskoj uslužnoj djelatnosti, ali i veoma dobra pokretna reklama za njemački brend. Zahvaljujući veoma kvalitetnim limuzinama proizvedenim između sedamdesetih i devedesetih godina prošlog vijeka Mercedes je dobio epitet nepoderivog i neuništivog, što se bilo dragocjeno nasljedstvo, ali i značajan kredit za modele iz 21. vijeka koji se nisu proslavili po ovom pitanju.",
                    PublishedDate = DateTime.Now,
                    IsPublished = true,
                },
                new BlogPost()
                {
                    Title = "Audi, BMW, Ford, Volkswagen: Prolazna kriza ili početak kraja njemačkog carstva ",
                    Content = " Stub njemačke, ali i evropske ekonomije je pred kolapsom, jer njemačka autoindustrija prolazi kroz turbulentna vremena. BMW, Audi, Ford i Volkswagen, dugogodišnji stubovi sektora, suočavaju se s naglim padom profita i velikim izazovima koji ugrožavaju njihovu budućnost.\r\n\r\nVeličinu krize kroz koju prolazi industrija u Njemačkoj najbolje otkrivaju brojke, krize, štrajkovi i protesti posljednih mjeseci. Njemačko carstvo, čini se, nikada prije nije bilo ovako u paketu uzdrmano.\r\n\r\nBMW je zabilježio pad profita za 84 posto u trećem kvartalu 2024. godine. Ne zaostaje ni Audi, koji se bori s mnogo brutalnijim padom profita od 91 posto. Što se tiče Volkswagena, on se priprema za pokretanje drastičnog plana uštede od više od 10 milijardi eura i uz prijetnju štrajkovima. Slično je i kod Forda. Naime, evropska filijala američkog giganta planira otpustiti 4.000 radnika, uglavnom u Njemačkoj zbog drastičnog pada potražnje za električnim vozilima.",
                    PublishedDate = DateTime.Now,
                    IsPublished = false

                }
            };

            db.AddRange(blogs);
            db.SaveChanges();
        }
        return "Data generation completed successfully.";
    }
}
