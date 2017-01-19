using NationPost.API.Context;
using NationPost.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NationPost.API.Controllers
{
    public class GoogleController : ApiController
    {
        private APIContext db = new APIContext();
        // GET: api/Google
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Google/5
        public string Get(string  id_token)
        {
            //validate 
            //https://www.googleapis.com/oauth2/v3/tokeninfo?id_token=
            //if user is found 
            User user = db.Users.FirstOrDefault(k => k.Email == email && k.Password == password );
            return user;

        }

        // POST: api/Google
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Google/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Google/5
        public void Delete(int id)
        {
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
