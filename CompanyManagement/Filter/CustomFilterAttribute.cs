using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CompanyManagement.Filter
{
    public class CustomFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string jsonUserInfo = context.HttpContext.Session.GetString("UserInfo");
            if (string.IsNullOrEmpty(jsonUserInfo))
            {
                context.Result = new RedirectResult("/Login/Login");
            }
        }
    }
}
