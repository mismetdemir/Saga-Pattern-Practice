using SagaPatternPractice.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace SagaPatternPractice.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
