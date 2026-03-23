using RS1_2024_25.API.Data.DTOs;
using System.Threading.Tasks;
using System;

namespace RS1_2024_25.API.Services
{

    public interface ICartService
    {
        
   

        // Metoda za spajanje anonimne (guest) korpe sa trajnom korpom prijavljenog korisnika.
        Task MergeGuestCartWithUser(string? userId, string? guestSessionId,CancellationToken cancellationToken);

    }
}