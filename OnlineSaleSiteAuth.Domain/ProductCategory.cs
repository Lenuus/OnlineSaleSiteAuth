using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSaleSiteAuth.Domain
{
    public class ProductCategory : IBaseEntity, ISoftDeletable
    {
        public Guid Id { get; set; }
        [Required]

        public Guid ProductId { get; set; }

        public Product Product { get; set; }
        [Required]

        public Guid CategoryId { get; set; }

        public Category Category { get; set; }

        public bool IsDeleted { get; set; }

    }
}
