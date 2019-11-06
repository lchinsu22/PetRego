/*
Contains PetController class with method definitions for CRUD operations on Pet Entity. 

Build 103 :
    Added Methods for CRUD operations which accepts and provides PetDTO Entity.
    Added constructor with service interface parameter to allow Dependency injection for service class. Allows Unit Testing.
    Used PetService methods to handle requests for CRUD operations on Pet Entity.
    Provides the Response in Type of petDTO Entity. Response also contains HATEOAS links

Build 104 :
    Added Unity Container Package to register Pet service interface with Pet service class. The configuration is added in WebApiConfig.
    Provides complete independency. i.e. Pet Controller is completely independent of pet class implementation.

Build 105 :
    Added condition in post method to achieve idempotency.
    Improved Exception Handling.
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using PetRego.Models;
using PetRego.Service;

namespace PetRego.Controllers
{
    public class PetsController : ApiController
    {
        private IPetService petservice;

        public PetsController() { }

        public PetsController(IPetService _petservice)
        {
            this.petservice = _petservice;
        }

        // GET: api/Pets
        [Route("api/Pets", Name = "GetPets")]
        public IQueryable<PetDTO> GetPets()
        {
            string url = Url.Link("GetPets", null);
            url = Request.RequestUri.GetLeftPart(UriPartial.Authority);
            IQueryable<PetDTO> petdtos = petservice.GetAll(url);
            return petdtos;
        }

        //Allows user to search Pets Owned by a User
        [Route("api/Owners/{id}/Pets", Name = "GetPetsByOwnerId")]
        public IQueryable<PetDTO> GetPetsByOwnerId(int id)
        {
            string url = Url.Link("GetPetById", id);
            url = Request.RequestUri.GetLeftPart(UriPartial.Authority);
            IQueryable<PetDTO> petdtos = petservice.GetPetsByOwnerId(id, url);
            return petdtos;
        }

        // GET: api/Pets/5
        [Route("api/Pets/{id}", Name = "GetPetById")]
        [ResponseType(typeof(PetDTO))]
        public IHttpActionResult GetPet(int id)
        {
            string url = Url.Link("GetPetById", id);
            url = Request.RequestUri.GetLeftPart(UriPartial.Authority);
            PetDTO petdto = petservice.GetDTOByID(id, url);
            if (petdto == null)
            {
                return NotFound();
            }
            return Ok(petdto);
        }

        // PUT: api/Pets/5
        [Route("api/Pets/{id}", Name = "UpdatePets")]
        [ResponseType(typeof(PetDTO))]
        public IHttpActionResult PutPet(int id, Pet pet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pet.PetId)
            {
                return BadRequest("Pet Id in the body does not match the Pet ID in url");
            }
            if (!petservice.PetExists(pet))
            {
                return NotFound();
            }
            PetDTO petdto = new PetDTO();
            try
            {
                string url = Url.Link("UpdatePets", id);
                url = Request.RequestUri.GetLeftPart(UriPartial.Authority);
                petdto = petservice.Update(pet, url);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!petservice.PetExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return InternalServerError();
                }
            }

            return Ok(petdto);
        }

        // POST: api/Pets
        [Route("api/Pets", Name = "CreatePets")]
        [ResponseType(typeof(PetDTO))]
        public IHttpActionResult PostPet(Pet pet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //Build 105 - Condition to maintain POST idempotence
            if (petservice.PetExists(pet))
            {
                return BadRequest("Pet already Exist");
            }
            if (pet.OwnerId == 0)
            {
                return BadRequest("Owner Id is required.");
            }
            PetDTO petdto = new PetDTO();
            try
            {
                string url = Url.Link("CreatePets", petdto.PetId) + "/" + petdto.PetId;
                url = Request.RequestUri.GetLeftPart(UriPartial.Authority);
                petdto = petservice.Add(pet, url);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            return Created("CreatePets", petdto);
        }

        // DELETE: api/Pets/5
        [Route("api/Pets/{id}", Name = "DeletePets")]
        [ResponseType(typeof(PetDTO))]
        public IHttpActionResult DeletePet(int id)
        {
            Pet pet = petservice.GetByID(id);
            if (pet == null)
            {
                return NotFound();
            }

            try
            {
                petservice.Remove(pet);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            if (!petservice.PetExists(id))
            {
                return Ok(pet);
            }
            return Ok(pet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                petservice.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}