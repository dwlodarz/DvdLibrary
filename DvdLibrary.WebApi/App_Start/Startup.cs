using DvdLibrary.WebApi.Controllers;
using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace DvdLibrary.WebApi
{
    [assembly: OwinStartup(typeof(Startup))]
    public partial class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            ConfigureAuth(appBuilder);
            HttpConfiguration httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            appBuilder.UseWebApi(httpConfiguration);

        }
    }
}