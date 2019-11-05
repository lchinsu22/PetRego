/*
Contains models of Owner and Pet Entities.
The Entities are upgraded as required and migrated using Entitty Framework Code First Migration.

Build 101 :
    Added Model of Owner and Pet Entities.
    Added Data Annotation that defines Table and Column Attributes with Error Message for Model Validation.

*/

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        public int OwnerId { get; set; }

        //"JsonIgnore" data annotation allows the Model to be flattened and provide a required Json Response.
        //The relation between Owner and Pet Entities is still maintained.
        [JsonIgnore]
        public virtual Owner Owner { get; set; }
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