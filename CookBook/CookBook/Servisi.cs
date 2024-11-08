using System;
using System.Collections.Generic;
using System.Linq;

public static class Servisi
{
    public static void DodajRecept(RecipeManager recipeManager)
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
        Console.WriteLine("Recept je uspješno dodan.");
    }


    public static void AzurirajRecept(RecipeManager recipeManager)
    {
        Console.Write("Unesite naziv recepta za ažuriranje: ");
        string naziv = Console.ReadLine() ?? "";

        try
        {
            Recipe receptZaAzuriranje = recipeManager.GetRecept(naziv);
            Console.WriteLine("Ažuriranje recepta:");
            Console.Write("Novi naziv (prazno za bez promjene): ");
            string noviNaziv = Console.ReadLine();
            if (!string.IsNullOrEmpty(noviNaziv)) receptZaAzuriranje.SetNaziv(noviNaziv);

            Console.Write("Novi sastojci (razdvojeni zarezom, prazno za bez promjene): ");
            string sastojciInput = Console.ReadLine();
            if (!string.IsNullOrEmpty(sastojciInput))
                receptZaAzuriranje.SetSastojci(sastojciInput.Split(',').Select(s => s.Trim()).ToList());

            Console.Write("Nova kategorija (prazno za bez promjene): ");
            string kategorija = Console.ReadLine();
            if (!string.IsNullOrEmpty(kategorija)) receptZaAzuriranje.SetKategorija(kategorija);

            Console.Write("Novo vrijeme pripreme (u minutama, -1 za bez promjene): ");
            string vrijeme = Console.ReadLine();
            if (int.TryParse(vrijeme, out int vrijemePripreme))
                receptZaAzuriranje.SetVrijemePripreme(vrijemePripreme);

            Console.Write("Nove upute (prazno za bez promjene): ");
            string upute = Console.ReadLine();
            if (!string.IsNullOrEmpty(upute)) receptZaAzuriranje.SetUpute(upute);

            Console.Write("Nova popularnost (1-5, -1 za bez promjene): ");
            string popularnostInput = Console.ReadLine();
            if (int.TryParse(popularnostInput, out int popularnost) && popularnost >= 1 && popularnost <= 5)
                receptZaAzuriranje.SetPopularnost(popularnost);

            Console.WriteLine("Recept je uspješno ažuriran.");
        }
        catch (RecipeNotFoundException e)
        {
            Console.WriteLine(e.Message);
        }
    }


    public static void ObrisiRecept(RecipeManager recipeManager)
    {
        Console.Write("Unesite naziv recepta za brisanje: ");
        string naziv = Console.ReadLine() ?? "";

        try
        {
            recipeManager.ObrisiRecept(naziv);
            Console.WriteLine("Recept je uspješno obrisan.");
        }
        catch (RecipeNotFoundException e)
        {
            Console.WriteLine(e.Message);
        }
    }


    public static void PrikaziRecepte(RecipeManager recipeManager)
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


    public static void PretraziRecepte(RecipeManager recipeManager)
    {
        Console.Write("Unesite naziv recepta za pretragu: ");
        string naziv = Console.ReadLine() ?? "";

        try
        {
            var recept = recipeManager.GetRecept(naziv);
            Console.WriteLine("Pronađeni recept:");
            Console.WriteLine(recept);
        }
        catch (RecipeNotFoundException e)
        {
            Console.WriteLine(e.Message);
        }
    }


    public static void IzveziRecepte(RecipeManager recipeManager, DataExportImportService dataService)
    {
        Console.Write("Odaberite format (JSON/XML): ");
        string format = Console.ReadLine() ?? "JSON";

        var recepti = recipeManager.GetRecepti().Select(r => r.ToString()).ToList();
        try
        {
            dataService.EksportujRecepte(format, recepti);
        }
        catch (ArgumentException e)
        {
            Console.WriteLine(e.Message);
        }
    }


    public static void UveziRecepte(RecipeManager recipeManager, DataExportImportService dataService)
    {
        Console.Write("Odaberite format (JSON/XML): ");
        string format = Console.ReadLine() ?? "JSON";

        try
        {
            List<string> receptiData = format.ToUpper() == "JSON" ? dataService.ImportFromJson() : dataService.ImportFromXml();

            foreach (var receptString in receptiData)
            {
                var parts = receptString.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 7)
                {
                    var naziv = parts[0].Split(": ")[1];
                    var sastojci = parts[1].Split(": ")[1].Split(',').Select(s => s.Trim()).ToList();
                    var kategorija = parts[2].Split(": ")[1];
                    var vrijemePripreme = int.Parse(parts[3].Split(": ")[1].Replace(" min", ""));
                    var upute = parts[4].Split(": ")[1];
                    var popularnost = int.Parse(parts[5].Split(": ")[1]);

                    var recept = new Recipe(new Random().Next(1, 1000), naziv, sastojci, kategorija, vrijemePripreme, upute, popularnost);
                    recipeManager.DodajRecept(recept);
                }
            }
            Console.WriteLine("Recepti su uspješno uvezeni.");
        }
        catch (Exception e)
        {
            Console.WriteLine("Došlo je do greške prilikom uvoza recepata: " + e.Message);
        }
    }
}

