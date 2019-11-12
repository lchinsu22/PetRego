using Microsoft.VisualStudio.TestTools.UnitTesting;
using PetRego.Controllers;
using PetRego.Models;
using PetRego.Service;
using PetRego.Tests.Context;
using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace PetRego.Tests.Controllers
{
    [TestClass]
    public class TestPetsV2Controllers
    {
        [TestMethod]
        public void PostPetV2_ShouldReturnSamePet()
        {
            var controller = new PetsV2Controller(new PetService<PetV2DTO>(new TestPetRegoContext()));

            controller.Request = new HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost/api/V2/Pets")
            };
            controller.Configuration = new HttpConfiguration();
            controller.Configuration.Routes.MapHttpRoute(
                name: "CreateV2Pets",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            Pet item = GetDemoPet();

            var result =
                controller.PostPet(item) as CreatedNegotiatedContentResult<PetV2DTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Content.PetName, item.PetName);
            Assert.AreEqual(result.Content.OwnerId, item.OwnerId);
            Assert.AreEqual(result.Content.PetType, item.PetType);
        }

        [TestMethod]
        public void PutPetV2_ShouldReturnNotFound()
        {
            var controller = new PetsV2Controller(new PetService<PetV2DTO>(new TestPetRegoContext()));
            controller.Request = new HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost/api/V2/Pets/{id}")
            };
            controller.Configuration = new HttpConfiguration();
            controller.Configuration.Routes.MapHttpRoute(
                name: "UpdateV2Pets",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            var item = GetDemoPet();

            var result = controller.PutPet(item.PetId, item) as IHttpActionResult;
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PutPetV2_ShouldFail_WhenDifferentID()
        {
            var controller = new PetsV2Controller(new PetService<PetV2DTO>(new TestPetRegoContext()));

            var badresult = controller.PutPet(999, GetDemoPet());
            //Build 105 - Changed BadRequest check to BadRequestErrorMessage Check.
            Assert.IsInstanceOfType(badresult, typeof(BadRequestErrorMessageResult));
        }

        [TestMethod]
        public void GetPetV2_ShouldReturnPetWithSameID()
        {
            var context = new TestPetRegoContext();
            context.Pets.Add(GetDemoPet());

            var controller = new PetsV2Controller(new PetService<PetV2DTO>(context));
            controller.Request = new HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost/api/V2/Pets/{id}")
            };
            controller.Configuration = new HttpConfiguration();
            controller.Configuration.Routes.MapHttpRoute(
                name: "GetV2PetById",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            var result = controller.GetPet(3) as OkNegotiatedContentResult<PetV2DTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Content.PetId);
        }

        [TestMethod]
        public void GetPetsV2_ShouldReturnAllPets()
        {
            var controller = new PetsV2Controller(new PetService<PetV2DTO>(getPetRegoContext()));

            controller.Request = new HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost/api/V2/Pets")
            };
            controller.Configuration = new HttpConfiguration();
            controller.Configuration.Routes.MapHttpRoute(
                name: "GetV2Pets",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            var result = controller.GetPets() as IQueryable<PetV2DTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(6, result.Count());
        }

        [TestMethod]
        public void DeletePetV2_ShouldReturnOK()
        {
            var context = new TestPetRegoContext();
            var item = GetDemoPet();
            context.Pets.Add(item);

            var controller = new PetsV2Controller(new PetService<PetV2DTO>(context));
            var result = controller.DeletePet(3) as OkNegotiatedContentResult<Pet>;

            Assert.IsNotNull(result);
            Assert.AreEqual(item.PetId, result.Content.PetId);
        }

        [TestMethod]
        public void PostPetV2_CheckIdempotency()
        {
            Pet pet1 = new Pet() { PetId = 1, PetName = "Demo Pet 1", PetType = "Demo Type 1", OwnerId = 1 };
            Pet pet2 = new Pet() { PetId = 2, PetName = "Demo Pet 2", PetType = "Demo Type 2", OwnerId = 2 };
            var context = new TestPetRegoContext();
            context.Pets.Add(pet1);
            context.Pets.Add(pet2);
            var service = new PetService<PetV2DTO>(context);

            Pet pet3 = new Pet() { PetName = "Demo Pet 2", PetType = "Demo Type 2" };
            bool result = service.PetExists(pet3);

            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);

            var controller = new PetsV2Controller(new PetService<PetV2DTO>(context));

            controller.Request = new HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost/api/V2/Pets")
            };
            controller.Configuration = new HttpConfiguration();
            controller.Configuration.Routes.MapHttpRoute(
                name: "CreateV2Pets",
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
            context.Pets.Add(new Pet() { PetId = 1, PetName = "Demo Pet 1", PetType = "Demo Type 1", OwnerId = 1 });
            context.Pets.Add(new Pet() { PetId = 2, PetName = "Demo Pet 2", PetType = "Demo Type 2", OwnerId = 1 });
            context.Pets.Add(new Pet() { PetId = 3, PetName = "Demo Pet 3", PetType = "Demo Type 3", OwnerId = 2 });
            context.Pets.Add(new Pet() { PetId = 4, PetName = "Demo Pet 4", PetType = "Demo Type 4", OwnerId = 2 });
            context.Pets.Add(new Pet() { PetId = 5, PetName = "Demo Pet 5", PetType = "Demo Type 5", OwnerId = 3 });
            context.Pets.Add(new Pet() { PetId = 6, PetName = "Demo Pet 6", PetType = "Demo Type 6", OwnerId = 3 });
            return context;
        }

    }
}
