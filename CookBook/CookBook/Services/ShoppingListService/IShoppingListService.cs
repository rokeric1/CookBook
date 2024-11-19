using Models;
using System.Collections.Generic;

namespace Services
{
    public interface IShoppingListService
    {
        void DodajNaListu(int ingredientId, double kolicina);
        decimal IzracunajUkupnuCijenu();
        Dictionary<Ingredient, double> GetLista();
    }
}
