using Microsoft.AspNetCore.Mvc.Filters;
using SchoolProject.Service.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Filters
{
    public class AuthenticationFilter : IAsyncActionFilter
    {
        private readonly ICurrentUserService _currentUserService;
        public AuthenticationFilter(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                var roles = await _currentUserService.GetCurrentUserRolesAsync();
                if (!roles.Contains("Instructor"))
                {
                    context.HttpContext.Response.StatusCode = 403; // Forbidden
                    return;
                }
                else
                {
                    await next();
                }
            }
        }
    }
}
