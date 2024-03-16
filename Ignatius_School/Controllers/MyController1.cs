using Microsoft.AspNetCore.Mvc;

namespace Ignatius_School.Controllers
{
    public class MyController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
