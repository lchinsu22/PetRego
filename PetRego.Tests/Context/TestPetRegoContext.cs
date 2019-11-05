/*
Implements Mocking - Mocks the Db Context that is used to add, retrieve, update or delete entities in mock database model.

Build 103 :
    Added method implementations of petRego Context. 

*/

using PetRego.Models;
using PetRego.Tests.DbSet;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetRego.Tests.Context
{

    public class TestPetRegoContext : IPetRegoContext
    {
        public TestPetRegoContext()
        {
            this.Owners = new TestOwnerDbSet();
            this.Pets = new TestpetDbSet();
        }

        public DbSet<Owner> Owners { get; set; }
        public DbSet<Pet> Pets { get; set; }

        public int SaveChanges()
        {
            return 0;
        }

        public void MarkAsModified(Owner item) { }
        public void MarkAsModified(Pet item) { }
        public void Dispose() { }
    }
}
