using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SemanticApi.Models
{
    [DataContract]
    public class WordRequest
    {
        [DataMember]
        public string Word { get; set; }
    }
}