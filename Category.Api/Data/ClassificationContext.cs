using Category.Api.Data.Interface;
using Category.Api.Entities;
using MongoDB.Driver;

namespace Category.Api.Data
{
    public class ClassificationContext : IClassification
    {
        public ClassificationContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));

            Classifications = database.GetCollection<Classification>(configuration.GetValue<string>("DatabaseSettings:CollectionName"));
        }

        public IMongoCollection<Classification> Classifications { get; }
    }
}
