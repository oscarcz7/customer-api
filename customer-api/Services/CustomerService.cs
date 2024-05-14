using customer_api.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace customer_api.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IMongoCollection<Customer> _customerCollection;

        public CustomerService(IMongoDBSettings settings)
        {
            var mongoClient = new MongoClient(
                settings.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                settings.DatabaseName);
            _customerCollection = mongoDatabase.GetCollection<Customer>("CustomerCollection");
        }

        public async Task<List<Customer>> GetAsync() =>
            await _customerCollection.Find(_ => true).ToListAsync();

        public async Task<Customer?> GetAsync(string id) =>
            await _customerCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Customer newCustomer) =>
            await _customerCollection.InsertOneAsync(newCustomer);

        public async Task UpdateAsync(string id, Customer updatedCustomer) =>
            await _customerCollection.ReplaceOneAsync(x => x.Id == id, updatedCustomer);

        public async Task RemoveAsync(string id) =>
            await _customerCollection.DeleteOneAsync(x => x.Id == id);
    }
}

