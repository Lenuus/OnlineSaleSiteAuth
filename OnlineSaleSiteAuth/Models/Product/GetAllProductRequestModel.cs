namespace OnlineSaleSiteAuth.Models.Product
{
    public class GetAllProductRequestModel
    {
        public string Search { get; set; }
        public List<Guid> Categories { get; set; } = new List<Guid>();
    }
}
