﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SemanticApi.Models
{
    [DataContract]
    public class CategoryMeta
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}