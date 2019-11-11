/*
provides implementation of Service classes providing business logic in handling requests.

Build 103 :
    Added Service method implementations.
    Suggestion For Future Build - Could use Genrics to simplify implementation of repetitive implementations.

Build 105 :
    Added OwnerExists and PetExists method for checking existing owner and pet entity.

Build 201 : 
    Converted the services to generic type and implemented generic interface method definitions 
    so as to accomadate various version of petDTO entity that has implemented IpetDTO interface.
*/

using PetRego.Models;
using PetRegoSample.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace PetRego.Service
{
    public class OwnerService<T> : IDisposable, IOwnerService<T>
    {
        private IPetRegoContext db;

        public OwnerService() { }

        public OwnerService(IPetRegoContext context)
        {
            db = context;
        }

        public IQueryable<OwnerDTO<T>> GetAll(string url, IPetDTO<T> Ipetdto)
        {

            List<OwnerDTO<T>> ownerdtos = Mapper<T>.MapTOOwnerDTOs(db.Owners.ToList(), url, Ipetdto);
            return ownerdtos.AsQueryable();
        }

        public OwnerDTO<T> GetDTOByID(int id, string url, IPetDTO<T> Ipetdto)
        {

            Owner owner = db.Owners.FirstOrDefault(p => p.OwnerId == id);
            return Mapper<T>.MapToOwnerDTO(owner, url, Ipetdto);
        }

        public Owner GetByID(int id)
        {
            return db.Owners.FirstOrDefault(p => p.OwnerId == id);
        }

        public OwnerDTO<T> Add(Owner owner, string url, IPetDTO<T> Ipetdto)
        {
            db.Owners.Add(owner);
            db.SaveChanges();
            return Mapper<T>.MapToOwnerDTO(owner, url, Ipetdto);
        }

        public OwnerDTO<T> Update(Owner owner, string url, IPetDTO<T> Ipetdto)
        {
            db.MarkAsModified(owner);
            foreach(Pet pet in owner.Pets)
            {
                db.MarkAsModified(pet);
            }
            db.SaveChanges();
            return Mapper<T>.MapToOwnerDTO(owner, url, Ipetdto);
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

        public bool OwnerExistsForPut(Owner owner)
        {
            Owner dbowner = db.Owners.FirstOrDefault(p => p.OwnerId == owner.OwnerId);
            if (dbowner == null)
                return false;

            foreach (Pet pet in owner.Pets)
            {
                //Debug.WriteLine("retrieved owner - " + own.ToString());
                if (!dbowner.Pets.Any(x => x.PetId == pet.PetId))
                {
                    Debug.WriteLine("Equal to input Owner");
                    return false;
                }
                if(pet.OwnerId != owner.OwnerId)
                {
                    return false;
                }
            }
            return true;
        }

        public bool OwnerExists(Owner owner)
        {
            List<Owner> owners = db.Owners.Where(ow => ow.Ownername.Equals(owner.Ownername)).ToList();
            //Debug.WriteLine("input owner - " + owner.ToString());
            foreach(Owner own in owners)
            {
                //Debug.WriteLine("retrieved owner - " + own.ToString());
                if (own.Equals(owner))
                {
                    Debug.WriteLine("Equal to input Owner");
                    return true;
                }
            }
            return false;
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

    public class PetService<T> : IDisposable, IPetService<T>
    {
        private IPetRegoContext db;

        public PetService() { }

        public PetService(IPetRegoContext context)
        {
            db = context;
        }

        public IQueryable<T> GetAll(string url, IPetDTO<T> Ipetdto)
        {

            List<T> petdtos = Mapper<T>.MapToPetDTOs(db.Pets.ToList(), url, Ipetdto);
            return petdtos.AsQueryable();
        }

        public IQueryable<T> GetPetsByOwnerId(int OwnerId, string url, IPetDTO<T> Ipetdto)
        {
            List<Pet> pets = db.Pets.Where(p => p.OwnerId == OwnerId).ToList();
            List<T> petdtos = Mapper<T>.MapToPetDTOs(pets, url, Ipetdto);
            return petdtos.AsQueryable();
        }

        public T GetDTOByID(int id, string url, IPetDTO<T> Ipetdto)
        {
            Pet pet = db.Pets.FirstOrDefault(p => p.PetId == id);
            if (pet != null)
            {
                return Mapper<T>.MapToPetDTO(pet, url, Ipetdto);
            }else
            {
                return default(T);
            }
            
        }

        public Pet GetByID(int id)
        {
            return db.Pets.FirstOrDefault(p => p.PetId == id);
        }

        public T Add(Pet pet, string url, IPetDTO<T> Ipetdto)
        {
            db.Pets.Add(pet);
            db.SaveChanges();
            return Mapper<T>.MapToPetDTO(pet, url, Ipetdto);
        }

        public T Update(Pet pet, string url, IPetDTO<T> Ipetdto)
        {
            db.MarkAsModified(pet);
            db.SaveChanges();
            return Mapper<T>.MapToPetDTO(pet, url, Ipetdto);
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

        public bool PetExists(Pet pet)
        {
            List<Pet> pets = db.Pets.Where(p => p.PetName == pet.PetName).ToList();
            foreach (Pet pt in pets)
            {
                if (pt.Equals(pet))
                {
                    return true;
                }
            }
            return false;
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