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
    public class CustomersController : ControllerBase
    {
        private readonly CustomerBackendContext _context;

        public CustomersController(CustomerBackendContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> Customers(string? email, int? id, bool? fetchPurchases = false)
        {
            if (email != null && id != null)
                return BadRequest("Please filter customers either by email or id");
            return await FetchCustomersManager(email,id, (bool)fetchPurchases);
        }


        [HttpPost]
        public async Task<ActionResult> Create([FromBody] List<CustomerCreateDTO> body)
        {
            if (body.Any(b => b.Name == null || b.Email == null))
                return BadRequest("Provide name and email for all customers.");

            return await HandleAddCustomerManager(body);
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] List<CustomerUpdateDTO> body)
        {
            return await HandleUpdateCustomerManager(body);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete([FromQuery] int? Id, [FromQuery] string? email)
        {
            return await HandleDeleteCustomer(Id, email);
        }

        #region Managers
        private async Task<ActionResult> FetchCustomersManager(string email, int? id, bool fetchPurchases = false)
        {
            var query = _context.Customers.AsNoTracking();

            if (fetchPurchases)
                query = query.Include(q => q.Purchases);
            
            if (email != null)
            {
                query = query.Where(c => c.Email == email);
            }

            else if (id != null)
            {
                query = query.Where(c => c.Id == id);
            }

            
            return Ok(query.ToList());
        }
        private async Task<ActionResult> HandleAddCustomerManager(List<CustomerCreateDTO> body)
        {
            var rowsAdded = 0;
            try
            {
                foreach (var customer in body)
                {
                    _context.Add(customer.toDb());
                }
                rowsAdded = _context.SaveChanges();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(new SuccessCreateUpdate() { effectedRows = rowsAdded , Success = true});
        }
        private async Task<ActionResult> HandleUpdateCustomerManager(List<CustomerUpdateDTO> body)
        {
            var rowsUpdated = 0;
            var customerIds = body.Select(b => b.Id);
            var customers = _context.Customers.Where(c => customerIds.Contains(c.Id));
            try
            {
                foreach (var customer in body)
                {
                    var updateCustomer = customers.Where(b => b.Id == customer.Id).FirstOrDefault();
                    if (updateCustomer == null)
                        continue;
                    updateCustomer.update(customer);
                }
                rowsUpdated = _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(new SuccessCreateUpdate() { effectedRows = rowsUpdated, Success = true });
        }
        private async Task<ActionResult> HandleDeleteCustomer(int? id = null, string email = null)
        {
            //Note: This will remove all purchases from the customer as well.
            //Many other implementations are available and we could add a flag that signifies deleted entries to have to option to revert this.
            //Doing this for simplicity.

            if ((id == null && email == null)
             || (id != null && email != null))
                return BadRequest("Specify the user either by email or id");

            var customer = await _context.Customers.FirstOrDefaultAsync(b => (id == null || b.Id == id)
                                                                          && (email == null || b.Email == email));
            if (customer == null)
                return NotFound("Requested customer not found");

            _context.Remove(customer);
            var rowsUpdated = _context.SaveChanges();
            return Ok(new SuccessCreateUpdate() { effectedRows = rowsUpdated, Success = true });

        }
        #endregion

    }
}
