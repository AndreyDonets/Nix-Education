using System;
using System.ComponentModel.DataAnnotations;

namespace Task5.WebApi.ViewModels.Categories
{
    public class ChangeCategoryViewModel : CategoryViewModel
    {

        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [MaxLength(50)]
        public string NewName { get; set; }
    }
}
