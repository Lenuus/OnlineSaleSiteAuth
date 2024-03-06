using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using OnlineSaleSiteAuth.Application.Service.Basket.Dtos;
using OnlineSaleSiteAuth.Application.Service.Claim;
using OnlineSaleSiteAuth.Application.Service.Product.Dtos;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace OnlineSaleSiteAuth.Application.Service.Basket
{
    /// <summary>
    ///  If you prefer not to use Redis and want to use Sessions instead, you can simply uncomment the regions below and use the modified code.
    /// </summary>
    public class BasketService : IBasketService
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;
        private readonly IClaimManager _claimManager;
        private readonly IRepository<Domain.Product> _productRepository;
        private readonly IDistributedCache _redisCache;


        public BasketService(IHttpContextAccessor httpContext, IClaimManager claimManager, IMapper mapper, IRepository<Domain.Product> productRepository, IDistributedCache redisCache)
        {
            _httpContext = httpContext;
            _claimManager = claimManager;
            _mapper = mapper;
            _productRepository = productRepository;
            _redisCache = redisCache;
        }
        //#region Seassion AddToBasket
        //public async Task<ServiceResponse> AddToBasket(Guid productId, int quantity = 1)
        //{
        //    var userId = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    var basketKey = $"Basket_{userId}";
        //    byte[] basket = null;
        //    var product = await _productRepository.GetById(productId).ConfigureAwait(false);
        //    if (!_httpContext.HttpContext.Session.TryGetValue(basketKey, out basket))
        //    {
        //        var newBasket = new List<BasketListSessionDto>();
        //        newBasket.Add(new BasketListSessionDto
        //        {
        //            ProductId = productId,
        //            Quantity = quantity
        //        });
        //        _httpContext.HttpContext.Session.Set(basketKey, Encoding.UTF8.GetBytes(JsonSerializer.Serialize(newBasket)));
        //    }
        //    else
        //    {
        //        var existedBasket = JsonSerializer.Deserialize<List<BasketListSessionDto>>(Encoding.UTF8.GetString(basket))!;
        //        if (existedBasket.Exists(f => f.ProductId == productId))
        //        {
        //            var existingQuantity = existedBasket.First(f => f.ProductId == productId).Quantity;

        //            if (existingQuantity < product.Stock)
        //            {
        //                existedBasket.FirstOrDefault(f => f.ProductId == productId)!.Quantity += quantity;
        //            }
        //            else
        //            {
        //                return new ServiceResponse(false, "Stocks are limited");

        //            }
        //        }
        //        else
        //        {
        //            existedBasket.Add(new BasketListSessionDto
        //            {
        //                ProductId = productId,
        //                Quantity = quantity
        //            });
        //        }
        //        _httpContext.HttpContext.Session.Set(basketKey, Encoding.UTF8.GetBytes(JsonSerializer.Serialize(existedBasket)));
        //    }
        //    return new ServiceResponse(true, string.Empty);
        //}
        //#endregion

        public async Task<ServiceResponse> AddToBasket(Guid productId, int quantity = 1)
        {
            var userId = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                userId = await _redisCache.GetStringAsync("TemporaryUserId");
                if (string.IsNullOrEmpty(userId))
                {
                    userId = Guid.NewGuid().ToString();
                    await _redisCache.SetStringAsync("TemporaryUserId", userId).ConfigureAwait(false);
                }
            }
            var basketKey = $"Basket_{userId}";
            string basket = await _redisCache.GetStringAsync(basketKey);
            var product = await _productRepository.GetById(productId).ConfigureAwait(false);
            if (basket == null)
            {
                var newBasket = new List<BasketListSessionDto>();
                newBasket.Add(new BasketListSessionDto
                {
                    ProductId = productId,
                    Quantity = quantity
                });
                await _redisCache.SetStringAsync(basketKey, JsonSerializer.Serialize(newBasket)).ConfigureAwait(false);
            }
            else
            {
                var existedBasket = JsonSerializer.Deserialize<List<BasketListSessionDto>>(basket)!;
                if (existedBasket.Exists(f => f.ProductId == productId))
                {
                    var existingQuantity = existedBasket.First(f => f.ProductId == productId).Quantity;

                    if (existingQuantity < product.Stock)
                    {
                        existedBasket.FirstOrDefault(f => f.ProductId == productId)!.Quantity += quantity;
                    }
                    else
                    {
                        return new ServiceResponse(false, "Stocks are limited");

                    }
                }
                else
                {
                    existedBasket.Add(new BasketListSessionDto
                    {
                        ProductId = productId,
                        Quantity = quantity
                    });
                }
                await _redisCache.SetStringAsync(basketKey, JsonSerializer.Serialize(existedBasket)).ConfigureAwait(false);
            }
            return new ServiceResponse(true, string.Empty);
        }
        //#region SessionRemove
        //public async Task<ServiceResponse> RemoveFromBasket(Guid productId, int quantity = 1)
        //{
        //    var userId = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //    var basketKey = $"Basket_{userId}";
        //    byte[] basket = null;
        //    if (!_httpContext.HttpContext.Session.TryGetValue(basketKey, out basket))
        //    {
        //        return new ServiceResponse(false, "Basket Not Found");
        //    }
        //    else
        //    {
        //        var existedBasket = JsonSerializer.Deserialize<List<BasketListSessionDto>>(Encoding.UTF8.GetString(basket))!;
        //        var existedItem = existedBasket.FirstOrDefault(b => b.ProductId == productId);
        //        if (existedItem != null)
        //        {
        //            existedItem.Quantity -= quantity;
        //            if (existedItem.Quantity <= 0)
        //            {
        //                existedBasket.Remove(existedItem);
        //            }
        //        }
        //        _httpContext.HttpContext.Session.Set(basketKey, Encoding.UTF8.GetBytes(JsonSerializer.Serialize(existedBasket)));
        //    }
        //    return new ServiceResponse();
        //}
        //#endregion

        //public async Task<ServiceResponse<List<BasketListDto>>> GetAllBasket()
        //{
        //    var userId = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    var basketKey = $"Basket_{userId}";
        //    byte[] basket = null;
        //    if (!_httpContext.HttpContext.Session.TryGetValue(basketKey, out basket))
        //    {
        //        return new ServiceResponse<List<BasketListDto>>(null, false, "Not Found");
        //    }
        //    else
        //    {
        //        var existedBasket = JsonSerializer.Deserialize<List<BasketListSessionDto>>(Encoding.UTF8.GetString(basket))!;
        //        if (existedBasket == null)
        //        {
        //            return new ServiceResponse<List<BasketListDto>>(null, false, "Basket is empty");
        //        }

        //        var basketList = new List<BasketListDto>();

        //        foreach (var f in existedBasket)
        //        {
        //            var product = await _productRepository.GetAll().Include(f => f.Images).FirstOrDefaultAsync(x => x.Id == f.ProductId).ConfigureAwait(false);
        //            basketList.Add(new BasketListDto
        //            {
        //                ProductId = f.ProductId,
        //                Quantity = f.Quantity,
        //                ProductName = product.Name,
        //                TotalPrice = product.Price * f.Quantity,
        //                Images = product.Images.Where(d => !d.IsDeleted).Select(i => new ProductListImageDto
        //                {
        //                    Path = i.Path
        //                }).ToList()
        //            });
        //        }

        //        return new ServiceResponse<List<BasketListDto>>(basketList);
        //    }
        //}


        //#region Session GetAllBasket
        //public async Task<ServiceResponse<List<BasketListDto>>> GetAllBasket()
        //{
        //    var userId = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    var basketKey = $"Basket_{userId}";
        //    byte[] basket = null;
        //    if (!_httpContext.HttpContext.Session.TryGetValue(basketKey, out basket))
        //    {
        //        return new ServiceResponse<List<BasketListDto>>(null, false, "Not Found");
        //    }
        //    else
        //    {
        //        var existedBasket = JsonSerializer.Deserialize<List<BasketListSessionDto>>(Encoding.UTF8.GetString(basket))!;
        //        if (existedBasket == null)
        //        {
        //            return new ServiceResponse<List<BasketListDto>>(null, false, "Basket is empty");
        //        }
        //        var basketList = new List<BasketListDto>();
        //        existedBasket.ForEach(f =>
        //        {
        //            var product = _productRepository.GetAll().Include(f => f.Images).Include(f => f.Campaigns).ThenInclude(f => f.Campaign).FirstOrDefault(x => x.Id == f.ProductId);
        //            basketList.Add(new BasketListDto
        //            {
        //                ProductId = f.ProductId,
        //                Quantity = f.Quantity,
        //                ProductName = product.Name,
        //                Campaigns = product.Campaigns.Where(c => !c.IsDeleted && c.Campaign.StartDate <= DateTime.UtcNow && c.Campaign.EndDate >= DateTime.UtcNow).Select(ca => new BasketListCampaignDto
        //                {
        //                    DiscountedPrice = product.Price - ((ca.Campaign.DiscountRate * product.Price) / 100),
        //                    DiscountedTotalPrice = f.Quantity * (product.Price - ((ca.Campaign.DiscountRate * product.Price) / 100)),
        //                }).ToList(),
        //                TotalPrice = product.Price * f.Quantity,
        //                Images = product.Images.Where(d => !d.IsDeleted).Select(i => new ProductListImageDto
        //                {
        //                    Path = i.Path
        //                }).ToList()
        //            });

        //            if (basketList.Last().Campaigns.Any(c => c.DiscountedPrice != 0))
        //            {
        //                basketList.Last().TotalPrice = basketList.Last().Campaigns.Sum(c => c.DiscountedTotalPrice);
        //            }

        //        });
        //        return new ServiceResponse<List<BasketListDto>>(basketList);
        //    }
        //}
        //#endregion
        public async Task<ServiceResponse> RemoveFromBasket(Guid productId, int quantity = 1)
        {
            var userId = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                userId = await _redisCache.GetStringAsync("TemporaryUserId");
                if (string.IsNullOrEmpty(userId))
                {
                    userId = Guid.NewGuid().ToString();
                    await _redisCache.SetStringAsync("TemporaryUserId", userId).ConfigureAwait(false);
                }
            }
            var basketKey = $"Basket_{userId}";

            string basket = await _redisCache.GetStringAsync(basketKey).ConfigureAwait(false);
            if (basket == null)
            {
                return new ServiceResponse(false, "Basket Not Found");
            }
            else
            {
                var existedBasket = JsonSerializer.Deserialize<List<BasketListSessionDto>>(basket)!;
                var existedItem = existedBasket.FirstOrDefault(b => b.ProductId == productId);
                if (existedItem != null)
                {
                    existedItem.Quantity -= quantity;
                    if (existedItem.Quantity <= 0)
                    {
                        existedBasket.Remove(existedItem);
                    }
                }
                await _redisCache.SetStringAsync(basketKey, JsonSerializer.Serialize(existedBasket)).ConfigureAwait(false);
            }
            return new ServiceResponse();
        }

        public async Task<ServiceResponse<List<BasketListDto>>> GetAllBasket()
        {
            var userId = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                userId = await _redisCache.GetStringAsync("TemporaryUserId");
                if (string.IsNullOrEmpty(userId))
                {
                    userId = Guid.NewGuid().ToString();
                    await _redisCache.SetStringAsync("TemporaryUserId", userId).ConfigureAwait(false);
                }
            }
            var basketKey = $"Basket_{userId}";

            string basket = await _redisCache.GetStringAsync(basketKey).ConfigureAwait(false);
            if (basket == null)
            {
                return new ServiceResponse<List<BasketListDto>>(null, false, "Basket Not Found");
            }
            else
            {
                var existedBasket = JsonSerializer.Deserialize<List<BasketListSessionDto>>(basket)!;
                if (existedBasket == null)
                {
                    return new ServiceResponse<List<BasketListDto>>(null, false, "Basket is empty");
                }

                var basketList = new List<BasketListDto>();

                foreach (var f in existedBasket)
                {
                    var product = await _productRepository.GetAll().Include(f => f.Images).FirstOrDefaultAsync(x => x.Id == f.ProductId).ConfigureAwait(false);
                    basketList.Add(new BasketListDto
                    {
                        ProductId = f.ProductId,
                        Quantity = f.Quantity,
                        ProductName = product.Name,
                        TotalPrice = product.Price * f.Quantity,
                        Images = product.Images.Where(d => !d.IsDeleted).Select(i => new ProductListImageDto
                        {
                            Path = i.Path
                        }).ToList()
                    });
                }

                return new ServiceResponse<List<BasketListDto>>(basketList);
            }
        }
        //#region SessionBasketClear
        //public async Task<ServiceResponse> ClearBasket()
        //{
        //    var userId = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    var basketKey = $"Basket_{userId}";
        //    byte[] basket = null;
        //    if (!_httpContext.HttpContext.Session.TryGetValue(basketKey, out basket))
        //    {
        //        return new ServiceResponse(false, "Sepet bulunamadı.");
        //    }
        //    else
        //    {
        //        var existedBasket = JsonSerializer.Deserialize<List<BasketListSessionDto>>(Encoding.UTF8.GetString(basket))!;

        //        foreach (var item in existedBasket)
        //        {
        //            var product = await _productRepository.GetById(item.ProductId).ConfigureAwait(false);
        //            if (product != null)
        //            {
        //                product.Stock -= item.Quantity;
        //                await _productRepository.Update(product).ConfigureAwait(false);
        //            }
        //        }

        //        existedBasket.Clear();
        //        _httpContext.HttpContext.Session.Set(basketKey, Encoding.UTF8.GetBytes(JsonSerializer.Serialize(existedBasket)));
        //    }

        //    return new ServiceResponse();
        //}
        //#endregion

        public async Task<ServiceResponse> ClearBasket()
        {
            var userId = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                userId = await _redisCache.GetStringAsync("TemporaryUserId");
                if (string.IsNullOrEmpty(userId))
                {
                    userId = Guid.NewGuid().ToString();
                    await _redisCache.SetStringAsync("TemporaryUserId", userId).ConfigureAwait(false);
                }
            }
            var basketKey = $"Basket_{userId}";

            string basket = await _redisCache.GetStringAsync(basketKey).ConfigureAwait(false);
            if (basket == null)
            {
                return new ServiceResponse(false, "Sepet bulunamadı.");
            }
            else
            {
                var existedBasket = JsonSerializer.Deserialize<List<BasketListSessionDto>>(basket)!;
                foreach (var item in existedBasket)
                {
                    var product = await _productRepository.GetById(item.ProductId).ConfigureAwait(false);
                    if (product != null)
                    {
                        product.Stock -= item.Quantity;
                        await _productRepository.Update(product).ConfigureAwait(false);
                    }
                }
                existedBasket.Clear();
                await _redisCache.SetStringAsync(basketKey, JsonSerializer.Serialize(existedBasket)).ConfigureAwait(false);
            }
            return new ServiceResponse();
        }
    }
}