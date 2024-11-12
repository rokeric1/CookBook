public class RecipeManager
{
    private List<Recipe> recepti = new List<Recipe>();

    public void DodajRecept(Recipe recept)
    {
        recepti.Add(recept);
        Console.WriteLine("Recept je uspjesno dodan.");
    }

    public void AzurirajRecept(string naziv, string? noviNaziv = null, List<string>? sastojci = null,
                               string? kategorija = null, int? vrijemePripreme = null,
                               string? upute = null, int? popularnost = null)
    {
        var recept = GetRecept(naziv);
        if (recept != null)
        {
            if (!string.IsNullOrEmpty(noviNaziv)) recept.SetNaziv(noviNaziv);
            if (sastojci != null && sastojci.Any()) recept.SetSastojci(sastojci);
            if (!string.IsNullOrEmpty(kategorija)) recept.SetKategorija(kategorija);
            if (vrijemePripreme.HasValue) recept.SetVrijemePripreme(vrijemePripreme.Value);
            if (!string.IsNullOrEmpty(upute)) recept.SetUpute(upute);
            if (popularnost.HasValue && popularnost >= 1 && popularnost <= 5)
                recept.SetPopularnost(popularnost.Value);

            Console.WriteLine("Recept je uspjesno ažuriran.");
        }
        else
        {
            throw new RecipeNotFoundException(naziv);
        }
    }

    public void ObrisiRecept(string naziv)
    {
        var recept = recepti.FirstOrDefault(r => r.GetNaziv().Equals(naziv, StringComparison.OrdinalIgnoreCase));
        if (recept == null) throw new RecipeNotFoundException(naziv);
        recepti.Remove(recept);
        Console.WriteLine("Recept je uspjesno obrisan.");
    }

    public Recipe GetRecept(string naziv)
    {
        var recept = recepti.FirstOrDefault(r => r.GetNaziv().Equals(naziv, StringComparison.OrdinalIgnoreCase));
        if (recept == null) throw new RecipeNotFoundException(naziv);
        return recept;
    }

    public List<Recipe> GetRecepti() => recepti;

    public Recipe PretraziReceptPoNazivu(string naziv)
    {
        return recepti.FirstOrDefault(r => r.GetNaziv().Equals(naziv, StringComparison.OrdinalIgnoreCase));
    }

    public List<Recipe> FiltrirajRecepte(int maxVrijemePripreme = -1, string sastojak = "", int minPopularnost = -1)
    {
        return recepti.Where(recept =>
            (maxVrijemePripreme == -1 || recept.vrijemePripreme <= maxVrijemePripreme) &&
            (string.IsNullOrEmpty(sastojak) || recept.sastojci.Contains(sastojak, StringComparer.OrdinalIgnoreCase)) &&
            (minPopularnost == -1 || recept.popularnost >= minPopularnost)
        ).ToList();
    }

    public List<string> GetSortedCategories()
    {
        HashSet<string> uniqueCategories = new HashSet<string>();
        foreach (var recipe in recepti)
        {
            if (!string.IsNullOrEmpty(recipe.kategorija))
            {
                uniqueCategories.Add(recipe.kategorija);
            }
        }

        List<string> categories = new List<string>(uniqueCategories);
        QuickSort(categories, 0, categories.Count - 1);
        return categories;
    }

    private void QuickSort(List<string> list, int left, int right)
    {
        if (left < right)
        {
            int pivotIndex = Partition(list, left, right);
            QuickSort(list, left, pivotIndex - 1);
            QuickSort(list, pivotIndex + 1, right);
        }
    }

    private int Partition(List<string> list, int left, int right)
    {
        string pivot = list[right];
        int i = left - 1;

        for (int j = left; j < right; j++)
        {
            if (string.Compare(list[j], pivot, StringComparison.OrdinalIgnoreCase) < 0)
            {
                i++;
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        (list[i + 1], list[right]) = (list[right], list[i + 1]);
        return i + 1;
    }
}
