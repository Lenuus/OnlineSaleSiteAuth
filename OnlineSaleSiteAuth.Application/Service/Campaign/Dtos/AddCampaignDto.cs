using OnlineSaleSiteAuth.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSaleSiteAuth.Application.Service.Campaign.Dtos
{
    public class AddCampaignDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal DiscountRate { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public List<Guid> Products { get; set; } = new List<Guid>();

    }
}
