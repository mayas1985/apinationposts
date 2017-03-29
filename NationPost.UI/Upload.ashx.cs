using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NationPost.UI
{
    /// <summary>
    /// Summary description for Upload
    /// </summary>
    public class Upload : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            HttpPostedFile uploads = context.Request.Files["upload"];
            string CKEditorFuncNum = context.Request["CKEditorFuncNum"];
            string file = System.IO.Path.GetFileName(uploads.FileName);
            uploads.SaveAs(context.Server.MapPath(".") + "\\Images\\" + file);
            string url = "/Images/" + file;
            context.Response.Write("<script>window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", \"" + url + "\");</script>");
            //context.Response.Headers.Remove("Access-Control-Allow-Origin");
            //context.Response.AddHeader("Access-Control-Allow-Origin", context.Request.UrlReferrer.GetLeftPart(UriPartial.Authority));

            //context.Response.Headers.Remove("Access-Control-Allow-Credentials");
            //context.Response.AddHeader("Access-Control-Allow-Credentials", "true");

            //context.Response.Headers.Remove("Access-Control-Allow-Methods");
            //context.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
            context.Response.End();
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}