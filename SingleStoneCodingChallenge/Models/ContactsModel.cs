
using System.Diagnostics.CodeAnalysis;

namespace SingleStoneCodingChallenge.Models
{
    [ExcludeFromCodeCoverage]
    public class ContactsModel
    {
        public Name Name { get; set; }
        public Address Address { get; set; }
        public Phone[] Phone { get; set; }
        public string Email { get; set; }

    }
    [ExcludeFromCodeCoverage]
    public class Name
    {
        public string First { get; set; }
        public string Middle { get; set; }
        public string Last { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int Zip { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Phone
    {
        public string Number { get; set; }
        public string Type { get; set; }
    }
}