using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using SemanticApi.Models;

namespace SemanticApi.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private MongoClient Client;
        private IMongoDatabase Database;
        private IMongoCollection<Person> Collection;

        public PersonRepository(string connection = null)
        {
            if (string.IsNullOrWhiteSpace(connection))
            {
                connection = "mongodb://admin:admin@ds033125.mlab.com:33125/test-db";
            }

            Client = new MongoClient(connection);
            Database = Client.GetDatabase("test-db");
            Collection = Database.GetCollection<Person>("person");

        }

        public Person AddPerson(Person person)
        {
            person.Id = Guid.NewGuid().ToString();
            Collection.InsertOne(person);

            return person;
        }

        public bool DeletePerson(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Person> GetAllPeople()
        {
            return Collection.Find(new BsonDocument()).ToList();
        }

        public Person GetPerson(string id)
        {
            throw new NotImplementedException();
        }

        public bool UpdatePerson(string id, Person person)
        {
            throw new NotImplementedException();
        }
    }
}