using SemanticApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SemanticApi.Repositories
{
    public interface IPersonRepository
    {
        IEnumerable<Person> GetAllPeople();

        Person GetPerson(string id);

        Person AddPerson(Person person);

        bool DeletePerson(string id);

        bool UpdatePerson(string id, Person person);

    }
}