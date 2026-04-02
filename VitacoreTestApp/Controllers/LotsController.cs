using Microsoft.AspNetCore.Mvc;
using VitacoreTestApp.ViewModels;

namespace VitacoreTestApp.Controllers
{
    public class LotsController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View(new LotDetailsViewModel());
        }
    }
}