using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SingleStoneCodingChallenge.Repositories;
using Moq;
using SingleStoneCodingChallenge.Context;
using System.Data.Entity;
using SingleStoneCodingChallenge.Models;
using System.Net;
using System.Data.Entity.Infrastructure;
using SingleStoneCodingChallenge.App_Start;
using System.Linq;

namespace SingleStoneCodingChallengeTests
{
    /// <summary>
    /// Summary description for RepositoryTests
    /// </summary>
    [TestClass]
    public class RepositoryTests
    {
        Mock<ContactsRepository> mockContactsRepository;
        Mock<IContactsDbContext> mockContext;
        Mock<DbSet<RawContact>> mockContact;
 
        Contact model = new Contact()
        {
            Name = new Name()
            {
                First = "Bill",
                Middle = "L",
                Last = "Buffalo"
            },
            Address = new Address()
            {
                Street = "123",
                City = "any",
                State = "here",
                Zip = 1234
            },
            Phone = new Phone[1] {
                    new Phone()
                    {
                        Number = "1234",
                        Type = "home"
                    }
                },
            EMail = "a@compton.com"
        };

        IQueryable<RawContact> contacts = new List<RawContact>() {
            new RawContact()
            {
                Id = 1,
                First = "Bill",
                Middle = "L",
                Last = "Buffalo",
                Email = "a@compton.com",
                Street = "123",
                City = "any",
                State = "here",
                Zip = 1234,
                MobileNumber = "12412451",
                MobileType = "mobile"
            }
         }.AsQueryable();

        RawContact updatedContact = new RawContact()
        {
            Id = 1,
            First = "Bret",
            Middle = "L",
            Last = "Buffalo",
            Email = "a@compton.com",
            Street = "123",
            City = "any",
            State = "here",
            Zip = 1234,
            MobileNumber = "12412452",
            MobileType = "mobile"
        };



        public RepositoryTests()
        {
            mockContext = new Mock<IContactsDbContext>();
            mockContact = new Mock<DbSet<RawContact>>();
            
            mockContactsRepository = new Mock<ContactsRepository>(mockContext.Object);
            
            mockContext.Setup(c => c.SetAdded(It.IsAny<object>()));
            mockContext.Setup(c => c.SetModified(It.IsAny<object>()));
            mockContext.Setup(c => c.SetDetached(It.IsAny<object>()));
            mockContact.As<IQueryable<RawContact>>().Setup(c => c.Provider).Returns(contacts.Provider);
            mockContact.As<IQueryable<RawContact>>().Setup(m => m.Expression).Returns(contacts.Expression);
            mockContact.As<IQueryable<RawContact>>().Setup(m => m.ElementType).Returns(contacts.ElementType);
            mockContact.As<IQueryable<RawContact>>().Setup(m => m.GetEnumerator()).Returns(() => contacts.GetEnumerator());
            
            mockContext.Setup(c => c.Contacts).Returns(mockContact.Object);
            mockContext.Setup(c => c.Set<RawContact>()).Returns(mockContact.Object);
        }

        [TestMethod]
        public void Get_Ok()
        {
            ContactsRepository contactsRepository = new ContactsRepository(mockContext.Object);
            var result = contactsRepository.GetContact(1);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetALL_Ok()
        {
            var result = mockContactsRepository.Object.GetContacts();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreateContact_Bad_Request()
        {
            mockContact.Setup(c => c.Attach(It.IsAny<RawContact>())).Returns<IDbSet<RawContact>, RawContact>(null);
            
            var result = mockContactsRepository.Object.CreateContact(model);
            Assert.AreEqual(HttpStatusCode.BadRequest, result);
        }

        [TestMethod]
        public void CreateContact_Ok()
        {
            mockContact.Setup(c => c.Attach(It.IsAny<RawContact>())).Returns<IDbSet<RawContact>, RawContact>(null);
            mockContext.Setup(c => c.SaveChanges()).Returns(1);
            var result = mockContactsRepository.Object.CreateContact(model);
            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod]
        public void UpdateContact_Ok()
        {
            mockContact.Setup(c => c.Attach(It.IsAny<RawContact>())).Returns(contacts.FirstOrDefault());
            var result = mockContactsRepository.Object.UpdateContact(model, 1);
            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod]
        public void DeleteContact_OK()
        {
            mockContact.Setup(c => c.Remove(It.IsAny<RawContact>())).Returns(contacts.FirstOrDefault());
            var result = mockContactsRepository.Object.DeleteContact(1);
            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod]
        public void UpdateContact_Not_Found()
        {
            var result = mockContactsRepository.Object.UpdateContact(model, 2);
            Assert.AreEqual(HttpStatusCode.NotFound, result);
        }        

        [TestMethod]
        public void DeleteContact_Not_Found()
        {
            var result = mockContactsRepository.Object.DeleteContact(2);
            Assert.AreEqual(HttpStatusCode.NotFound, result);
        }

        [TestMethod]
        public void Get_Not_Found()
        {
            var result = mockContactsRepository.Object.GetContact(2);
            Assert.IsNull(result);
        }
    }
}
