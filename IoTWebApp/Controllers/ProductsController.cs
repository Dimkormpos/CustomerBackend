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
    public class ProductsController : ControllerBase
    {
        private readonly CustomerBackendContext _context;

        public ProductsController(CustomerBackendContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> Products(int? id, bool? fetchPurchases = false)
        {
            return await FetchProducts(id, (bool)fetchPurchases);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] List<ProductsCreateDTO> body)
        {
            if (body.Any(b => b.Price == null || b.Name == null))
                return BadRequest("Provide price and name for all products.");

            return await HandleAddProductsManager(body);
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] List<ProductsUpdateDTO> body)
        {
            return await HandleUpdateCustomerManager(body);
        }

        [HttpDelete]
        public async Task<ActionResult> Delte([FromQuery] int Id)
        {
            return await HandleDeleteProduct(Id);
        }

        #region Managers
        private async Task<ActionResult> FetchProducts(int? id, bool fetchPurchases = false)
        {
            var products = _context.Products.AsNoTracking();

            if(fetchPurchases)
                products= products.Include(p => p.Purchases);

            if (id != null)
                products = products.Where(c => c.Id == id);


            return Ok(await products.ToListAsync());
        }
        private async Task<ActionResult> HandleAddProductsManager(List<ProductsCreateDTO> body)
        {
            var rowsAdded = 0;
            try
            {
                foreach (var product in body)
                {
                    _context.Add(product.toDb());
                }
                rowsAdded = _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(new SuccessCreateUpdate() { effectedRows = rowsAdded, Success = true });
        }
        private async Task<ActionResult> HandleUpdateCustomerManager(List<ProductsUpdateDTO> body)
        {
            var rowsUpdated = 0;
            var productIds = body.Select(b => b.Id);
            var products = _context.Products.Where(c => productIds.Contains(c.Id));
            try
            {
                foreach (var product in body)
                {
                    var updateproduct = products.Where(b => b.Id == product.Id).FirstOrDefault();
                    if (updateproduct == null)
                        continue;
                    updateproduct.update(product);
                }
                rowsUpdated = _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(new SuccessCreateUpdate() { effectedRows = rowsUpdated, Success = true });
        }
        private async Task<ActionResult> HandleDeleteProduct(int? id = null)
        {
            //Note: This will remove all purchases from the customer as well.
            //Many other implementations are available and we could add a flag that signifies deleted entries to have to option to revert this.
            //Doing this for simplicity.

            if (id == null)
                return BadRequest("Specify the product id");

            var product = await _context.Products.FirstOrDefaultAsync(b =>  b.Id == id);
            _context.Remove(product);
            var rowsUpdated = _context.SaveChanges();
            return Ok(new SuccessCreateUpdate() { effectedRows = rowsUpdated, Success = true });

        }
        #endregion

    }
}
