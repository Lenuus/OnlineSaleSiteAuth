using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineSaleSiteAuth.Application.Service.Category;
using OnlineSaleSiteAuth.Application.Service.Category.Dtos;
using OnlineSaleSiteAuth.Application.Service.Product;
using OnlineSaleSiteAuth.Domain;
using OnlineSaleSiteAuth.Models.Category;

namespace OnlineSaleSiteAuth.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly ICategoryService _categoryService;

        public CategoryController(IProductService productService, IMapper mapper, UserManager<User> userManager, ICategoryService categoryService)
        {
            _mapper = mapper;
            _userManager = userManager;
            _categoryService = categoryService;
        }

        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Create()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]

        [HttpPost]
        public async Task<IActionResult> Create(AddCategoryModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var entity= _mapper.Map<AddCategoryDto>(model);
            await _categoryService.AddCategory(entity).ConfigureAwait(false);
            return View();
        }
    }
}
