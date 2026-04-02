using Microsoft.AspNetCore.Mvc;

namespace VitacoreTestApp.Controllers
{
    public class HomeViewController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
