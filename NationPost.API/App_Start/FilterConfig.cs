using NationPost.API.Helper;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NationPost.API
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new CustomExceptionFilter());
            //filters.Add(new CorsAuthorizationFilterFactory("AllowSpecificOrigin"));

        }
    }


}