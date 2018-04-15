using System;

namespace Bunnypro.SimpleFactory.Exceptions
{
    public class FactoryNotRegisteredException<T> : Exception
    {
        public FactoryNotRegisteredException() : base("Factory for " + typeof(T) + " is not registered") {}
        public FactoryNotRegisteredException(Exception e) : base("Factory for " + typeof(T) + " is not registered", e) {}
    }
}