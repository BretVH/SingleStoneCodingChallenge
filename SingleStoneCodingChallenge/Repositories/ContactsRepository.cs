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
        private ContactsDbContext DbContext;
        public ContactsRepository(IContactsDbContext context)
        {
            this.DbContext = (ContactsDbContext)context;
        }

        public HttpStatusCode CreateContact(Contact model)
        {
            SqlConnection conn = new SqlConnection(DbContext.Database.Connection.ConnectionString);
            SqlCommand comm = new SqlCommand("SELECT IDENT_CURRENT ('dbo.Contact')", conn);
            conn.Open();
            int id = Convert.ToInt32(comm.ExecuteScalar()) + 1;
            conn.Close();
            var contactToCreate = AutoMapperConfig.RegisterMappings().Map<ContactModel>(model);
            DbContext.Name.Attach(contactToCreate.Name);
            DbContext.Entry(contactToCreate.Name).State = EntityState.Added;
            DbContext.Address.Attach(contactToCreate.Address);
            DbContext.Entry(contactToCreate.Address).State = EntityState.Added;
            foreach (var phone in contactToCreate.Phone)
            {
                phone.Contact = id;
                DbContext.Phones.Attach(phone);
                DbContext.Entry(phone).State = EntityState.Added;
            }
            DbContext.SaveChanges();

            return HttpStatusCode.OK;
        }

        public HttpStatusCode DeleteContact(int id)
        {
            var model = GetContact(id);
            DbContext.Name.Remove(model.Name);
            DbContext.Address.Remove(model.Address);
            foreach(var phone in model.Phone)
                DbContext.Phones.Remove(phone);
            DbContext.SaveChanges();
            return HttpStatusCode.OK;
        }

        public ContactModel GetContact(int id)
        {
            var name = DbContext.Name.Where(c => c.Id == id).FirstOrDefault();
            if(name == null)
            {
                return null;
            }
            ContactModel model = new ContactModel()
            {
                Address = DbContext.Address.Where(c => c.Id == id).FirstOrDefault(),
                Name = name,
                Phone = DbContext.Phones.Where(c => c.Contact == id).ToArray()
            };

            model.EMail = model.Name.EMail;
            return model;
        }

        public IEnumerable<ContactModel> GetContacts()
        {
            var addresses = DbContext.Address.ToList();
            var names = DbContext.Name.ToList();
            var groupedPhones = DbContext.Phones.GroupBy(c => c.Contact).ToDictionary(group => group.Key, group => group.ToArray());
            PhoneModel[] phones;
            var models = (from name in names
                          join address in addresses on name.Id equals address.Id
                          select new ContactModel()
                          {
                              Name = name,
                              Address = address,
                              Phone = groupedPhones.TryGetValue(name.Id, out phones)  ? phones  : null,
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
            DbContext.Entry(origModel.Name).State = EntityState.Detached;
            DbContext.Entry(origModel.Address).State = EntityState.Detached;
            foreach(var phone in origModel.Phone)
                DbContext.Entry(phone).State = EntityState.Detached;
            var updatedEntity = AutoMapperConfig.RegisterMappings().Map<ContactModel>(model);
            updatedEntity.Name.Id = id;
            updatedEntity.Address.Id = id;
            DbContext.Name.Attach(updatedEntity.Name);
            DbContext.Entry(updatedEntity.Name).State = EntityState.Modified;
            DbContext.Address.Attach(updatedEntity.Address);
            DbContext.Entry(updatedEntity.Address).State = EntityState.Modified;
            foreach (var phone in updatedEntity.Phone)
            {
                phone.Contact = id;
                DbContext.Phones.Attach(phone);
                DbContext.Entry(phone).State = EntityState.Modified;
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
    }
}