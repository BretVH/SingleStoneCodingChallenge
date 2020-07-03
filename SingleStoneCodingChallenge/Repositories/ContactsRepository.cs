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
        private DbSet<RawContact> Contact => DbContext.Set<RawContact>();


        public ContactsRepository(IContactsDbContext context)
        {
            this.DbContext = context;
             
    }

        public HttpStatusCode CreateContact(Contact model)
        {
            var contactToCreate = AutoMapperConfig.RegisterMappings().Map<RawContact>(model);
            Contact.Add(contactToCreate);
            if (DbContext.SaveChanges() <= 0)
                return HttpStatusCode.BadRequest;
            else
                return HttpStatusCode.OK;
        }  

        public HttpStatusCode DeleteContact(int id)
        {
            var contactWithId = Contact.FirstOrDefault(c => c.Id == id);
            if (contactWithId == null)
            {
                return HttpStatusCode.NotFound;
            }
            var model = AutoMapperConfig.RegisterMappings().Map<RawContact>(contactWithId);
            Contact.Remove(model);
            DbContext.SaveChanges();
            return HttpStatusCode.OK;
        }

        public virtual ContactWithId GetContact(int id)
        {
            var contactWithId = Contact.FirstOrDefault(c => c.Id == id);
            if (contactWithId == null)
            {
                return null;
            }
            var model = AutoMapperConfig.RegisterMappings().Map<ContactWithId>(contactWithId);
            return model;
        }

        public IEnumerable<ContactWithId> GetContacts()
        {
            var contactsWithIds = Contact.ToList();
            var models = AutoMapperConfig.RegisterMappings().Map<IEnumerable<ContactWithId>>(contactsWithIds);
            return models;
        }

        public HttpStatusCode UpdateContact(Contact model, int id)
        {
            var origModel = Contact.FirstOrDefault(c => c.Id == id);
            if (origModel == null)
            {
                return HttpStatusCode.NotFound;
            }
                
            var updatedEntity = AutoMapperConfig.RegisterMappings().Map<RawContact>(model);
            updatedEntity.Id = id;
            DbContext.SetDetached(origModel);
            Contact.Attach(updatedEntity);
            DbContext.SetModified(updatedEntity);
            DbContext.SaveChanges();
            return HttpStatusCode.OK;
        }

 
    }

    public interface IContactsRepository
    {
        IEnumerable<ContactWithId> GetContacts();
        HttpStatusCode CreateContact(Contact model);
        HttpStatusCode UpdateContact(Contact model, int id);
        ContactWithId GetContact(int id);
        HttpStatusCode DeleteContact(int id);
    }
}