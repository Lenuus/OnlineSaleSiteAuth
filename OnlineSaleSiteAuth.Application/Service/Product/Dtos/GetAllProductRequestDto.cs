using OnlineSaleSiteAuth.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSaleSiteAuth.Application.Service.Product.Dtos
{
    public class GetAllProductRequestDto : PagedRequestDto
    {
        public string Search { get; set; }
        public List<Guid> Categories { get; set; } = new List<Guid>();
    }
}
