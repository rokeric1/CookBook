using Models;
using System.Collections.Generic;

namespace Services
{
    public interface IRecipeService
    {
        void DodajRecept(Recipe recept);
        void AzurirajRecept(int id, Recipe azuriraniRecept);
        void ObrisiRecept(int id);
        Recipe GetReceptPoId(int id);
        List<Recipe> GetSviRecepti();
        List<Recipe> PretraziPoNazivu(string naziv);
        List<Recipe> PretraziPoAtributima(int maxVrijemePripreme = -1, string sastojak = "", int minPopularnost = -1);
        int GetNextId();
    }
}
