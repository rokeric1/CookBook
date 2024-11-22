using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Models
{
    [ExcludeFromCodeCoverage]
    public class ShoppingList
    {
        public Dictionary<Ingredient, double> Stavke { get; private set; } 

        public ShoppingList()
        {
            Stavke = new Dictionary<Ingredient, double>();
        }

        public void AddItem(Ingredient ingredient, double quantity)
        {
            if (Stavke.ContainsKey(ingredient))
            {
                Stavke[ingredient] += quantity;
            }
            else
            {
                Stavke[ingredient] = quantity;
            }
        }

        public decimal Total()
        {
            return Stavke.Sum(kvp => kvp.Key.Cijena * (decimal)kvp.Value);
        }

        public override string ToString()
        {
            string StavkeString = string.Join("\n", Stavke.Select(kvp => $"{kvp.Key.Naziv}: {kvp.Value} (Price: {kvp.Key.Cijena:C})"));
            return $"Shopping lista:\n{StavkeString}\nTotal: {Total():C}";
        }
    }
}
