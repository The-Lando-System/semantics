using MongoOrm;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace SemanticApi.Models
{
    [DataContract]
    public class Category : MongoModel
    {
        [DataMember]
        [Required]
        public string Name { get; set; }

        [DataMember]
        public IDictionary<string, bool> Words { get; set; }
    }
}