/*
Contains OwnersController class with method definitions for CRUD operations.


Build 101 :
    Added Methods for CRUD operations which accepts and provides Owner Entity.

Build 102 :
    Added contructor with Context interface parameter to allow Dependency injection for Context class.
    Removed Controller's dependency on Entity Framework.


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

namespace PetRego.Controllers
{
    public class OwnersController : ApiController
    {
        private IPetRegoContext db = new PetRegoContext();

        public OwnersController() { }

        //Constructor based Dependency Injection
        public OwnersController(IPetRegoContext context)
        {
            db = context;
        }

        // GET: api/Owners
        public IQueryable<Owner> GetOwners()
        {
            return db.Owners;
        }

        // GET: api/Owners/5
        [ResponseType(typeof(Owner))]
        public IHttpActionResult GetOwner(int id)
        {
            Owner owner = db.Owners.Find(id);
            if (owner == null)
            {
                return NotFound();
            }

            return Ok(owner);
        }

        // PUT: api/Owners/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOwner(int id, Owner owner)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != owner.OwnerId)
            {
                return BadRequest();
            }

            //Build 102 - calling the method from Context removing Controller's dependency on Entity Framework.
            db.MarkOwnerAsModified(owner);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OwnerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Owners
        [ResponseType(typeof(Owner))]
        public IHttpActionResult PostOwner(Owner owner)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Owners.Add(owner);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = owner.OwnerId }, owner);
        }

        // DELETE: api/Owners/5
        [ResponseType(typeof(Owner))]
        public IHttpActionResult DeleteOwner(int id)
        {
            Owner owner = db.Owners.Find(id);
            if (owner == null)
            {
                return NotFound();
            }

            db.Owners.Remove(owner);
            db.SaveChanges();

            return Ok(owner);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OwnerExists(int id)
        {
            return db.Owners.Count(e => e.OwnerId == id) > 0;
        }
    }
}