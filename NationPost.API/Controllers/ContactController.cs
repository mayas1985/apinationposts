using NationPost.API.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NationPost.API.Controllers
{
    [Route("api/Contact/{id?}")]
    public class ContactController : ApiController
    {
        // GET api/Contact
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        //// GET api/Contact/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/Contact
        public void Post(customMessage msg)
        {
            MailHelper.Send(msg.message, "Contacted by someone on nationpost", msg.fromEmail, "mayas1985@gmail.com, rahuldwivedi.rld@gmail.com");

        }

        // PUT api/Contact/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/Contact/5
        public void Delete(int id)
        {
        }
    }

    public class customMessage
    {
        public string message { get; set; }
        public string fromEmail { get; set; }


    }
}
