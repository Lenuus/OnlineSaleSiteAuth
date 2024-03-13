using Nest;
using OnlineSaleSiteAuth.Application.Service.Product.Dtos;

namespace OnlineSaleSiteAuth.Application.Service.ElasticSearch
{
    public interface IElasticSearchService
    {
        bool CreateIndex(string indexName);

        Task<ISearchResponse<ProductListDto>> SearchProducts(GetAllProductRequestDto request);



    }
}