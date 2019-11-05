﻿/*
Contains Db Context which is used to add, retrieve, update or delete entities in database model.

Build 101 :
    Added MPetRegoContext extending DbContext.
    Added Dbsets of Owners and Pets entities in the context.
    Implemented Required methods for implememting DbContext interface.

*/

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PetRego.Models
{
    public class PetRegoContext : DbContext
    {
        //The Db connection string is configured in Web.config with name PetRegoContext.
        //Change the connection string under the name PetRegoContext to connect to the new database. 
        //The model will  be built automatically by Entity Framework.
        public PetRegoContext() : base("name=PetRegoContext")
        {
        }

        public DbSet<Owner> Owners { get; set; }
        public DbSet<Pet> Pets { get; set; }
    }
}
