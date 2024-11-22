using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace Models
{
    [ExcludeFromCodeCoverage]
    public class Recipe
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        [XmlIgnore] 
        public Dictionary<int, double> Sastojci { get; set; }
        public string Kategorija { get; set; }
        public int VrijemePripreme { get; set; }
        public string Upute { get; set; }
        public int Popularnost { get; set; }

        [XmlArray("Sastojci")]
        [XmlArrayItem("SastojakKolicina")]
        public List<SastojakKolicina> SastojciList
        {
            get
            {
                var list = new List<SastojakKolicina>();
                foreach (var kvp in Sastojci)
                {
                    list.Add(new SastojakKolicina(kvp.Key, kvp.Value));
                }
                return list;
            }
            set
            {
                Sastojci = new Dictionary<int, double>();
                if (value != null)
                {
                    foreach (var sastojak in value)
                    {
                        Sastojci[sastojak.IngredientId] = sastojak.Kolicina;
                    }
                }
            }
        }

        public Recipe()
        {
            Sastojci = new Dictionary<int, double>();
        }

        public Recipe(int id, string naziv, Dictionary<int, double> sastojci, string kategorija, int vrijemePripreme, string upute, int popularnost)
        {
            Id = id;
            Naziv = naziv;
            Sastojci = sastojci;
            Kategorija = kategorija;
            VrijemePripreme = vrijemePripreme;
            Upute = upute;
            Popularnost = popularnost;
        }

        public override string ToString()
        {
            return $"Id: {Id}, Naziv: {Naziv}, Kategorija: {Kategorija}, Vrijeme pripreme: {VrijemePripreme} min, Popularnost: {Popularnost}";
        }
    }
}
