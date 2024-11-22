using System.Diagnostics.CodeAnalysis;

namespace Models
{
    [ExcludeFromCodeCoverage]
    public class Ingredient
    {
        public int Id { get; set; } 
        public string Naziv { get; set; } 
        public string Nutrijenti { get; set; } 
        public double EkoUtjecaj { get; set; } 
        public decimal Cijena { get; set; } 
        public bool Dostupan { get; set; } 

        public Ingredient() { }

        public Ingredient(int id, string naziv, string nutrijenti, double ekoUtjecaj, decimal cijena, bool dostupan)
        {
            Id = id;
            Naziv = naziv;
            Nutrijenti = nutrijenti;
            EkoUtjecaj = ekoUtjecaj;
            Cijena = cijena;
            Dostupan = dostupan;
        }

        public override string ToString()
        {
            return $"Id: {Id}, Naziv: {Naziv}, Nutrijenti: {Nutrijenti}, Eko Utjecaj: {EkoUtjecaj}, Cijena: {Cijena:C}, Dostupan: {Dostupan}";
        }
    }
}
