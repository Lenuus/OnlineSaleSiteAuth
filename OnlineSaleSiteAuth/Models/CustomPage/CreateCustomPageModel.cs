using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace OnlineSaleSiteAuth.Models.CustomPage
{
    public class CreateCustomPageModel
    {
        public Guid Id { get; set; }
        [AllowHtml]
        public string Title { get; set; }
        [AllowHtml]
        public string HtmlContent { get; set; }
    }
}
