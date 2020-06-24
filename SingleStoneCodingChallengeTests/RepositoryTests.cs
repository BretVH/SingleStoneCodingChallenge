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
        Mock<DbSet<NameModel>> mockName;
        Mock<DbSet<AddressModel>> mockAddress;
        Mock<DbSet<PhoneModel>> mockPhone;
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

        IQueryable<NameModel> name = new List<NameModel>() {
            new NameModel()
            {
                Id = 1,
                First = "Bill",
                Middle = "L",
                Last = "Buffalo",
                EMail = "a@compton.com"
            }
            }.AsQueryable();

        IQueryable<AddressModel> address = new List<AddressModel>()
            {
                new AddressModel()
            {
                Id = 1,
                Street = "123",
                City = "any",
                State = "here",
                Zip = 1234
            }
            }.AsQueryable();

        IQueryable<PhoneModel> phone = new PhoneModel[1] {
                new PhoneModel()
                {
                    Contact = 1,
                    Number = "1234",
                    Type = "home"
                }
            }.AsQueryable();

        public RepositoryTests()
        {
            mockContext = new Mock<IContactsDbContext>();
            mockName = new Mock<DbSet<NameModel>>();
            mockAddress = new Mock<DbSet<AddressModel>>();
            mockPhone = new Mock<DbSet<PhoneModel>>();
            mockContactsRepository = new Mock<ContactsRepository>(mockContext.Object);
            mockContext.Setup(c => c.SetAdded(It.IsAny<object>()));
            mockContext.Setup(c => c.SetModified(It.IsAny<object>()));
            mockContext.Setup(c => c.SetDetached(It.IsAny<object>()));
            mockName.As<IQueryable<NameModel>>().Setup(c => c.Provider).Returns(name.Provider);
            mockName.As<IQueryable<NameModel>>().Setup(m => m.Expression).Returns(name.Expression);
            mockName.As<IQueryable<NameModel>>().Setup(m => m.ElementType).Returns(name.ElementType);
            mockName.As<IQueryable<NameModel>>().Setup(m => m.GetEnumerator()).Returns(() => name.GetEnumerator());
            mockAddress.As<IQueryable<AddressModel>>().Setup(c => c.Provider).Returns(address.Provider);
            mockAddress.As<IQueryable<AddressModel>>().Setup(m => m.Expression).Returns(address.Expression);
            mockAddress.As<IQueryable<AddressModel>>().Setup(m => m.ElementType).Returns(address.ElementType);
            mockAddress.As<IQueryable<AddressModel>>().Setup(m => m.GetEnumerator()).Returns(() => address.GetEnumerator());
            mockPhone.As<IQueryable<PhoneModel>>().Setup(c => c.Provider).Returns(phone.Provider);
            mockPhone.As<IQueryable<PhoneModel>>().Setup(m => m.Expression).Returns(phone.Expression);
            mockPhone.As<IQueryable<PhoneModel>>().Setup(m => m.ElementType).Returns(phone.ElementType);
            mockPhone.As<IQueryable<PhoneModel>>().Setup(m => m.GetEnumerator()).Returns(() => phone.GetEnumerator());
            mockContext.Setup(c => c.Name).Returns(mockName.Object);
            mockContext.Setup(c => c.Address).Returns(mockAddress.Object);
            mockContext.Setup(c => c.Phones).Returns(mockPhone.Object);
            mockContext.Setup(c => c.Set<NameModel>()).Returns(mockName.Object);
            mockContext.Setup(c => c.Set<AddressModel>()).Returns(mockAddress.Object);
            mockContext.Setup(c => c.Set<PhoneModel>()).Returns(mockPhone.Object);
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
        public void CreateContact_OK()
        {
            mockContactsRepository.Setup(c => c.GetId()).Returns(0);
            mockName.Setup(c => c.Attach(It.IsAny<NameModel>())).Returns<IDbSet<NameModel>, NameModel>(null);
            mockAddress.Setup(c => c.Attach(It.IsAny<AddressModel>())).Returns<IDbSet<AddressModel>, AddressModel>(null);
            mockPhone.Setup(c => c.Attach(It.IsAny<PhoneModel>())).Returns<IDbSet<PhoneModel>, PhoneModel>(null);
            var result = mockContactsRepository.Object.CreateContact(model);
            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod]
        public void UpdateContact_OK()
        {
            mockContactsRepository.Setup(c => c.GetContact(It.IsAny<int>())).Returns(
                AutoMapperConfig.RegisterMappings().Map<ContactModel>(model));
            mockName.Setup(c => c.Attach(It.IsAny<NameModel>())).Returns<IDbSet<NameModel>, NameModel>(null);
            mockAddress.Setup(c => c.Attach(It.IsAny<AddressModel>())).Returns<IDbSet<AddressModel>, AddressModel>(null);
            mockPhone.Setup(c => c.Attach(It.IsAny<PhoneModel>())).Returns<IDbSet<PhoneModel>, PhoneModel>(null);
            var result = mockContactsRepository.Object.UpdateContact(model, 1);
            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod]
        public void UpdateContact_Not_Found()
        {
            mockContactsRepository.Setup(c => c.GetContact(It.IsAny<int>())).Returns<ContactsRepository, ContactModel>(null);
            mockName.Setup(c => c.Attach(It.IsAny<NameModel>())).Returns<IDbSet<NameModel>, NameModel>(null);
            mockAddress.Setup(c => c.Attach(It.IsAny<AddressModel>())).Returns<IDbSet<AddressModel>, AddressModel>(null);
            mockPhone.Setup(c => c.Attach(It.IsAny<PhoneModel>())).Returns<IDbSet<PhoneModel>, PhoneModel>(null);
            var result = mockContactsRepository.Object.UpdateContact(model, 1);
            Assert.AreEqual(HttpStatusCode.NotFound, result);
        }

        [TestMethod]
        public void DeleteContact_OK()
        {
            mockContactsRepository.Setup(c => c.GetContact(It.IsAny<int>())).Returns(
                AutoMapperConfig.RegisterMappings().Map<ContactModel>(model));
            mockName.Setup(c => c.Remove(It.IsAny<NameModel>())).Returns<IDbSet<NameModel>, NameModel>(null);
            mockAddress.Setup(c => c.Remove(It.IsAny<AddressModel>())).Returns<IDbSet<AddressModel>, AddressModel>(null);
            mockPhone.Setup(c => c.Remove(It.IsAny<PhoneModel>())).Returns<IDbSet<PhoneModel>, PhoneModel>(null);
            var result = mockContactsRepository.Object.DeleteContact(1);
            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod]
        public void DeleteContact_Not_Found()
        {
            mockContactsRepository.Setup(c => c.GetContact(It.IsAny<int>())).Returns<ContactsRepository, ContactModel>(null);
            mockName.Setup(c => c.Remove(It.IsAny<NameModel>())).Returns<IDbSet<NameModel>, NameModel>(null);
            mockAddress.Setup(c => c.Remove(It.IsAny<AddressModel>())).Returns<IDbSet<AddressModel>, AddressModel>(null);
            mockPhone.Setup(c => c.Remove(It.IsAny<PhoneModel>())).Returns<IDbSet<PhoneModel>, PhoneModel>(null);
            var result = mockContactsRepository.Object.DeleteContact(1);
            Assert.AreEqual(HttpStatusCode.NotFound, result);
        }

        [TestMethod]
        public void Get_Not_Found()
        {
            var result = mockContactsRepository.Object.GetContact(1);
            Assert.IsNull(result);
        }
    }
}
