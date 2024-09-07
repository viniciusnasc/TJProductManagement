using System.ComponentModel.DataAnnotations;

namespace TJ.ProductManagement.Domain.Models
{
    public class ProductInsertModel
    {
        [Required(ErrorMessage = "The field {0} is required!")]
        [DeniedValues("", null)]
        public required string Name { get; set; }

        [Required(ErrorMessage = "The field {0} is required!")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public decimal Price { get; set; }

        public string? Description { get; set; }
    }
}
