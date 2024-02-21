using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace OnlineSaleSiteAuth.Models.TinyMce
{
    public class TinyMCE
    {
        [AllowHtml]
        public string HtmlContent { get; set; }
    }
}
