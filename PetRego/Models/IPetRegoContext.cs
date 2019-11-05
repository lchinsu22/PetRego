/*
Interface defines the Context. 
Allows for Dependency Injection on Context. Used in Testing.

Build 101 :
    Added Conext Class Methods Defitions.

*/

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetRego.Models
{

    public interface IPetRegoContext : IDisposable
    {
        DbSet<Owner> Owners { get; }
        DbSet<Pet> Pets { get; }
        int SaveChanges();
        void MarkOwnerAsModified(Owner item);
        void MarkPetAsModified(Pet item);
    }
}
