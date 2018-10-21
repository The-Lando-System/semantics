using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

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