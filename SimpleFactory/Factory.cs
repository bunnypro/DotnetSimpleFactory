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

        public IEnumerable<T> Create(int count, Func<T, Faker, T> extender) => Create(count).Select(o => extender(o, _faker));

        public IEnumerable<T> Create(int count)
        {
            if (count < 1)
            {
                throw new Exception("Minimum Factory Create is One");
            }

            return Enumerable.Range(0, count).Select(_ => _generator(_faker));
        }

        public IEnumerable<T> CreateUnique(int count, Func<T, Faker, T> extender) => CreateUnique(count).Select(o => extender(o, _faker));

        public IEnumerable<T> CreateUnique(int count)
        {
            if (count < 1)
            {
                throw new Exception("Minimum Factory Create is One");
            }

            var data = new T[count];

            T GenerateUnique()
            {
                var o = CreateOne();

                return data.Contains(o) ? GenerateUnique() : o;
            }

            for (var i = 0; i < count; i++)
            {
                data[i] = GenerateUnique();
            }

            return data.AsEnumerable();
        }

        public T CreateOne(Func<T, Faker, T> extender) => extender(CreateOne(), _faker);

        public T CreateOne() => _generator(_faker);
    }
}
