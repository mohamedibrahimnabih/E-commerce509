using System.ComponentModel.DataAnnotations;

namespace E_commerce.Models
{
    public class RequestCompnay
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; set; }
        public string Description { get; set; }
        [RegularExpression("Cairo|Alex|Mansoura")]
        public string Address { get; set; }

        [Required]
        public string Details { get; set; }
        public string status { get; set; }
    }
}
