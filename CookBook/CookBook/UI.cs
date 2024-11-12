using System;
using System.Collections.Generic;
using System.Linq;

public static class UI
{
    public static void DodajReceptUI(RecipeManager recipeManager)
    {
        Console.Write("Naziv recepta: ");
        string naziv = Console.ReadLine() ?? "";

        Console.Write("Sastojci (razdvojeni zarezom): ");
        List<string> sastojci = Console.ReadLine()?.Split(',').Select(s => s.Trim()).ToList() ?? new List<string>();

        Console.Write("Kategorija: ");
        string kategorija = Console.ReadLine() ?? "";

        Console.Write("Vrijeme pripreme (u minutama): ");
        int.TryParse(Console.ReadLine(), out int vrijemePripreme);

        Console.Write("Upute za pripremu: ");
        string upute = Console.ReadLine() ?? "";

        Console.Write("Popularnost (1-5): ");
        int.TryParse(Console.ReadLine(), out int popularnost);

        Recipe recept = new Recipe(new Random().Next(1, 1000), naziv, sastojci, kategorija, vrijemePripreme, upute, popularnost);
        recipeManager.DodajRecept(recept);
    }

    public static void AzurirajReceptUI(RecipeManager recipeManager)
    {
        Console.Write("Unesite naziv recepta za ažuriranje: ");
        string naziv = Console.ReadLine() ?? "";

        Console.Write("Novi naziv (prazno za bez promjene): ");
        string noviNaziv = Console.ReadLine();

        Console.Write("Novi sastojci (razdvojeni zarezom, prazno za bez promjene): ");
        string sastojciInput = Console.ReadLine();
        List<string>? sastojci = string.IsNullOrEmpty(sastojciInput) ? null : sastojciInput.Split(',').Select(s => s.Trim()).ToList();

        Console.Write("Nova kategorija (prazno za bez promjene): ");
        string kategorija = Console.ReadLine();

        Console.Write("Novo vrijeme pripreme (u minutama, -1 za bez promjene): ");
        int? vrijemePripreme = int.TryParse(Console.ReadLine(), out int tempVrijeme) ? tempVrijeme : (int?)null;

        Console.Write("Nove upute (prazno za bez promjene): ");
        string upute = Console.ReadLine();

        Console.Write("Nova popularnost (1-5, -1 za bez promjene): ");
        int? popularnost = int.TryParse(Console.ReadLine(), out int tempPopularnost) ? tempPopularnost : (int?)null;

        recipeManager.AzurirajRecept(naziv, noviNaziv, sastojci, kategorija, vrijemePripreme, upute, popularnost);
    }

    public static void ObrisiReceptUI(RecipeManager recipeManager)
    {
        Console.Write("Unesite naziv recepta za brisanje: ");
        string naziv = Console.ReadLine() ?? "";
        recipeManager.ObrisiRecept(naziv);
    }

    public static void PrikaziSveRecepte(RecipeManager recipeManager)
    {
        var recepti = recipeManager.GetRecepti();
        if (recepti.Count == 0)
        {
            Console.WriteLine("Nema dostupnih recepata.");
            return;
        }

        Console.WriteLine("Dostupni recepti:");
        foreach (var recept in recepti)
        {
            Console.WriteLine(recept);
            Console.WriteLine();
        }
    }

    public static void PretraziRecepteUI(RecipeManager recipeManager)
    {
        Console.Write("Unesite naziv recepta za pretragu: ");
        string naziv = Console.ReadLine() ?? "";
        var recept = recipeManager.PretraziReceptPoNazivu(naziv);
        if (recept != null)
        {
            Console.WriteLine("Pronadjeni recept:");
            Console.WriteLine(recept);
        }
        else
        {
            Console.WriteLine("Recept nije pronadjen.");
        }
    }

    public static void IzveziRecepteUI(RecipeManager recipeManager, DataExportImportService dataService)
    {
        Console.Write("Odaberite format (JSON/XML): ");
        string format = Console.ReadLine() ?? "JSON";
        var recepti = recipeManager.GetRecepti(); 
        try
        {
            dataService.EksportujRecepte(format, recepti); 
        }
        catch (ArgumentException e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public static void UveziRecepteUI(RecipeManager recipeManager, DataExportImportService dataService)
    {
        Console.Write("Odaberite format (JSON/XML): ");
        string format = Console.ReadLine() ?? "JSON";
        try
        {
            List<Recipe> receptiData = format.ToUpper() == "JSON" ? dataService.ImportFromJson() : dataService.ImportFromXml();
            foreach (var recept in receptiData)
            {
                recipeManager.DodajRecept(recept); 
                Console.WriteLine("Uvezeni recept: " + recept.GetNaziv());
            }
            Console.WriteLine("Recepti su uspješno uvezeni.");
        }
        catch (Exception e)
        {
            Console.WriteLine("Došlo je do greške prilikom uvoza recepata: " + e.Message);
        }
    }

    public static void FiltrirajReceptePoSvojstvima(RecipeManager recipeManager)
    {
        Console.WriteLine("\nUnesite kriterije za filtriranje recepata.");

        Console.Write("Maksimalno vrijeme pripreme (u minutama, -1 za preskoci): ");
        int maxVrijemePripreme = int.TryParse(Console.ReadLine(), out int vrijeme) ? vrijeme : -1;

        Console.Write("Sastojak (prazno za preskoci): ");
        string sastojak = Console.ReadLine();

        Console.Write("Minimalna popularnost (1-5, -1 za preskoci): ");
        int minPopularnost = int.TryParse(Console.ReadLine(), out int popularnost) && popularnost >= 1 && popularnost <= 5 ? popularnost : -1;

        var filteredRecipes = recipeManager.FiltrirajRecepte(maxVrijemePripreme, sastojak, minPopularnost);

        if (filteredRecipes.Count > 0)
        {
            Console.WriteLine("\nFiltrirani recepti:");
            foreach (var recept in filteredRecipes)
            {
                Console.WriteLine(recept);
                Console.WriteLine();
            }
        }
        else
        {
            Console.WriteLine("Nema recepata koji zadovoljavaju zadane kriterije.");
        }
    }

    public static void PrikaziSveKategorije(RecipeManager recipeManager)
    {
        var sortedCategories = recipeManager.GetSortedCategories();

        Console.WriteLine("\nDostupne kategorije (sortirane):");
        foreach (var category in sortedCategories)
        {
            Console.WriteLine(category);
        }
    }
}
