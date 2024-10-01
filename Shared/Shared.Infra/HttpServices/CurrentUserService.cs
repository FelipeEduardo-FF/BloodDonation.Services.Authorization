using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Shared.Infra.HttpServices
{
    public class CurrentUserService : ICurrentUserService
    {

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            var HttpContext = httpContextAccessor.HttpContext;

            if (HttpContext is not null && HttpContext.User is not null && HttpContext.User.Identity.IsAuthenticated)
            {
                 Name = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
                 Id = HttpContext.User.FindFirst(ClaimTypes.Sid)!.Value;
            }


        }

        public string? Name { get; }
        public string? Id { get; }


    }
}
