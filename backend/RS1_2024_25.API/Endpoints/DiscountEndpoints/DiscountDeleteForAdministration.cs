using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models.Modul2_Basic;
using RS1_2024_25.API.Helper.Api;

namespace RS1_2024_25.API.Endpoints.DiscountEndpoints
{
    [Route("delete-discount")]
    public class DiscountDeleteForAdministration(ApplicationDbContext db) : MyEndpointBaseAsync
    .WithRequest<int>
    .WithoutResult
    {

        [HttpDelete("{id}")]
        public override async Task HandleAsync(int id, CancellationToken cancellationToken = default)
        {
            var discount = await db.Discounts.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (discount == null)
                throw new KeyNotFoundException("Discount not found");

            db.Remove(discount);
            await db.SaveChangesAsync(cancellationToken);
        }
    }
}
