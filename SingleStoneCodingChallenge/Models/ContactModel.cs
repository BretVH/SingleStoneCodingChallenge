using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Diagnostics.CodeAnalysis;

namespace SingleStoneCodingChallenge.Models
{
    [ExcludeFromCodeCoverage]
    [Table("Contact")]
    public class RawContact
    {
        [Key]
        public int Id { get; set; }
        public string First { get; set; }
        public string Middle { get; set; }
        public string Last { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int Zip { get; set; }
        public string MobileNumber { get; set; }
        public string MobileType { get; set; }
        public string HomeNumber { get; set; }
        public string HomeType { get; set; }
        public string WorkNumber { get; set; }
        public string WorkType { get; set; }
        public string Email { get; set; } 

    }

    [ExcludeFromCodeCoverage]
    [NotMapped]
    public class ContactWithId
    {
        public int Id { get; set; }
        public Name Name { get; set; }
        public Address Address { get; set; }
        public Phone[] Phone { get; set; }
        public string EMail { get; set; }
    }

    [ExcludeFromCodeCoverage]
    [NotMapped]
    public class Contact
    {
        public Name Name { get; set; }
        public Address Address { get; set; }
        public Phone[] Phone { get; set; }
        public string EMail { get; set; }
    }

    [ExcludeFromCodeCoverage]
    [NotMapped]
    public class Name
    {
        public string First { get; set; }
        public string Middle { get; set; }
        public string Last { get; set; }
    }

    [NotMapped]
    [ExcludeFromCodeCoverage]
    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int Zip { get; set; }
    }

    [NotMapped]
    [ExcludeFromCodeCoverage]
    public class Phone
    {
        public string Number { get; set; }
        public string Type { get; set; }
    }  
}