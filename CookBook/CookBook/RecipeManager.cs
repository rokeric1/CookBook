public class RecipeManager
{
    List<Recipe> recepti = new List<Recipe>();

    public void DodajRecept(Recipe recept) => recepti.Add(recept);

    public void ObrisiRecept(string naziv)
    {
        var recept = recepti.FirstOrDefault(r => r.GetNaziv().Equals(naziv, StringComparison.OrdinalIgnoreCase));
        if (recept == null) throw new RecipeNotFoundException(naziv);
        recepti.Remove(recept);
    }

    public Recipe GetRecept(string naziv)
    {
        var recept = recepti.FirstOrDefault(r => r.GetNaziv().Equals(naziv, StringComparison.OrdinalIgnoreCase));
        if (recept == null) throw new RecipeNotFoundException(naziv);
        return recept;
    }

    public List<Recipe> GetRecepti() => recepti;
}
