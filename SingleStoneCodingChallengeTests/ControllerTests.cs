using System;
using System.Net;
using System.Threading;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SingleStoneCodingChallenge.Controllers;
using SingleStoneCodingChallenge.Models;
using SingleStoneCodingChallenge.Repositories;

namespace SingleStoneCodingChallengeTests
{
    [TestClass]
    public class ControllerTests
    {
        [TestMethod]
        public void ContactsAPIController_Constructor_Is_Not_Null()
        {
            Mock<IContactsRepository> mockRepo = new Mock<IContactsRepository>();
            var controller = new ContactsAPIController(mockRepo.Object);
            Assert.IsNotNull(controller);
        }

        [TestMethod]
        public void ContactsAPIController_Get_Returns_Ok()
        {
            Mock<IContactsRepository> mockRepo = new Mock<IContactsRepository>();
            mockRepo.Setup(c => c.GetContact(1)).Returns(new ContactWithId());
            var controller = new ContactsAPIController(mockRepo.Object);
            controller.Configuration = new System.Web.Http.HttpConfiguration();
            controller.Request = new System.Net.Http.HttpRequestMessage();
            var result = controller.Get(1).ExecuteAsync(new CancellationToken());
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.Result.StatusCode);
        }

        [TestMethod]
        public void ContactsAPIController_Get_Returns_Not_Found()
        {
            Mock<IContactsRepository> mockRepo = new Mock<IContactsRepository>();
            mockRepo.Setup(c => c.GetContact(1)).Returns<IContactsRepository, ContactWithId>(null);
            var controller = new ContactsAPIController(mockRepo.Object);
            controller.Configuration = new System.Web.Http.HttpConfiguration();
            controller.Request = new System.Net.Http.HttpRequestMessage();
            var result = controller.Get(1).ExecuteAsync(new CancellationToken());
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotFound, result.Result.StatusCode);
        }

        [TestMethod]
        public void ContactsAPIController_GetAll_Returns_Ok()
        {
            Mock<IContactsRepository> mockRepo = new Mock<IContactsRepository>();
            mockRepo.Setup(c => c.GetContacts()).Returns<IContactsRepository, ContactWithId>(null);
            var controller = new ContactsAPIController(mockRepo.Object);
            controller.Configuration = new System.Web.Http.HttpConfiguration();
            controller.Request = new System.Net.Http.HttpRequestMessage();
            var result = controller.Get().ExecuteAsync(new CancellationToken());
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.Result.StatusCode);
        }

        [TestMethod]
        public void ContactsAPIController_Post_Returns_OK()
        {
            Mock<IContactsRepository> mockRepo = new Mock<IContactsRepository>();
            mockRepo.Setup(c => c.CreateContact(new Contact())).Returns(HttpStatusCode.OK);
            var controller = new ContactsAPIController(mockRepo.Object);
            controller.Configuration = new System.Web.Http.HttpConfiguration();
            controller.Request = new System.Net.Http.HttpRequestMessage();
            var result = controller.Post("{ \"name\": { \"first\": \"test\" } }").ExecuteAsync(new CancellationToken());
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.Result.StatusCode);
        }

        [TestMethod]
        public void ContactsAPIController_Post_Returns_Bad_Request()
        {
            Mock<IContactsRepository> mockRepo = new Mock<IContactsRepository>();
            mockRepo.Setup(c => c.CreateContact(new Contact())).Returns(HttpStatusCode.OK);
            var controller = new ContactsAPIController(mockRepo.Object);
            controller.Configuration = new System.Web.Http.HttpConfiguration();
            controller.Request = new System.Net.Http.HttpRequestMessage();
            var result = controller.Post(null).ExecuteAsync(new CancellationToken());
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.Result.StatusCode);
        }

        [TestMethod]
        public void ContactsAPIController_Put_Returns_Bad_Request()
        {
            Mock<IContactsRepository> mockRepo = new Mock<IContactsRepository>();
            mockRepo.Setup(c => c.UpdateContact(new Contact(), 1)).Returns(HttpStatusCode.OK);
            mockRepo.Setup(c => c.GetContact(1)).Returns(new ContactWithId());
            var controller = new ContactsAPIController(mockRepo.Object);
            controller.Configuration = new System.Web.Http.HttpConfiguration();
            controller.Request = new System.Net.Http.HttpRequestMessage();
            var result = controller.Put(1, null).ExecuteAsync(new CancellationToken());
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.Result.StatusCode);
        }
        [TestMethod]
        public void ContactsAPIController_Put_Returns_Not_Found()
        {
            Mock<IContactsRepository> mockRepo = new Mock<IContactsRepository>();
            mockRepo.Setup(c => c.GetContact(1)).Returns<IContactsRepository, ContactWithId>(null);
            var controller = new ContactsAPIController(mockRepo.Object);
            controller.Configuration = new System.Web.Http.HttpConfiguration();
            controller.Request = new System.Net.Http.HttpRequestMessage();
            var result = controller.Put(1, "{ \"name\": { \"first\": \"test\" } }").ExecuteAsync(new CancellationToken());
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotFound, result.Result.StatusCode);
        }

        [TestMethod]
        public void ContactsAPIController_Put_Returns_OK()
        {
            Mock<IContactsRepository> mockRepo = new Mock<IContactsRepository>();
            mockRepo.Setup(c => c.GetContact(1)).Returns(new ContactWithId());
            mockRepo.Setup(c => c.UpdateContact(new Contact(), 1)).Returns(HttpStatusCode.OK);
            var controller = new ContactsAPIController(mockRepo.Object);
            controller.Configuration = new System.Web.Http.HttpConfiguration();
            controller.Request = new System.Net.Http.HttpRequestMessage();
            var result = controller.Put(1, null).ExecuteAsync(new CancellationToken());
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.Result.StatusCode);
        }

        [TestMethod]
        public void ContactsAPIController_Delete_Returns_Not_Found()
        {
            Mock<IContactsRepository> mockRepo = new Mock<IContactsRepository>();
            mockRepo.Setup(c => c.GetContact(1)).Returns<IContactsRepository, ContactWithId>(null);
            var controller = new ContactsAPIController(mockRepo.Object);
            controller.Configuration = new System.Web.Http.HttpConfiguration();
            controller.Request = new System.Net.Http.HttpRequestMessage();
            var result = controller.Delete(1).ExecuteAsync(new CancellationToken());
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotFound, result.Result.StatusCode);
        }
        [TestMethod]
        public void ContactsAPIController_Delete_Returns_OK()
        {
            Mock<IContactsRepository> mockRepo = new Mock<IContactsRepository>();
            mockRepo.Setup(c => c.GetContact(1)).Returns(new ContactWithId());
            mockRepo.Setup(c => c.DeleteContact(1)).Returns(HttpStatusCode.OK);
            var controller = new ContactsAPIController(mockRepo.Object);
            controller.Configuration = new System.Web.Http.HttpConfiguration();
            controller.Request = new System.Net.Http.HttpRequestMessage();
            var result = controller.Delete(1).ExecuteAsync(new CancellationToken());
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.Result.StatusCode);
        }
    }
}
