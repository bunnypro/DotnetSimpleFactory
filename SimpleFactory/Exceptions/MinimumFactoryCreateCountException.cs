using System;

namespace Bunnypro.SimpleFactory.Exceptions
{
    public class MinimumFactoryCreateCountException : Exception
    {
        public MinimumFactoryCreateCountException(int minimum, int count) :
            base("Minimum Factory Create Count is " + minimum + ", Given: " + count)
        {
        }
    }
}