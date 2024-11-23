using System;
using System.Diagnostics.CodeAnalysis;
using Models;
using Services;

[ExcludeFromCodeCoverage]
class Program
{
    static void Main()
    {
        IIngredientService ingredientService = new IngredientService();
        IRecipeService recipeService = new RecipeService(ingredientService);
        IOcjenaService ocjenaService = new OcjenaService();
        var authService = new AuthenticationService();
        var dataService = new DataExportImportService(ingredientService);
        var shoppingListService = new ShoppingListService(ingredientService);
        var tagService = new TagService(); // Dodavanje TagService

        Console.WriteLine("Dobrodošli u aplikaciju za upravljanje receptima!");
        Console.WriteLine("Registracija korisnika:");
        Console.Write("Unesite korisničko ime za registraciju: ");
        string username = Console.ReadLine() ?? "";
        Console.Write("Unesite lozinku za registraciju: ");
        string password = authService.CitajLozinku();

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
        while (true)
        {
            Console.Write("Korisničko ime: ");
            string loginUsername = Console.ReadLine() ?? "";
            Console.Write("Lozinka: ");
            string loginPassword = authService.CitajLozinku();

            if (authService.AutentifikujKorisnika(loginUsername, loginPassword))
            {
                Console.WriteLine("Prijava uspješna! Dobrodošli, " + loginUsername + ".");
                break;
            }
            else
            {
                Console.WriteLine("Neispravno korisničko ime ili lozinka. Pokušajte ponovo.");
            }
        }

        try
        {
            var sastojci = dataService.ImportujSastojke("JSON");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Greška prilikom uvoza sastojaka: {e.Message}");
            return;
        }

        while (true)
        {
            Console.WriteLine("\nOdaberite opciju:");
            Console.WriteLine("1. Dodaj recept");
            Console.WriteLine("2. Prikaži sve recepte");
            Console.WriteLine("3. Prikaži sve kategorije");
            Console.WriteLine("4. Ažuriraj recept");
            Console.WriteLine("5. Obriši recept");
            Console.WriteLine("6. Pretraži recept po nazivu");
            Console.WriteLine("7. Pretraži recepte po atributima");
            Console.WriteLine("8. Izvezi recepte (JSON/XML)");
            Console.WriteLine("9. Uvezi recepte (JSON/XML)");
            Console.WriteLine("10. Kreiraj listu kupovine");
            Console.WriteLine("11. Ocijeni recept");
            Console.WriteLine("12. Prikaži prosjecnu ocjenu recepta");
            Console.WriteLine("13. Tagovi (podmeni)");
            Console.WriteLine("0. Izlaz");
            Console.Write("Vaš izbor: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    UI.DodajReceptUI(recipeService, ingredientService);
                    break;
                case "2":
                    UI.PrikaziSveRecepte(recipeService, ocjenaService);
                    break;
                case "3":
                    UI.PrikaziSveKategorijeUI(recipeService);
                    break;
                case "4":
                    UI.AzurirajReceptUI(recipeService, ingredientService);
                    break;
                case "5":
                    UI.ObrisiReceptUI(recipeService);
                    break;
                case "6":
                    UI.PretraziPoNazivuUI(recipeService);
                    break;
                case "7":
                    UI.PretraziPoAtributimaUI(recipeService);
                    break;
                case "8":
                    UI.IzveziRecepteUI(recipeService, dataService);
                    break;
                case "9":
                    UI.UveziRecepteUI(recipeService, dataService);
                    break;
                case "10":
                    UI.KreirajListuKupovine(recipeService, shoppingListService);
                    break;
                case "11":
                    UI.OcijeniReceptUI(recipeService, ocjenaService);
                    break;
                case "12":
                    UI.PrikaziProsjecnuOcjenuUI(recipeService, ocjenaService);
                    break;
                case "13":
                    TagMenu(tagService, recipeService);
                    break;
                case "0":
                    Console.WriteLine("Doviđenja!");
                    return;
                default:
                    Console.WriteLine("Nepoznata opcija, pokušajte ponovo.");
                    break;
            }
        }
    }

    static void TagMenu(TagService tagService, IRecipeService recipeService)
    {
        while (true)
        {
            Console.WriteLine("\nTagovi - Podmeni:");
            Console.WriteLine("1. Kreiraj tag");
            Console.WriteLine("2. Dodaj recept u tag");
            Console.WriteLine("3. Prikaži recepte za tag");
            Console.WriteLine("4. Prikaži sve tagove");
            Console.WriteLine("5. Obriši tag");
            Console.WriteLine("0. Povratak u glavni meni");
            Console.Write("Vaš izbor: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    UI.KreirajTagUI(tagService);
                    break;
                case "2":
                    UI.DodajReceptUTagUI(tagService, recipeService);
                    break;
                case "3":
                    UI.PrikaziRecepteZaTagUI(tagService, recipeService);
                    break;
                case "4":
                    UI.PrikaziSveTagoveUI(tagService);
                    break;
                case "5":
                    UI.ObrisiTagUI(tagService);
                    break;
                case "0":
                    return; // Izlazak iz podmenija
                default:
                    Console.WriteLine("Nepoznata opcija, pokušajte ponovo.");
                    break;
            }
        }
    }
}
