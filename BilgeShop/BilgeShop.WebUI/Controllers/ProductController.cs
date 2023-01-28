using BilgeShop.Business.Services;
using BilgeShop.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BilgeShop.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Detail(int id)
        {
            var productDetailDto = _productService.GetProductDetailById(id);

            var viewModel = new ProductDetailViewModel()
            {
                Name = productDetailDto.Name,
                Description = productDetailDto.Description,
                ImagePath = productDetailDto.ImagePath,
                UnitInStock = productDetailDto.UnitInStock,
                UnitPrice = productDetailDto.UnitPrice,
                CategoryName = productDetailDto.CategoryName,
                ModifiedDate = productDetailDto.ModifiedDate,
                CategoryId = productDetailDto.CategoryId
            };

            return View(viewModel);
        }
    }
}
