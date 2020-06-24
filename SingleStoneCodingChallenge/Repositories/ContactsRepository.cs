using SingleStoneCodingChallenge.Context;
using SingleStoneCodingChallenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using AutoMapper;
using SingleStoneCodingChallenge.App_Start;
using System.Text.RegularExpressions;
using System.Data.Entity;
using Microsoft.Ajax.Utilities;
using System.Web.Http.Results;
using System.Net.Http;
using System.Net;
using EO.Internal;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace SingleStoneCodingChallenge.Repositories
{
    public class ContactsRepository : IContactsRepository
    {
        public IContactsDbContext DbContext;
        private DbSet<NameModel> Name => DbContext.Set<NameModel>();
        private DbSet<PhoneModel> Phones => DbContext.Set<PhoneModel>();
        private DbSet<AddressModel> Address => DbContext.Set<AddressModel>();


        public ContactsRepository(IContactsDbContext context)
        {
            this.DbContext = context;
             
    }

        public HttpStatusCode CreateContact(Contact model)
        {
            int id = GetId();
            var contactToCreate = AutoMapperConfig.RegisterMappings().Map<ContactModel>(model);
            Name.Attach(contactToCreate.Name);
            DbContext.SetAdded(contactToCreate.Name); 
            Address.Attach(contactToCreate.Address);
            DbContext.SetAdded(contactToCreate.Address); 
            foreach (var phone in contactToCreate.Phone)
            {
                phone.Contact = id;
                Phones.Attach(phone);
                DbContext.SetAdded(phone);
            }
            DbContext.SaveChanges();

            return HttpStatusCode.OK;
        }

        public virtual int GetId()
        {
            SqlConnection conn = new SqlConnection(DbContext.Database.Connection.ConnectionString);
            SqlCommand sql = new SqlCommand("SELECT IDENT_CURRENT ('dbo.Contact')", conn);
            conn.Open();
            var value = Convert.ToInt32(sql.ExecuteScalar()) + 1;
            conn.Close();
            return value;
        }

        public HttpStatusCode DeleteContact(int id)
        {
            var model = GetContact(id);
            if(model == null)
            {
                return HttpStatusCode.NotFound;
            }
            Name.Remove(model.Name);
            Address.Remove(model.Address);
            foreach (var phone in model.Phone)
                Phones.Remove(phone);
            DbContext.SaveChanges();
            return HttpStatusCode.OK;
        }

        public virtual ContactModel GetContact(int id)
        {
            var name = Name.FirstOrDefault(c => c.Id == id);
            if (name == null)
            {
                return null;
            }
            ContactModel model = new ContactModel()
            {
                Address = Address.FirstOrDefault(c => c.Id == id),
                Name = name,
                Phone = Phones.Where(c => c.Contact == id).ToArray()
            };

            model.EMail = model.Name.EMail;
            return model;
        }

        public IEnumerable<ContactModel> GetContacts()
        {
            var addresses = Address.ToList();
            var names = Name.ToList();
            var groupedPhones = Phones.GroupBy(c => c.Contact).ToDictionary(group => group.Key, group => group.ToArray());
            PhoneModel[] phones;
            var models = (from name in names
                          join address in addresses on name.Id equals address.Id
                          select new ContactModel()
                          {
                              Name = name,
                              Address = address,
                              Phone = groupedPhones.TryGetValue(name.Id, out phones) ? phones : null,
                              EMail = name.EMail
                          }).ToList();

            return models;
        }

        public HttpStatusCode UpdateContact(Contact model, int id)
        {
            var origModel = GetContact(id);
            if (GetContact(id) == null)
            {
                return HttpStatusCode.NotFound;
            }
            DbContext.SetDetached(origModel.Name);
            DbContext.SetDetached(origModel.Address);
            foreach (var phone in origModel.Phone)
                DbContext.SetDetached(phone);
            var updatedEntity = AutoMapperConfig.RegisterMappings().Map<ContactModel>(model);
            updatedEntity.Name.Id = id;
            updatedEntity.Address.Id = id;
            Name.Attach(updatedEntity.Name);
            DbContext.SetModified(updatedEntity.Name);
            Address.Attach(updatedEntity.Address);
            DbContext.SetModified(updatedEntity.Address);
            foreach (var phone in updatedEntity.Phone)
            {
                phone.Contact = id;
                Phones.Attach(phone);
                if(origModel.Phone.Length < updatedEntity.Phone.Length)
                {
                    if (origModel.Phone.Length == 0 || !origModel.Phone.Contains(phone))
                        DbContext.SetAdded(phone);
                    else
                        DbContext.SetModified(phone);
                }                    
            }
            DbContext.SaveChanges();

            return HttpStatusCode.OK;
        }
    }

    public interface IContactsRepository
    {
        IEnumerable<ContactModel> GetContacts();
        HttpStatusCode CreateContact(Contact model);
        HttpStatusCode UpdateContact(Contact model, int id);
        ContactModel GetContact(int id);
        HttpStatusCode DeleteContact(int id);
        int GetId();
    }
}