using System.ComponentModel.DataAnnotations;

namespace SagaPatternPractice.DTOs
{
    public class CreateProdcutDto
    {
        [Required]
        public string Name { get; set; }
        [Range(0.01, 1000000)]
        public decimal Price { get; set; }
        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }

    }
}
