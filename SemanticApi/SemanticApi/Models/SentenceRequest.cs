using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SemanticApi.Models
{
    [DataContract]
    public class SentenceRequest
    {
        [DataMember]
        public string Sentence { get; set; } 
    }
}