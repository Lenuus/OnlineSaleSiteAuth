using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSaleSiteAuth.Domain
{
    public class User : IdentityUser<Guid>, IBaseEntity, ISoftDeletable
    {
       
        public ICollection<Order> Order { get; set; }
        public bool IsDeleted { get; set; }

    }
}
