using Microsoft.AspNetCore.Mvc;

namespace VSFlyWebsite.Controllers
{
    public class StatisticsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
