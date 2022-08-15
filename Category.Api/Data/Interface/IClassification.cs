using Category.Api.Entities;
using MongoDB.Driver;

namespace Category.Api.Data.Interface
{
    public interface IClassification
    {
        IMongoCollection<Classification> Classifications { get; }
    }
}
