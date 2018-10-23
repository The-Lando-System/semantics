using MongoOrm;
using SemanticApi.Models;

namespace SemanticApi.Repositories
{
    public class CategoryRepository : MongoRepository<Category>
    {
        public CategoryRepository() : base() { }
    }
}