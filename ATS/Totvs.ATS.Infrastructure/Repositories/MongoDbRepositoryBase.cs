using System.Linq.Expressions;
using MongoDB.Driver;
using Totvs.ATS.Domain.Interfaces;
using Totvs.ATS.Infrastructure.Context;

namespace Totvs.ATS.Infrastructure.Repositories;

public class MongoDbRepositoryBase<T> : IMongoDbRepositoryBase<T>  where T : class
{
    private readonly IMongoCollection<T> _collection;

    public MongoDbRepositoryBase(MongoDbContext context, string collectionName)
    {
        _collection = context.GetDatabase().GetCollection<T>(collectionName);
    }
    
    public async Task<IEnumerable<T>> GetAllAsync()
        => await _collection.Find(_ => true).ToListAsync();

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        => await _collection.Find(predicate).ToListAsync();

    public async Task<T?> FindByIdAsync(string id)
    {
        var filter = Builders<T>.Filter.Eq("_id", id);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public Task AddAsync(T entity)
        => _collection.InsertOneAsync(entity);

    public Task UpdateAsync(string id, T entity)
    {
        var filter = Builders<T>.Filter.Eq("_id", id);
        return _collection.ReplaceOneAsync(filter, entity);
    }

    public Task DeleteAsync(string id)
    {
        var filter = Builders<T>.Filter.Eq("_id", id);
        return _collection.DeleteOneAsync(filter);
    }
}