/*
Implements Mocking - Mocks Owner database table.
Test Owner Db set contains the Owner Entities mocking a Owner Db table. 

Build 103 :
    Added implementation of Db Set for Owner DbSet mocking the Owner Db Table. 

*/

using PetRego.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetRego.Tests.DbSet
{
    class TestOwnerDbSet : TestDbSet<Owner>
    {
        public override Owner Find(params object[] keyValues)
        {
            return this.SingleOrDefault(owner => owner.OwnerId == (int)keyValues.Single());
        }
    }
}
