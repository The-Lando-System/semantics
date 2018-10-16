using SemanticApi.Models;
using SemanticApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SemanticApi.Controllers
{
    [RoutePrefix("test")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TestController : ApiController
    {
        private static readonly IPersonRepository PersonRepository = new PersonRepository();

        [Route("post")]
        [HttpPost]
        public Person TestPostRoute([FromBody] Person person)
        {
            return PersonRepository.AddPerson(person);
        }

        [Route("getAll")]
        [HttpGet]
        public IEnumerable<Person> TestGetAllRoute()
        {
            return PersonRepository.GetAllPeople();
        }
    }
}
