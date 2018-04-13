using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;

namespace Bunnypro.SimpleFactory
{
    public class Factory<T>
    {
        private readonly Func<Faker, object> _generator;
        private readonly Faker _faker;

        public Factory(Func<Faker, object> generator)
        {
            _generator = generator;
            _faker = new Faker();
        }

        public Func<Faker, object> Generator => _generator;

        public T CreateOne(Func<T, Faker, T> extender)
        {
            return extender(CreateOne(), _faker);
        }

        public T CreateOne()
        {
            return Create().First();
        }

        public IEnumerable<T> Create(int count, Func<T, Faker, T> extender)
        {
            return Create(count).Select(o => extender(o, _faker));
        }

        public IEnumerable<T> Create(int count = 1)
        {
            if (count < 1)
            {
                throw new Exception("Minimum Factory Create is One");
            }

            return Enumerable.Range(0, count).Select(_ => (T)_generator(_faker));
        }

        public IEnumerable<T> CreateUnique(int count, Func<T, Faker, T> extender)
        {
            return CreateUnique(count).Select(o => extender(o, _faker));
        }

        public IEnumerable<T> CreateUnique(int count = 1)
        {
            if (count < 1)
            {
                throw new Exception("Minimum Factory Create is One");
            }

            var data = new List<T>();

            T GenerateUnique(Func<Faker, object> generator)
            {
                var o = (T)generator(_faker);

                return data.Contains(o) ? GenerateUnique(generator) : o;
            }

            for (var i = 0; i < count; i++)
            {
                data.Add(GenerateUnique(_generator));
            }

            return data;
        }
    }

    public class Factory
    {
        private static readonly Dictionary<Type, Func<Faker, object>> Generators = new Dictionary<Type, Func<Faker, object>>();

        public static Factory<T> Register<T>(Func<Faker, object> generator)
        {
            Generators.Add(typeof(T), generator);

            return Once<T>(generator);
        }

        public static bool Has<T>()
        {
            return Generators.ContainsKey(typeof(T));
        }

        public static Func<Faker, object> Generator<T>()
        {
            try
            {
                return Generators[typeof(T)];
            }
            catch (KeyNotFoundException e)
            {
                throw new Exception("Factory for " + typeof(T) + " is not registered", e);
            }
        }
        
        public static bool Remove<T>()
        {
            try
            {
                return Generators.Remove(typeof(T));
            }
            catch (KeyNotFoundException e)
            {
                throw new Exception("Factory for " + typeof(T) + " is not registered", e);
            }
        }

        public static void Clear()
        {
            Generators.Clear();
        }

        public static T CreateOne<T>(Func<T, Faker, T> extender)
        {
            try
            {
                return Once<T>(Generators[typeof(T)]).CreateOne(extender);
            }
            catch (KeyNotFoundException e)
            {
                throw new Exception("Factory for " + typeof(T) + " is not registered", e);
            }
        }

        public static T CreateOne<T>()
        {
            try
            {
                return Once<T>(Generators[typeof(T)]).CreateOne();
            }
            catch (KeyNotFoundException e)
            {
                throw new Exception("Factory for " + typeof(T) + " is not registered", e);
            }
        }

        public static IEnumerable<T> Create<T>(int count, Func<T, Faker, T> extender)
        {
            try
            {
                return Once<T>(Generators[typeof(T)]).Create(count, extender);
            }
            catch (KeyNotFoundException e)
            {
                throw new Exception("Factory for " + typeof(T) + " is not registered", e);
            }
        }

        public static IEnumerable<T> Create<T>(int count = 1)
        {
            try
            {
                return Once<T>(Generators[typeof(T)]).Create(count);
            }
            catch (KeyNotFoundException e)
            {
                throw new Exception("Factory for " + typeof(T) + " is not registered", e);
            }
        }

        public static IEnumerable<T> CreateUnique<T>(int count, Func<T, Faker, T> extender)
        {
            try
            {
                return Once<T>(Generators[typeof(T)]).CreateUnique(count, extender);
            }
            catch (KeyNotFoundException e)
            {
                throw new Exception("Factory for " + typeof(T) + " is not registered", e);
            }
        }

        public static IEnumerable<T> CreateUnique<T>(int count = 1)
        {
            try
            {
                return Once<T>(Generators[typeof(T)]).CreateUnique(count);
            }
            catch (KeyNotFoundException e)
            {
                throw new Exception("Factory for " + typeof(T) + " is not registered", e);
            }
        }

        public static Factory<T> Once<T>(Func<Faker, object> generator)
        {
            return new Factory<T>(generator);
        }
    }
}
