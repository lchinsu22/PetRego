/*
Data Transfer Object (DTO) classes used for model representation for clients.
DTO also include Links in the Entities to implement HATEOAS in Response. 


Build 103 :
    Added DTO classes for Owner and Pet Entity.
    Defined Contructor which transforms a Owner or Pet Entity into corresponding DTO objects.


*/

using Newtonsoft.Json;
using PetRego.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetRego.Models
{
    public class OwnerDTO
    {
        public int OwnerId { get; set; }
        public string Ownername { get; set; }
        public virtual List<PetDTO> Pets { get; set; }
        public List<Link> Links { get; set; }

        //"JsonIgnore" data annotation allows the OwnerUrl field to ignored in Json response back to user.
        //OwnerUrl specifies the Owners Route
        [JsonIgnore]
        public string OwnerUrl = "/api/Owners/";

        public OwnerDTO()
        {
            Links = new List<Link>();
        }

        public OwnerDTO(int id, string name, List<Pet> pets, string Url)
        {
            this.OwnerId = id;
            this.Ownername = Ownername;
            this.Pets = new List<PetDTO>();
            foreach (Pet pet in pets)
            {
                this.Pets.Add(new PetDTO(pet, Url));
            }
            Links = getOwnerLinks(Url + this.OwnerUrl + id);
        }

        //Transforms Owner into OwnerDTO.
        //Url - Authority partial Url contains the hostname and port. 
        public OwnerDTO(Owner owner, string Url)
        {
            this.OwnerId = owner.OwnerId;
            this.Ownername = owner.Ownername;
            this.Pets = new List<PetDTO>();
            foreach (Pet pet in owner.Pets)
            {
                this.Pets.Add(new PetDTO(pet, Url));
            }
            Links = getOwnerLinks(Url + this.OwnerUrl + owner.OwnerId);
        }

        public static List<Link> getOwnerLinks(string uri)
        {
            List<Link> links = new List<Link>();
            links.Add(new Link(uri, "self", "GET"));
            links.Add(new Link(uri + "/Pets", "get-pets", "GET"));
            links.Add(new Link(uri, "update-Owner", "PUT"));
            links.Add(new Link(uri, "delete-Owner", "DELETE"));
            return links;
        }
    }

    public class PetDTO
    {
        public int PetId { get; set; }
        public string PetName { get; set; }
        public string PetType { get; set; }
        public int OwnerId { get; set; }
        public List<Link> Links { get; set; }

        //"JsonIgnore" data annotation allows the PetUrl field to ignored in Json response back to user.
        //PetUrl specifies the Pets Route
        [JsonIgnore]
        public string PetUrl = "/api/Pets/";

        public PetDTO()
        {
            Links = new List<Link>();
        }

        public PetDTO(int PetId, string PetName, string PetType, int OwnerId, string Url)
        {
            this.PetId = PetId;
            this.PetName = PetName;
            this.PetType = PetType;
            this.OwnerId = OwnerId;
            Links = getPetLinks(Url + this.PetUrl + PetId);
        }

        //Transforms Pet into PetDTO.
        //Url - Authority partial Url contains the hostname and port. 
        public PetDTO(Pet pet, string Url)
        {
            this.PetId = pet.PetId;
            this.PetName = pet.PetName;
            this.PetType = pet.PetType;
            this.OwnerId = pet.OwnerId;
            Links = getPetLinks(Url + this.PetUrl + pet.PetId);
        }

        public static List<Link> getPetLinks(string uri)
        {
            List<Link> links = new List<Link>();
            links.Add(new Link(uri, "self", "GET"));
            links.Add(new Link(uri, "update-Pet", "PUT"));
            links.Add(new Link(uri, "delete-Pet", "DELETE"));
            return links;
        }
    }
}