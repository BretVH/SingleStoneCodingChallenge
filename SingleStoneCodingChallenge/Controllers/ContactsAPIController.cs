using SingleStoneCodingChallenge.Models;
using SingleStoneCodingChallenge.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Routing;
using Newtonsoft.Json;
using Swashbuckle.Swagger.Annotations;
using SingleStoneCodingChallenge.App_Start;
using System.Web.Http.Results;


namespace SingleStoneCodingChallenge.Controllers
{
    public class ContactsAPIController : ApiController
    {
        private readonly IContactsRepository _repository;

        public ContactsAPIController(IContactsRepository repository)
        {
            _repository = repository;
        }
        // GET api/values
        [HttpGet]
        [Route("contacts")]
        public IHttpActionResult Get()
        {
            return Ok(AutoMapperConfig.RegisterMappings().Map<IEnumerable<ContactWithId>>(_repository.GetContacts()));
        }

        // GET api/values/5
        [Route("contacts/{id}")]
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var result = _repository.GetContact(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(AutoMapperConfig.RegisterMappings().Map<ContactWithId>(result));

        }

        [HttpPost]
        [Route("contacts")]
        // POST api/values
        public IHttpActionResult Post([FromBody]string value)
        {
            if (value != null)
            {
                Contact contact = JsonConvert.DeserializeObject<Contact>(value);
                _repository.CreateContact(contact);
                return Ok();
            }
            else
                return BadRequest();
        }

        [HttpPut]
        // PUT api/values/5
        [Route("contacts/{id}")]
        public IHttpActionResult Put(int id, [FromBody]string value)
        {
            var result = _repository.GetContact(id);
            if (result == null)
            {
                return NotFound();
            }
            if (value != null)
            {
                Contact contact = JsonConvert.DeserializeObject<Contact>(value);
                return Content(_repository.UpdateContact(contact, id), value);
            }
            return BadRequest();
        }

        [Route("contacts/{id}")]
        [HttpDelete]
        // DELETE api/values/5
        public IHttpActionResult Delete(int id)
        {
            var result = _repository.GetContact(id);
            if (result == null)
            {
                return NotFound();
            }
            return Content(_repository.DeleteContact(id), result);
        }
    }
}
