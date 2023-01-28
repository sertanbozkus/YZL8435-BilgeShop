using BilgeShop.Business.Dtos;
using BilgeShop.Business.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilgeShop.Business.Services
{
    public interface IProductService
    {
        ServiceMessage AddProduct(ProductDto productDto);

        List<ProductDto> GetProducts();

        ProductDto GetProductById(int id);

        void EditProduct(ProductDto productDto);

        void DeleteProduct(int id);

        List<ProductDto> GetProductsByCategoryId(int? categoryId = null);

        ProductDetailDto GetProductDetailById(int id);
    }
}
