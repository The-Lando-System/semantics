using SemanticApi.Models;
using SemanticApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Net;
using System.Net.Http;
using SemanticApi.Filters;

namespace SemanticApi.Controllers
{
    [RoutePrefix("categories")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RequestValidator]
    public class CategoryController : ApiController
    {
        private CategoryRepository CategoryRepo = new CategoryRepository();

        [Route("")]
        [HttpPost]
        public IHttpActionResult CreateCategory([FromBody] Category category)
        {
            if (category.Words == null)
            {
                category.Words = new Dictionary<string, bool>();
            }

            if (CategoryRepo.FindByProperty("Name",category.Name) != null)
            {
                
                return BadRequest($"Category with name [{category.Name}] already exists");
            }

            Category createdCategory = null;

            try
            {
                createdCategory = CategoryRepo.Add(category);
            }
            catch (Exception e)
            {
                Exception exception = new Exception("Failed to create new category", e);
                return InternalServerError(exception);
            }

            if (createdCategory == null)
            {
                Exception exception = new Exception("Failed to create new category");
                return InternalServerError(exception);
            }

            return Ok(createdCategory);
        }

        [Route("")]
        [HttpGet]
        public IHttpActionResult GetAllCategoryIdsAndNames()
        {
            return Ok(
                CategoryRepo.FindAll().Select(category => new CategoryMeta { Id = category.Id, Name = category.Name })
            );
        }

        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult GetCategoryById(string id)
        {
            return Ok(CategoryRepo.FindById(id));
        }

        [Route("{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteCategory(string id)
        {
            return Ok(CategoryRepo.Delete(id));
        }

        [Route("{id}/add-words")]
        [HttpPost]
        public IHttpActionResult AddWordsToCategory(string id, [FromBody] IEnumerable<WordRequest> words)
        {
            Category category = CategoryRepo.FindById(id);

            if (category == null)
            {
                return BadRequest($"Could not find category with ID of [{id}]");
            }

            foreach(WordRequest wordRequest in words)
            {
                if (!category.Words.ContainsKey(wordRequest.Word))
                {
                    category.Words.Add(wordRequest.Word, true);
                }
            }
            
            if (!CategoryRepo.Update(id, category))
            {
                InternalServerError(new Exception("Failed to add any words to the category"));
            }

            return Ok(category);
        }

        [Route("{id}/remove-words")]
        [HttpPost]
        public IHttpActionResult RemoveWordsFromCategory(string id, [FromBody] IEnumerable<WordRequest> words)
        {
            Category category = CategoryRepo.FindById(id);

            if (category == null)
            {
                return BadRequest($"Could not find category with ID of [{id}]");
            }

            foreach (WordRequest wordRequest in words)
            {
                if (category.Words.ContainsKey(wordRequest.Word))
                {
                    category.Words.Remove(wordRequest.Word);
                }
            }

            if (!CategoryRepo.Update(id, category))
            {
                InternalServerError(new Exception("Failed to remove any words from the category"));
            }

            return Ok(category);
        }

        [Route("{id}/contains-words")]
        [HttpPost]
        public IHttpActionResult CategoryContainsWords(string id, [FromBody] IEnumerable<WordRequest> words)
        {
            Category category = CategoryRepo.FindById(id);

            if (category == null)
            {
                return BadRequest($"Could not find category with ID of [{id}]");
            }

            List<string> containedWords = new List<string>();

            foreach (WordRequest wordRequest in words)
            {
                if (category.Words.ContainsKey(wordRequest.Word))
                {
                    containedWords.Add(wordRequest.Word);
                }
            }

            return Ok(containedWords);
        }

        [Route("{id}/contains-sentence")]
        [HttpPost]
        public IHttpActionResult CategoryContainsSentence(string id, [FromBody] SentenceRequest sentenceRequest)
        {
            Category category = CategoryRepo.FindById(id);

            if (category == null)
            {
                return BadRequest($"Could not find category with ID of [{id}]");
            }

            List<string> containedWords = new List<string>();

            Regex regex = new Regex(@"\s");
            string[] sentence = regex.Split(sentenceRequest.Sentence.Trim());

            foreach (string word in sentence)
            {
                if (category.Words.ContainsKey(word))
                {
                    containedWords.Add(word);
                }
            }

            return Ok(containedWords);
        }
    }
}