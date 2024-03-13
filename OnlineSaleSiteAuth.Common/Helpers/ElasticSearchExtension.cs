using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using OnlineSaleSiteAuth.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSaleSiteAuth.Common.Helpers
{
    public static class ElasticSearchExtension
    {
        public static void AddElasticSearch(this IServiceCollection services, IConfiguration configuration)
        {
            var url = configuration["Elasticsearch:Uri"]!;
            var pool = new SingleNodeConnectionPool(new Uri(url));
            var settings = new ConnectionSettings(new Uri(url))
                .DefaultIndex("product")
                .DefaultMappingFor<Domain.Product>(m => m.IndexName("product"))
                .RequestTimeout(TimeSpan.FromMinutes(2));

            var client = new ElasticClient(settings);
            services.AddSingleton(client);
        }
    }
}

//var url = configuration["Elasticsearch:Uri"]!; 
//var pool = new SingleNodeConnectionPool(new Uri(url));
//var settings = new ConnectionSettings(pool)
//    .EnableApiVersioningHeader()
//    .DefaultIndex("Product");
//var client = new ElasticClient(settings);
//services.AddSingleton(client);