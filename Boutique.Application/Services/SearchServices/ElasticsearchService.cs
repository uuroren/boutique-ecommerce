using Boutique.Domain.Entities;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Application.Services.SearchServices {
    public class ElasticsearchService:IElasticsearchService {
        private readonly ElasticsearchClient _elasticClient;

        public ElasticsearchService(ElasticsearchClient elasticClient) {
            _elasticClient = elasticClient;
        }

        public async Task IndexProductAsync(Product product) {
            var response = await _elasticClient.IndexAsync(product,idx => idx.Index("products"));
            if(!response.IsValidResponse) {
                return;
            }
        }

        public async Task IndexCategoryAsync(Category category) {
            var response = await _elasticClient.IndexAsync(category,idx => idx.Index("categories"));
            if(!response.IsValidResponse) {
                return;
            }
        }

        public async Task<IEnumerable<Product>> SearchProductsAsync(string query) {
            var searchResponse = await _elasticClient.SearchAsync<Product>(s => s
                .Index("products")
                .From(0)
                .Size(100)
                .Query(q => q
                    .Bool(b => b
                        .Should(
                            bs => bs
                                .MultiMatch(mm => mm
                                    .Fields(new[] { "name","description","productCode" })
                                    .Query(query)
                                    .Fuzziness(new Elastic.Clients.Elasticsearch.Fuzziness("Auto"))
                                    .PrefixLength(1)
                                    .MinimumShouldMatch("2<100%")
                                ),
                            bs => bs
                                .Wildcard(w => w
                                    .Field("name")
                                    .Value($"*{query}*")
                                ),
                            bs => bs
                                .Wildcard(w => w
                                    .Field("description")
                                    .Value($"*{query}*")
                                ),
                            bs => bs
                                .Wildcard(w => w
                                    .Field("tags")
                                    .Value($"*{query}*")
                                ),
                            bs => bs
                                .Wildcard(w => w
                                    .Field("productCode")
                                    .Value($"*{query}*")
                                ),
                            bs => bs
                                .Nested(n => n
                                    .Path(p => p.Variants)
                                    .Query(nq => nq
                                        .Bool(nb => nb
                                            .Should(
                                                nm => nm
                                                    .Match(m => m
                                                        .Field("variants.color")
                                                        .Query(query)
                                                        .Fuzziness(new Elastic.Clients.Elasticsearch.Fuzziness("Auto"))
                                                        .PrefixLength(1)
                                                    ),
                                                nm => nm
                                                    .Wildcard(w => w
                                                        .Field("variants.color")
                                                        .Value($"*{query}*")
                                                    )
                                            )
                                        )
                                    )
                                )
                        )
                    )
                )
            );

            if(searchResponse == null) {
                Console.WriteLine("Search response is null");
                return Enumerable.Empty<Product>();
            }

            if(!searchResponse.IsValidResponse) {
                Console.WriteLine("Search failed. Error:");
                Console.WriteLine(searchResponse.DebugInformation);
                return Enumerable.Empty<Product>();
            }

            Console.WriteLine("Search was successful. Response:");
            Console.WriteLine(searchResponse.DebugInformation);

            return searchResponse.Documents;
        }




        public async Task<IEnumerable<Category>> SearchCategoriesAsync(string query) {
            var searchResponse = await _elasticClient.SearchAsync<Category>(s => s
                .Index("categories")
                .Query(q => q
                    .QueryString(qs => qs
                        .Query(query)
                    )
                )
            );

            return searchResponse.Documents;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync() {
            var searchResponse = await _elasticClient.SearchAsync<Product>(s => s
            .Index("products")
            .Query(q => q
                .MatchAll(new Elastic.Clients.Elasticsearch.QueryDsl.MatchAllQuery() { })
            )
            .Size(1000)
        );

            return searchResponse.Documents;
        }

        public async Task DeleteProductFromIndexAsync(string productId) {
            var response = await _elasticClient.DeleteAsync<Product>(productId,idx => idx.Index("products"));
            if(!response.IsValidResponse) {
                return;
            }
        }
    }

}
