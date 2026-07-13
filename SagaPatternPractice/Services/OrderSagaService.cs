using Microsoft.AspNetCore.Http.HttpResults;
using SagaPatternPractice.Data;
using SagaPatternPractice.DTOs;
using SagaPatternPractice.Enums;
using SagaPatternPractice.Models;

namespace SagaPatternPractice.Services
{
    public class OrderSagaService
    {
        private readonly AppDbContext _context;
        private readonly StockService _stockService;
        private readonly PaymentService _paymentService;
        private readonly CargoService _cargoService;

        public OrderSagaService(AppDbContext context, StockService stockService,
                                    PaymentService paymentService, CargoService cargoService)
        {
            _context = context;
            _stockService = stockService;
            _paymentService = paymentService;
            _cargoService = cargoService;
        }

        public async Task<SagaResponseDto> CreateOrderAsync(int productId, int quantity,
                                                    bool paymentFail, bool cargoFail)
        {
            var steps = new List<string>();

            var product = await _context.Products.FindAsync(productId);

            if (product == null)
            {
                steps.Add("[ERR] Product not found");

                return new SagaResponseDto
                {
                    Success = false,
                    Status = "Failed",
                    Steps = steps
                };
            }

            steps.Add("[OK] Product found");

            var order = new Order
            {
                ProductId = productId,
                Quantity = quantity,
                TotalPrice = product.Price * quantity,
                Status = OrderStatus.Created,
                CreatedAt = DateTime.Now
            };

            _context.Orders.Add(order);

            await _context.SaveChangesAsync();

            steps.Add("[OK] Order created (Order status = Created)");  

            // ########## Stok Rezerv ##########

            var stockReserved = await _stockService.ReserveStockAsync(productId, quantity);

            if (!stockReserved)
            {
                order.Status = OrderStatus.Cancelled;

                await _context.SaveChangesAsync();

                steps.Add("[ERR] Stock reservation failed");
                steps.Add("[EXIT] Order cancelled (Order status = Cancelled)");

                return new SagaResponseDto
                {
                    Success = false,
                    OrderId = order.Id,
                    Status = order.Status.ToString(),
                    Steps = steps
                };
            }

            order.Status = OrderStatus.StockReserved;

            await _context.SaveChangesAsync();

            steps.Add("[OK] Stock reserved successfully (Order status = StockReserved)");

            // ########## Ödeme ##########

            var payment = await _paymentService.CreatePaymentAsync(order);

            steps.Add("[OK] Payment created (Payment status = Pending)");

            var paymentSuccess = await _paymentService.ProcessPaymentAsync(payment.Id, paymentFail);

            if (!paymentSuccess)
            {
                steps.Add("[ERR] Payment failed (Payment status = Failed)");

                await _stockService.UnreserveStockAsync(productId, quantity);

                steps.Add("[COM] Stock unreserved");

                order.Status = OrderStatus.Cancelled;

                await _context.SaveChangesAsync();

                steps.Add("[EXIT] Order cancelled (Order status = Cancelled)");

                return new SagaResponseDto
                {
                    Success = false,
                    OrderId = order.Id,
                    Status = order.Status.ToString(),
                    Steps = steps
                };
            }

            order.Status = OrderStatus.PaymentCompleted;

            await _context.SaveChangesAsync();

            steps.Add("[OK] Payment completed successfully (Payment status = Completed)");
            steps.Add("[OK] Cargo process is initiated (Order status = PaymentCompleted)");

            // ########## Kargo ##########

            var cargoSuccess = await _cargoService.PrepareCargoAsync(cargoFail);

            if (!cargoSuccess)
            {
                steps.Add("[ERR] Cargo preparation failed");

                await _paymentService.RefundPaymentAsync(payment.Id);

                steps.Add("[COM] Payment refunded (Payment status = Refunded)");

                await _stockService.UnreserveStockAsync(productId, quantity);

                steps.Add("[COM] Stock unreserved");

                order.Status = OrderStatus.Cancelled;

                await _context.SaveChangesAsync();

                steps.Add("[EXIT] Order cancelled (Order status = Cancelled)");

                return new SagaResponseDto
                {
                    Success = false,
                    OrderId = order.Id,
                    Status = order.Status.ToString(),
                    Steps = steps
                };
            }

            order.Status = OrderStatus.CargoPrepared;

            await _context.SaveChangesAsync();

            steps.Add("[OK] Cargo prepared successfully (Order status = CargoPrepared)");

            order.Status = OrderStatus.Completed;

            await _context.SaveChangesAsync();

            steps.Add("[EXIT] Order completed successfully (Order status = Complted)");

            return new SagaResponseDto
            {
                Success = true,
                OrderId = order.Id,
                Status = order.Status.ToString(),
                Steps = steps
            };
        }
    }
}
