using System;

namespace Task5.BLL.DTO
{
    public class CategoryDTO : BaseModelDTO
    {
        public CategoryDTO() => Id = Guid.NewGuid();

        public string Name { get; set; }
    }
}