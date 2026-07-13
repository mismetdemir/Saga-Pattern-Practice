using SagaPatternPractice.Enums;

namespace SagaPatternPractice.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
