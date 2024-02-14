using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;


namespace OnlineSaleSiteAuth.Application.Service.Claim
{
    public class ClaimManager:IClaimManager
    {
        private readonly IHttpContextAccessor _context;

        public ClaimManager(IHttpContextAccessor context)
        {
            _context = context;
        }

        public IEnumerable<System.Security.Claims.Claim> GetClaims()
        {
            return _context.HttpContext.User.Claims;
        }

        public string GetEmail()
        {
            return GetClaims().FirstOrDefault(f => f.Type == ClaimTypes.Email).Value;
        }

        public string GetRole()
        {
            return GetClaims().FirstOrDefault(f=>f.Type==ClaimTypes.Role).Value;
        }

        public Guid GetUserId()
        {
            var userIdStr=GetClaims().FirstOrDefault(f => f.Type == ClaimTypes.Upn).Value;
            if (string.IsNullOrEmpty(userIdStr))
            {
                return Guid.Empty;
            }

            return new Guid(userIdStr);
        }
    }
}
