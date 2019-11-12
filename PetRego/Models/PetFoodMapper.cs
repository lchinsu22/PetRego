/*
Static public PetFoodMapper class contains static methods that sets the food for the pet depending on the Pet Type. 

Build 202 :
    Added methods to set the food for the pet depending on the Pet Type.

*/

namespace PetRego.Models
{
    public static class PetFoodMapper
    {
        public static Owner setPetFood(Owner owner)
        {
            if (owner != null)
            {
                foreach (Pet pet in owner.Pets)
                {
                    pet.PetFood = getPetFood(pet.PetType);
                }
                return owner;
            }
            else
            {
                return null;
            }
        }

        public static Pet setPetFood(Pet pet)
        {
            if (pet != null)
            {
                pet.PetFood = getPetFood(pet.PetType);
                return pet;
            }
            else
            {
                return null;
            }
        }

        private static string getPetFood(string PetType)
        {
            string PetFood = "";
            switch (PetType.ToLower())
            {
                case "dog":
                    PetFood = "Bone";
                    break;
                case "cat":
                    PetFood = "Fish";
                    break;
                case "chicken":
                    PetFood = "Corn";
                    break;
                case "snake":
                    PetFood = "Mice";
                    break;
                default:
                    PetFood = "Unknown";
                    break;
            }
            return PetFood;
        }
    }
}