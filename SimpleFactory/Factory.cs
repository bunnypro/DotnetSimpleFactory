using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;

namespace Bunnypro.SimpleFactory
{
    public class Factory<T>
    {
        private readonly Func<Faker, T> _generator;
        private readonly Faker _faker;

        public Factory(Func<Faker, T> generator)
        {
            _generator = generator;
            _faker = new Faker();
        }

        public IEnumerable<T> CreateUnique(int count) => CreateUnique(count, (r, f) => r);

        public IEnumerable<T> CreateUnique(int count, Func<T, Faker, T> extender)
        {
            AssertMinimumCreate(2, count);

            var data = new T[count];

            T GenerateUnique()
            {
                var o = Create();

                return data.Contains(o) ? GenerateUnique() : o;
            }

            for (var i = 0; i < count; i++)
            {
                data[i] = extender(GenerateUnique(), _faker);
            }

            return data.AsEnumerable();
        }

        public T CreateOne() => Create();

        public T CreateOne(Func<T, Faker, T> extender) => Create(extender);

        public IEnumerable<T> Create(int count) => Create(count, (r, f) => r);

        public IEnumerable<T> Create(int count, Func<T, Faker, T> extender)
        {
            AssertMinimumCreate(1, count);

            return Enumerable.Range(0, count).Select(_ => Create(extender));
        }

        public T Create() => Create((r, f) => r);
        
        public T Create(Func<T, Faker, T> extender) => extender(_generator(_faker), _faker);

        private static void AssertMinimumCreate(int minimum, int count)
        {
            if (count < minimum)
            {
                throw new Exception("Minimum Factory Create is " + minimum + ", given: " + count);
            }
        }
    }
}
