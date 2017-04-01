using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using NationPost.API.Models;
using NationPost.API.Helper;
using System.Web.SessionState;
using NationPost.DAL;

namespace NationPost.API.Controllers
{
    public class UserController : ApiController
    {
        private BlogDBContextLinqDataContext db = new BlogDBContextLinqDataContext();

        // GET api/User
        //public IEnumerable<User> GetUsers()
        //{
        //    return db.Users.AsEnumerable();
        //}

        // GET api/User/5


        public Models.User GetUser(Guid id)
        {
            var user = db.Users.FirstOrDefault(k => k.UserId == id);
            if (user == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return user.ToDTO();
        }

        public Models.User GetUser(string email, string password)
        {

            throw new Exception("Moved to extended controller");
        }

        // PUT api/User/5
        //public HttpResponseMessage PutUser(Guid id, User user)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        //    }

        //    if (id != user.UserId)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest);
        //    }

        //    db.Entry(user).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
        //    }

        //    return Request.CreateResponse(HttpStatusCode.OK);
        //}

        // POST api/User
        public Models.User PostUser(Models.User user)
        {
            if (ModelState.IsValid)
            {

                if (string.IsNullOrWhiteSpace(user.Email))
                {
                    throw new Exception("Email cannot be blank");
                }
                if (Guid.Empty == user.UserId)
                {

                    if (!db.Users.Any(k => k.Email == user.Email))
                    {
                        var userDAL = user.ToDAL(db);
                        userDAL.CreatedOn = DateTime.Now;
                        userDAL.UserId = Guid.NewGuid();
                        db.Users.InsertOnSubmit(userDAL);
                        db.SubmitChanges();

                        //HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, user);
                        //response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = user.UserId }));
                        return userDAL.ToDTO();
                    }
                    else
                    {
                        throw new Exception("Email already exists");
                    }
                }
                else
                {
                    if (db.Users.Any(k => k.Email == user.Email && k.UserId != user.UserId))
                    {
                        throw new Exception("Email already associated to other user");

                    }
                    var userDAL = user.ToDAL(db);

                    try
                    {
                        db.SubmitChanges();
                    }
                    catch (Exception ex)
                    {
                        throw ex;//return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
                    }
                    //HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, user);
                    //response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = user.UserId }));
                    return userDAL.ToDTO();
                }


            }
            else
            {
                throw new Exception("view invalid");
            }
        }

        // DELETE api/User/5
        public HttpResponseMessage DeleteUser(Guid id)
        {
            var user = db.Users.FirstOrDefault(k => k.UserId == id);
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Users.DeleteOnSubmit(user);

            try
            {
                db.SubmitChanges();
            }
            catch (Exception ex)
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