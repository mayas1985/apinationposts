using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using NationPost.API.Models;
using NationPost.API.Context;

namespace NationPost.API.Controllers
{
    public class UserController : ApiController
    {
        private APIContext db = new APIContext();

        // GET api/User
        public IEnumerable<User> GetUsers()
        {
            return db.Users.AsEnumerable();
        }

        // GET api/User/5
        public User GetUser(Guid id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return user;
        }

        // PUT api/User/5
        public HttpResponseMessage PutUser(Guid id, User user)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != user.UserId)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/User
        public HttpResponseMessage PostUser(User user)
        {
            if (ModelState.IsValid)
            {
                user.CreatedOn = DateTime.Now;
                user.UserId = Guid.NewGuid();
                db.Users.Add(user);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, user);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = user.UserId }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/User/5
        public HttpResponseMessage DeleteUser(Guid id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Users.Remove(user);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, user);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}