using Microsoft.AspNetCore.Mvc;
using SagaPatternPractice.Data;
using SagaPatternPractice.DTOs;
using SagaPatternPractice.Models;

namespace SagaPatternPractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context) 
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(CreateProdcutDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity
            };

            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            return Ok("Product created successfully");
        }
    }
}
