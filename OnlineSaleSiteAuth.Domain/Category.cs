using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSaleSiteAuth.Domain
{
    public class Category:IBaseEntity,ISoftDeletable
    {
        public Guid Id { get; set; }
        [Required]

        public string Name { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<ProductCategory> Products { get; set; }
    }
}
