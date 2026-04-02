using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RS1_2024_25.API.Data;

Console.WriteLine("Migracija starih blog slika...");

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory()) 
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var connectionString = config.GetConnectionString("db1");

var options = new DbContextOptionsBuilder<ApplicationDbContext>()
    .UseSqlServer(connectionString) 
    .Options;

using var db = new ApplicationDbContext(options);

var blogsWithImages = await db.BlogPosts
    .Where(b => b.Image != null)
    .ToListAsync();

var productsWithImages = await db.Products
    .Where(p => p.Image != null)

    .ToListAsync();
var exeFolder = AppContext.BaseDirectory; 
var solutionFolder = Directory.GetParent(exeFolder)!.Parent!.Parent!.Parent!.Parent!.FullName;

var apiProjectFolder = Path.Combine(solutionFolder, "RS1_2024_25.API");
var uploadsFolder = Path.Combine(apiProjectFolder, "wwwroot", "blogs");

if (!Directory.Exists(uploadsFolder))
    Directory.CreateDirectory(uploadsFolder);

foreach (var blog in blogsWithImages)
{
    var fileName = $"{blog.Id}_{Guid.NewGuid()}.jpg";
    var filePath = Path.Combine(uploadsFolder, fileName);

    await File.WriteAllBytesAsync(filePath, blog.Image);

    blog.ImagePath = $"/blogs/{fileName}";
    blog.Image = null;
}

await db.SaveChangesAsync();

Console.WriteLine("Migracija završena!");


Console.WriteLine("Migracija starih slika proizvoda...");


var productsFolder = Path.Combine(apiProjectFolder, "wwwroot", "images", "products");
if (!Directory.Exists(productsFolder))
    Directory.CreateDirectory(productsFolder);

foreach (var product in productsWithImages)
{
    var fileName = $"{product.Id}_{Guid.NewGuid()}.jpg";
    var filePath = Path.Combine(productsFolder, fileName);
    await File.WriteAllBytesAsync(filePath, product.Image);
    product.ImagePath = $"/images/products/{fileName}";
    product.Image = null;
}

await db.SaveChangesAsync();
Console.WriteLine("Migracija slika proizvoda završena!");