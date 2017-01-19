using NationPost.API.Context;
using NationPost.API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NationPost.API.Controllers
{
    public class TokenResult
    {
        public string email { get; set; }
        public string email_verified { get; set; }
        public string name { get; set; }
        public string picture { get; set; }
        public string given_name { get; set; }
        public string family_name { get; set; }
        public string locale { get; set; }
    }

    public class GoogleController : ApiController
    {
        private APIContext db = new APIContext();
        // GET: api/Google
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Google/5
        public User Get(string id_token)
        {
            //validate 
            //https://www.googleapis.com/oauth2/v3/tokeninfo?id_token=
            //if user is found 
            using (var webClient = new WebClient())
            {
                var json_data = string.Empty;
                try
                {
                    json_data = webClient.DownloadString("https://www.googleapis.com/oauth2/v3/tokeninfo?id_token=" + id_token);
                    var result = JsonConvert.DeserializeObject<TokenResult>(json_data);

                    User user = db.Users.FirstOrDefault(k => k.Email == result.email);

                    if (user == null)
                    {
                        user = new Models.User();
                        user.CreatedOn = DateTime.Now;
                        user.UserId = Guid.NewGuid();
                        user.UserName = "Auto";
                        user.Email = result.email;
                        user.Password = "Password";
                        db.Users.Add(user);
                        db.SaveChanges();
                    }

                    return user;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return null;
        }

        //// POST: api/Google
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/Google/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/Google/5
        //public void Delete(int id)
        //{
        //}
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
