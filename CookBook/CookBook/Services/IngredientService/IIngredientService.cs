using Models;
using System.Collections.Generic;

namespace Services
{
    public interface IIngredientService
    {
        void DodajIngredient(Ingredient sastojak, bool preserveId = false);
        Ingredient GetIngredientById(int id);
        Ingredient GetIngredientByName(string naziv);
        int GetNextId();
        List<Ingredient> GetAllIngredients(); 
    }
}