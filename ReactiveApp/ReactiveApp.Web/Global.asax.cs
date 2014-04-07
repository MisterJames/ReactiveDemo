using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ReactiveApp.Web
{
    public enum DbServiceState
    {
        RealTime,
        TableStorage,
        LocalCache
    }

    public class MvcApplication : System.Web.HttpApplication
    {
        public static DbServiceState DbServiceState { get; set; }
        public const string SessionCartIdKey = "shopping_cart_id";

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            DbServiceState = DbServiceState.RealTime;

        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Session[SessionCartIdKey] = Guid.NewGuid().ToString();
        }

    }
}
