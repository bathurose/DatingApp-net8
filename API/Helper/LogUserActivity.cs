using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Helper
{
    public class LogUserActivity : IAsyncActionFilter
    {
        // compily api before and after
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //action before
            var resultContext = await next();
            //action after 

            if (context.HttpContext.User.Identity?.IsAuthenticated != true) return;

            var userid = resultContext.HttpContext.User.GetUseId();

            var unitOfWork = resultContext.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();

            var user = await unitOfWork.UserRepository.GetUserByIdAsync(userid);

            if (user == null) return;

            user.LastActive = DateTime.Now;

            await unitOfWork.Complete();
        }
    }
}
