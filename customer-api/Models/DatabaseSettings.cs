namespace customer_api.Models
{
    public interface IMongoDBSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }

  
    public class MongoDBSettings : IMongoDBSettings
	{
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        //public string CustomersCollectionName { get; set; } = null!;
    }
}

