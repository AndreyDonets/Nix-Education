using System.Threading.Tasks;
using Task5.BLL.DTO;

namespace Task5.BLL.Interfaces
{
    public interface ICategoryService : IBaseService<CategoryDTO>
    {
        CategoryDTO GetCategoryByName(string name);
        Task<CategoryDTO> GetCategoryByNameAsync(string name);
    }
}
