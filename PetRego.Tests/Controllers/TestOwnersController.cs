/*
Unit Testing :

Test Class and Test methods to test all the Owner Entity controller methods.
Used Mock database context with getPetRegoContext and getDemoOwner methods for sample data.

Build 103 :
    Added test methods to test all the owner entity Controller methods.

Build 105 :
    Changed BadRequest check to BadRequestErrorMessage Check.
*/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using PetRego.Controllers;
using PetRego.Models;
using PetRego.Service;
using PetRego.Tests.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace PetRego.Tests.Controllers
{
    [TestClass]
    public class TestOwnersController
    {
        [TestMethod]
        public void PostOwner_ShouldReturnSameOwner()
        {
            var controller = new OwnersController(new OwnerService(new TestPetRegoContext()));

            controller.Request = new HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost/api/Owners")
            };
            controller.Configuration = new HttpConfiguration();
            controller.Configuration.Routes.MapHttpRoute(
                name: "CreateOwners",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            Owner item = GetDemoOwner();

            var result =
                controller.PostOwner(item) as CreatedNegotiatedContentResult<OwnerDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Content.Ownername, item.Ownername);
            Assert.AreEqual(result.Content.Pets.Count, item.Pets.Count);
        }

        [TestMethod]
        public void PutOwner_ShouldReturnNotFound()
        {
            var controller = new OwnersController(new OwnerService(new TestPetRegoContext()));
            controller.Request = new HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost/api/Owners/{id}")
            };
            controller.Configuration = new HttpConfiguration();
            controller.Configuration.Routes.MapHttpRoute(
                name: "UpdateOwners",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            var item = GetDemoOwner();

            var result = controller.PutOwner(item.OwnerId, item) as IHttpActionResult;
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PutOwner_ShouldFail_WhenDifferentID()
        {
            var controller = new OwnersController(new OwnerService(new TestPetRegoContext()));

            var badresult = controller.PutOwner(999, GetDemoOwner());
            //Build 105 - Changed BadRequest check to BadRequestErrorMessage Check.
            Assert.IsInstanceOfType(badresult, typeof(BadRequestErrorMessageResult));
        }

        [TestMethod]
        public void GetOwner_ShouldReturnOwnerWithSameID()
        {
            var context = new TestPetRegoContext();
            context.Owners.Add(GetDemoOwner());

            var controller = new OwnersController(new OwnerService(context));
            controller.Request = new HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost/api/Owners/{id}")
            };
            controller.Configuration = new HttpConfiguration();
            controller.Configuration.Routes.MapHttpRoute(
                name: "GetOwnerById",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            var result = controller.GetOwner(1) as OkNegotiatedContentResult<OwnerDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Content.OwnerId);
        }

        [TestMethod]
        public void GetOwners_ShouldReturnAllOwners()
        {
            var controller = new OwnersController(new OwnerService(getPetRegoContext()));

            controller.Request = new HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost/api/Owners")
            };
            controller.Configuration = new HttpConfiguration();
            controller.Configuration.Routes.MapHttpRoute(
                name: "GetOwners",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            var result = controller.GetOwners() as IQueryable<OwnerDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count());
        }

        [TestMethod]
        public void DeleteOwner_ShouldReturnOK()
        {
            var context = new TestPetRegoContext();
            var item = GetDemoOwner();
            context.Owners.Add(item);

            var controller = new OwnersController(new OwnerService(context));
            var result = controller.DeleteOwner(1) as OkNegotiatedContentResult<Owner>;

            Assert.IsNotNull(result);
            Assert.AreEqual(item.OwnerId, result.Content.OwnerId);
        }

        [TestMethod]
        public void PostOwner_CheckIdempotency()
        {
            
            var service = new OwnerService(getPetRegoContext());

            
            bool result = service.OwnerExists(GetDemoOwner());

            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);

            var controller = new OwnersController(new OwnerService(getPetRegoContext()));

            controller.Request = new HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost/api/Owners")
            };
            controller.Configuration = new HttpConfiguration();
            controller.Configuration.Routes.MapHttpRoute(
                name: "CreateOwners",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            Owner item = GetDemoOwner();

            var controllerresult =
                controller.PostOwner(item) as IHttpActionResult;

            Assert.IsNotNull(controllerresult);
            Assert.IsInstanceOfType(controllerresult, typeof(BadRequestErrorMessageResult));
        }

        Owner GetDemoOwner()
        {
            List<Pet> pets = new List<Pet>();
            pets.Add(new Pet() { PetId = 1, PetName = "Demo Pet 1", PetType = "Demo Type 1", OwnerId = 1 });
            pets.Add(new Pet() { PetId = 2, PetName = "Demo Pet 2", PetType = "Demo Type 2", OwnerId = 1 });
            return new Owner() { OwnerId = 1, Ownername = "Demo Owner name 1", Pets = pets };
        }

        TestPetRegoContext getPetRegoContext()
        {
            var context = new TestPetRegoContext();
            List<Pet> pets = new List<Pet>();
            pets.Add(new Pet() { PetId = 1, PetName = "Demo Pet 1", PetType = "Demo Type 1", OwnerId = 1 });
            pets.Add(new Pet() { PetId = 2, PetName = "Demo Pet 2", PetType = "Demo Type 2", OwnerId = 1 });
            context.Owners.Add(new Owner() { OwnerId = 1, Ownername = "Demo Owner name 1", Pets = pets });
            pets = new List<Pet>();
            pets.Add(new Pet() { PetId = 3, PetName = "Demo Pet 3", PetType = "Demo Type 3", OwnerId = 2 });
            pets.Add(new Pet() { PetId = 4, PetName = "Demo Pet 4", PetType = "Demo Type 4", OwnerId = 2 });
            context.Owners.Add(new Owner() { OwnerId = 2, Ownername = "Demo Owner name 2", Pets = pets });
            pets = new List<Pet>();
            pets.Add(new Pet() { PetId = 5, PetName = "Demo Pet 5", PetType = "Demo Type 5", OwnerId = 3 });
            pets.Add(new Pet() { PetId = 6, PetName = "Demo Pet 6", PetType = "Demo Type 6", OwnerId = 3 });
            context.Owners.Add(new Owner() { OwnerId = 3, Ownername = "Demo Owner name 3", Pets = pets });
            return context;
        }

    }
}
