using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task5.BLL.DTO;

namespace Task5.BLL.Interfaces
{
    public interface ICategoryDateService : IBaseService<CategoryDateDTO>
    {
        IEnumerable<CategoryDateDTO> GetCategoryDatesByCategoryId(Guid categoryId);
        Task<IEnumerable<CategoryDateDTO>> GetCategoryDatesByCategoryIdAsync(Guid categoryId);
        IEnumerable<CategoryDateDTO> GetCategoryDatesByCategoryName(string categoryName);
        Task<IEnumerable<CategoryDateDTO>> GetCategoryDatesByCategoryNameAsync(string categoryName);
    }
}
