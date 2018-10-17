using MongoDB.Bson.Serialization.Attributes;
using System.Runtime.Serialization;

namespace SemanticApi.Models
{
    [DataContract]
    public class MongoModel
    {
        [DataMember]
        [BsonId]
        public string Id { get; set; }
    }
}