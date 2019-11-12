using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Unity;
using PetRego.Service;
using PetRego.Models;
using Unity.Lifetime;
using PetRego.App_Start;
using PetRego.Controllers;

namespace PetRego
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            
            //Build 104 - Configuration to register interface to respective classes in Unity Container to facilitate Dependency Injection. 
            var container = new UnityContainer();
            //Build 201 - Added petDTO as generic paramter to register service interface against its class with same petDTO type. 
            container.RegisterType<IOwnerService<PetDTO>, OwnerService<PetDTO>>(new HierarchicalLifetimeManager());
            container.RegisterType<IPetService<PetDTO>, PetService<PetDTO>>(new HierarchicalLifetimeManager());
            container.RegisterType<IOwnerService<PetV2DTO>, OwnerService<PetV2DTO>>(new HierarchicalLifetimeManager());
            container.RegisterType<IPetService<PetV2DTO>, PetService<PetV2DTO>>(new HierarchicalLifetimeManager());

            container.RegisterType<IPetRegoContext, PetRegoContext>(new HierarchicalLifetimeManager());
            container.RegisterType<IPetDTO<PetDTO>, PetDTO > (new HierarchicalLifetimeManager());
            container.RegisterType<IPetDTO<PetV2DTO>, PetV2DTO>(new HierarchicalLifetimeManager());
            config.DependencyResolver = new UnityResolver(container);

            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
