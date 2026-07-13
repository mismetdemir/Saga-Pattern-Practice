using SagaPatternPractice.Data;
using SagaPatternPractice.Enums;
using SagaPatternPractice.Models;

namespace SagaPatternPractice.Services
{
    public class PaymentService
    {
        private readonly AppDbContext _context;

        public PaymentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Payment> CreatePaymentAsync(Order order)
        {
            var payment = new Payment
            {
                OrderId = order.Id,
                Amount = order.TotalPrice,
                Status = PaymentStatus.Pending,
                CreatedAt = DateTime.Now
            };

            _context.Payments.Add(payment);

            await _context.SaveChangesAsync();

            return payment;
        }

        public async Task<bool> ProcessPaymentAsync(int paymentId, bool failed)
        {
            var payment = await _context.Payments.FindAsync(paymentId);

            if (payment == null) return false;

            payment.Status = failed ? PaymentStatus.Failed : PaymentStatus.Completed;

            await _context.SaveChangesAsync();

            return !failed;
        }

        public async Task RefundPaymentAsync(int paymentId)
        {
            var payment = await _context.Payments.FindAsync(paymentId);

            if (payment == null) return;

            payment.Status = PaymentStatus.Refunded;

            await _context.SaveChangesAsync();
        }
    }
}
