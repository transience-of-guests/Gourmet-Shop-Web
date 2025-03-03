using Microsoft.AspNetCore.Mvc;

namespace Admin.WebApp.Controllers
{
    public class Suppliers : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
