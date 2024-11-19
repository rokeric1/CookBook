using Models;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class IngredientService : IIngredientService
    {
        private readonly List<Ingredient> ingredients;
        private int currentId;

        public IngredientService()
        {
            ingredients = new List<Ingredient>();
            currentId = 1;
        }

        public void DodajIngredient(Ingredient sastojak, bool preserveId = false)
        {
            if (preserveId)
            {
                if (ingredients.Any(i => i.Id == sastojak.Id))
                {
                    throw new ArgumentException($"Sastojak sa ID {sastojak.Id} ve? postoji.");
                }
                ingredients.Add(sastojak);
                if (sastojak.Id >= currentId)
                {
                    currentId = sastojak.Id + 1;
                }
            }
            else
            {
                sastojak.Id = currentId++;
                ingredients.Add(sastojak);
            }
        }

        public Ingredient GetIngredientById(int id)
        {
            return ingredients.FirstOrDefault(i => i.Id == id);
        }

        public Ingredient GetIngredientByName(string naziv)
        {
            return ingredients.FirstOrDefault(i => i.Naziv.Equals(naziv, System.StringComparison.OrdinalIgnoreCase));
        }

        public int GetNextId()
        {
            return currentId;
        }

        public List<Ingredient> GetAllIngredients()
        {
            return new List<Ingredient>(ingredients);
        }
    }
}
