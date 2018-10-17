using SemanticApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SemanticApi.Repositories
{
    public class CategoryRepository : MongoRepository<Category>
    {
        public CategoryRepository() : base() { }
    }
}