using System.ComponentModel.DataAnnotations;

namespace Task7.WebApplication.Models.Categories
{
    public class CategoryViewModel
    {

        [Required(ErrorMessage = "Required field")]
        [MaxLength(50)]
        public virtual string Name { get; set; }

        [Range(0, double.MaxValue)]
        public virtual decimal Price { get; set; }
    }
}
