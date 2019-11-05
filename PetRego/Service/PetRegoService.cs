/*
provides implementation of Service classes providing budiness logic in handling requests.

Build 103 :
    Added Service method implementations.
    Suggestion For Future Build - Could use Genrics to simplify implementation of repetitive implementations.

*/

using PetRego.Models;
using PetRegoSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetRego.Service
{
    public class OwnerService : IDisposable, IOwnerService
    {
        private IPetRegoContext db = new PetRegoContext();

        public OwnerService() { }

        public OwnerService(IPetRegoContext context)
        {
            db = context;
        }

        public IQueryable<OwnerDTO> GetAll(string url)
        {

            List<OwnerDTO> ownerdtos = Mapper.MapTOOwnerDTOs(db.Owners.ToList(), url);
            return ownerdtos.AsQueryable();
        }

        public OwnerDTO GetDTOByID(int id, string url)
        {

            Owner owner = db.Owners.FirstOrDefault(p => p.OwnerId == id);
            return Mapper.MapToOwnerDTO(owner, url);
        }

        public Owner GetByID(int id)
        {
            return db.Owners.FirstOrDefault(p => p.OwnerId == id);
        }

        public OwnerDTO Add(Owner owner, string url)
        {
            db.Owners.Add(owner);
            db.SaveChanges();
            return Mapper.MapToOwnerDTO(owner, url);
        }

        public OwnerDTO Update(Owner owner, string url)
        {
            db.MarkAsModified(owner);
            db.SaveChanges();
            return Mapper.MapToOwnerDTO(owner, url);
        }

        public void Remove(Owner owner)
        {
            db.Owners.Remove(owner);
            db.SaveChanges();
        }

        public bool OwnerExists(int id)
        {
            return db.Owners.Count(e => e.OwnerId == id) > 0;
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                {
                    db.Dispose();
                    db = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }

    public class PetService : IDisposable, IPetService
    {
        private IPetRegoContext db;

        public PetService() { }

        public PetService(IPetRegoContext context)
        {
            db = context;
        }

        public IQueryable<PetDTO> GetAll(string url)
        {

            List<PetDTO> petdtos = Mapper.MapToPetDTOs(db.Pets.ToList(), url);
            return petdtos.AsQueryable();
        }

        public IQueryable<PetDTO> GetPetsByOwnerId(int OwnerId, string url)
        {
            List<Pet> pets = db.Pets.Where(p => p.OwnerId == OwnerId).ToList();
            List<PetDTO> petdtos = Mapper.MapToPetDTOs(pets, url);
            return petdtos.AsQueryable();
        }

        public PetDTO GetDTOByID(int id, string url)
        {
            Pet pet = db.Pets.FirstOrDefault(p => p.OwnerId == id);
            return Mapper.MapToPetDTO(pet, url);
        }

        public Pet GetByID(int id)
        {
            return db.Pets.FirstOrDefault(p => p.PetId == id);
        }

        public PetDTO Add(Pet pet, string url)
        {
            db.Pets.Add(pet);
            db.SaveChanges();
            return Mapper.MapToPetDTO(pet, url);
        }

        public PetDTO Update(Pet pet, string url)
        {
            db.MarkAsModified(pet);
            db.SaveChanges();
            return Mapper.MapToPetDTO(pet, url);
        }

        public void Remove(Pet pet)
        {
            db.Pets.Remove(pet);
            db.SaveChanges();
        }

        public bool PetExists(int id)
        {
            return db.Pets.Count(e => e.PetId == id) > 0;
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                {
                    db.Dispose();
                    db = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}