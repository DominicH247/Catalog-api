using System.Threading.Tasks;
using System.Collections.Immutable;
using System;
using System.Collections.Generic;
using Catalog.Api.Entities;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Catalog.Api.Repositories
{
    public class MongoDbItemsRepository : IItemsRepository
    {
        private const string DATABASE_NAME = "catalog";
        private const string COLLECTION_NAME = "items";
        private readonly IMongoCollection<Item> _itemsCollection;

        private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;

        public MongoDbItemsRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(DATABASE_NAME);
            _itemsCollection = database.GetCollection<Item>(COLLECTION_NAME);
        }
        public async Task CreateItemAsync(Item item)
        {
            await _itemsCollection.InsertOneAsync(item);
        }

        public async Task DeleteItemAsync(Guid id)
        {
            var filter = filterBuilder.Eq(item => item.Id, id);
            await _itemsCollection.DeleteOneAsync(filter);
        }

        public async Task<Item> GetItemAsync(Guid id)
        {
            var filter = filterBuilder.Eq(item => item.Id, id);
            return await _itemsCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Item>> GetItemsAsync()
        {
            return await _itemsCollection
                .Find(new BsonDocument())
                .ToListAsync();
        }

        public async Task UpdateItemAsync(Item item)
        {
            var filter = filterBuilder.Eq(existingItem => existingItem.Id, item.Id);
            await _itemsCollection.ReplaceOneAsync(filter, item);
        }
    }
}
