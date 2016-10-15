using DvdLibrary.WebApi.Controllers;
//using DvdLibrary.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace DvdLibrary.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //Database.SetInitializer(new DvdLibraryDbInitialization());
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
