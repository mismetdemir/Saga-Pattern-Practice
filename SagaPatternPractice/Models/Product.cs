using System.ComponentModel.DataAnnotations;

namespace SagaPatternPractice.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        
    }
}
