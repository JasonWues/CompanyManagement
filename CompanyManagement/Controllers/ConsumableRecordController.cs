using Entity;
using ICompanyBll;
using Microsoft.AspNetCore.Mvc;
using Utility.ApiResult;

namespace CompanyManagement.Controllers
{
    public class ConsumableRecordController : Controller
    {
        readonly IConsumableRecordBll _iConsumableRecordBll;
        public ConsumableRecordController(IConsumableRecordBll iConsumableRecordBll)
        {
            _iConsumableRecordBll = iConsumableRecordBll;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateView()
        {
            return View();
        }
    }
}
