using NationPost.API.Context;
using NationPost.API.Helper;
using NationPost.API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace NationPost.API.Controllers
{
    public class ExtendedController : Controller
    {
        private APIContext db = new APIContext();

        [HttpGet]
        public ActionResult Login(string Email, string Password)
        {
            User user = db.Users.FirstOrDefault(k => k.Email == Email && k.Password == Password);
            if (user != null)
            {
                this.Session.SetDataToSession<Guid>(SessionExtensions.Keys.LoggedInUserId, user.UserId);
            }

            return Json(user, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult LoggedInUserId()
        {
            return Json(new { LoggedInUserId = this.Session.GetDataFromSession<Guid>(SessionExtensions.Keys.LoggedInUserId) }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult LogOut()
        {
            this.Session.RemoveAll();
            this.Session.Clear();
            return new HttpStatusCodeResult(200, "Signed Out successfully");

        }


        [HttpGet]
        public ActionResult LoginFromGoogle(string id_token)
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

                        user.Email = result.email;
                        user.Password = "Password";
                        db.Users.Add(user);
                        db.SaveChanges();
                    }
                    this.Session.SetDataToSession<Guid>(SessionExtensions.Keys.LoggedInUserId, user.UserId);

                    return Json(user, JsonRequestBehavior.AllowGet);

                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return null;
        }

        [HttpPost]
        public ActionResult UpdateUserInfo(UpdateUserDto userDto)
        {
            var userId = this.Session.GetDataFromSession<Guid>(SessionExtensions.Keys.LoggedInUserId);
            if (userId == Guid.Empty)
            {
                return Json(new ResponseDTO() { IsSuccess = false, Message = "User not logged In" }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                var user = db.Users.Find(userId);
                if (user != null)
                {
                    user.AboutMe = userDto.AboutMe;
                    user.IsAboutMeVisible = userDto.IsAboutMeVisible;
                    user.FacebookLink = userDto.FacebookLink;
                    user.IsFacebookLinkVisible = userDto.IsFacebookLinkVisible;
                    user.TwitterLink = userDto.TwitterLink;
                    user.IsTwitterLinkVisible = userDto.IsTwitterLinkVisible;
                    user.Contact = userDto.Contact;
                    user.IsContactVisible = userDto.IsContactVisible;
                    user.GoogleLink = userDto.GoogleLink;
                    user.IsGoogleLinkVisible = userDto.IsGoogleLinkVisible;

                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new ResponseDTO() { IsSuccess = true, Message = "User information updated successfully" }, JsonRequestBehavior.AllowGet);

                }
                return Json(new ResponseDTO() { IsSuccess = false, Message = "User not logged In" }, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpPost]
        public ActionResult ToggleTag(Guid articleId, int Id)
        {
            var userId = this.Session.GetDataFromSession<Guid>(SessionExtensions.Keys.LoggedInUserId);
            if (userId == Guid.Empty)
            {
                return Json(new ResponseDTO() { IsSuccess = false, Message = "User not logged In" }, JsonRequestBehavior.AllowGet);

            }
            var article = db.Articles.Include(p => p.ArticleTypeId).Include(m => m.CreatedBy).FirstOrDefault(j => j.CreatedBy.UserId == userId && j.ArticleId == articleId);
            if (article != null)
            {
                if (db.ArticleTags.Include(p => p.article).Include(s => s.Tag).Any(k => k.article.ArticleId == articleId && k.Tag.Id == Id))
                {
                    var tag = db.ArticleTags.Include(p => p.article).Include(s => s.Tag).FirstOrDefault(k => k.article.ArticleId == articleId && k.Tag.Id == Id);
                    db.ArticleTags.Remove(tag);
                    db.SaveChanges();
                    return Json(new ResponseDTO() { IsSuccess = true, Message = "Tag removed successfully" }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    var articleTag = new ArticleTags();
                    articleTag.article = article;
                    articleTag.Tag = db.Tags.FirstOrDefault(k => k.Id == Id);
                    db.ArticleTags.Add(articleTag);
                    db.SaveChanges();
                    return Json(new ResponseDTO() { IsSuccess = true, Message = "Tag added successfully" }, JsonRequestBehavior.AllowGet);

                }
            }
            else
            {
                return Json(new ResponseDTO() { IsSuccess = false, Message = "Cannot edit this article!!" }, JsonRequestBehavior.AllowGet);

            }

        }

        [HttpPost]
        public ActionResult DeleteArticle(Guid articleId)
        {
            var userId = this.Session.GetDataFromSession<Guid>(SessionExtensions.Keys.LoggedInUserId);
            if (userId == Guid.Empty)
            {
                return Json(new ResponseDTO() { IsSuccess = false, Message = "User not logged In" }, JsonRequestBehavior.AllowGet);

            }

            var article = db.Articles.Include(p => p.ArticleTypeId).Include(m => m.CreatedBy).FirstOrDefault(j => j.CreatedBy.UserId == userId && j.ArticleId == articleId);
            if (article != null)
            {
                article.IsVisible = false;
                db.SaveChanges();
                return Json(new ResponseDTO() { IsSuccess = true, Message = "Article updated successfully" }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(new ResponseDTO() { IsSuccess = false, Message = "Cannot edit this article!!" }, JsonRequestBehavior.AllowGet);

            }

        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateArticle(UpdateArticleDto updateArticleDto)
        {
            var userId = this.Session.GetDataFromSession<Guid>(SessionExtensions.Keys.LoggedInUserId);
            if (userId == Guid.Empty)
            {
                return Json(new ResponseDTO() { IsSuccess = false, Message = "User not logged In" }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                var article = db.Articles.Where(j => j.CreatedBy.UserId == userId && j.ArticleId == updateArticleDto.AritcleId)
                    .Include(p => p.ArticleTypeId).Include(m => m.CreatedBy).FirstOrDefault();
                if (article != null)
                {
                    article.Title = updateArticleDto.Title;
                    article.Description = updateArticleDto.Description;
                    article.Summary = updateArticleDto.Summary;
                    article.Body = updateArticleDto.Body;
                    article.IsValid = updateArticleDto.IsValid;
                    article.IsVisible = updateArticleDto.IsVisible;
                    db.Entry(article).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new ResponseDTO() { IsSuccess = true, Message = "Article updated successfully" }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json(new ResponseDTO() { IsSuccess = false, Message = "Cannot edit this article!!" }, JsonRequestBehavior.AllowGet);

                }

            }
        }


        [HttpGet]
        public ActionResult ForgotPassword(string emailToCheck)
        {
            User user = db.Users.FirstOrDefault(k => k.Email == emailToCheck);
            if (user != null)
            {
                MailHelper.Send("Your Username is " + user.Email + " and password is " + user.Password, "NationPost - Password retrieval", "admin@nationpost.com", user.Email);
                return Json(new ResponseDTO() { IsSuccess = true, Message = "Mail sent successfully" });
            }
            return Json(new ResponseDTO() { IsSuccess = false, Message = "No account found for this email" }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult IsEmailExists(string emailToCheckDuplicate)
        {
            var foundDuplicate = db.Users.Any(k => k.Email == emailToCheckDuplicate);
            return Json(new ResponseDTO() { IsSuccess = foundDuplicate, Message = "Email already registered" }, JsonRequestBehavior.AllowGet);

        }


        // GET: Extended
        public ActionResult Index()
        {
            return View();
        }

        // GET: Extended/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Extended/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Extended/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Extended/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Extended/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Extended/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Extended/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
