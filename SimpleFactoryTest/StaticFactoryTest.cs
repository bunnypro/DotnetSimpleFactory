using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Bunnypro.SimpleFactory;
using Xunit;

namespace SimpleFactoryTest
{
    public class StaticFactoryTest
    {
        public StaticFactoryTest()
        {
            Factory.Clear();
        }

        private Func<Faker, Person> CreateGenerator => faker => new Person
        {
            Name = faker.Name.FullName(),
            Phone = faker.Phone.PhoneNumber(),
            Email = faker.Internet.Email()
        };

        private Factory<Person> RegisterFactory() => Factory.Register(CreateGenerator);

        [Fact]
        public void InitialFactoryShouldBeEmpty()
        {
            Assert.True(Factory.IsEmpty());
        }

        [Fact]
        public void StaticFactoryCanBeCleared()
        {
            RegisterFactory();
            Factory.Clear();
            
            Assert.True(Factory.IsEmpty());
        }

        [Fact]
        public void CanRegisterFactory()
        {
            RegisterFactory();
            
            Assert.True(Factory.Has<Person>());
        }

        [Fact]
        public void CanRemoveRegisteredFactory()
        {
            RegisterFactory();
            Assert.True(Factory.Remove<Person>());
            Assert.False(Factory.Has<Person>());
        }

        [Fact]
        public void RegisterFactoryShouldReturnFactoryClass()
        {
            var factory = RegisterFactory();

            Assert.IsType<Factory<Person>>(factory);
        }

        [Fact]
        public void CanCreateOneDataFromRegisteredFactory()
        {
            RegisterFactory();
            var person = Factory.CreateOne<Person>();
            
            Assert.IsType<Person>(person);
            Assert.NotNull(person.Name);
            Assert.NotNull(person.Phone);
            Assert.NotNull(person.Email);
        }
        
        [Fact]
        public void CanCreateOneExtendedDataFromRegisteredFactory()
        {
            RegisterFactory();
            const string email = "john@doe.com";
            var person = Factory.CreateOne<Person>((p, faker) =>
            {
                p.Email = email;
                return p;
            });

            Assert.Equal(email, person.Email);
        }

        [Fact]
        public void CanCreateManyValidDataFromRegisteredFactory()
        {
            RegisterFactory();
            var people = Factory.Create<Person>(5);
            
            Assert.IsType<Person[]>(people.ToArray());
            Assert.IsType<List<Person>>(people.ToList());
            Assert.True(people.All(person => person.Name != null && person.Email != null && person.Phone != null));
        }

        [Fact]
        public void CanCreateManyExtendedDataFromRegisteredFactory()
        {
            RegisterFactory();
            const string email = "john@doe.com";
            var people = Factory.Create<Person>(5, (person, faker) =>
            {
                person.Email = email;
                return person;
            });
            
            Assert.True(people.All(person => person.Email == email));
        }

        [Fact]
        public void CanCreateUniqueDataFromRegisteredFactory()
        {
            RegisterFactory();
            var people = Factory.CreateUnique<Person>(5);
            
            Assert.True(people.Distinct().Count() == people.Count());
        }
        
        [Fact]
        public void CanCreateUniqueExtendedDataFromRegisteredFactory()
        {
            RegisterFactory();
            const string email = "john@doe.com";
            var people = Factory.CreateUnique<Person>(5, (person, faker) =>
            {
                person.Email = email;
                return person;
            });
            
            Assert.True(people.Distinct().Count() == people.Count());
            Assert.True(people.All(person => person.Email == email));
        }

        [Fact]
        public void CanCreateFactoryWithoutRegiteringToStaticFactory()
        {
            var factory = Factory.Once(CreateGenerator);

            Assert.False(Factory.Has<Person>());
            Assert.IsType<Factory<Person>>(factory);
        }
    }
}