/*
Interface defines the Context. 
Allows for Dependency Injection on Context. Used in Testing.

Build 102 :
    Added Conext Class Methods Defitions.
    Suggestion for future build - could use Generics to remove dependency on Entity in MarkAsModified method. 

Build 103 :
    Added overload for MarkAsModified methods to remove type dependency while calling the methods.
    Suggestion for future build - Look into using Genrics to remove dependency.

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
        void MarkAsModified(Owner item);
        void MarkAsModified(Pet item);
    }
}
