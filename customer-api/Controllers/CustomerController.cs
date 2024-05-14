
using customer_api.Models;
using customer_api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace customer_api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerService _customerService;

        public CustomerController(CustomerService customerService) =>
        _customerService = customerService;

        [HttpGet]
        public async Task<List<Customer>> Get() =>
            await _customerService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Customer>> Get(string id)
        {
            var Customer = await _customerService.GetAsync(id);

            if (Customer is null)
            {
                return NotFound();
            }

            return Customer;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Customer newCustomer)
        {
            await _customerService.CreateAsync(newCustomer);

            return CreatedAtAction(nameof(Get), new { id = newCustomer.Id }, newCustomer);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Customer updatedCustomer)
        {
            var Customer = await _customerService.GetAsync(id);

            if (Customer is null)
            {
                return NotFound();
            }

            updatedCustomer.Id = Customer.Id;

            await _customerService.UpdateAsync(id, updatedCustomer);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var Customer = await _customerService.GetAsync(id);

            if (Customer is null)
            {
                return NotFound();
            }

            await _customerService.RemoveAsync(id);

            return NoContent();
        }
    }
}

