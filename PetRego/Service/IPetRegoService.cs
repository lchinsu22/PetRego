/*
provide class definitions for Service classes that helps in providing the budiness logic for requests.

Build 103 :
    Added Service classes definition.
    Suggestion For Future Build - Could use Genrics to simplify implementation of repetitive definitions.

Build 105 :
    Added Definition for OwnerExists and PetExists method for checking existing owner and pet entity.

Build 201 : 
    Converted interface method definitions to a generic type so as to accomadate various version of petDTO entity 
    that has implemented IpetDTO interface.
*/

using PetRego.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetRego.Service
{
    public interface IOwnerService<T>
    {
        IQueryable<OwnerDTO<T>> GetAll(string url, IPetDTO<T> Ipetdto);
        OwnerDTO<T> GetDTOByID(int id, string url, IPetDTO<T> Ipetdto);
        Owner GetByID(int id);
        OwnerDTO<T> Add(Owner owner, string url, IPetDTO<T> Ipetdto);
        OwnerDTO<T> Update(Owner owner, string url, IPetDTO<T> Ipetdto);
        void Remove(Owner owner);
        bool OwnerExists(int id);
        bool OwnerExists(Owner owner);
        bool OwnerExistsForPut(Owner owner);
        void Dispose();
    }

    public interface IPetService<T>
    {
        IQueryable<T> GetAll(string url, IPetDTO<T> Ipetdto);
        IQueryable<T> GetPetsByOwnerId(int OwnerId, string url, IPetDTO<T> Ipetdto);
        T GetDTOByID(int id, string url, IPetDTO<T> Ipetdto);
        Pet GetByID(int id);
        T Add(Pet pet, string url, IPetDTO<T> Ipetdto);
        T Update(Pet pet, string url, IPetDTO<T> Ipetdto);
        void Remove(Pet pet);
        bool PetExists(int id);
        bool PetExists(Pet pet);
        void Dispose();
    }

}
