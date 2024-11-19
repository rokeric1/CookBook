using Models;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class ShoppingListService : IShoppingListService
    {
        private readonly Dictionary<int, double> listaKupovine = new Dictionary<int, double>();
        private readonly IIngredientService ingredientService;

        public ShoppingListService(IIngredientService ingredientService)
        {
            this.ingredientService = ingredientService;
        }

        public void DodajNaListu(int ingredientId, double kolicina)
        {
            if (listaKupovine.ContainsKey(ingredientId))
            {
                listaKupovine[ingredientId] += kolicina;
            }
            else
            {
                listaKupovine[ingredientId] = kolicina;
            }
        }

        public decimal IzracunajUkupnuCijenu()
        {
            return listaKupovine.Sum(kvp =>
            {
                var ingredient = ingredientService.GetIngredientById(kvp.Key);
                return ingredient != null ? (decimal)kvp.Value * ingredient.Cijena : 0;
            });
        }

        public Dictionary<Ingredient, double> GetLista()
        {
            return listaKupovine.ToDictionary(
                kvp => ingredientService.GetIngredientById(kvp.Key),
                kvp => kvp.Value
            );
        }

        public override string ToString()
        {
            string listaString = string.Join("\n", listaKupovine.Select(kvp =>
            {
                var ingredient = ingredientService.GetIngredientById(kvp.Key);
                if (ingredient == null)
                {
                    return $"Id: {kvp.Key} nije pronađen.";
                }
                return $"{ingredient.Naziv}:\n" +
                       $"  - Količina: {kvp.Value}\n" +
                       $"  - Cijena: {ingredient.Cijena:C} (Ukupno: {(decimal)kvp.Value * ingredient.Cijena:C})\n" +
                       $"  - Nutrijenti: {ingredient.Nutrijenti}\n" +
                       $"  - Eko Utjecaj: {ingredient.EkoUtjecaj:0.00}\n" +
                       $"  - Dostupnost: {(ingredient.Dostupan ? "Dostupan" : "Nije Dostupan")}";
            }));
            return $"Lista kupovine:\n{listaString}\n\nUkupna Cijena: {IzracunajUkupnuCijenu():C}";
        }
    }
}
