using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSaleSiteAuth.Application.Service.CustomPage.Dtos
{
    public class CustomPageDetailDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public string HtmlContent { get; set; }
    }
}
