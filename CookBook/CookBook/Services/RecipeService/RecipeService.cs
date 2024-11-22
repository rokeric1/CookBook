using Models;
using Services; 
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class RecipeService : IRecipeService
    {
        private readonly List<Recipe> recepti;
        private readonly IIngredientService ingredientService;
        private int currentRecipeId;

        public RecipeService(IIngredientService ingredientService)
        {
            this.ingredientService = ingredientService;
            recepti = new List<Recipe>();
            currentRecipeId = 1; 
        }

        public void DodajRecept(Recipe recept)
        {
            if (recepti.Any(r => r.Naziv.Equals(recept.Naziv, System.StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException($"Recept sa nazivom \"{recept.Naziv}\" već postoji.");
            }

            recept.Id = currentRecipeId++;
            recepti.Add(recept);
        }


        public void AzurirajRecept(int id, Recipe azuriraniRecept)
        {
            var recept = GetReceptPoId(id);
            if (recept == null)
                throw new Exception($"Recept sa ID {id} nije pronađen.");
            recept.Naziv = azuriraniRecept.Naziv;
            recept.Sastojci = azuriraniRecept.Sastojci;
            recept.Kategorija = azuriraniRecept.Kategorija;
            recept.VrijemePripreme = azuriraniRecept.VrijemePripreme;
            recept.Upute = azuriraniRecept.Upute;
            recept.Popularnost = azuriraniRecept.Popularnost;
        }

        public void ObrisiRecept(int id)
        {
            var recept = GetReceptPoId(id);
            if (recept == null)
                throw new Exception($"Recept sa ID {id} nije pronađen.");
            recepti.Remove(recept);
        }

        public Recipe GetReceptPoId(int id)
        {
            return recepti.FirstOrDefault(r => r.Id == id);
        }

        public List<Recipe> GetSviRecepti()
        {
            return new List<Recipe>(recepti);
        }

        public List<Recipe> PretraziPoNazivu(string Naziv)
        {
            return recepti
                .Where(r => r.Naziv.Equals(Naziv, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }


        public List<Recipe> PretraziPoAtributima(int maxVrijemePripreme = -1, string sastojak = "", int minPopularnost = -1)
        {
            return recepti.Where(recept =>
                (maxVrijemePripreme == -1 || recept.VrijemePripreme <= maxVrijemePripreme) &&
                (string.IsNullOrEmpty(sastojak) || recept.Sastojci.Keys.Any(s =>
                {
                    var ingredient = ingredientService.GetIngredientById(s);
                    return ingredient?.Naziv.Contains(sastojak, StringComparison.OrdinalIgnoreCase) == true;
                })) &&
                (minPopularnost == -1 || recept.Popularnost >= minPopularnost)
            ).ToList();
        }

        public int GetNextId()
        {
            return currentRecipeId++;
        }
    }
}
