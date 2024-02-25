using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace OnlineSaleSiteAuth.Application.Service.CustomPage.Dtos
{
    public class CreateCustomPageDto
    {
        [AllowHtml]
        public string Title { get; set; }
        [AllowHtml]
        public string HtmlContent { get; set; }
    }
}
