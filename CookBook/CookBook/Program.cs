using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        var recipeManager = new RecipeManager();
        var authService = new AuthenticationService();
        var dataService = new DataExportImportService();

        Console.WriteLine("Dobrodošli u aplikaciju za upravljanje receptima!");

        Console.Write("Unesite korisničko ime za registraciju: ");
        string username = Console.ReadLine() ?? "";
        Console.Write("Unesite lozinku za registraciju: ");
        string password = Console.ReadLine() ?? "";

        if (authService.RegistrujKorisnika(username, password))
        {
            Console.WriteLine("Registracija uspješna!");
        }
        else
        {
            Console.WriteLine("Korisničko ime je već zauzeto.");
            return;
        }

        Console.WriteLine("Molimo vas da se prijavite.");

        bool authenticated = false;
        while (!authenticated)
        {
            Console.Write("Korisničko ime: ");
            string loginUsername = Console.ReadLine() ?? "";
            Console.Write("Lozinka: ");
            string loginPassword = Console.ReadLine() ?? "";

            if (authService.AutentifikujKorisnika(loginUsername, loginPassword))
            {
                Console.WriteLine("Prijava uspješna!");
                authenticated = true;
            }
            else
            {
                Console.WriteLine("Neispravno korisničko ime ili lozinka. Pokušajte ponovo.");
            }
        }

        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("\nOdaberite opciju:");
            Console.WriteLine("1. Dodaj recept");
            Console.WriteLine("2. Ažuriraj recept");
            Console.WriteLine("3. Obriši recept");
            Console.WriteLine("4. Prikaži recepte");
            Console.WriteLine("5. Pretraži recepte po nazivu");
            Console.WriteLine("6. Izvezi recepte (JSON/XML)");
            Console.WriteLine("7. Uvezi recepte (JSON/XML)");
            Console.WriteLine("0. Izlaz");
            Console.Write("Vaš izbor: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    DodajRecept(recipeManager);
                    break;
                case "2":
                    AzurirajRecept(recipeManager);
                    break;
                case "3":
                    ObrisiRecept(recipeManager);
                    break;
                case "4":
                    PrikaziRecepte(recipeManager);
                    break;
                case "5":
                    PretraziRecepte(recipeManager);
                    break;
                case "6":
                    IzveziRecepte(recipeManager, dataService);
                    break;
                case "7":
                    UveziRecepte(recipeManager, dataService);
                    break;
                case "0":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Nepoznata opcija, pokušajte ponovo.");
                    break;
            }
        }
    }


    static void DodajRecept(RecipeManager recipeManager)
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


    static void AzurirajRecept(RecipeManager recipeManager)
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


    static void ObrisiRecept(RecipeManager recipeManager)
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


    static void PrikaziRecepte(RecipeManager recipeManager)
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


    static void PretraziRecepte(RecipeManager recipeManager)
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


    static void IzveziRecepte(RecipeManager recipeManager, DataExportImportService dataService)
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


    static void UveziRecepte(RecipeManager recipeManager, DataExportImportService dataService)
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
