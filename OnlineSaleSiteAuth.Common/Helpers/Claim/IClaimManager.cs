using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

namespace OnlineSaleSiteAuth.Application.Service.Claim
{
    public interface IClaimManager
    {
        IEnumerable<System.Security.Claims.Claim> GetClaims();

        string GetEmail();

        Guid GetUserId();

        string GetRole();
    }
}
