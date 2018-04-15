using System;
using System.Collections.Generic;
using Bogus;
using Bunnypro.SimpleFactory.Exceptions;

namespace Bunnypro.SimpleFactory
{
    public static class Factory
    {
        private static readonly Dictionary<Type, Func<Faker, object>> Generators = new Dictionary<Type, Func<Faker, object>>();

        public static Factory<T> Register<T>(Func<Faker, T> generator)
        {
            Generators.Add(typeof(T), generator as Func<Faker, object>);

            return Once(generator);
        }

        public static bool Has<T>()
        {
            return Generators.ContainsKey(typeof(T));
        }

        private static Func<Faker, T> Generator<T>()
        {
            try
            {
                return Generators[typeof(T)] as Func<Faker, T>;
            }
            catch (KeyNotFoundException e)
            {
                throw new FactoryNotRegisteredException<T>(e);
            }
        }

        public static bool IsEmpty()
        {
            return Generators.Count == 0;
        }
        
        public static bool Remove<T>()
        {
            try
            {
                return Generators.Remove(typeof(T));
            }
            catch (KeyNotFoundException e)
            {
                throw new FactoryNotRegisteredException<T>(e);
            }
        }

        /// <summary>
        /// Used only for testing. This method can cause unexpected behaviour
        /// </summary>
        [Obsolete("Static Factory Clear() method is deprecated and can be only used in TEST mode in future MINOR release")]
        public static void Clear()
        {
            Generators.Clear();
        }

        public static T CreateOne<T>(Func<T, Faker, T> extender)
        {
            return Once(Generator<T>()).CreateOne(extender);
        }

        public static T CreateOne<T>()
        {
            return Once(Generator<T>()).CreateOne();
        }

        public static IEnumerable<T> Create<T>(int count, Func<T, Faker, T> extender)
        {
            return Once(Generator<T>()).Create(count, extender);
        }

        public static IEnumerable<T> Create<T>(int count)
        {
            return Once(Generator<T>()).Create(count);
        }

        public static T Create<T>(Func<T, Faker, T> extender)
        {
            return Once(Generator<T>()).Create(extender);
        }

        public static T Create<T>()
        {
            return Once(Generator<T>()).Create();
        }

        public static IEnumerable<T> CreateUnique<T>(int count, Func<T, Faker, T> extender)
        {
            return Once(Generator<T>()).CreateUnique(count, extender);
        }

        public static IEnumerable<T> CreateUnique<T>(int count)
        {
            return Once(Generator<T>()).CreateUnique(count);
        }

        public static Factory<T> Once<T>(Func<Faker, T> generator)
        {
            return new Factory<T>(generator);
        }
    }
}