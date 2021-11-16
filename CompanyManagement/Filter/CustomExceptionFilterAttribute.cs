using Microsoft.AspNetCore.Mvc.Filters;

namespace CompanyManagement.Filter
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            
        }
    }
}
