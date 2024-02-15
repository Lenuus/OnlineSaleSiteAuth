namespace OnlineSaleSiteAuth.Models.Product
{
    public class GetAllProductRequestModel:PagedRequestModel
    {
        public string Search { get; set; }
        public List<Guid> Categories { get; set; } = new List<Guid>();
    }
}
