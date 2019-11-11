/*
Data Transfer Object (DTO) classes used for model representation for clients.
DTO also include Links in the Entities to implement HATEOAS in Response. 


Build 103 :
    Added DTO classes for Owner and Pet Entity.
    Defined Contructor which transforms a Owner or Pet Entity into corresponding DTO objects.

Build 201 : 
    Added the second version of petDTO - petV2DTO which contains the Pet Food as an added attribute. 
    petDTO remains unchanged and serves the pet information with out any change to the requests from first version of the application.
    Added an interface IpetDTO and changed both versions of pet DTO to be implemented from the interface 
    so as to achieve dependency injection along with generic types in code.
    

*/

using Newtonsoft.Json;
using PetRego.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetRego.Models
{
    public class OwnerDTO<T>
    {
        public int OwnerId { get; set; }
        public string Ownername { get; set; }
        public virtual List<T> Pets { get; set; }
        public List<Link> Links { get; set; }

        //"JsonIgnore" data annotation allows the OwnerUrl field to ignored in Json response back to user.
        //OwnerUrl specifies the Owners Route
        [JsonIgnore]
        public string OwnerUrl = "/api/Owners/";
        [JsonIgnore]
        private IPetDTO<T> Ipetdto;

        public OwnerDTO(IPetDTO<T> _Ipetdto)
        {
            Ipetdto = _Ipetdto;
            Links = new List<Link>();
        }

        public OwnerDTO(int id, string name, List<Pet> pets, string Url, IPetDTO<T> _Ipetdto)
        {
            Ipetdto = _Ipetdto;
            this.OwnerId = id;
            this.Ownername = Ownername;
            this.Pets = new List<T>();
            foreach (Pet pet in pets)
            {
                this.Pets.Add(Ipetdto.getDTO(pet,Url));

            }
            Links = getOwnerLinks(Url + this.OwnerUrl + id);
        }

        //Transforms Owner into OwnerDTO.
        //Url - Authority partial Url contains the hostname and port. 
        public OwnerDTO(Owner owner, string Url, IPetDTO<T> _Ipetdto)
        {
            Ipetdto = _Ipetdto;
            this.OwnerId = owner.OwnerId;
            this.Ownername = owner.Ownername;
            this.Pets = new List<T>();
            if (owner.Pets != null)
            {
                foreach (Pet pet in owner.Pets)
                {
                    this.Pets.Add(Ipetdto.getDTO(pet, Url));
                }
            }
            Links = getOwnerLinks(Url + this.OwnerUrl + owner.OwnerId);
        }

        private static List<Link> getOwnerLinks(string uri)
        {
            List<Link> links = new List<Link>();
            links.Add(new Link(uri, "self", "GET"));
            links.Add(new Link(uri + "/Pets", "get-pets", "GET"));
            links.Add(new Link(uri, "update-Owner", "PUT"));
            links.Add(new Link(uri, "delete-Owner", "DELETE"));
            return links;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    public class PetDTO : IPetDTO<PetDTO>
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

        public PetDTO getDTO(Pet pet, string Url)
        {
            PetDTO dto = new PetDTO();
            dto.PetId = pet.PetId;
            dto.PetName = pet.PetName;
            dto.PetType = pet.PetType;
            dto.OwnerId = pet.OwnerId;
            dto.Links = getPetLinks(Url + this.PetUrl + pet.PetId);
            return dto;
        }

        public static List<Link> getPetLinks(string uri)
        {
            List<Link> links = new List<Link>();
            links.Add(new Link(uri, "self", "GET"));
            links.Add(new Link(uri, "update-Pet", "PUT"));
            links.Add(new Link(uri, "delete-Pet", "DELETE"));
            return links;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    public class PetV2DTO : IPetDTO<PetV2DTO>
    {
        public int PetId { get; set; }
        public string PetName { get; set; }
        public string PetType { get; set; }
        public string PetFood { get; set; }
        public int OwnerId { get; set; }
        public List<Link> Links { get; set; }

        //"JsonIgnore" data annotation allows the PetUrl field to ignored in Json response back to user.
        //PetUrl specifies the Pets Route
        [JsonIgnore]
        public string PetUrl = "/api/Pets/";
        PetV2DTO()
        {
            Links = new List<Link>();
        }

        public PetV2DTO(int PetId, string PetName, string PetType, string PetFood, int OwnerId, string Url)
        {
            this.PetId = PetId;
            this.PetName = PetName;
            this.PetType = PetType;
            this.OwnerId = OwnerId;
            this.PetFood = PetFood;
            this.Links = getPetLinks(Url + this.PetUrl + PetId);
        }

        //Transforms Pet into PetDTO.
        //Url - Authority partial Url contains the hostname and port. 
        public PetV2DTO(Pet pet, string Url)
        {
            this.PetId = pet.PetId;
            this.PetName = pet.PetName;
            this.PetType = pet.PetType;
            this.OwnerId = pet.OwnerId;
            this.PetFood = pet.PetFood;
            this.Links = getPetLinks(Url + this.PetUrl + pet.PetId);
        }

        public PetV2DTO getDTO(Pet pet, string Url)
        {
            PetV2DTO dto = new PetV2DTO();
            dto.PetId = pet.PetId;
            dto.PetName = pet.PetName;
            dto.PetType = pet.PetType;
            dto.OwnerId = pet.OwnerId;
            dto.PetFood = pet.PetFood;
            dto.Links = getPetLinks(Url + this.PetUrl + pet.PetId);
            return dto;
        }

        public static List<Link> getPetLinks(string uri)
        {
            List<Link> links = new List<Link>();
            links.Add(new Link(uri, "self", "GET"));
            links.Add(new Link(uri, "update-Pet", "PUT"));
            links.Add(new Link(uri, "delete-Pet", "DELETE"));
            return links;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    public interface IPetDTO<T>
    {
        T getDTO(Pet pet, string Url);
    }
}