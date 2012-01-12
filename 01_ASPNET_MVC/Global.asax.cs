using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Domain;
using StructureMap;

namespace _01_ASPNET_MVC
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RegisterRoutes(RouteTable.Routes);
            RavenDbHelper.ConfigureRaven(ObjectFactory.Container);
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(ObjectFactory.Container));
        }

        protected void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                "List",
                "articles",
                new { controller = "Blog", action = "List" },
                new { httpMethod = new HttpMethodConstraint("GET") });

            routes.MapRoute(
                "Get",
                "articles/{articletitle}",
                new { controller = "Blog", action = "Get", articletitle = "" },
                new { httpMethod = new HttpMethodConstraint("GET") });

            routes.MapRoute(
                "Post",
                "articles",
                new { controller = "Blog", action = "Create" },
                new { httpMethod = new HttpMethodConstraint("POST") });

            routes.MapRoute(
                "Put",
                "articles/{articletitle}",
                new { controller = "Blog", action = "Update", articletitle = "" },
                new { httpMethod = new HttpMethodConstraint("PUT") });

            routes.MapRoute(
                "Delete",
                "articles/{articletitle}",
                new { controller = "Blog", action = "Delete", articletitle = "" },
                new { httpMethod = new HttpMethodConstraint("DELETE") });
        }

        protected void Application_EndRequest(object sender, EventArgs args)
        {
            RavenDbHelper.CleanUp(ObjectFactory.Container);
            ObjectFactory.ReleaseAndDisposeAllHttpScopedObjects();
        }
    }
}