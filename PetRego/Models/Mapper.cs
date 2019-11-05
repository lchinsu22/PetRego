/*
Static public Mapper class contains static methods that converts the Entity and List of Entities into 
respective DTOs and List of DTOs.

Build 103 :
    Added methods to convert Owner to OwnerDTO
    Added methods to convert List of Owners to List of OwnerDTOs.
    Added methods to convert Pet to petDTO
    Added methods to convert List of Pets to List of PetDTOs.
    Suggestion For Future Build - Could use Genrics to simplify implementation of repetitive methods.

*/

using PetRego.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetRegoSample.Models
{
    public static class Mapper
    {
        public static OwnerDTO MapToOwnerDTO(Owner owner, string url)
        {
            return new OwnerDTO(owner, url);
        }

        public static List<OwnerDTO> MapTOOwnerDTOs(List<Owner> owners, string url)
        {
            List<OwnerDTO> ownerdtos = new List<OwnerDTO>();
            foreach (Owner owner in owners)
            {
                ownerdtos.Add(MapToOwnerDTO(owner, url));
            }
            return ownerdtos;
        }

        public static PetDTO MapToPetDTO(Pet pet, string url)
        {
            return new PetDTO(pet, url); ;
        }

        public static List<PetDTO> MapToPetDTOs(List<Pet> pets, string url)
        {
            List<PetDTO> petdtos = new List<PetDTO>();
            foreach (Pet pet in pets)
            {
                petdtos.Add(MapToPetDTO(pet, url));
            }
            return petdtos;
        }
    }
}