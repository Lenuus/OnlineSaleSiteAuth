using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineSaleSiteAuth.Application.Service.Basket;
using OnlineSaleSiteAuth.Application.Service.Category;
using OnlineSaleSiteAuth.Application.Service.Product;
using OnlineSaleSiteAuth.Application.Service.Product.Dtos;
using OnlineSaleSiteAuth.Domain;
using OnlineSaleSiteAuth.Models;
using OnlineSaleSiteAuth.Models.Basket;
using OnlineSaleSiteAuth.Models.Product;
using System.Text;
using System.Text.Json;

namespace OnlineSaleSiteAuth.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly ICategoryService _categoryService;
        private readonly IBasketService _basketService;

        public ProductController(IProductService productService, IMapper mapper, UserManager<User> userManager, ICategoryService categoryService, IBasketService basketService)
        {
            _productService = productService;
            _mapper = mapper;
            _userManager = userManager;
            _categoryService = categoryService;
            _basketService = basketService;
        }

        public async Task<IActionResult> Index(GetAllProductRequestModel request)
        {
            var categories = await _categoryService.GetAllCategory().ConfigureAwait(false);
            var categoryList = categories.Data.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();
            ViewBag.Categories = new SelectList(categoryList, "Value", "Text");

            var mappedRequest = _mapper.Map<GetAllProductRequestDto>(request);
            var response = await _productService.GetAllProducts(mappedRequest).ConfigureAwait(false);
            if (response == null)
            {
                return NotFound();
            }
            var mappedResponse = _mapper.Map<PagedResponseModel<ProductListModel>>(response.Data);
            return View(mappedResponse);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAllCategory().ConfigureAwait(false);
            var categoryList = categories.Data.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();
            ViewBag.Categories = new SelectList(categoryList, "Value", "Text");
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(AddProductRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var mappedRequest = _mapper.Map<AddProductRequestDto>(request);
            await _productService.AddProduct(mappedRequest).ConfigureAwait(false);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var response = await _productService.DeleteProduct(Id).ConfigureAwait(false);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> DeleteImage(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var response = await _productService.DeleteImage(id).ConfigureAwait(false);
            if (!response.IsSuccessful)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        public async Task<IActionResult> Update(Guid Id)
        {
            var product = await _productService.GetProductById(Id).ConfigureAwait(!false);
            if (product == null)
            {
                return NotFound();
            }
            var categories = await _categoryService.GetAllCategory().ConfigureAwait(false);
            var categoryList = categories.Data.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            ViewBag.Categories = new SelectList(categoryList, "Value", "Text");
            var response = _mapper.Map<UpdateProductRequestModel>(product.Data);
            return View(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Update(Guid Id, UpdateProductRequestModel request)
        {
            if (request.Id != Id)
            {
                return NotFound();

            }
            var mappedRequest = _mapper.Map<UpdateProductRequestDto>(request);
            var response = await _productService.UpdateProduct(mappedRequest).ConfigureAwait(false);
            return RedirectToAction("Index");

        }

    }
}
