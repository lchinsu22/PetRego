/*
Contains OwnersV2Controller class with method definitions for CRUD operations on Owner Entity.

Build 202 :
    Added Methods for CRUD operations which accepts Owner Entity and provides OwnerDTO Entity.
    Added contructor with service interface containing petV2DTO as generic parameter 
    so that the service methods provides implementation related to petV2DTO.
    Added the configuration in Unity Container Package to register Owner service interface with Owner service class 
    both with petV2DTO as generic parameter. The configuration is added in WebApiConfig.

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
    public class OwnersV2Controller : ApiController
    {
        private IOwnerService<PetV2DTO> ownerservice;

        public OwnersV2Controller() { }
        private PetV2DTO petV2DTO = new PetV2DTO();

        public OwnersV2Controller(IOwnerService<PetV2DTO> _ownerservice)
        {
            this.ownerservice = _ownerservice;
            this.petV2DTO = new PetV2DTO();
        }

        // GET: api/Owners
        [HttpGet]
        [Route("api/V2/Owners", Name = "GetV2Owners")]
        public IQueryable<OwnerDTO<PetV2DTO>> GetOwners()
        {
            string url = Request.RequestUri.GetLeftPart(UriPartial.Authority);
            IQueryable<OwnerDTO<PetV2DTO>> ownerdtos = ownerservice.GetAll(url, petV2DTO);
            return ownerdtos;
        }

        // GET: api/Owners/5
        [HttpGet]
        [Route("api/V2/Owners/{id}", Name = "GetV2OwnerById")]
        [ResponseType(typeof(OwnerDTO<PetV2DTO>))]
        public IHttpActionResult GetOwner(int id)
        {
            string url = Request.RequestUri.GetLeftPart(UriPartial.Authority);
            OwnerDTO<PetV2DTO> ownerdto = ownerservice.GetDTOByID(id, url, petV2DTO);
            if (ownerdto == null)
            {
                return NotFound();
            }
            return Ok(ownerdto);
        }

        // PUT: api/Owners/5
        [HttpPut]
        [Route("api/V2/Owners/{id}", Name = "UpdateV2Owners")]
        [ResponseType(typeof(OwnerDTO<PetV2DTO>))]
        public IHttpActionResult PutOwner(int id, Owner owner)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != owner.OwnerId)
            {
                return BadRequest("Owner Id in the body does not match the Owner ID in url");
            }
            if (!ownerservice.OwnerExists(owner.OwnerId))
            //if (!ownerservice.OwnerExistsForPut(owner))
            {
                return NotFound();
            }
            OwnerDTO<PetV2DTO> ownerdto = new OwnerDTO<PetV2DTO>(new PetV2DTO());
            try
            {
                string url = Request.RequestUri.GetLeftPart(UriPartial.Authority);
                ownerdto = ownerservice.Update(owner, url, petV2DTO);
            }
            catch (DbUpdateConcurrencyException e)
            {
                return BadRequest("The Pet Ids do not belong to the user.");
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            return Ok(ownerdto);
        }

        // POST: api/Owners
        [HttpPost]
        [Route("api/V2/Owners", Name = "CreateV2Owners")]
        [ResponseType(typeof(OwnerDTO<PetV2DTO>))]
        public IHttpActionResult PostOwner(Owner owner)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (ownerservice.OwnerExists(owner))
            {
                return BadRequest("Owner already Exist");
            }
            OwnerDTO<PetV2DTO> ownerdto = new OwnerDTO<PetV2DTO>(new PetV2DTO());
            try
            {
                string url = Request.RequestUri.GetLeftPart(UriPartial.Authority);
                ownerdto = ownerservice.Add(owner, url, petV2DTO);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            return Created("CreateOwners", ownerdto);
        }

        // DELETE: api/Owners/5
        [HttpDelete]
        [Route("api/V2/Owners/{id}", Name = "DeleteV2Owners")]
        [ResponseType(typeof(Owner))]
        public IHttpActionResult DeleteOwner(int id)
        {
            Owner owner = ownerservice.GetByID(id);
            if (owner == null)
            {
                return NotFound();
            }

            try
            {
                ownerservice.Remove(owner);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            if (!ownerservice.OwnerExists(id))
            {
                return Ok(owner);
            }
            return Ok(owner);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ownerservice.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}