using System;

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
        string password = authService.ReadPassword();

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
            string loginPassword = authService.ReadPassword();

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
            Console.WriteLine("8. Filtriraj recepte po svojstvima");
            Console.WriteLine("9. Prikaži sve kategorije");
            Console.WriteLine("0. Izlaz");
            Console.Write("Vaš izbor: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    UI.DodajReceptUI(recipeManager);
                    break;
                case "2":
                    UI.AzurirajReceptUI(recipeManager);
                    break;
                case "3":
                    UI.ObrisiReceptUI(recipeManager);
                    break;
                case "4":
                    UI.PrikaziSveRecepte(recipeManager);
                    break;
                case "5":
                    UI.PretraziRecepteUI(recipeManager);
                    break;
                case "6":
                    UI.IzveziRecepteUI(recipeManager, dataService);
                    break;
                case "7":
                    UI.UveziRecepteUI(recipeManager, dataService);
                    break;
                case "8":
                    UI.FiltrirajReceptePoSvojstvima(recipeManager);
                    break;
                case "9":
                    UI.PrikaziSveKategorije(recipeManager);
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
}
