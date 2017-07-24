using System.Collections.Generic;
using ReativeUI_GridView.Model;

namespace ReativeUI_GridView.Service
{
    public class SampleService : IService<Person>
    {
        public IEnumerable<Person> Get()
        {
            return new List<Person>()
            {
                new Person("Peter","Pan", 12),
                new Person("Steve","Jobs",60),
                new Person("Bill","Gate",50),
                new Person("Michael","Jackson",50),
                new Person("Walt","Disney",80)
            };
        }
    }
}