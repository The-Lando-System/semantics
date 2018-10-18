using SemanticApi.Models;
using SemanticApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
            if (category.Words == null)
            {
                category.Words = new Dictionary<string, bool>();
            }

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

        [Route("{id}/add-words")]
        [HttpPost]
        public Category AddWordsToCategory(string id, [FromBody] IEnumerable<WordRequest> words)
        {
            Category category = CategoryRepo.FindById(id);

            foreach(WordRequest wordRequest in words)
            {
                if (!category.Words.ContainsKey(wordRequest.Word))
                {
                    category.Words.Add(wordRequest.Word, true);
                }
            }
            
            if (!CategoryRepo.Update(id, category))
            {
                throw new Exception("Failed to add any words to the category");
            }

            return category;
        }

        [Route("{id}/remove-words")]
        [HttpPost]
        public Category RemoveWordsFromCategory(string id, [FromBody] IEnumerable<WordRequest> words)
        {
            Category category = CategoryRepo.FindById(id);

            foreach (WordRequest wordRequest in words)
            {
                if (category.Words.ContainsKey(wordRequest.Word))
                {
                    category.Words.Remove(wordRequest.Word);
                }
            }

            if (!CategoryRepo.Update(id, category))
            {
                throw new Exception("Failed to remove any words from the category");
            }

            return category;
        }

        [Route("{id}/contains-words")]
        [HttpPost]
        public IEnumerable<string> CategoryContainsWords(string id, [FromBody] IEnumerable<WordRequest> words)
        {
            Category category = CategoryRepo.FindById(id);

            List<string> containedWords = new List<string>();

            foreach (WordRequest wordRequest in words)
            {
                if (category.Words.ContainsKey(wordRequest.Word))
                {
                    containedWords.Add(wordRequest.Word);
                }
            }

            return containedWords;
        }

        [Route("{id}/contains-sentence")]
        [HttpPost]
        public IEnumerable<string> CategoryContainsSentence(string id, [FromBody] SentenceRequest sentenceRequest)
        {
            Category category = CategoryRepo.FindById(id);

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

            return containedWords;
        }
    }
}