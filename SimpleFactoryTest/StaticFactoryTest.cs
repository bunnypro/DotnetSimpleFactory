#define TEST

using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Bunnypro.SimpleFactory.Exceptions;
using Xunit;

namespace Bunnypro.SimpleFactory.Test
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
        public void CreateFromUnregisteredFactoryShouldThrowExcepton()
        {
            Assert.Throws<FactoryNotRegisteredException<Person>>(() => Factory.CreateUnique<Person>(2));
            Assert.Throws<FactoryNotRegisteredException<Person>>(() => Factory.CreateUnique<Person>(2, (r, f) => r));
            Assert.Throws<FactoryNotRegisteredException<Person>>(() => Factory.CreateOne<Person>());
            Assert.Throws<FactoryNotRegisteredException<Person>>(() => Factory.CreateOne<Person>((r, f) => r));
            Assert.Throws<FactoryNotRegisteredException<Person>>(() => Factory.Create<Person>());
            Assert.Throws<FactoryNotRegisteredException<Person>>(() => Factory.Create<Person>((r, f) => r));
            Assert.Throws<FactoryNotRegisteredException<Person>>(() => Factory.Create<Person>(1));
            Assert.Throws<FactoryNotRegisteredException<Person>>(() => Factory.Create<Person>(1, (r, f) => r));
        }

        [Fact]
        public void CreateDataMinimumIsOne()
        {
            RegisterFactory();

            Assert.Throws<MinimumFactoryCreateCountException>(() => Factory.Create<Person>(0).ToArray());
            Assert.Throws<MinimumFactoryCreateCountException>(() => Factory.Create<Person>(0, (r, f) => r).ToArray());
        }

        [Fact]
        public void CreateUniqueDataMinimumIsTwo()
        {
            RegisterFactory();

            Assert.Throws<MinimumFactoryCreateCountException>(() => Factory.CreateUnique<Person>(1).ToArray());
            Assert.Throws<MinimumFactoryCreateCountException>(() => Factory.CreateUnique<Person>(1, (r, f) => r).ToArray());
        }

        [Fact]
        public void RegisterFactoryShouldReturnFactoryClass()
        {
            Assert.IsType<Factory<Person>>(RegisterFactory());
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
            
            var person1 = Factory.Create<Person>();
            
            Assert.IsType<Person>(person1);
            Assert.NotNull(person1.Name);
            Assert.NotNull(person1.Phone);
            Assert.NotNull(person1.Email);
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
            
            var person1 = Factory.Create<Person>((p, faker) =>
            {
                p.Email = email;
                return p;
            });

            Assert.Equal(email, person1.Email);
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