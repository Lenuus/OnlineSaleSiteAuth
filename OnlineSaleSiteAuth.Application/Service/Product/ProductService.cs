using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IRepository<Domain.Image> _imageRepository;
        private readonly IRepository<Domain.ProductCategory> _productCategoryRepository;

        public ProductService(
            IRepository<Domain.Product> showRepository,
            IMapper mapper,
            IFileService fileService,
            IRepository<Domain.Image> imageRepository,
            IRepository<ProductCategory> productCategoryRepository)
        {
            _productRepository = showRepository;
            _mapper = mapper;
            _fileService = fileService;
            _imageRepository = imageRepository;
            _productCategoryRepository = productCategoryRepository;
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
            productEntity.Categories.AddRange(request.Categories.Select(f => new ProductCategory { CategoryId = f }));
            await _productRepository.Create(productEntity).ConfigureAwait(false);
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
            return new ServiceResponse();
        }

        public async Task<ServiceResponse<PagedResponseDto<ProductListDto>>> GetAllProducts(GetAllProductRequestDto request)
        {
            var query = await _productRepository.GetAll()
                .Include(f => f.Images)
                .Include(f => f.Categories).ThenInclude(f => f.Category).Include(f=>f.Campaigns).ThenInclude(f=>f.Campaign)
                .Where(x => !x.IsDeleted &&
                            (!string.IsNullOrEmpty(request.Search) ? x.Name.Contains(request.Search) : true) &&
                            (request.Categories.Any() ? request.Categories.Any(y => x.Categories.Any(k => k.CategoryId == y && !k.IsDeleted)) : true))
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
                    Campaigns=f.Campaigns.Where(c => !c.IsDeleted && c.Campaign.StartDate<=DateTime.UtcNow && c.Campaign.EndDate>= DateTime.UtcNow).Select(ca=> new ProductListCampaignDto
                    {
                        DiscountRate=ca.Campaign.DiscountRate,
                        DiscountedPrice=f.Price-((ca.Campaign.DiscountRate*f.Price)/100),
                     
                    }).ToList(),
                    Price = f.Price,
                    Stock = f.Stock,
                }).ToPagedListAsync(request.PageSize, request.PageIndex).ConfigureAwait(false);
            return new ServiceResponse<PagedResponseDto<ProductListDto>>(query);
        }

        public async Task<ServiceResponse<ProductListDto>> GetProductById(Guid id)
        {
            var query = await _productRepository.GetAll()
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
    }
}
