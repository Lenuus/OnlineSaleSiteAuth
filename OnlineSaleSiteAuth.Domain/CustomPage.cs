using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace OnlineSaleSiteAuth.Domain
{
    public class CustomPage : IBaseEntity, ISoftDeletable
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string HtmlContent { get; set; }

        public bool IsDeleted { get; set; }
    }
}
