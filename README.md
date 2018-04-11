# Dotnet Simple Object Factory

This library provide a simple object factory for generating data using Bogus as fake data generator.

```c#
using Bunnypro.SimpleFactory;

// Register Factory
Factory.Register<Person>(faker => new Person(
{
    FullName = faker.Person.FullName,
    Address = faker.Person.Address
}));

// Generate Data
var people = Factory.Create<Person>(4); // return IEnumerable<Person>

// Generate One Data
var person = Factory.CreateOne<Person>(); // return Person

// Extending Data
var people = Factory.Create<Person>(4, (person, faker) =>
{
    person.Email = faker.Person.Email;

    return person;
});

// Register With Generate Nested Data
Factory.Register<Schedule>(faker => new Schedule(
{
    People = Factory.Create<Person>(4).ToList(),
    Date = faker.Date
}));
var schedules = Factory.Create<Schedule>(10);


// Unregister Factory
Factory.Remove<Person>();

// Generate Without Register
Factory
    .Once<double>(
        faker => Math.Round(faker.Random.Double(2, 4), 2)
    ).Create(4).ToArray(); // Generate 4 double in two decimal places


// Or Another Way
var DoubleFactory = new Factory<double>(
    faker => Math.Round(faker.Random.Double(2, 4), 2)
);

var doubles = DoubleFactory.Create(4).ToArray();

var PersonFactory = new Factory<Person>(faker => new Person(
{
    FullName = faker.Person.FullName,
    Address = faker.Person.Address
}));

// same as

var PersonFactory = Factory.Once<Person>(faker => new Person(
{
    FullName = faker.Person.FullName,
    Address = faker.Person.Address
}));

var people = PersonFactory.Create(100);
```