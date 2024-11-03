using System;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.Text.Json;

public class Recipe
{
    int id;
    String naziv;
    List<String> sastojci;
    String kategorija;
    int vrijemePripreme;
    String upute;
    int popularnost;
    public Recipe()
    {
        this.sastojci = new List<String>();
        this.naziv = string.Empty;
        this.kategorija = string.Empty;
        this.vrijemePripreme = 0;
        this.upute = string.Empty;
        this.popularnost = 0;
    }


    public Recipe(int id, string naziv, List<string> sastojci, string kategorija, int vrijemePripreme, string upute, int popularnost)
    {
        this.id = id;
        this.naziv = naziv;
        this.sastojci = sastojci;
        this.kategorija = kategorija;
        this.vrijemePripreme = vrijemePripreme;
        this.upute = upute;
        this.popularnost = popularnost;
    }

    public int getId() { return id; }
    public String getNaziv() { return naziv; }
    public List<String> getSastojci() { return sastojci; }
    public String getKategorija() { return kategorija; }
    public int getVrijemePripreme() { return vrijemePripreme; }
    public String getUpute() { return upute; }  
    public int getPopularnost() { return popularnost; }
    public void setId(int id) { this.id = id; }
    public void setNaziv(String naziv) {  this.naziv = naziv;}
    public void setSastojci(List<String> sastojci) { this.sastojci = sastojci; }
    public void setVrijemePripreme(int vrijemePripreme) { this.vrijemePripreme = vrijemePripreme; }
    public void setUpute(String upute) { this.upute = upute;}
    public void setPopularnost(int popularnost) { this.popularnost = popularnost;}
    public void setKategorija(String kategorija) { this.kategorija = kategorija; }
    public override string ToString()
    {
        string sastojciString = string.Join(", ", sastojci);
        return "Naziv recepta: "+naziv+"\nSastojci: "+sastojciString+"\nKategorija: "+kategorija+"\nVrijeme pripreme: "+vrijemePripreme+"\nUpute za pripremu: "+upute+"\nPopularnost= "+popularnost;
    }
}

public class RecipeNotFoundException : Exception
{
    public RecipeNotFoundException(int id)
        : base($"Recept sa ID-jem {id} nije pronađen.")
    {
    }
}

public class RecipeManager
{
    List<Recipe> recepti = new List<Recipe>();

    public void dodajRecept(Recipe recept)
    {
        recepti.Add(recept);
    }
    public void obrisiRecept(int id)
    {
        recepti.RemoveAll(r => r.getId() == id);
    }
    public void azurirajRecept(int id, Recipe noviPodaci)
    {
        Recipe receptZaAzuriranje = recepti.Find(r => r.getId() == id);
        if (receptZaAzuriranje != null)
        {
            receptZaAzuriranje.setNaziv(noviPodaci.getNaziv());
            receptZaAzuriranje.setSastojci(noviPodaci.getSastojci());
            receptZaAzuriranje.setVrijemePripreme(noviPodaci.getVrijemePripreme());
            receptZaAzuriranje.setUpute(noviPodaci.getUpute());
            receptZaAzuriranje.setPopularnost(noviPodaci.getPopularnost());
            receptZaAzuriranje.setKategorija(noviPodaci.getKategorija());
        }
    }
    public Recipe getRecept(int id)
    {
       Recipe recept = recepti.Find(r =>  id == r.getId());
        if (recept == null)
        {
            throw new RecipeNotFoundException(id);
        }
        return recept;
    }
    public List<Recipe> getRecepti() { return recepti; }
}

public class SearchService
{
    RecipeManager recipeManager;

    public SearchService(RecipeManager manager)
    {
        recipeManager = manager;
    }

    public List<Recipe> pretraziPoNazivu(string naziv)
    {
        List<Recipe> recepti = recipeManager.getRecepti();
        recepti.Sort((r1, r2) => string.Compare(r1.getNaziv(), r2.getNaziv(), StringComparison.OrdinalIgnoreCase));
        return BinarnaPretragaPoNazivu(recepti, naziv);
    }

    private List<Recipe> BinarnaPretragaPoNazivu(List<Recipe> recepti, string naziv)
    {
        int left = 0;
        int right = recepti.Count - 1;
        List<Recipe> results = new List<Recipe>();

        while (left <= right)
        {
            int mid = left + (right - left) / 2;
            int comparison = string.Compare(recepti[mid].getNaziv(), naziv, StringComparison.OrdinalIgnoreCase);

            if (comparison == 0)
            {
                results.Add(recepti[mid]);
                int temp = mid - 1;
                while (temp >= left && string.Compare(recepti[temp].getNaziv(), naziv, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    results.Add(recepti[temp]);
                    temp--;
                }
                temp = mid + 1;
                while (temp <= right && string.Compare(recepti[temp].getNaziv(), naziv, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    results.Add(recepti[temp]);
                    temp++;
                }
                break;
            }
            else if (comparison < 0)
            {
                left = mid + 1;
            }
            else
            {
                right = mid - 1;
            }
        }

        return results;
    }

    public List<Recipe> pretraziPoSastojcima(List<string> sastojci)
    {
        return recipeManager.getRecepti()
            .Where(r => sastojci.All(sastojak =>
                r.getSastojci().Any(s => s.IndexOf(sastojak, StringComparison.OrdinalIgnoreCase) >= 0)))
            .ToList();
    }

    public List<Recipe> filtrirajPoKategoriji(string kategorija)
    {
        return recipeManager.getRecepti()
            .Where(r => r.getKategorija().Equals(kategorija, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public List<Recipe> filtrirajPoVremenuPripreme(int maxVrijeme)
    {
        return recipeManager.getRecepti()
            .Where(r => r.getVrijemePripreme() <= maxVrijeme)
            .ToList();
    }
}

public class SortService
{
    public List<Recipe> sortirajPoNazivu(List<Recipe> recepti)
    {
        return QuickSort(recepti, 0, recepti.Count - 1);
    }

    private List<Recipe> QuickSort(List<Recipe> recepti, int low, int high)
    {
        if (low < high)
        {
            int pi = Partition(recepti, low, high);
            QuickSort(recepti, low, pi - 1);
            QuickSort(recepti, pi + 1, high);
        }
        return recepti;
    }

    private int Partition(List<Recipe> recepti, int low, int high)
    {
        Recipe pivot = recepti[high];
        int i = low - 1;

        for (int j = low; j < high; j++)
        {
            if (string.Compare(recepti[j].getNaziv(), pivot.getNaziv(), StringComparison.OrdinalIgnoreCase) < 0)
            {
                i++;
                Swap(recepti, i, j);
            }
        }
        Swap(recepti, i + 1, high);
        return i + 1;
    }

    private void Swap(List<Recipe> recepti, int i, int j)
    {
        Recipe temp = recepti[i];
        recepti[i] = recepti[j];
        recepti[j] = temp;
    }

    public List<Recipe> sortirajPoPopularnosti(List<Recipe> recepti)
    {
        return recepti.OrderByDescending(r => r.getPopularnost()).ToList();
    }
}

public class AuthenticationService
{
    Dictionary<String, String> korisnici;

    public AuthenticationService()
    {
        korisnici = new Dictionary<String, String>();
    }

    public bool RegistrujKorisnika(String korisnickoIme, String lozinka)
    {
        if (korisnici.ContainsKey(korisnickoIme))
        {
            return false;
        }

        String hashedLozinka = HashLozinka(lozinka);
        korisnici[korisnickoIme] = hashedLozinka;
        return true;
    }

    public bool AutentifikujKorisnika(String korisnickoIme, String lozinka)
    {
        if (korisnici.TryGetValue(korisnickoIme, out String hashedLozinka))
        {
            String hashedInputLozinka = HashLozinka(lozinka);
            return hashedInputLozinka == hashedLozinka;
        }

        return false; 
    }

    private String HashLozinka(String lozinka)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(lozinka));

            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }
    }
}

public class DataExportImportService
{
    public void EksportujRecepte(string format, List<String> recepti)
    {
        if (format.Equals("JSON", StringComparison.OrdinalIgnoreCase))
        {
            ExportToJson(recepti);
        }
        else if (format.Equals("XML", StringComparison.OrdinalIgnoreCase))
        {
            ExportToXml(recepti);
        }
        else
        {
            throw new ArgumentException("Nepodržan format: " + format);
        }
    }

    public List<String> ImportujRecepte(string format)
    {
        if (format.Equals("JSON", StringComparison.OrdinalIgnoreCase))
        {
            return ImportFromJson();
        }
        else if (format.Equals("XML", StringComparison.OrdinalIgnoreCase))
        {
            return ImportFromXml();
        }
        else
        {
            throw new ArgumentException("Nepodržan format: " + format);
        }
    }

    private void ExportToJson(List<String> recepti)
    {
        if (recepti == null || !recepti.Any())
        {
            Console.WriteLine("Lista recepata je prazna, ništa neće biti eksportovano.");
            return;
        }

        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        string json = JsonSerializer.Serialize(recepti, options);
        File.WriteAllText("recepti.json", json);
    }

    private void ExportToXml(List<String> recepti)
    {
        if (recepti == null || !recepti.Any())
        {
            Console.WriteLine("Lista recepata je prazna, ništa neće biti eksportovano.");
            return;
        }

        XmlSerializer serializer = new XmlSerializer(typeof(List<String>));
        using (FileStream stream = new FileStream("recepti.xml", FileMode.Create))
        {
            serializer.Serialize(stream, recepti);
        }
    }


    private List<string> ImportFromJson()
    {
        string json = File.ReadAllText("recepti.json");
        var recepti = JsonSerializer.Deserialize<List<List<string>>>(json);
        return recepti.Select(r => string.Join(";", r)).ToList(); 
    }

    private List<string> ImportFromXml()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<List<string>>));
        using (StreamReader reader = new StreamReader("recepti.xml"))
        {
            var recepti = (List<List<string>>)serializer.Deserialize(reader);
            return recepti.Select(r => string.Join(";", r)).ToList();
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        RecipeManager recipeManager = new RecipeManager();
        AuthenticationService authService = new AuthenticationService();
        DataExportImportService dataService = new DataExportImportService();

        Console.WriteLine("Dobrodošli u aplikaciju za upravljanje receptima!");

        Console.Write("Unesite korisničko ime: ");
        string username = Console.ReadLine();
        Console.Write("Unesite lozinku: ");
        string password = Console.ReadLine();

        authService.RegistrujKorisnika(username, password);

        Console.WriteLine("Registracija uspješna!");

        Console.WriteLine("Dobrodošli u aplikaciju za upravljanje receptima!");
        Console.WriteLine("Molimo vas da se prijavite.");

        if (!AuthenticateUser(authService))
        {
            Console.WriteLine("Neuspješna prijava. Zatvaranje aplikacije.");
            return;
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
                    AžurirajRecept(recipeManager);
                    break;
                case "3":
                    ObrišiRecept(recipeManager);
                    break;
                case "4":
                    PrikažiRecepte(recipeManager);
                    break;
                case "5":
                    PretražiRecepte(recipeManager);
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

    static bool AuthenticateUser(AuthenticationService authService)
    {
        Console.Write("Korisničko ime: ");
        string username = Console.ReadLine();
        Console.Write("Lozinka: ");
        string password = Console.ReadLine();

        return authService.AutentifikujKorisnika(username, password);
    }

    static void DodajRecept(RecipeManager recipeManager)
    {
        Console.Write("Naziv recepta: ");
        string naziv = Console.ReadLine();

        Console.Write("Sastojci (razdvojeni zarezom): ");
        List<String> sastojci = Console.ReadLine().Split(',').Select(s => s.Trim()).ToList();

        Console.Write("Kategorija: ");
        string kategorija = Console.ReadLine();

        Console.Write("Vrijeme pripreme (u minutama): ");
        int vrijemePripreme = int.Parse(Console.ReadLine());

        Console.Write("Upute za pripremu: ");
        string upute = Console.ReadLine();

        Console.Write("Popularnost (1-5): ");
        int popularnost = int.Parse(Console.ReadLine());

        Recipe recept = new Recipe(GenerateId(), naziv, sastojci, kategorija, vrijemePripreme, upute, popularnost);
        recipeManager.dodajRecept(recept);
        Console.WriteLine("Recept je uspješno dodan.");
    }

    static void AžurirajRecept(RecipeManager recipeManager)
    {
        Console.Write("Unesite ID recepta za ažuriranje: ");
        int id = int.Parse(Console.ReadLine());
        try
        {
            Recipe recept = recipeManager.getRecept(id);
            Console.WriteLine("Ažurirate recept: ");
            Console.WriteLine(recept.ToString());

            Console.Write("Novi naziv recepta (prazno za bez promjene): ");
            string naziv = Console.ReadLine();
            if (!string.IsNullOrEmpty(naziv)) recept.setNaziv(naziv);

            Console.Write("Novi sastojci (razdvojeni zarezom, prazno za bez promjene): ");
            string sastojciInput = Console.ReadLine();
            if (!string.IsNullOrEmpty(sastojciInput))
                recept.setSastojci(sastojciInput.Split(',').Select(s => s.Trim()).ToList());

            Console.Write("Nova kategorija (prazno za bez promjene): ");
            string kategorija = Console.ReadLine();
            if (!string.IsNullOrEmpty(kategorija)) recept.setKategorija(kategorija);

            Console.Write("Novo vrijeme pripreme (u minutama, -1 za bez promjene): ");
            int vrijemePripreme = int.Parse(Console.ReadLine());
            if (vrijemePripreme >= 0) recept.setVrijemePripreme(vrijemePripreme);

            Console.Write("Nove upute za pripremu (prazno za bez promjene): ");
            string upute = Console.ReadLine();
            if (!string.IsNullOrEmpty(upute)) recept.setUpute(upute);

            Console.Write("Nova popularnost (1-5, -1 za bez promjene): ");
            int popularnost = int.Parse(Console.ReadLine());
            if (popularnost >= 1 && popularnost <= 5) recept.setPopularnost(popularnost);

            recipeManager.azurirajRecept(id, recept);
            Console.WriteLine("Recept je uspješno ažuriran.");
        }
        catch (RecipeNotFoundException e)
        {
            Console.WriteLine(e.Message);
        }
    }

    static void ObrišiRecept(RecipeManager recipeManager)
    {
        Console.Write("Unesite ID recepta za brisanje: ");
        int id = int.Parse(Console.ReadLine());
        recipeManager.obrisiRecept(id);
        Console.WriteLine("Recept je uspješno obrisan.");
    }

    static void PrikažiRecepte(RecipeManager recipeManager)
    {
        var recepti = recipeManager.getRecepti();
        if (recepti.Count == 0)
        {
            Console.WriteLine("Nema dostupnih recepata.");
            return;
        }

        Console.WriteLine("Dostupni recepti:");
        foreach (var recept in recepti)
        {
            Console.WriteLine(recept.ToString());
            Console.WriteLine();
        }
    }

    static void PretražiRecepte(RecipeManager recipeManager)
    {
        Console.Write("Unesite naziv recepta za pretragu: ");
        string naziv = Console.ReadLine();
        var rezultati = new SearchService(recipeManager).pretraziPoNazivu(naziv);
        if (rezultati.Count == 0)
        {
            Console.WriteLine("Nema recepata koji odgovaraju pretrazi.");
            return;
        }

        Console.WriteLine("Rezultati pretrage:");
        foreach (var recept in rezultati)
        {
            Console.WriteLine(recept.ToString());
            Console.WriteLine();
        }
    }

    static void IzveziRecepte(RecipeManager recipeManager, DataExportImportService dataService)
    {
        Console.Write("Odaberite format (JSON/XML): ");
        string format = Console.ReadLine();
        List<Recipe> recepti = recipeManager.getRecepti(); 
        List<String> receptiStringovi = recepti.Select(r => r.ToString()).ToList();
        dataService.EksportujRecepte(format, receptiStringovi);
        Console.WriteLine($"Recepti su uspješno eksportovani u format {format}.");
    }

    public static Recipe CreateRecipeFromStrings(List<string> recipeAttributes)
    {
        if (recipeAttributes == null || recipeAttributes.Count < 7)
        {
            throw new ArgumentException("Nedovoljno podataka.");
        }

        int id = int.Parse(recipeAttributes[0]);
        string naziv = recipeAttributes[1];
        List<string> sastojci = recipeAttributes[2].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
        string kategorija = recipeAttributes[3];
        int vrijemePripreme = int.Parse(recipeAttributes[4]);
        string upute = recipeAttributes[5];
        int popularnost = int.Parse(recipeAttributes[6]);

        return new Recipe(id, naziv, sastojci, kategorija, vrijemePripreme, upute, popularnost);
    }

    static void UveziRecepte(RecipeManager recipeManager, DataExportImportService dataService)
    {
        Console.Write("Odaberite format (JSON/XML): ");
        string format = Console.ReadLine();

        List<string> receptiData = dataService.ImportujRecepte(format);

        foreach (var recept in receptiData)
        {
            var recipeAttributes = recept.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            Recipe recipe = CreateRecipeFromStrings(recipeAttributes);
            recipeManager.dodajRecept(recipe);
        }

        Console.WriteLine($"Recepti su uspješno uvezeni iz formata {format}.");
    }

    static int GenerateId()
    {
        return new Random().Next(1, 1000); 
    }
}