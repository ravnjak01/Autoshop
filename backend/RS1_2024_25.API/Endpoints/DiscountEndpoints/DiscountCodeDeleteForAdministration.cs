using Microsoft.AspNetCore.Mvc;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Helper.Api;

namespace RS1_2024_25.API.Endpoints.DiscountEndpoints
{
    [Route("delete-discount-code")]
    public class DiscountCodeDeleteForAdministration(ApplicationDbContext db) : MyEndpointBaseAsync
    .WithRequest<int>
    .WithoutResult
    {

        [HttpDelete("{id}")]
        public override async Task HandleAsync(int id, CancellationToken cancellationToken = default)
        {
            var code = await db.DiscountCodes.FindAsync(new object[] { id }, cancellationToken);
            if (code == null) throw new KeyNotFoundException("Discount code not found");
            db.DiscountCodes.Remove(code);
            await db.SaveChangesAsync(cancellationToken);
        }
    }
}
