
using Models;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ITagService
    {
        Tag KreirajTag(string naziv);
        void DodajReceptUTag(string nazivTaga, int receptId);
        List<Recipe> PrikaziRecepteZaTag(string nazivTaga, IRecipeService recipeService);
        List<Tag> PrikaziSveTagove();
        void ObrisiTag(string nazivTaga);
    }
}

