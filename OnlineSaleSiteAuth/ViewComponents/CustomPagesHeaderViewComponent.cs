using Microsoft.AspNetCore.Mvc;
using OnlineSaleSiteAuth.Application.Service.CustomPage;

namespace OnlineSaleSiteAuth.ViewComponents
{
    public class CustomPagesHeaderViewComponent : ViewComponent
    {
        private readonly ICustomPageService _customPageService;

        public CustomPagesHeaderViewComponent(ICustomPageService customPageService)
        {
            _customPageService = customPageService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var customPages = await _customPageService.GetAllCustomPage().ConfigureAwait(false);
            return View(customPages.Data);
        }
    }
}
