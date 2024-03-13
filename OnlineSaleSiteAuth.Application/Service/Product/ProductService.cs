using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Nest;
using OnlineSaleSiteAuth.Application.Service.Category.Dtos;
using OnlineSaleSiteAuth.Application.Service.File;
using OnlineSaleSiteAuth.Application.Service.Product.Dtos;
using OnlineSaleSiteAuth.Common.Dtos;
using OnlineSaleSiteAuth.Common.Helpers;
using OnlineSaleSiteAuth.Domain;
using System;


namespace OnlineSaleSiteAuth.Application.Service.Product
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Domain.Product> _productRepository;
        private readonly IRepository<Domain.Category> _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IRepository<Domain.Image> _imageRepository;
        private readonly IRepository<Domain.ProductCategory> _productCategoryRepository;
        private readonly ElasticClient _elasticClient;

        public ProductService(
            IRepository<Domain.Product> showRepository,
            IMapper mapper,
            IFileService fileService,
            IRepository<Domain.Image> imageRepository,
            IRepository<ProductCategory> productCategoryRepository,
            ElasticClient elasticClient,
            IRepository<Domain.Category> categoryRepository)
        {
            _productRepository = showRepository;
            _mapper = mapper;
            _fileService = fileService;
            _imageRepository = imageRepository;
            _productCategoryRepository = productCategoryRepository;
            _elasticClient = elasticClient;
            _categoryRepository = categoryRepository;
        }

        public async Task<ServiceResponse> AddProduct(AddProductRequestDto request)
        {
            var productEntity = _mapper.Map<Domain.Product>(request);
            if (request.Images != null && request.Images.Any())
            {
                var order = 0;
                foreach (var image in request.Images)
                {
                    var fileUploadResult = await _fileService.UploadFile(image).ConfigureAwait(false);
                    if (fileUploadResult.IsSuccessful)
                    {
                        order++;
                        productEntity.Images.Add(new Domain.Image()
                        {
                            Name = fileUploadResult.Data.Name,
                            Path = fileUploadResult.Data.Path,
                            DisplayOrder = order,
                        });

                    }
                }
            }

            var categories = new List<ProductListCategoryDto>();
            foreach (var categoryId in request.Categories)
            {
                var category = await _categoryRepository.GetById(categoryId);
                categories.Add(new ProductListCategoryDto { Id = categoryId, Name = category.Name });
            }
            productEntity.Categories.AddRange(request.Categories.Select(f => new ProductCategory { CategoryId = f }));
            await _productRepository.Create(productEntity).ConfigureAwait(false);
            await AddProductToElasticSearch(new ProductListDto
            {
                Id = productEntity.Id,
                Name = productEntity.Name,
                Price = productEntity.Price,
                Categories = categories,
                Stock = productEntity.Stock,
                Images = productEntity.Images.Select(f => new ProductListImageDto
                {
                    Id = f.Id,
                    Path = f.Path,
                }).ToList()
            });

            return new ServiceResponse();
        }

        public async Task<ServiceResponse> DeleteProduct(Guid id)
        {
            var request = await _productRepository.GetById(id).ConfigureAwait(false);
            if (request == null)
            {
                return new ServiceResponse(false, "Oge bulunamadı");
            }
            await _productRepository.DeleteById(id).ConfigureAwait(false);
            await DeleteProductFromElasticSearch(id).ConfigureAwait(false);
            return new ServiceResponse();
        }

        public async Task<ServiceResponse> UpdateProduct(UpdateProductRequestDto request)
        {
            var product = await _productRepository.GetAll()
                .Include(f => f.Categories)
                .Include(f => f.Images)
                .FirstOrDefaultAsync(f => f.Id == request.Id).ConfigureAwait(false);
            if (product == null)
            {
                return new ServiceResponse(false, "Urun Bulunamadı");
            }

            product.Price = request.Price;
            product.Stock = request.Stock;
            product.Name = request.Name;

            var willAddCategories = request.SelectedCategories.Where(f => !product.Categories.Any(y => f == y.CategoryId && !y.IsDeleted));
            var willDeleteCategories = product.Categories.Where(f => !request.SelectedCategories.Exists(y => f.CategoryId == y) && !f.IsDeleted);

            product.Categories.AddRange(willAddCategories.Select(f => new ProductCategory
            {
                CategoryId = f
            }));

            if (!willDeleteCategories.IsNullOrEmpty())
            {
                foreach (var deletedItem in willDeleteCategories)
                {
                    await _productCategoryRepository.Delete(deletedItem).ConfigureAwait(false);
                }
            }

            if (!request.NewImages.IsNullOrEmpty())
            {
                int lastOrder = 0;
                var existingImages = _imageRepository.GetAll().Where(f => f.ProductId == request.Id);
                if (existingImages.Any())
                {
                    lastOrder = existingImages.Max(f => f.DisplayOrder);
                }

                foreach (var image in request.NewImages)
                {
                    if (image.Length > 0)
                    {
                        var fileUploadResult = await _fileService.UploadFile(image).ConfigureAwait(false);
                        if (fileUploadResult.IsSuccessful)
                        {
                            product.Images.Add(new Image()
                            {
                                Name = fileUploadResult.Data.Name,
                                Path = fileUploadResult.Data.Path,
                                DisplayOrder = lastOrder + 1
                            });
                            lastOrder++;
                        }
                    }
                }
            }
            await _productRepository.Update(product).ConfigureAwait(false);
            await UpdateProductElasticSearch(request).ConfigureAwait(false);
            return new ServiceResponse();
        }

        public async Task<ServiceResponse<PagedResponseDto<ProductListDto>>> GetAllProducts(GetAllProductRequestDto request)
        {
            var categoryIds = request.Categories.Select(d => d).ToList();

            // ürün name, category
            // ürün id

            // db -> ürünler -> ürün id
            var result = _elasticClient.Search<ProductListDto>(s => s
                 .Index("product")
                 .Query(q => q
                     .Bool(b => b
                         .Must(mu => mu.MatchPhrasePrefix(m => m.Field(f => f.Name)
                                                                .Query(request.Search)),
                               mu => mu.Terms(t => t.Field(f => f.Categories.Select(c => c.Id))
                                                  .Terms(categoryIds))
                         )
                     )
                 )
                 .From(request.PageSize * request.PageIndex)
                 .Take(request.PageSize)
              ).Documents.Select(f => f.Id);

            var query = await _productRepository.GetAll()
                .Include(f => f.Images)
                .Include(f => f.Categories).ThenInclude(f => f.Category).Include(f => f.Campaigns).ThenInclude(f => f.Campaign)
                .Where(x => !x.IsDeleted &&
                #region withoutElasticSearch
                //(!string.IsNullOrEmpty(request.Search) ? x.Name.Contains(request.Search) : true) &&
                //            (request.Categories.Any() ? request.Categories.Any(y => x.Categories.Any(k => k.CategoryId == y && !k.IsDeleted)) : true))
                #endregion
                result.Contains(x.Id))
                .Select(f => new ProductListDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    Categories = f.Categories.Select(x => new ProductListCategoryDto
                    {
                        Id = x.CategoryId,
                        Name = x.Category.Name,
                    }).ToList(),

                    Images = f.Images.Where(d => !d.IsDeleted).OrderBy(d => d.DisplayOrder).Select(i => new ProductListImageDto
                    {
                        Path = i.Path
                    }).ToList(),
                    Campaigns = f.Campaigns.Where(c => !c.IsDeleted && c.Campaign.StartDate <= DateTime.UtcNow && c.Campaign.EndDate >= DateTime.UtcNow).Select(ca => new ProductListCampaignDto
                    {
                        DiscountRate = ca.Campaign.DiscountRate,
                        DiscountedPrice = f.Price - ((ca.Campaign.DiscountRate * f.Price) / 100),
                        StartDate = ca.Campaign.StartDate,
                        EndDate = ca.Campaign.EndDate,
                    }).ToList(),
                    Price = f.Price,
                    Stock = f.Stock,
                }).ToPagedListAsync(request.PageSize, request.PageIndex).ConfigureAwait(false);
            return new ServiceResponse<PagedResponseDto<ProductListDto>>(query);
        }

        public async Task<ServiceResponse<ProductListDto>> GetProductById(Guid id)
        {
            var query = await _productRepository.GetAll().Where(f => !f.IsDeleted && f.Id == id)
                .Include(f => f.Categories)
                .Include(f => f.Images)
                .Select(f => new ProductListDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    Price = f.Price,
                    Stock = f.Stock,
                    Categories = f.Categories.Where(f => !f.IsDeleted).Select(x => new ProductListCategoryDto
                    {
                        Id = x.CategoryId,
                        Name = x.Category.Name
                    }).ToList(),
                    Images = f.Images.Where(d => !d.IsDeleted).Select(x => new ProductListImageDto { Path = x.Path, Id = x.Id }).ToList()
                })
                .FirstOrDefaultAsync(f => f.Id == id).ConfigureAwait(false);
            if (query == null)
                return new ServiceResponse<ProductListDto>(null, true, string.Empty);

            return new ServiceResponse<ProductListDto>(query);
        }

        public async Task<ServiceResponse> DeleteImage(Guid deleteImageId)
        {
            await _imageRepository.DeleteById(deleteImageId).ConfigureAwait(false);
            return new ServiceResponse();
        }

        public async Task<ServiceResponse<List<ProductListDto>>> GetAllProductIds()
        {
            var query = _productRepository.GetAll().Where(f => !f.IsDeleted).Select(x => new ProductListDto
            {
                Id = x.Id,
                Name = x.Name,
            }).ToList();
            if (query == null)
            {
                return new ServiceResponse<List<ProductListDto>>(null, false, "Not Found List");
            }
            return new ServiceResponse<List<ProductListDto>>(query, true, string.Empty);
        }

        private async Task<ServiceResponse> AddProductToElasticSearch(ProductListDto product)
        {
            if (!(await CheckProductIndexOrUpdate()).IsSuccessful)
            {
                return new ServiceResponse { IsSuccessful = false, ErrorMessage = "Product Index can not be created in Elastic Search" };
            }

            var result = await _elasticClient.BulkAsync(b => b
                  .Index("product")
                  .IndexMany(new List<ProductListDto>() { product })).ConfigureAwait(false);
            return new ServiceResponse { IsSuccessful = true };
        }

        private async Task<ServiceResponse> UpdateProductElasticSearch(UpdateProductRequestDto request)
        {
            var searchResponse = await _elasticClient.SearchAsync<ProductListDto>(s => s
                .Query(q => q
                    .Term(t => t
                        .Field(f => f.Id)
                        .Value(request.Id)
                    )
                )
            );

            if (searchResponse.IsValid && searchResponse.Documents != null && searchResponse.Documents.Any())
            {
                var existingDocument = searchResponse.Documents.First();
                existingDocument.Name = request.Name;
                existingDocument.Price = request.Price;
                existingDocument.Stock = request.Stock;

                existingDocument.Categories = request.SelectedCategories.Select(categoryId => new ProductListCategoryDto
                {
                    Id = categoryId,
                }).ToList();

                var updateResponse = await _elasticClient.UpdateAsync<ProductListDto>(existingDocument.Id, u => u.Doc(existingDocument));

                if (updateResponse.IsValid)
                {
                    return new ServiceResponse { IsSuccessful = true };
                }
                else
                {
                    return new ServiceResponse { IsSuccessful = false, ErrorMessage = "Elasticsearch'te belge güncellenemedi." };
                }
            }
            else
            {
                return new ServiceResponse { IsSuccessful = false, ErrorMessage = "Elasticsearch'te güncellenecek belge bulunamadı." };
            }
        }

        private async Task<ServiceResponse> CheckProductIndexOrUpdate()
        {
            if (!(await _elasticClient.Indices.ExistsAsync("product")).Exists)
            {
                var result = await _elasticClient.Indices.CreateAsync("product",
                     index => index.Map<ProductListDto>(
                          x => x
                         .AutoMap()
                  )).ConfigureAwait(false);
            }

            return new ServiceResponse { IsSuccessful = true };
        }

        private async Task<ServiceResponse> DeleteProductFromElasticSearch(Guid id)
        {
            var response = await _elasticClient.DeleteAsync<ProductListDto>(id, d => d.Index("product"));

            if (!response.IsValid)
            {
                return new ServiceResponse { IsSuccessful = false, ErrorMessage = "Failed to delete product from Elasticsearch" };
            }

            return new ServiceResponse { IsSuccessful = true };
        }


    }
}
