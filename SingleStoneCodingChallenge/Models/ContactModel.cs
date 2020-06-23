using Swashbuckle.Swagger;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Diagnostics.CodeAnalysis;

namespace SingleStoneCodingChallenge.Models
{
    [ExcludeFromCodeCoverage]
    public class ContactWithId
    {
        public int Id { get; set; }
        public NameWithoutEmail Name { get; set; }
        public Address Address { get; set; }
        public Phone[] Phone { get; set; }
        public string EMail { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class NameWithoutEmail
    {
        public string First { get; set; }
        public string Middle { get; set; }
        public string Last { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Contact
    {
        public Name Name { get; set; }
        public Address Address { get; set; }
        public Phone[] Phone { get; set; }
        public string EMail { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class ContactModel
    {
        public NameModel Name { get; set; }
        public AddressModel Address { get; set; }
        public PhoneModel[] Phone { get; set; }
        public string EMail { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Name
    {    
        public string First { get; set; }
        public string Middle { get; set; }
        public string Last { get; set; }
        public string EMail { get; set; }
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

    [ExcludeFromCodeCoverage]
    [Table("Contact")]
    public class NameModel 
    {
        [Key]
        public int Id { get; set; }
        [Column("FirstName")]
        public string First { get; set; }
        [Column("MiddleName")]
        public string Middle { get; set; }
        [Column("LastName")]
        public string Last { get; set; }
        public string EMail { get; set; }
    }

    [ExcludeFromCodeCoverage]
    [Table("Address")]
    public class AddressModel
    {
        [Key]
        public int Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int Zip { get; set; }
    }

    [ExcludeFromCodeCoverage]
    [Table("Phone")]
    public class PhoneModel
    {
        [Column(Order = 1), Key]
        public int Contact { get; set; }
        [Column("PhoneNumber", Order = 2), Key]
        
        public string Number { get; set; }
        [Column("PhoneType", Order = 3), Key]
        public string Type { get; set; }
    }
}