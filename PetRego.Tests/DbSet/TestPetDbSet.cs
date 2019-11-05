/*
Implements Mocking - Mocks Pet database table.
Test Pet Db set contains the Pet Entities mocking a Pet Db table. 

Build 103 :
    Added implementation of Db Set for Pet DbSet mocking the Pet Db Table. 

*/

using PetRego.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetRego.Tests.DbSet
{
    class TestpetDbSet : TestDbSet<Pet>
    {
        public override Pet Find(params object[] keyValues)
        {
            return this.SingleOrDefault(pet => pet.PetId == (int)keyValues.Single());
        }
    }
}
