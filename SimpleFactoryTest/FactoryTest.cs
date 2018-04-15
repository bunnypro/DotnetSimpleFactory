#define TEST

using System;
using System.Linq;
using Bogus;
using Bunnypro.SimpleFactory;
using Xunit;

namespace SimpleFactoryTest
{
    public class FactoryTest
    {
        private Func<Faker, Person> CreatePersonGenerator() => faker => new Person
        {
            Name = faker.Name.FullName(),
            Phone = faker.Phone.PhoneNumber(),
            Email = faker.Internet.Email()
        };

        private Factory<Person> CreatePersonFactory() => new Factory<Person>(CreatePersonGenerator());

        [Fact]
        public void CanCreateOneValidData()
        {
            var personFactory = CreatePersonFactory();
            var person = personFactory.CreateOne();

            Assert.IsType<Person>(person);
            Assert.NotNull(person.Name);
            Assert.NotNull(person.Phone);
            Assert.NotNull(person.Email);

            var person1 = personFactory.Create();
            
            Assert.IsType<Person>(person1);
            Assert.NotNull(person1.Name);
            Assert.NotNull(person1.Phone);
            Assert.NotNull(person1.Email);
        }

        [Fact]
        public void CanCreateOneExtendedData()
        {
            var personFactory = CreatePersonFactory();
            const string email = "john@doe.com";
            var person = personFactory.CreateOne((p, faker) =>
            {
                p.Email = email;
                return p;
            });

            Assert.Equal(email, person.Email);
            
            var person1 = personFactory.Create((p, faker) =>
            {
                p.Email = email;
                return p;
            });

            Assert.Equal(email, person1.Email);
        }

        [Fact]
        public void CanCreateManyValidData()
        {
            var personFactory = CreatePersonFactory();
            var people = personFactory.Create(5);
            
            Assert.IsType<Person[]>(people.ToArray());
            Assert.True(people.All(person => person.Name != null && person.Email != null && person.Phone != null));
        }

        [Fact]
        public void CanCreateManyExtendedData()
        {
            var personFactory = CreatePersonFactory();
            const string email = "john@doe.com";
            var people = personFactory.Create(5, (person, faker) =>
            {
                person.Email = email;
                return person;
            });
            
            Assert.True(people.All(person => person.Email == email));
        }

        [Fact]
        public void CanCreateUniqueData()
        {
            var personFactory = CreatePersonFactory();
            var people = personFactory.CreateUnique(5);
            
            Assert.True(people.Distinct().Count() == people.Count());
        }
        
        [Fact]
        public void CanCreateUniqueExtendedData()
        {
            var personFactory = CreatePersonFactory();
            const string email = "john@doe.com";
            var people = personFactory.CreateUnique(5, (person, faker) =>
            {
                person.Email = email;
                return person;
            });
            
            Assert.True(people.Distinct().Count() == people.Count());
            Assert.True(people.All(person => person.Email == email));
        }
    }
}