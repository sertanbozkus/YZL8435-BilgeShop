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
    public class CategoryManager : ICategoryService
    {
        private readonly IRepository<CategoryEntity> _categoryRepository;
        public CategoryManager(IRepository<CategoryEntity> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public ServiceMessage AddCategory(CategoryDto categoryDto)
        {
            var hasCategory = _categoryRepository.GetAll(x => x.Name.ToLower() == categoryDto.Name.ToLower()).ToList();

            if(hasCategory.Any()) // hiç veri gelmediyse
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Bu isimde bir kategori zaten mevcut."
                };
            }

            var categoryEntity = new CategoryEntity
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description
            };

            _categoryRepository.Add(categoryEntity);

            return new ServiceMessage
            {
                IsSucceed = true
            };

        }

        public void DeleteCategory(int id)
        {
            _categoryRepository.Delete(id);
        }

        public List<CategoryDto> GetCategories()
        {
            var categoryEntities = _categoryRepository.GetAll().OrderBy(x => x.Name);


            // Listedeki tipi CategoryEntity olan her bir verinin istediğim bilgilerini CategoryDto'ya çevirip , CategoryDto listesi elde edeceğim.

            var categoryDtoList = categoryEntities.Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            }).ToList();


            return categoryDtoList;
        }

        public CategoryDto GetCategoryById(int id)
        {
            // Bu Id'ye sahip category entity çekilecek.
            var categoryEntity = _categoryRepository.GetById(id);

            // Ihtiyacım olan propertyler , dto'da doldurulup , return edilecek.

            var categoryDto = new CategoryDto()
            {
                Id = categoryEntity.Id,
                Name = categoryEntity.Name,
                Description = categoryEntity.Description
            };

            return categoryDto;
        }

        public void UpdateCategory(CategoryDto categoryDto)
        {
            var categoryEntity = _categoryRepository.GetById(categoryDto.Id);

            categoryEntity.Name = categoryDto.Name;
            categoryEntity.Description = categoryDto.Description;

            _categoryRepository.Update(categoryEntity);
        }
    }
}
