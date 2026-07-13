using System.ComponentModel.DataAnnotations;

namespace SagaPatternPractice.DTOs
{
    public class OrderDto
    {
        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }
        [Range(1, 10)]
        public int Quantity { get; set; }
    }
}
