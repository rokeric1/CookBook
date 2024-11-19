using Models;
using System.Collections.Generic;

namespace Services
{
    public interface IDataExportImportService
    {
        void EksportujRecepte(string format, List<Recipe> recepti);
        List<Recipe> ImportujRecepte(string format);
        void EksportujSastojke(string format, List<Ingredient> sastojci);
        List<Ingredient> ImportujSastojke(string format);
    }
}
