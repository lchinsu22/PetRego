/*
Unit Testing :

Test Class and Test methods to test all the pet Entity controller methods.
Used Mock database context with getPetRegoContext and getDemoPet methods for sample data.

Build 103 :
    Added test methods to test all the Pet entity Controller methods.

Build 105 :
    Changed BadRequest check to BadRequestErrorMessage Check.
    Added method to check idempotency in post request
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
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace PetRego.Tests.Controllers
{

    [TestClass]
    public class TestPetControllers
    {
        [TestMethod]
        public void PostPet_ShouldReturnSamePet()
        {
            var controller = new PetsController(new PetService(new TestPetRegoContext()));

            controller.Request = new HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost/api/Pets")
            };
            controller.Configuration = new HttpConfiguration();
            controller.Configuration.Routes.MapHttpRoute(
                name: "CreatePets",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            Pet item = GetDemoPet();

            var result =
                controller.PostPet(item) as CreatedNegotiatedContentResult<PetDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Content.PetName, item.PetName);
            Assert.AreEqual(result.Content.OwnerId, item.OwnerId);
            Assert.AreEqual(result.Content.PetType, item.PetType);
        }

        [TestMethod]
        public void PutPet_ShouldReturnNotFound()
        {
            var controller = new PetsController(new PetService(new TestPetRegoContext()));
            controller.Request = new HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost/api/Pets/{id}")
            };
            controller.Configuration = new HttpConfiguration();
            controller.Configuration.Routes.MapHttpRoute(
                name: "UpdatePets",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            var item = GetDemoPet();

            var result = controller.PutPet(item.PetId, item) as IHttpActionResult;
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PutPet_ShouldFail_WhenDifferentID()
        {
            var controller = new PetsController(new PetService(new TestPetRegoContext()));

            var badresult = controller.PutPet(999, GetDemoPet());
            //Build 105 - Changed BadRequest check to BadRequestErrorMessage Check.
            Assert.IsInstanceOfType(badresult, typeof(BadRequestErrorMessageResult));
        }

        [TestMethod]
        public void GetPet_ShouldReturnPetWithSameID()
        {
            var context = new TestPetRegoContext();
            context.Pets.Add(GetDemoPet());

            var controller = new PetsController(new PetService(context));
            controller.Request = new HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost/api/Pets/{id}")
            };
            controller.Configuration = new HttpConfiguration();
            controller.Configuration.Routes.MapHttpRoute(
                name: "GetPetById",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            var result = controller.GetPet(3) as OkNegotiatedContentResult<PetDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Content.PetId);
        }

        [TestMethod]
        public void GetPets_ShouldReturnAllPets()
        {
            var controller = new PetsController(new PetService(getPetRegoContext()));

            controller.Request = new HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost/api/Pets")
            };
            controller.Configuration = new HttpConfiguration();
            controller.Configuration.Routes.MapHttpRoute(
                name: "GetPets",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            var result = controller.GetPets() as IQueryable<PetDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(6, result.Count());
        }

        [TestMethod]
        public void DeletePet_ShouldReturnOK()
        {
            var context = new TestPetRegoContext();
            var item = GetDemoPet();
            context.Pets.Add(item);

            var controller = new PetsController(new PetService(context));
            var result = controller.DeletePet(3) as OkNegotiatedContentResult<Pet>;

            Assert.IsNotNull(result);
            Assert.AreEqual(item.PetId, result.Content.PetId);
        }

        [TestMethod]
        public void PostPet_CheckIdempotency()
        {
            Pet pet1 = new Pet() { PetId = 1, PetName = "Demo Pet 1", PetType = "Demo Type 1", OwnerId = 1 };
            Pet pet2 = new Pet() { PetId = 2, PetName = "Demo Pet 2", PetType = "Demo Type 2", OwnerId = 2 };
            var context = new TestPetRegoContext();
            context.Pets.Add(pet1);
            context.Pets.Add(pet2);
            var service = new PetService(context);

            Pet pet3 = new Pet() { PetName = "Demo Pet 2", PetType = "Demo Type 2" };
            bool result = service.PetExists(pet3);

            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);

            var controller = new PetsController(new PetService(context));

            controller.Request = new HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost/api/Pets")
            };
            controller.Configuration = new HttpConfiguration();
            controller.Configuration.Routes.MapHttpRoute(
                name: "CreatePets",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            var controllerresult =
                controller.PostPet(pet3) as IHttpActionResult;

            Assert.IsNotNull(controllerresult);
            Assert.IsInstanceOfType(controllerresult, typeof(BadRequestErrorMessageResult));
        }

        Pet GetDemoPet()
        {
            return new Pet() { PetId = 3, PetName = "Demo Pet 2", PetType = "Demo Type 2", OwnerId = 3 };
        }

        TestPetRegoContext getPetRegoContext()
        {
            var context = new TestPetRegoContext();
            context.Pets.Add(new Pet() { PetId = 3, PetName = "Demo Pet 1", PetType = "Demo Type 1", OwnerId = 1 });
            context.Pets.Add(new Pet() { PetId = 4, PetName = "Demo Pet 2", PetType = "Demo Type 2", OwnerId = 1 });
            context.Pets.Add(new Pet() { PetId = 3, PetName = "Demo Pet 3", PetType = "Demo Type 3", OwnerId = 2 });
            context.Pets.Add(new Pet() { PetId = 4, PetName = "Demo Pet 4", PetType = "Demo Type 4", OwnerId = 2 });
            context.Pets.Add(new Pet() { PetId = 3, PetName = "Demo Pet 5", PetType = "Demo Type 5", OwnerId = 3 });
            context.Pets.Add(new Pet() { PetId = 4, PetName = "Demo Pet 6", PetType = "Demo Type 6", OwnerId = 3 });
            return context;
        }

    }
}
