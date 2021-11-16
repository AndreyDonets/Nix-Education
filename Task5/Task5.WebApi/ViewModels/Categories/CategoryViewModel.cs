using System;
using System.ComponentModel.DataAnnotations;

namespace Task5.WebApi.ViewModels.Categories
{
    public class CategoryViewModel : BaseCategoryViewModel
    {
        [Required]
        public Guid Id { get; set; }
    }
}
