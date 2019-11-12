/*
Static public Mapper class contains static methods that converts the Entity and List of Entities into 
respective DTOs and List of DTOs.

Build 103 :
    Added methods to convert Owner to OwnerDTO
    Added methods to convert List of Owners to List of OwnerDTOs.
    Added methods to convert Pet to petDTO
    Added methods to convert List of Pets to List of PetDTOs.
    Suggestion For Future Build - Could use Genrics to simplify implementation of repetitive methods.

Build 201 : 
    Converted mapper to a generic type and changed the mapping methods with generic type paramater and return type 
    so as to accomadate various version of petDTO entity that has implemented IpetDTO interface.
*/

using PetRego.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetRegoSample.Models
{
    public static class Mapper<T>
    {

        public static OwnerDTO<T> MapToOwnerDTO(Owner owner, string url, IPetDTO<T> Ipetdto)
        {
            return new OwnerDTO<T>(owner, url, Ipetdto);
        }

        public static List<OwnerDTO<T>> MapTOOwnerDTOs(List<Owner> owners, string url, IPetDTO<T> Ipetdto)
        {
            List<OwnerDTO<T>> ownerdtos = new List<OwnerDTO<T>>();
            foreach (Owner owner in owners)
            {
                ownerdtos.Add(MapToOwnerDTO(owner, url, Ipetdto));
            }
            return ownerdtos;
        }

        public static T MapToPetDTO(Pet pet, string url, IPetDTO<T> Ipetdto)
        {
            return Ipetdto.getDTO(pet, url);
        }

        public static List<T> MapToPetDTOs(List<Pet> pets, string url, IPetDTO<T> Ipetdto)
        {
            List<T> petdtos = new List<T>();
            foreach (Pet pet in pets)
            {
                petdtos.Add(MapToPetDTO(pet, url, Ipetdto));
            }
            return petdtos;
        }
    }
}