using Microsoft.AspNetCore.Identity;
using RS1_2024_25.API.Data.Models.Modul1_Auth;
using System.ComponentModel.DataAnnotations;

namespace RS1_2024_25.API.Data.Models
{
    public class User:IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]

        public string LastName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now; 
        
        /*
     
     Ako sistem nije zamišljen da podržava česte promjene rola i 
     ako se dodavanje novih rola svodi na manje promjene u kodu, 
    tada može biti dovoljno koristiti boolean polja kao što su IsAdmin, IsManager itd. 
    
    Ovaj pristup je jednostavan i efektivan u situacijama gdje su role stabilne i unaprijed definirane.

    Međutim, glavna prednost korištenja role entiteta dolazi do izražaja kada aplikacija potencijalno raste i 
    zahtjeva kompleksnije role i ovlaštenja. U scenarijima gdje se očekuje veći broj različitih rola ili kompleksniji 
    sistem ovlaštenja, dodavanje nove bool varijable može postati nepraktično i otežati održavanje.

    Dakle, za stabilne sisteme s manjim brojem fiksnih rola, boolean polja su sasvim razumno rješenje.
     */
        // Number of failed login attempts
        public int FailedLoginAttempts { get; set; } = 0;

        // Timestamp for when the account might be locked (optional)
        public DateTime? LockoutUntil { get; set; }

        public void SetPassword(string password)
        {
            var hasher = new PasswordHasher<User>();
            PasswordHash = hasher.HashPassword(this, password);
        }


        public bool VerifyPassword(string password)
        {
            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(this, PasswordHash, password);
            if (result == PasswordVerificationResult.Success)
            {
             
                FailedLoginAttempts = 0;
                LockoutUntil = null; 
                return true;
            }
            else
            {
                
                FailedLoginAttempts++;
                return false;
            }
        }

      
        public bool IsLocked()
        {
            return LockoutUntil.HasValue && LockoutUntil.Value > DateTime.UtcNow;
        }

 
        public void LockAccount(int minutes)
        {
            LockoutUntil = DateTime.UtcNow.AddMinutes(minutes);
        }

    }
}
