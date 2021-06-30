using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bank.Api.Models;

namespace Bank.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : Controller
    {
        private readonly BankContext _bankContext;

        public CustomersController(BankContext bankContext)
        {
            _bankContext = bankContext;
        }

        // GET: api/customers
        [HttpGet]
        public IEnumerable<Customer> GetCustomers()
        {
            return _bankContext.Customers;
        }
        
        // GET: api/customers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer([FromRoute] long id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customer = await _bankContext.Customers.FindAsync(id);

            return customer != null ? Ok(customer) : NotFound();
        }
        
        // PUT: api/Customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer([FromRoute] long id, [FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customer.Id)
            {
                return BadRequest();
            }

            _bankContext.Entry(customer).State = EntityState.Modified;

            try
            {
                await _bankContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        // POST: api/Customers
        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _bankContext.Customers.Add(customer);
            await _bankContext.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = customer.Id }, customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = await _bankContext.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _bankContext.Customers.Remove(customer);
            await _bankContext.SaveChangesAsync();

            return Ok(customer);
        }

        private bool CustomerExists(long id)
        {
            return _bankContext.Customers.Any(e => e.Id == id);
        }
    }
}