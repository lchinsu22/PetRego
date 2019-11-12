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
    public class PetsV2Controller : ApiController
    {

        private IPetService<PetV2DTO> petservice;
        private PetV2DTO petV2DTO = new PetV2DTO();

        public PetsV2Controller() { }

        public PetsV2Controller(IPetService<PetV2DTO> _petservice)
        {
            this.petservice = _petservice;
            this.petV2DTO = new PetV2DTO();
        }

        // GET: api/Pets
        [HttpGet]
        [Route("api/V2/Pets", Name = "GetV2Pets")]
        public IQueryable<PetV2DTO> GetPets()
        {
            string url = Request.RequestUri.GetLeftPart(UriPartial.Authority);
            IQueryable<PetV2DTO> petv2dtos = petservice.GetAll(url, petV2DTO);
            return petv2dtos;
        }

        //Allows user to search Pets Owned by a User
        [HttpGet]
        [Route("api/V2/Owners/{id}/Pets", Name = "GetV2PetsByOwnerId")]
        public IQueryable<PetV2DTO> GetPetsByOwnerId(int id)
        {
            string url = Request.RequestUri.GetLeftPart(UriPartial.Authority);
            IQueryable<PetV2DTO> petv2dtos = petservice.GetPetsByOwnerId(id, url, petV2DTO);
            return petv2dtos;
        }

        // GET: api/Pets/5
        [HttpGet]
        [Route("api/V2/Pets/{id}", Name = "GetV2PetById")]
        [ResponseType(typeof(PetV2DTO))]
        public IHttpActionResult GetPet(int id)
        {
            string url = Request.RequestUri.GetLeftPart(UriPartial.Authority);
            PetV2DTO petv2dto = petservice.GetDTOByID(id, url, petV2DTO);
            if (petv2dto == null)
            {
                return NotFound();
            }
            return Ok(petv2dto);
        }

        // PUT: api/Pets/5
        [HttpPut]
        [Route("api/V2/Pets/{id}", Name = "UpdateV2Pets")]
        [ResponseType(typeof(PetV2DTO))]
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
            PetV2DTO petv2dto = new PetV2DTO();
            try
            {
                string url = Request.RequestUri.GetLeftPart(UriPartial.Authority);
                petv2dto = petservice.Update(pet, url, petV2DTO);
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

            return Ok(petv2dto);
        }

        // POST: api/Pets
        [HttpPost]
        [Route("api/V2/Pets", Name = "CreateV2Pets")]
        [ResponseType(typeof(PetV2DTO))]
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
            PetV2DTO petv2dto = new PetV2DTO();
            try
            {
                string url = Request.RequestUri.GetLeftPart(UriPartial.Authority);
                petv2dto = petservice.Add(pet, url, petV2DTO);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            return Created("CreatePets", petv2dto);
        }

        // DELETE: api/Pets/5
        [HttpDelete]
        [Route("api/V2/Pets/{id}", Name = "DeleteV2Pets")]
        [ResponseType(typeof(PetV2DTO))]
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