using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSaleSiteAuth.Domain
{
    public class Image : IBaseEntity, ISoftDeletable
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
        public string Path { get; set; }
        public bool IsDeleted { get; set; }
    }
}
