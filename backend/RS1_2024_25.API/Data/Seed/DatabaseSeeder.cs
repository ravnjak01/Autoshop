using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data.Models.ShoppingCart;

namespace RS1_2024_25.API.Data.Seed
{
    public class DatabaseSeeder
    {
        public static async Task SeedCategoriesAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();



            var categories = new List<Category>
                {
                    new Category { Name = "Engine parts",Description="Essential components that ensure your engine runs smoothly and efficiently. Find everything from filters and spark plugs to belts and timing components for peak performance." ,Code="ENGP"},
                    new Category { Name = "Brake system",Description="Your ultimate source for vehicle safety. We stock high-quality pads, rotors, and calipers to ensure reliable and responsive stopping power every time." ,Code="BRK"},
                    new Category { Name = "Fluids and chemicals",Description="Maintain and protect your vehicle with our wide selection of essential oils, coolants, and specialized additives. Keep your systems lubricated, clean, and running longer." ,Code="CHEM"},

                };


            foreach (var category in categories)
            {
                if (!await context.Categories.AnyAsync(c => c.Name == category.Name))
                {

                    await context.Categories.AddAsync(category);
                }
              
                    await context.SaveChangesAsync();


            }
        }
    }
}