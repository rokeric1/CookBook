using Models;
using System;
using System.Collections.Generic;

namespace Services
{
    public class OcjenaService : IOcjenaService
    {
        private readonly Dictionary<int, Ocjena> ocjene = new Dictionary<int, Ocjena>();

        public void DodajOcjenu(int recipeId, int ocjena)
        {
            if (!ocjene.ContainsKey(recipeId))
            {
                ocjene[recipeId] = new Ocjena(recipeId);
            }
            ocjene[recipeId].DodajOcjenu(ocjena);
        }

        public double GetProsjecnaOcjena(int recipeId)
        {
            if (ocjene.ContainsKey(recipeId))
            {
                return ocjene[recipeId].GetProsjecnaOcjena();
            }
            return 0.0;
        }

        public List<int> GetSveOcjene(int recipeId)
        {
            if (ocjene.ContainsKey(recipeId))
            {
                return ocjene[recipeId].GetSveOcjene();
            }
            return new List<int>();
        }
    }
}
