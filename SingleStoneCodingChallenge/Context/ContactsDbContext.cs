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
        public IDbSet<NameModel> Name => this.Set<NameModel>();
        public IDbSet<PhoneModel> Phones => this.Set<PhoneModel>();
        public IDbSet<AddressModel> Address => this.Set<AddressModel>();
        public virtual void SetModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }

        public virtual void SetDetached(object entity)
        {
            Entry(entity).State = EntityState.Detached;
        }

        public virtual void SetAdded(object entity)
        {
            Entry(entity).State = EntityState.Added;
        }
    }

    public interface IContactsDbContext
    {
        IDbSet<NameModel> Name { get; }
        IDbSet<PhoneModel> Phones { get; }
        IDbSet<AddressModel> Address { get; }
        Database Database { get; }
        void SetAdded(object entity);
        void SetModified(object entity);
        void SetDetached(object entity);
        int SaveChanges();
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}