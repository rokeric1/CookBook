
using System.Diagnostics.CodeAnalysis;

namespace Models
{
    [ExcludeFromCodeCoverage]
    public class Tag
	{
		public int Id { get; set; }
		public string Naziv { get; set; }
		public List<int> Recepti { get; set; } = new List<int>();

		public Tag(int id, string naziv)
		{
			Id = id;
			Naziv = naziv;
		}

		public override string ToString()
		{
			return $"Id: {Id}, Naziv: {Naziv}, Broj povezanih recepata: {Recepti.Count}";
		}
	}
}