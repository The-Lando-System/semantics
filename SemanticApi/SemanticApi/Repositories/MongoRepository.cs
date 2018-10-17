using MongoDB.Bson;
using MongoDB.Driver;
using SemanticApi.Models;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace SemanticApi.Repositories
{
    public class MongoRepository<T> where T : MongoModel
    {
        internal MongoClient Client;
        internal IMongoDatabase Database;
        internal IMongoCollection<T> Collection;

        public MongoRepository()
        {
            Client = new MongoClient(Environment.GetEnvironmentVariable(ConfigurationManager.AppSettings["ConnectionEnvVar"]));
            Database = Client.GetDatabase(ConfigurationManager.AppSettings["DatabaseName"]);
            Collection = Database.GetCollection<T>(typeof(T).Name);
        }

        public virtual T Add(T item)
        {
            item.Id = Guid.NewGuid().ToString();
            Collection.InsertOne(item);
            return item;
        }

        public virtual bool Delete(string id)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            DeleteResult dr = Collection.DeleteOne(filter);
            return dr.IsAcknowledged && dr.DeletedCount > 0;
        }

        public virtual IEnumerable<T> FindAll()
        {
           return Collection.Find(new BsonDocument()).ToList();
        }

        public virtual T FindById(string id)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            return Collection.Find(filter).FirstOrDefault();
        }

        public virtual bool Update(string id, T item)
        {
            throw new NotImplementedException();
        }
    }
}