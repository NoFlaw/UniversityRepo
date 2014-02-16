using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using StructureMap;
using University.Data.Initialization;
using ResourceManager = University.Data.Initialization.ResourceManager;

namespace University.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            //ObjectFactory Init
            ResourceManager.Initialize();
            ControllerBuilder.Current.SetControllerFactory(new StructureMapControllerFactory(new Container()));

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_End()
        {
            ResourceManager.Dispose();
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            ResourceManager.EndRequest(sender, e);
        }

    }
}
