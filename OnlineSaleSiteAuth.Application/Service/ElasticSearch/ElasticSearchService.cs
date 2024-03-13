using Microsoft.Extensions.Configuration;
using Nest;
using OnlineSaleSiteAuth.Application.Service.Product.Dtos;
using System;

namespace OnlineSaleSiteAuth.Application.Service.ElasticSearch
{
    public class ElasticSearchService : IElasticSearchService
    {
        private readonly IConfiguration _configuration;
        private readonly ElasticClient _elasticClient;

        public ElasticSearchService(IConfiguration configuration)
        {
            _configuration = configuration;

            var settings = new ConnectionSettings(new Uri(_configuration["Elasticsearch:Uri"]))
                .DefaultIndex(_configuration["Elasticsearch:DefaultIndex"]);

            _elasticClient = new ElasticClient(settings);
        }

        public bool CreateIndex(string indexName)
        {
            try
            {
                var createIndexResponse = _elasticClient.Indices.Create(indexName, c => c
                    .Settings(s => s
                        .NumberOfShards(1)
                        .NumberOfReplicas(1)
                    )
                );

                return createIndexResponse.IsValid;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Index oluşturma hatası: {ex.Message}");
                return false;
            }
        }
        public async Task<ISearchResponse<ProductListDto>> SearchProducts(GetAllProductRequestDto request)
        {
            var searchResponse = await _elasticClient.SearchAsync<ProductListDto>(s => s
                .Index("products")
                .Query(q => q
                    .Match(m => m.Field(f => f.Name).Query(request.Search))
                )
                .Size(request.PageSize)
                .From(request.PageIndex * request.PageSize)
            );

            return searchResponse;
        }
    }

}
