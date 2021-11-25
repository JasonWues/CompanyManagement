using Microsoft.AspNetCore.Mvc;

namespace CompanyManagement.Controllers
{
    public class WorkFlowInstanceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateView()
        {
            return View();
        }

        //public Task<IActionResult> Create(int modelId,int status,string description,string reason,string creator,string outNum,string outGoodsId)
        //{

        //}
    }
}
