using SemanticApi.Models;
using SemanticApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SemanticApi.Controllers
{
    [RoutePrefix("categories")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CategoryController : ApiController
    {
        private CategoryRepository CategoryRepo = new CategoryRepository();

        [Route("")]
        [HttpPost]
        public Category CreateCategory([FromBody] Category category)
        {
            return CategoryRepo.Add(category);
        }

        [Route("")]
        [HttpGet]
        public IEnumerable<Category> GetAllCategories()
        {
            return CategoryRepo.FindAll();
        }

        [Route("{id}")]
        [HttpGet]
        public Category GetCategoryById(string id)
        {
            return CategoryRepo.FindById(id);
        }

        [Route("{id}")]
        [HttpDelete]
        public bool DeleteCategory(string id)
        {
            return CategoryRepo.Delete(id);
        }
    }
}