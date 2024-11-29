using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace E_commerce.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Minimum length must be 3")]
        [MaxLength(50, ErrorMessage = "Maximum length must be 50")]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Range(0, 100000)]
        public decimal Price { get; set; }

        [ValidateNever]
        public string Img { get; set; }

        [Range(0, 1000)]
        public int Quantity { get; set; }

        [Range(0, 5)]
        public double Rate { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [ValidateNever]
        public Category Category { get; set; }
    }
}
