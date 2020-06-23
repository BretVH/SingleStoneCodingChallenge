using SingleStoneCodingChallenge.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SingleStoneCodingChallenge.Context
{
    public class ContactsDbContext : DbContext, IContactsDbContext
    {
        public DbSet<NameModel> Name { get; set; }
        public DbSet<PhoneModel> Phones { get; set; }
        public DbSet<AddressModel> Address { get; set; }
    }

    public interface IContactsDbContext
    {
        DbSet<NameModel> Name { get; set; }
        DbSet<PhoneModel> Phones { get; set; }
        DbSet<AddressModel> Address { get; set; }
    }
}