using System;
using System.Linq;
using Bogus;
using Bunnypro.SimpleFactory;
using Xunit;

namespace SimpleFactoryTest
{
    public class FactoryTest
    {
        public FactoryTest()
        {
            Factory.Clear();
        }

        [Fact]
        public void CanRegisterFactoryAndGetGeneratorBack()
        {
            Func<Faker, string> generator = faker => faker.Random.String();
            Factory.Register<string>(generator);
            Assert.True(Factory.Has<string>());
            Assert.True(generator.Equals(Factory.Generator<string>()));
        }

        [Fact]
        public void CanRemoveFactory()
        {
            Factory.Register<string>(faker => faker.Random.String());
            Assert.True(Factory.Has<string>());
            Factory.Remove<string>();
            Assert.False(Factory.Has<string>());
        }

        [Fact]
        public void CanGenerateData()
        {
            const int count = 3;
            Factory.Register<string>(faker => faker.Random.String());
            var strings = Factory.Create<string>(count).ToArray();
            Assert.True(strings.GetType() == typeof(string[]));
            Assert.True(strings.Length == count);

            Factory.Clear();
            const string str = "Hello";
            var generated = Factory.Once<string>(_ => str).CreateOne();
            Assert.True(str == generated);
        }

        [Fact]
        public void FactoryGeneratorsIsNotAffectedWhenUsingOnce()
        {
            Factory.Once<string>(faker => faker.Random.String());
            Assert.False(Factory.Has<string>());
        }

        [Fact]
        public void CanGenerateUniqueData()
        {
            var strings = Factory.Once<string>(faker => faker.Random.String()).CreateUnique(10).ToArray();
            Assert.True(strings.Distinct().ToArray().Length == strings.Length);
        }

        [Fact]
        public void CanExtendData()
        {
            const string name = "John";
            var personFactory = Factory.Once<Person>(faker => new Person
            {
                Name = faker.Person.FullName,
                Email = faker.Person.Email
            });

            var people = personFactory.Create(10, (person, faker) =>
            {
                person.Name = name;

                return person;
            });

            Assert.True(people.All(p => p.Name == name));

            var per = personFactory.CreateOne((person, faker) =>
            {
                person.Name = name;

                return person;
            });

            Assert.True(name == per.Name);
        }

        [Fact]
        public void StaticFactoryAndFactoryClassShouldReturnSameGenerator()
        {
            Func<Faker, string> generator = faker => faker.Random.String();
            Factory.Register<string>(generator);
            var staticFactory = Factory.Once<string>(generator);
            var instantiatedFactory = new Factory<string>(generator);

            Assert.True(
                generator == Factory.Generator<string>() &&
                generator == staticFactory.Generator &&
                generator == instantiatedFactory.Generator
            );
        }
    }

    internal struct Person
    {
        public string Name;
        public string Email;
    }
}