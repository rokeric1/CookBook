using Models;
using Services;
using System.Collections.Generic;
using System.Linq;

namespace Services
{

    public class TagService : ITagService
    {
        private readonly List<Tag> tagovi = new List<Tag>();
        private int currentTagId = 1;

        public Tag KreirajTag(string naziv)
        {
            if (tagovi.Any(t => t.Naziv.Equals(naziv, StringComparison.OrdinalIgnoreCase)))
                throw new Exception($"Tag sa nazivom '{naziv}' već postoji.");

            var noviTag = new Tag(currentTagId++, naziv);
            tagovi.Add(noviTag);
            return noviTag;
        }

        public void DodajReceptUTag(string nazivTaga, int receptId)
        {
            var tag = tagovi.FirstOrDefault(t => t.Naziv.Equals(nazivTaga, StringComparison.OrdinalIgnoreCase));
            if (tag == null)
                throw new Exception($"Tag sa nazivom '{nazivTaga}' ne postoji.");

            if (!tag.Recepti.Contains(receptId))
                tag.Recepti.Add(receptId);
        }

        public List<Recipe> PrikaziRecepteZaTag(string nazivTaga, IRecipeService recipeService)
        {
            var tag = tagovi.FirstOrDefault(t => t.Naziv.Equals(nazivTaga, StringComparison.OrdinalIgnoreCase));
            if (tag == null)
                throw new Exception($"Tag sa nazivom '{nazivTaga}' ne postoji.");

            var recepti = new List<Recipe>();
            foreach (var receptId in tag.Recepti)
            {
                var recept = recipeService.GetReceptPoId(receptId);
                if (recept != null)
                    recepti.Add(recept);
            }

            return recepti;
        }

        public List<Tag> PrikaziSveTagove()
        {
            return new List<Tag>(tagovi);
        }

        public void ObrisiTag(string nazivTaga)
        {
            var tag = tagovi.FirstOrDefault(t => t.Naziv.Equals(nazivTaga, StringComparison.OrdinalIgnoreCase));
            if (tag != null)
                tagovi.Remove(tag);
        }
    }
}
