using Microsoft.AspNetCore.Mvc;

[Route("")]
public class HomeController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return Content("Dobrodošli na backend!");
    }
}
