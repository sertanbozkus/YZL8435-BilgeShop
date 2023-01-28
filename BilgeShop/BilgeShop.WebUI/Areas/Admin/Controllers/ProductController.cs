using BilgeShop.Business.Dtos;
using BilgeShop.Business.Services;
using BilgeShop.WebUI.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BilgeShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IWebHostEnvironment _environment;
        public ProductController(ICategoryService categoryService, IProductService productService, IWebHostEnvironment environment)
        {
            _categoryService = categoryService;
            _productService = productService;
            _environment = environment;
        }

        public IActionResult List()
        {
            var productDtos = _productService.GetProducts();

            var viewModel = productDtos.Select(x => new ProductListViewModel
            {
                Id = x.Id,
                Name = x.Name,
                UnitInStock = x.UnitInStock,
                UnitPrice = x.UnitPrice,
                CategoryName = x.CategoryName,
                ImagePath = x.ImagePath
            }).ToList();

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult New()
        {
            ViewBag.Categories = _categoryService.GetCategories();
            return View("Form", new ProductFormViewModel());
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var productDto = _productService.GetProductById(id);

            var viewModel = new ProductFormViewModel()
            {
                Id = productDto.Id,
                Name = productDto.Name,
                Description = productDto.Description,
                UnitInStock = productDto.UnitInStock,
                UnitPrice = productDto.UnitPrice,
                CategoryId = productDto.CategoryId,
            };

            ViewBag.ImagePath = productDto.ImagePath;
            ViewBag.Categories = _categoryService.GetCategories();
            return View("Form", viewModel);
        }

        [HttpPost]
        public IActionResult Save(ProductFormViewModel formData)
        {

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _categoryService.GetCategories();
                return View("Form", formData);
            }
            // Dosya Adı -> Telefon.png
            // YeniDosyaAdi -> Telefon-12312312313213.png  

            var newFileName = "";

            if (formData.File != null)
            {
                var allowedFileContentTypes = new string[] { "image/jpeg", "image/jpg", "image/png", "image/jfif" };

                var allowedFileExtensions = new string[] { ".jpg", ".jpeg", ".png", ".jfif" };

                var fileContentType = formData.File.ContentType;
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(formData.File.FileName);
                var fileExtension = Path.GetExtension(formData.File.FileName);

                // Güvenlik Önlemi

                if (!allowedFileContentTypes.Contains(fileContentType) || 
                    !allowedFileExtensions.Contains(fileExtension))
                {
                    ViewBag.FileError = "Lütfen jpg , jpeg , png veya jfif tipinde geçerli bir dosya yükleyiniz.";
                    ViewBag.Categories = _categoryService.GetCategories();
                    return View("Form", formData);
                }

                newFileName = fileNameWithoutExtension + "-" + Guid.NewGuid() + fileExtension;

                var folderPath = Path.Combine("images", "products");
                var wwwRootFolderPath = Path.Combine(_environment.WebRootPath, folderPath);
                var wwwRootFilePath = Path.Combine(wwwRootFolderPath, newFileName);

                Directory.CreateDirectory(wwwRootFolderPath); // wwwroot yoksa oluştur.

                using (var fileStream = new FileStream(wwwRootFilePath , FileMode.Create))
                {
                    formData.File.CopyTo(fileStream);
               } // using bitiminde içerisinde new'lenen nesne silinir.



            }

            if (formData.Id == 0)
            {
                var productDto = new ProductDto()
                {
                    Id = formData.Id,
                    Name = formData.Name,
                    Description = formData.Description,
                    UnitPrice = formData.UnitPrice,
                    UnitInStock = formData.UnitInStock,
                    CategoryId = formData.CategoryId,
                    ImagePath = newFileName
                };


                var response = _productService.AddProduct(productDto);

                if (response.IsSucceed)
                {
                    return RedirectToAction("List");
                }
                else
                {
                    ViewBag.ErrorMessage = response.Message;
                    ViewBag.Categories = _categoryService.GetCategories();
                    return View("Form", formData);
                }
            }
            else // id 0'dan farklıysa , güncelleme
            {
                var productDto = new ProductDto
                {
                    Id = formData.Id,
                    Name = formData.Name,
                    Description = formData.Description,
                    UnitPrice = formData.UnitPrice,
                    UnitInStock = formData.UnitInStock,
                    CategoryId = formData.CategoryId
                };

                if (formData.File != null)
                    productDto.ImagePath = newFileName;

                _productService.EditProduct(productDto);
            }


            return RedirectToAction("List");

        }


        public IActionResult Delete(int id)
        {
            _productService.DeleteProduct(id);
            return RedirectToAction("list");


        }

    }
}
