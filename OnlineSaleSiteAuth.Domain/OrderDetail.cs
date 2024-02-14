using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSaleSiteAuth.Domain
{
    public class OrderDetail : IBaseEntity, ISoftDeletable
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }

        public Order Order { get; set; }

        public Guid ProductId { get; set; }

        public Product Product { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public bool IsDeleted { get; set; }


    }
}
