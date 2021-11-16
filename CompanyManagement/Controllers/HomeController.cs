using CompanyManagement.Filter;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManagement.Controllers
{
    public class HomeController : Controller
    {
        [CustomFilter]
        public IActionResult Index()
        {
            return View();
        }
    }
}
