using System.ComponentModel.DataAnnotations;

namespace OnlineSaleSiteAuth.Models.Campaign
{
    public class AddCampaignModel
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
