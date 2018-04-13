# SimpleFactory

[![Nuget](https://img.shields.io/nuget/v/Bunnypro.SimpleFactory.svg)](https://www.nuget.org/packages/Bunnypro.SimpleFactory)
[![License](http://img.shields.io/:license-MIT-blue.svg)](https://github.com/bunnypro/DotnetSimpleFactory/blob/master/LICENSE)

This library provide a simple object factory for generating data using Bogus as fake data generator.

**Installation**
```
dotnet add package Bunnypro.SimpleFactory  --version 1.1.1
```

**Usage Example**
```c#
using Bunnypro.SimpleFactory;

// Register Factory
Factory.Register<Person>(faker => new Person(
{
    Name = faker.Name.FullName(),
    Phone = faker.Phone.PhoneNumber(),
    Email = faker.Internet.Email()
}));

// Generate Data
IEnumerable<Person> people = Factory.Create<Person>(4);
Person[] arrayPeople = people.ToArray();
List<Person> listPeople = people.ToList();

// Generate Unique Data
IEnumerable<Person> people = Factory.CreateUnique<Person>(4);

// Generate One Data
Person person = Factory.CreateOne<Person>();

// Extending Data
static const email = "john@doe.com";

IEnumerable<Person> people = Factory.CreateUnique<Person>(4, (person, faker) =>
{
    person.Email = email;

    return person;
});


// Register With Generate Nested Data
Factory.Register<Schedule>(faker => new Schedule(
{
    People = Factory.CreateUnique<Person>(4).ToList(),
    Date = faker.Date
}));

IEnumerable<Schedule> schedules = Factory.Create<Schedule>(10);


// Check Registered Factory Existence
bool personFactoryExists = Factory.Has<Person>();

// Unregister Factory
bool personFactoryRemoved = Factory.Remove<Person>();

// Clear Factory
Factory.Clear();

// Generate Without Register
double[] doubles = Factory.Once<double>(
    faker => Math.Round(faker.Random.Double(2, 4), 2) // generate double with two decimal places
).Create(4).ToArray();


// Or Another Way
Factory<double> DoubleFactory = new Factory<double>(
    faker => Math.Round(faker.Random.Double(2, 4), 2)
);

double[] doubles = DoubleFactory.Create(4).ToArray();

Factory<Person> PersonFactory = new Factory<Person>(faker => new Person(
{
    FullName = faker.Person.FullName,
    Address = faker.Person.Address
}));

// same as
Factory<Person> PersonFactory = Factory.Once<Person>(faker => new Person(
{
    FullName = faker.Person.FullName,
    Address = faker.Person.Address
}));

var people = PersonFactory.Create(100);
```
