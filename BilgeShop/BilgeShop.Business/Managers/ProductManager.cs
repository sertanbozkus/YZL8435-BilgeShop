using BilgeShop.Business.Dtos;
using BilgeShop.Business.Services;
using BilgeShop.Business.Types;
using BilgeShop.Data.Entities;
using BilgeShop.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilgeShop.Business.Managers
{
    public class ProductManager : IProductService
    {
        private readonly IRepository<ProductEntity> _productRepository;

        public ProductManager(IRepository<ProductEntity> productRepository)
        {
            _productRepository = productRepository;
        }

        public ServiceMessage AddProduct(ProductDto productDto)
        {
            var hasProduct = _productRepository.GetAll(x => x.Name.ToLower() == productDto.Name.ToLower()).ToList(); // O isimde kayıtlı bütün verileri getiriyorum.

            if(hasProduct.Any()) // eğer aynı isimde product bulunduysa
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Bu isimde bir ürün zaten mevcut."
                };
            }

            var productEntity = new ProductEntity()
            {
                Id = productDto.Id,
                Name = productDto.Name,
                Description = productDto.Description,
                UnitInStock = productDto.UnitInStock,
                UnitPrice = productDto.UnitPrice,
                CategoryId = productDto.CategoryId,
                ImagePath = productDto.ImagePath
            };

            _productRepository.Add(productEntity);

            return new ServiceMessage
            {
                IsSucceed = true
            };
            
        }

        public void DeleteProduct(int id)
        {
            _productRepository.Delete(id);
        }

        public void EditProduct(ProductDto productDto)
        {
            var productEntity = _productRepository.GetById(productDto.Id);

            productEntity.Name = productDto.Name;
            productEntity.Description = productDto.Description;
            productEntity.UnitPrice = productDto.UnitPrice;
            productEntity.UnitInStock = productDto.UnitInStock;
            productEntity.CategoryId = productDto.CategoryId;

            if(productDto.ImagePath != null)
            productEntity.ImagePath = productDto.ImagePath;

            _productRepository.Update(productEntity);
            
        }

        public ProductDto GetProductById(int id)
        {
            var productEntity = _productRepository.GetById(id);

            var productDto = new ProductDto()
            {
                Id = productEntity.Id,
                Name = productEntity.Name,
                Description = productEntity.Description,
                UnitInStock = productEntity.UnitInStock,
                UnitPrice = productEntity.UnitPrice,
                CategoryId = productEntity.CategoryId,
                ImagePath = productEntity.ImagePath
            };

            return productDto;
        }

        public ProductDetailDto GetProductDetailById(int id)
        {
            var productEntity = _productRepository.GetAll(x => x.Id == id);

            var productDetailDto = productEntity.Select(x => new ProductDetailDto()
            {
                Name = x.Name,
                Description = x.Description,
                UnitInStock = x.UnitInStock,
                UnitPrice = x.UnitPrice,
                ImagePath = x.ImagePath,
                CategoryName = x.Category.Name,
                ModifiedDate = x.ModifiedDate,
                CategoryId = x.CategoryId
            }).ToList();

            // Product içerisinde , category içerisindeki bir property'e giderken GetByID metodu yeterli değil. Bu nedenle 1 elemanlı bile olsa liste olarak çekme işlemi yaptım GetAll ile.

            // 2. yol olarak -> Category Repository eklenip, çekilen productEntity'nin categoryID'si category adı bulunup atılabilirdi.
            // var category = _categoryRepositry.GetById(productEntity.CategoryId)
            // atama kısmında da productDetailDto.CategoryName = category.Name

            return productDetailDto[0];


        }

        public List<ProductDto> GetProducts()
        {
            var productEntities = _productRepository.GetAll().OrderBy(x => x.Category.Name).ThenBy(x => x.Name); // Önce kategori adına , sonra da ürüh adına göre sırala.

            var ProductDtoList = productEntities.Select(x => new ProductDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                UnitInStock = x.UnitInStock,
                UnitPrice = x.UnitPrice,
                CategoryId = x.CategoryId,
                CategoryName = x.Category.Name,
                ImagePath = x.ImagePath
            }).ToList();

            return ProductDtoList;

        }

        public List<ProductDto> GetProductsByCategoryId(int? categoryId = null)
        {
            if (categoryId.HasValue)
            {
                var productEntities = _productRepository.GetAll(x => x.CategoryId == categoryId).OrderBy(x => x.Name);

                var productDtos = productEntities.Select(x => new ProductDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    UnitInStock = x.UnitInStock,
                    UnitPrice = x.UnitPrice,
                    CategoryId = x.CategoryId,
                    ImagePath = x.ImagePath,
                    CategoryName = x.Category.Name
                }).ToList();

                return productDtos;
            }

            return GetProducts();


        }
    }
}
