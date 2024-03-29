﻿/*
Contains OwnersController class with method definitions for CRUD operations on Owner Entity.

Build 101 :
    Added Methods for CRUD operations which accepts and provides Owner Entity.

Build 102 :
    Added contructor with Context interface parameter to allow Dependency injection for Context class.
    Removed Controller's dependency on Entity Framework.

Build 103 :
    Updated constructor with service interface parameter to allow Dependency injection for service class. Allows Unit Testing.
    Used OwnerService methods to handle requests for CRUD operations on Owner Entity.
    Changes the Response in Type of OwnerDTO instead of Owner. Allows Response to contain HATEOAS links
    Added Unit Testing in Tests package to perform unit testing.

Build 104 :
    Added Unity Container Package to register Owner service interface with Owner service class. The configuration is added in WebApiConfig.
    Provides complete independency. i.e. owner Controller is completely independent of owner Service class implementation.

Build 105 :
    Added condition in post method to achieve idempotency.
    Improved Exception Handling.

Build 201 :
    Added petDTO as generic parameter to implement generic Owner service class.
    This does not affect any of the version one CRUD operations.
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
    public class OwnersController : ApiController
    {
        private IOwnerService<PetDTO> ownerservice;

        public OwnersController() { }
        private PetDTO petDTO = new PetDTO();

        public OwnersController(IOwnerService<PetDTO> _ownerservice)
        {
            this.ownerservice = _ownerservice;
            this.petDTO = new PetDTO();
        }

        // GET: api/Owners
        [Route("api/Owners", Name = "GetOwners")]
        public IQueryable<OwnerDTO<PetDTO>> GetOwners()
        {
            string url = Request.RequestUri.GetLeftPart(UriPartial.Authority);
            IQueryable<OwnerDTO<PetDTO>> ownerdtos = ownerservice.GetAll(url, petDTO);
            return ownerdtos;
        }

        // GET: api/Owners/5
        [Route("api/Owners/{id}", Name = "GetOwnerById")]
        [ResponseType(typeof(OwnerDTO<PetDTO>))]
        public IHttpActionResult GetOwner(int id)
        {
            string url = Request.RequestUri.GetLeftPart(UriPartial.Authority);
            OwnerDTO<PetDTO> ownerdto = ownerservice.GetDTOByID(id, url, petDTO);
            if (ownerdto == null)
            {
                return NotFound();
            }
            return Ok(ownerdto);
        }

        // PUT: api/Owners/5
        [Route("api/Owners/{id}", Name = "UpdateOwners")]
        [ResponseType(typeof(OwnerDTO<PetDTO>))]
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
            OwnerDTO<PetDTO> ownerdto = new OwnerDTO<PetDTO>(new PetDTO());
            try
            {
                string url = Request.RequestUri.GetLeftPart(UriPartial.Authority);
                ownerdto = ownerservice.Update(owner, url, petDTO);
            }
            catch(DbUpdateConcurrencyException e)
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
        [Route("api/Owners", Name = "CreateOwners")]
        [ResponseType(typeof(OwnerDTO<PetDTO>))]
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
            OwnerDTO<PetDTO> ownerdto = new OwnerDTO<PetDTO>(new PetDTO());
            try
            {
                string url = Request.RequestUri.GetLeftPart(UriPartial.Authority);
                ownerdto = ownerservice.Add(owner, url, petDTO);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            return Created("CreateOwners", ownerdto);
        }

        // DELETE: api/Owners/5
        [Route("api/Owners/{id}", Name = "DeleteOwners")]
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