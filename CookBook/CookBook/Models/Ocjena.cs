using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Models
{
    [ExcludeFromCodeCoverage]
    public class Ocjena
    {
        public int recipeId { get; private set; }
        private List<int> ocjene { get; set; }

        public Ocjena(int recipeId)
        {
            this.recipeId = recipeId;
            ocjene = new List<int>();
        }



        public void DodajOcjenu(int rating)
        {
            if (rating < 1 || rating > 5)
            {
                throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be between 1 and 5.");
            }
            ocjene.Add(rating);
        }

        public double GetProsjecnaOcjena()
        {
            if (ocjene.Count == 0)
            {
                return 0.0; 
            }
            return ocjene.Average();
        }

        public List<int> GetSveOcjene()
        {
            return new List<int>(ocjene); 
        }

        public override string ToString()
        {
            return ocjene.Count > 0
                ? $"ID: {recipeId}, Prosjecna ocjena: {GetProsjecnaOcjena():0.00}, Total ocjene: {ocjene.Count}"
                : $"ID: {recipeId}, No ocjene yet.";
        }
    }
}
