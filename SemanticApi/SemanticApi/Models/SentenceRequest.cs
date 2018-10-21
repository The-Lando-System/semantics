using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SemanticApi.Models
{
    [DataContract]
    public class SentenceRequest
    {
        [DataMember]
        [Required]
        public string Sentence { get; set; } 
    }
}