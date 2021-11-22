using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task5.BLL.DTO;
using Task5.BLL.Interfaces;
using Task5.DAL.Entities;
using Task5.DAL.Interfaces;

namespace Task5.BLL.Services
{
    public class CategoryDateService : ICategoryDateService
    {
        private IUnitOfWork db;
        private ICategoryService categoryService;

        public CategoryDateService(IUnitOfWork db, ICategoryService categoryService)
        {
            this.db = db;
            this.categoryService = categoryService;
        }

        private IMapper GetMapperToCategoryDateDTO() => new MapperConfiguration(cfg => cfg.CreateMap<CategoryDate, CategoryDateDTO>()).CreateMapper();
        private IMapper GetMapperToCategoryDate() => new MapperConfiguration(cfg => cfg.CreateMap<CategoryDateDTO, CategoryDate>()).CreateMapper();

        public IEnumerable<CategoryDateDTO> GetAll() => GetMapperToCategoryDateDTO().Map<IEnumerable<CategoryDate>, List<CategoryDateDTO>>(db.CategoryDateRepository.GetAll());
        public CategoryDateDTO Get(Guid id) => GetMapperToCategoryDateDTO().Map<CategoryDate, CategoryDateDTO>(db.CategoryDateRepository.Get(id));
        public void Create(CategoryDateDTO item)
        {
            if (db.CategoryDateRepository.Get(item.Id) == null && db.CategoryRepository.Get(item.CategoryId) != null)
                db.CategoryDateRepository.Create(GetMapperToCategoryDate().Map<CategoryDateDTO, CategoryDate>(item));
        }
        public IEnumerable<CategoryDateDTO> GetCategoryDatesByCategoryId(Guid categoryId)
        {
            var categoryDates = db.CategoryDateRepository.GetAll();
            return GetMapperToCategoryDateDTO().Map<IEnumerable<CategoryDate>, List<CategoryDateDTO>>(categoryDates.Where(x => x.CategoryId == categoryId));
        }
        public IEnumerable<CategoryDateDTO> GetCategoryDatesByCategoryName(string categoryName)
        {
            var category = categoryService.GetCategoryByName(categoryName);
            if (category == null)
                throw new ArgumentNullException();
            var categoryDates = db.CategoryDateRepository.GetAll();
            return GetMapperToCategoryDateDTO().Map<IEnumerable<CategoryDate>, List<CategoryDateDTO>>(categoryDates.Where(x => x.CategoryId == category.Id));
        }
        public void Update(CategoryDateDTO item)
        {
            if (db.CategoryDateRepository.Get(item.Id) != null && db.CategoryRepository.Get(item.CategoryId) != null)
                db.CategoryDateRepository.Update(GetMapperToCategoryDate().Map<CategoryDateDTO, CategoryDate>(item));
        }
        public void Delete(Guid id)
        {
            if (db.CategoryDateRepository.Get(id) != null)
                db.CategoryDateRepository.Delete(id);
        }
        public void Save() => db.Save();
        public async Task<IEnumerable<CategoryDateDTO>> GetAllAsync() => GetMapperToCategoryDateDTO().Map<IEnumerable<CategoryDate>, List<CategoryDateDTO>>(await db.CategoryDateRepository.GetAllAsync());
        public async Task<CategoryDateDTO> GetAsync(Guid id) => GetMapperToCategoryDateDTO().Map<CategoryDate, CategoryDateDTO>(await db.CategoryDateRepository.GetAsync(id));
        public async Task<IEnumerable<CategoryDateDTO>> GetCategoryDatesByCategoryIdAsync(Guid categoryId)
        {
            var categoryDates = await db.CategoryDateRepository.GetAllAsync();
            return GetMapperToCategoryDateDTO().Map<IEnumerable<CategoryDate>, List<CategoryDateDTO>>(categoryDates.Where(x => x.CategoryId == categoryId));
        }
        public async Task<IEnumerable<CategoryDateDTO>> GetCategoryDatesByCategoryNameAsync(string categoryName)
        {
            var category = await categoryService.GetCategoryByNameAsync(categoryName);
            if (category == null)
                throw new ArgumentNullException();
            var categoryDates = await db.CategoryDateRepository.GetAllAsync();
            return GetMapperToCategoryDateDTO().Map<IEnumerable<CategoryDate>, List<CategoryDateDTO>>(categoryDates.Where(x => x.CategoryId == category.Id));
        }
        public async Task CreateAsync(CategoryDateDTO item)
        {
            if (db.CategoryDateRepository.Get(item.Id) == null && db.CategoryRepository.Get(item.CategoryId) != null)
                await db.CategoryDateRepository.CreateAsync(GetMapperToCategoryDate().Map<CategoryDateDTO, CategoryDate>(item));
        }
        public async Task UpdateAsync(CategoryDateDTO item)
        {
            if (db.CategoryDateRepository.Get(item.Id) != null && db.CategoryRepository.Get(item.CategoryId) != null)
                await db.CategoryDateRepository.UpdateAsync(GetMapperToCategoryDate().Map<CategoryDateDTO, CategoryDate>(item));
        }
        public async Task DeleteAsync(Guid id)
        {
            if (db.CategoryDateRepository.Get(id) != null)
                await db.CategoryDateRepository.DeleteAsync(id);
        }
        public void Dispose() => db.Dispose();
    }
}
