/*
Contains models of Owner and Pet Entities.
The Entities are upgraded as required and migrated using Entitty Framework Code First Migration.

Build 101 :
    Added Model of Owner and Pet Entities.
    Added Data Annotation that defines Table and Column Attributes with Error Message for Model Validation.

Build 105 :
    Added Override Equals method to compare two Owners or Pets Entities.

Build 201 :
    Added the field "PetFood" in Pet Model to accomadate second version of the application.
    Entity framework Code first migration is used to update the database. 
    Commands used in Package Manager Console are :
        Enable-Migrations -ContextTypeName PetRego.Models.PetRegoContext
        Add-Migration AddPetFood
        Update-Database
        
*/

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;

namespace PetRego.Models
{
    public class Owner
    {
        [Key]
        public int OwnerId { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        [Display(Name = "OwnerName")]
        public string Ownername { get; set; }

        public virtual List<Pet> Pets { get; set; }

        public bool Equals(Owner owner)
        {
                        
            if (owner == null)
            {
                Debug.WriteLine("Object is null");
                return false;
            }
            else
            {
                /*
                if ( (this.OwnerId != 0 && owner.OwnerId != 0)  &&  (this.OwnerId != owner.OwnerId) ){
                    return false;
                }
                */
                //Debug.WriteLine("input owner in equal method- " + owner.ToString());
                if (this.Ownername.Equals(owner.Ownername))
                {
                    Debug.WriteLine("Owner names are same - " + this.Ownername + " - " + owner.Ownername);
                }
                
                return (this.Ownername.Equals(owner.Ownername)) 
                    && ComparePetLists(this.Pets, owner.Pets);
            }
        }

        private bool ComparePetLists(List<Pet> pets1, List<Pet> pets2)
        {
            if (pets1 == null && pets2 == null)
            {
                Debug.WriteLine("Pets are nulls" );
                return true;
            }

            if (pets1 == null)
                pets1 = new List<Pet>();

            if (pets2 == null)
                pets2 = new List<Pet>();

            if (pets1.Count != pets2.Count)
            {
                Debug.WriteLine("Pet counts are not same - " + pets1.Count + " - " + pets2.Count);
                return false;
            }

            foreach (Pet pet1 in pets1)
            {
                if(!pets2.Any(x => x.Equals(pet1))){
                    Debug.WriteLine("Pets2 does not contain pet1 - " + pet1.ToString());
                    return false;
                }
            }
            foreach (Pet pet2 in pets2)
            {
                if (!pets1.Any(x => x.Equals(pet2)))
                {
                    Debug.WriteLine("Pets1 does not contain pet2 - " + pet2.ToString());
                    return false;
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Ownername, Pets).GetHashCode();
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    public class Pet
    {
        public int PetId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [Display(Name = "PetName")]
        public string PetName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [Display(Name = "PetType")]
        public string PetType { get; set; }

        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [Display(Name = "PetFood")]
        public string PetFood { get; set; }

        public int OwnerId { get; set; }

        //"JsonIgnore" data annotation allows the Model to be flattened and provide a required Json Response.
        //The relation between Owner and Pet Entities is still maintained.
        [JsonIgnore]
        public virtual Owner Owner { get; set; }

        public bool Equals(Pet pet)
        {

            if (pet == null)
            {
                Debug.WriteLine("Object is null");
                return false;
            }
            else
            {
                /*
                if ((this.OwnerId != 0 && pet.OwnerId != 0) && (this.OwnerId != pet.OwnerId))
                {
                    return false;
                }
                if ((this.PetId != 0 && pet.PetId != 0) && (this.PetId != pet.PetId))
                {
                    return false;
                }
                */
                if (this.PetName.Equals(pet.PetName))
                {
                    Debug.WriteLine("Pet names are same - " + this.PetName + " - " + pet.PetName);
                }
                if (this.PetType.Equals(pet.PetType))
                {
                    Debug.WriteLine("Pet Types are same - " + this.PetType + " - " + pet.PetType);
                }
                if (this.OwnerId.Equals(pet.OwnerId))
                {
                    Debug.WriteLine("Owner Ids are same - " + this.PetType + " - " + pet.PetType);
                }
                return (this.PetName.Equals(pet.PetName)) && (this.PetType.Equals(pet.PetType));
            }
        }

        public override int GetHashCode()
        {
            return Tuple.Create(PetName, PetType).GetHashCode();
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

    }

    public class Link
    {
        public string Href { get; private set; }
        public string Rel { get; private set; }
        public string Method { get; private set; }

        public Link(string href, string rel, string method)
        {
            this.Href = href;
            this.Rel = rel;
            this.Method = method;
        }
    }
}