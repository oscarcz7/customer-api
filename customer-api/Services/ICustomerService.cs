using customer_api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace customer_api.Services
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetAsync();
        Task<Customer?> GetAsync(string id);
        Task CreateAsync(Customer newCustomer);
        Task UpdateAsync(string id, Customer updatedCustomer);
        Task RemoveAsync(string id);
    }
}
