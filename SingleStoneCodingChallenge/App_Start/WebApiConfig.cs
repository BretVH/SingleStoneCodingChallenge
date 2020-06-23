using AutoMapper;
using AutoMapper.Configuration.Conventions;
using SingleStoneCodingChallenge.Context;
using SingleStoneCodingChallenge.Models;
using SingleStoneCodingChallenge.Repositories;
using System.Data.SqlClient;
using System.Net.Http.Formatting;
using System.Web.Http;
using Unity;
using Unity.WebApi;

namespace SingleStoneCodingChallenge
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var container = new UnityContainer();
            container.RegisterType<IContactsDbContext, ContactsDbContext>();
            container.RegisterType<IContactsRepository, ContactsRepository>();

            config.DependencyResolver = new UnityDependencyResolver(container);

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
