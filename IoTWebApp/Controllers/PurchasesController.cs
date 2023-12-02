using CustomerBackend.Database;
using CustomerBackend.DbConverters;
using CustomerBackend.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace CustomerBackend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly CustomerBackendContext _context;

        public PurchasesController(CustomerBackendContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreatePurchaseDTO body)
        {
            return await HandelAddPurchases(body);
        }


        #region Managers
        private async Task<ActionResult> HandelAddPurchases(CreatePurchaseDTO body)
        {
            var rowsAdded = 0;
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == body.CustomerId);
            if (customer == null)
                return NotFound($"Customer with id {body.CustomerId} not found");
            if (body.Products == null || !body.Products.Any())
                return BadRequest("Specify products for purchase.");
            try
            {
                foreach (var product in body.Products)
                {
                    _context.Add(new Purchases()
                    {
                        CustomerId = body.CustomerId,
                        ProductId = product.ProductId,
                        Quantity = product.Quantity,
                        PurchaseDate = body.PurchaseTimestamp
                    });
                }
                rowsAdded = _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(new SuccessCreateUpdate() { effectedRows = rowsAdded, Success = true });
        }
        #endregion

    }
}
