/*
provide class definitions for Service classes that helps in providing the budiness logic for requests.

Build 103 :
    Added Service classes definition.
    Suggestion For Future Build - Could use Genrics to simplify implementation of repetitive definitions.

Build 105 :
    Added Definition for OwnerExists and PetExists method for checking existing owner and pet entity.
*/

using PetRego.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetRego.Service
{
    public interface IOwnerService
    {
        IQueryable<OwnerDTO> GetAll(string url);
        OwnerDTO GetDTOByID(int id, string url);
        Owner GetByID(int id);
        OwnerDTO Add(Owner owner, string url);
        OwnerDTO Update(Owner owner, string url);
        void Remove(Owner owner);
        bool OwnerExists(int id);
        bool OwnerExists(Owner owner);
        bool OwnerExistsForPut(Owner owner);
        void Dispose();
    }

    public interface IPetService
    {
        IQueryable<PetDTO> GetAll(string url);
        IQueryable<PetDTO> GetPetsByOwnerId(int OwnerId, string url);
        PetDTO GetDTOByID(int id, string url);
        Pet GetByID(int id);
        PetDTO Add(Pet pet, string url);
        PetDTO Update(Pet pet, string url);
        void Remove(Pet pet);
        bool PetExists(int id);
        bool PetExists(Pet pet);
        void Dispose();
    }

}
