using Models;
using System.Collections.Generic;

namespace Services
{
    public interface IDataExportImportService
    {
        void EksportujRecepte(string format, List<Recipe> recepti, string filePath = "recepti.json");
        List<Recipe> ImportujRecepte(string format, string filePath = "recepti.json");
        void EksportujSastojke(string format, List<Ingredient> sastojci, string filePath = "ingredients.json");
        List<Ingredient> ImportujSastojke(string format, string filePath = "ingredients.json");
    }
}
