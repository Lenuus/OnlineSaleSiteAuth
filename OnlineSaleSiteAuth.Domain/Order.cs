using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineSaleSiteAuth.Domain
{
    public class Order : IBaseEntity, ISoftDeletable
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public User User { get; set; }

        public decimal TotalAmount { get; set; }

        public string Adress { get; set; }

        public DateTime OrderDate { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }

        public bool IsDeleted { get; set; }
    }
}
