using System;

namespace Bunnypro.SimpleFactory.Exceptions
{
    public class MinimumFactoryCreateCountException : Exception
    {
        public MinimumFactoryCreateCountException(int minimum, int count) :
            base("Minimum Factory Create Count is " + minimum + ", Given: " + count)
        {
        }

        public static void Assert(int minimum, int count)
        {
            if (count < minimum)
            {
                throw new MinimumFactoryCreateCountException(minimum, count);
            }
        }
    }
}