using System;

namespace SimpleFactoryTest
{
    public class Person : IEquatable<Person>
    {
        public string Name;
        public string Phone;
        public string Email;

        public bool Equals(Person other)
        {
            return string.Equals(Name, other.Name) && string.Equals(Phone, other.Phone) &&
                   string.Equals(Email, other.Email);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Person && Equals((Person) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Phone != null ? Phone.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Email != null ? Email.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}