using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;

namespace VitacoreTestApp.Controllers
{
   
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("Home")]
        public IActionResult Index()
        {
            return View(new Views_HomeView());
        }
    }
}
