using Models;
using Services;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

[ExcludeFromCodeCoverage]
public static class UI
{
    public static void DodajReceptUI(IRecipeService recipeService, IIngredientService ingredientService)
    {
        Console.Write("Naziv recepta: ");
        string Naziv = Console.ReadLine() ?? "";

        Console.WriteLine("Unos sastojaka:");
        var sastojci = new Dictionary<int, double>();
        while (true)
        {
            Console.Write("Naziv sastojka (ili 'kraj' za završetak): ");
            string NazivSastojka = Console.ReadLine() ?? "";
            if (NazivSastojka.Trim().ToLower() == "kraj")
                break;

            var existingIngredient = ingredientService.GetIngredientByName(NazivSastojka);
            if (existingIngredient != null)
            {
                Console.Write("Količina: ");
                if (!double.TryParse(Console.ReadLine(), out double kolicina))
                {
                    Console.WriteLine("InvalId quantity. Please enter a number.");
                    continue;
                }
                sastojci[existingIngredient.Id] = kolicina;
            }
            else
            {
                Console.Write("Nutrijenti: ");
                string Nutrijenti = Console.ReadLine() ?? "";

                Console.Write("Eko utjecaj: ");
                if (!double.TryParse(Console.ReadLine(), out double EkoUtjecaj))
                {
                    Console.WriteLine("InvalId eco impact. Please enter a number.");
                    continue;
                }

                Console.Write("Cijena: ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal Cijena))
                {
                    Console.WriteLine("InvalId price. Please enter a decimal number.");
                    continue;
                }

                Console.Write("Dostupnost (true/false): ");
                if (!bool.TryParse(Console.ReadLine(), out bool Dostupan))
                {
                    Console.WriteLine("InvalId availability. Please enter true or false.");
                    continue;
                }

                Console.Write("Količina: ");
                if (!double.TryParse(Console.ReadLine(), out double kolicinaNovo))
                {
                    Console.WriteLine("InvalId quantity. Please enter a number.");
                    continue;
                }

                var sastojak = new Ingredient(ingredientService.GetNextId(), NazivSastojka, Nutrijenti, EkoUtjecaj, Cijena, Dostupan);
                ingredientService.DodajIngredient(sastojak);

                sastojci[sastojak.Id] = kolicinaNovo;
            }
        }

        Console.Write("Kategorija: ");
        string kategorija = Console.ReadLine() ?? "";

        Console.Write("Vrijeme pripreme (u minutama): ");
        if (!int.TryParse(Console.ReadLine(), out int vrijemePripreme))
        {
            Console.WriteLine("InvalId input for preparation time. Setting to 0.");
            vrijemePripreme = 0;
        }

        Console.Write("Upute za pripremu: ");
        string upute = Console.ReadLine() ?? "";

        int popularnost;
        while (true)
        {
            Console.Write("Popularnost (1-5): ");
            if (int.TryParse(Console.ReadLine(), out popularnost) && popularnost >= 1 && popularnost <= 5)
                break;
            Console.WriteLine("InvalId input. Please enter a number between 1 and 5.");
        }

        int receptId = recipeService.GetNextId();

        Recipe recept = new Recipe(receptId, Naziv, sastojci, kategorija, vrijemePripreme, upute, popularnost);
        recipeService.DodajRecept(recept);
        Console.WriteLine("Recept je uspješno dodan.");
    }

    public static void PrikaziSveKategorijeUI(IRecipeService recipeService)
    {
        var recepti = recipeService.GetSviRecepti();
        if (!recepti.Any())
        {
            Console.WriteLine("Nema dostupnih kategorija.");
            return;
        }

        var kategorije = recepti.Select(r => r.Kategorija).Distinct().OrderBy(k => k).ToList();
        Console.WriteLine("\nDostupne kategorije:");
        foreach (var kategorija in kategorije)
        {
            Console.WriteLine($"- {kategorija}");
        }
    }

    public static void AzurirajReceptUI(IRecipeService recipeService, IIngredientService ingredientService)
    {
        Console.Write("Unesite naziv recepta za ažuriranje: ");
        string naziv = Console.ReadLine()?.Trim();

        if (string.IsNullOrEmpty(naziv))
        {
            Console.WriteLine("Naziv recepta ne može biti prazan.");
            return;
        }

        var recepti = recipeService.PretraziPoNazivu(naziv);

        if (!recepti.Any())
        {
            Console.WriteLine("Recept nije pronađen.");
            return;
        }

        Recipe recept = null;

        if (recepti.Count > 1)
        {
            Console.WriteLine("Pronađeno je više recepata sa tim nazivom. Molimo odaberite ID recepta za ažuriranje:");
            foreach (var r in recepti)
            {
                Console.WriteLine($"ID: {r.Id}, Naziv: {r.Naziv}, Kategorija: {r.Kategorija}");
            }

            Console.Write("Unesite ID recepta: ");
            if (!int.TryParse(Console.ReadLine(), out int selectedId))
            {
                Console.WriteLine("Neispravan ID.");
                return;
            }

            recept = recepti.FirstOrDefault(r => r.Id == selectedId);
            if (recept == null)
            {
                Console.WriteLine("Recept sa unesenim ID-om nije pronađen.");
                return;
            }
        }
        else
        {
            recept = recepti.First();
        }

        Console.WriteLine($"Ažuriranje recepta: {recept.Naziv}");

        Console.Write("Novi Naziv (prazno za bez promjene): ");
        string noviNaziv = Console.ReadLine()?.Trim();
        if (!string.IsNullOrEmpty(noviNaziv))
        {
            if (recipeService.PretraziPoNazivu(noviNaziv).Any(r => r.Id != recept.Id))
            {
                Console.WriteLine($"Recept sa nazivom \"{noviNaziv}\" već postoji. Naziv nije promijenjen.");
            }
            else
            {
                recept.Naziv = noviNaziv;
            }
        }

        Console.WriteLine("Ažuriranje sastojaka:");
        var noviSastojci = new Dictionary<int, double>();
        while (true)
        {
            Console.Write("Naziv sastojka (ili 'kraj' za završetak): ");
            string NazivSastojka = Console.ReadLine()?.Trim() ?? "";
            if (NazivSastojka.ToLower() == "kraj")
                break;

            Console.Write("Količina: ");
            if (!double.TryParse(Console.ReadLine(), out double kolicina))
            {
                Console.WriteLine("Neispravna količina. Molimo unesite broj.");
                continue;
            }

            var ingredient = ingredientService.GetIngredientByName(NazivSastojka);
            if (ingredient != null)
            {
                noviSastojci[ingredient.Id] = kolicina;
            }
            else
            {
                Console.WriteLine("Sastojak nije pronađen. Morate dodati novi sastojak.");

                Console.Write("Nutrijenti: ");
                string Nutrijenti = Console.ReadLine()?.Trim() ?? "";

                Console.Write("Eko utjecaj: ");
                if (!double.TryParse(Console.ReadLine(), out double EkoUtjecaj))
                {
                    Console.WriteLine("Neispravan eko utjecaj. Molimo unesite broj.");
                    continue;
                }

                Console.Write("Cijena: ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal Cijena))
                {
                    Console.WriteLine("Neispravna cijena. Molimo unesite decimalni broj.");
                    continue;
                }

                Console.Write("Dostupnost (true/false): ");
                if (!bool.TryParse(Console.ReadLine(), out bool Dostupan))
                {
                    Console.WriteLine("Neispravna dostupnost. Molimo unesite true ili false.");
                    continue;
                }

                var noviSastojak = new Ingredient(ingredientService.GetNextId(), NazivSastojka, Nutrijenti, EkoUtjecaj, Cijena, Dostupan);
                try
                {
                    ingredientService.DodajIngredient(noviSastojak);
                    Console.WriteLine($"Sastojak \"{noviSastojak.Naziv}\" je dodan sa ID {noviSastojak.Id}.");
                    noviSastojci[noviSastojak.Id] = kolicina;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Greška prilikom dodavanja sastojka: {ex.Message}");
                    continue;
                }
            }
        }
        recept.Sastojci = noviSastojci;

        Console.Write("Nova kategorija (prazno za bez promjene): ");
        string novaKategorija = Console.ReadLine()?.Trim();
        if (!string.IsNullOrEmpty(novaKategorija))
        {
            recept.Kategorija = novaKategorija;
        }

        Console.Write("Novo vrijeme pripreme (u minutama, -1 za bez promjene): ");
        if (int.TryParse(Console.ReadLine(), out int novoVrijeme) && novoVrijeme > 0)
        {
            recept.VrijemePripreme = novoVrijeme;
        }

        Console.Write("Nove upute (prazno za bez promjene): ");
        string noveUpute = Console.ReadLine()?.Trim();
        if (!string.IsNullOrEmpty(noveUpute))
        {
            recept.Upute = noveUpute;
        }

        Console.Write("Nova popularnost (1-5, -1 za bez promjene): ");
        if (int.TryParse(Console.ReadLine(), out int novaPopularnost) && novaPopularnost >= 1 && novaPopularnost <= 5)
        {
            recept.Popularnost = novaPopularnost;
        }

        try
        {
            recipeService.AzurirajRecept(recept.Id, recept);
            Console.WriteLine("Recept je uspješno ažuriran.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Greška prilikom ažuriranja recepta: {ex.Message}");
        }
    }


    public static void ObrisiReceptUI(IRecipeService recipeService)
    {
        Console.Write("Unesite naziv recepta za brisanje: ");
        string naziv = Console.ReadLine()?.Trim();

        if (string.IsNullOrEmpty(naziv))
        {
            Console.WriteLine("Naziv recepta ne može biti prazan.");
            return;
        }

        var recepti = recipeService.PretraziPoNazivu(naziv);

        if (!recepti.Any())
        {
            Console.WriteLine("Recept nije pronađen.");
            return;
        }

        var recept = recepti.First();

        Console.WriteLine($"Jeste li sigurni da želite obrisati recept: {recept.Naziv}? (da/ne)");
        string potvrda = Console.ReadLine()?.Trim().ToLower();

        if (potvrda == "da" || potvrda == "d")
        {
            try
            {
                recipeService.ObrisiRecept(recept.Id);
                Console.WriteLine("Recept je uspješno obrisan.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška prilikom brisanja recepta: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Brisanje je otkazano.");
        }
    }


    public static void IzveziRecepteUI(IRecipeService recipeService, IDataExportImportService dataService)
    {
        Console.Write("Odaberite format za izvoz (JSON/XML): ");
        string format = Console.ReadLine()?.ToUpper() ?? "JSON";
        try
        {
            var recepti = recipeService.GetSviRecepti();
            dataService.EksportujRecepte(format, recepti);
            Console.WriteLine($"Recepti su uspješno izvezeni u format {format}.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Greška prilikom izvoza: {e.Message}");
        }
    }

    public static void UveziRecepteUI(IRecipeService recipeService, IDataExportImportService dataService)
    {
        Console.Write("Odaberite format za uvoz (JSON/XML): ");
        string format = Console.ReadLine()?.ToUpper() ?? "JSON";
        try
        {
            var recepti = dataService.ImportujRecepte(format);
            foreach (var recept in recepti)
            {
                recipeService.DodajRecept(recept);
            }

            Console.WriteLine("Recepti su uspješno uvezeni.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Greška prilikom uvoza: {e.Message}");
        }
    }

    public static void KreirajListuKupovine(IRecipeService recipeService, IShoppingListService shoppingListService)
    {
        Console.Write("Unesite Naziv recepta za koji želite kreirati listu kupovine: ");
        string Naziv = Console.ReadLine() ?? "";

        var recepti = recipeService.PretraziPoNazivu(Naziv);
        if (!recepti.Any())
        {
            Console.WriteLine("Nije pronađen recept sa unesenim Nazivom.");
            return;
        }

        var recept = recepti.First();
        foreach (var sastojak in recept.Sastojci)
        {
            shoppingListService.DodajNaListu(sastojak.Key, sastojak.Value);
        }

        Console.WriteLine("Lista kupovine je uspješno kreirana za recept:");
        Console.WriteLine(recept);
        Console.WriteLine(shoppingListService.ToString());
    }

    public static void PretraziPoNazivuUI(IRecipeService recipeService)
    {
        Console.Write("Unesite Naziv recepta za pretragu: ");
        string Naziv = Console.ReadLine() ?? "";

        var rezultati = recipeService.PretraziPoNazivu(Naziv);
        if (!rezultati.Any())
        {
            Console.WriteLine("Nema rezultata za pretragu.");
            return;
        }

        Console.WriteLine("Pronađeni recepti:");
        foreach (var recept in rezultati)
        {
            Console.WriteLine(recept);
        }
    }

    public static void PretraziPoAtributimaUI(IRecipeService recipeService)
    {
        Console.WriteLine("Pretraga recepata po atributima:");

        Console.Write("Maksimalno vrijeme pripreme (u minutama, -1 za preskakanje): ");
        if (!int.TryParse(Console.ReadLine(), out int maxVrijeme))
        {
            maxVrijeme = -1;
        }

        Console.Write("Sastojak (prazno za preskakanje): ");
        string sastojak = Console.ReadLine();

        Console.Write("Minimalna popularnost (1-5, -1 za preskakanje): ");
        if (!int.TryParse(Console.ReadLine(), out int minPopularnost))
        {
            minPopularnost = -1;
        }

        var rezultati = recipeService.PretraziPoAtributima(maxVrijemePripreme: maxVrijeme, sastojak: sastojak, minPopularnost: minPopularnost);
        if (!rezultati.Any())
        {
            Console.WriteLine("Nema recepata koji zadovoljavaju kriterije.");
            return;
        }

        Console.WriteLine("Pronađeni recepti:");
        foreach (var recept in rezultati)
        {
            Console.WriteLine(recept);
        }
    }
    public static void OcijeniReceptUI(IRecipeService recipeService, IOcjenaService ocjenaService)
    {
        Console.Write("Unesite naziv recepta koji želite ocijeniti: ");
        string naziv = Console.ReadLine()?.Trim();

        if (string.IsNullOrEmpty(naziv))
        {
            Console.WriteLine("Naziv recepta ne može biti prazan.");
            return;
        }

        var recepti = recipeService.PretraziPoNazivu(naziv);

        if (!recepti.Any())
        {
            Console.WriteLine("Recept nije pronađen.");
            return;
        }

        Recipe recept = null;
        if (recepti.Count > 1)
        {
            Console.WriteLine("Pronađeno je više recepata sa tim nazivom. Molimo odaberite ID:");
            foreach (var r in recepti)
            {
                Console.WriteLine($"ID: {r.Id}, Naziv: {r.Naziv}, Kategorija: {r.Kategorija}");
            }

            Console.Write("Unesite ID recepta: ");
            if (!int.TryParse(Console.ReadLine(), out int selectedId))
            {
                Console.WriteLine("Neispravan ID.");
                return;
            }

            recept = recepti.FirstOrDefault(r => r.Id == selectedId);
            if (recept == null)
            {
                Console.WriteLine("Recept sa unesenim ID-om nije pronađen.");
                return;
            }
        }
        else
        {
            recept = recepti.First();
        }

        Console.WriteLine($"Ocjenjujete recept: {recept.Naziv}");
        Console.Write("Unesite ocjenu (1-5): ");
        if (!int.TryParse(Console.ReadLine(), out int ocjena) || ocjena < 1 || ocjena > 5)
        {
            Console.WriteLine("Neispravna ocjena. Ocjena mora biti između 1 i 5.");
            return;
        }

        try
        {
            ocjenaService.DodajOcjenu(recept.Id, ocjena);
            Console.WriteLine("Ocjena je uspješno dodana.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Greška prilikom dodavanja ocjene: {ex.Message}");
        }
    }

    public static void PrikaziProsjecnuOcjenuUI(IRecipeService recipeService, IOcjenaService ocjenaService)
    {
        Console.Write("Unesite naziv recepta za prikaz prosječne ocjene: ");
        string naziv = Console.ReadLine()?.Trim();

        if (string.IsNullOrEmpty(naziv))
        {
            Console.WriteLine("Naziv recepta ne može biti prazan.");
            return;
        }

        var recepti = recipeService.PretraziPoNazivu(naziv);

        if (!recepti.Any())
        {
            Console.WriteLine("Recept nije pronađen.");
            return;
        }

        Recipe recept = null;
        if (recepti.Count > 1)
        {
            Console.WriteLine("Pronađeno je više recepata sa tim nazivom. Molimo odaberite ID:");
            foreach (var r in recepti)
            {
                Console.WriteLine($"ID: {r.Id}, Naziv: {r.Naziv}, Kategorija: {r.Kategorija}");
            }

            Console.Write("Unesite ID recepta: ");
            if (!int.TryParse(Console.ReadLine(), out int selectedId))
            {
                Console.WriteLine("Neispravan ID.");
                return;
            }

            recept = recepti.FirstOrDefault(r => r.Id == selectedId);
            if (recept == null)
            {
                Console.WriteLine("Recept sa unesenim ID-om nije pronađen.");
                return;
            }
        }
        else
        {
            recept = recepti.First();
        }

        double prosjecnaOcjena = ocjenaService.GetProsjecnaOcjena(recept.Id);
        Console.WriteLine($"Recept: {recept.Naziv}");
        if (prosjecnaOcjena > 0)
        {
            Console.WriteLine($"Prosječna ocjena: {prosjecnaOcjena:0.00} / 5");
        }
        else
        {
            Console.WriteLine("Ovaj recept još nema ocjene.");
        }
    }


    public static void PrikaziSveRecepte(IRecipeService recipeService, IOcjenaService ocjenaService)
    {
        var recepti = recipeService.GetSviRecepti();
        if (!recepti.Any())
        {
            Console.WriteLine("Nema recepata za prikaz.");
            return;
        }

        Console.WriteLine("Svi recepti:");
        foreach (var recept in recepti)
        {
            double prosjecnaOcjena = ocjenaService.GetProsjecnaOcjena(recept.Id);
            string ocjenaText = prosjecnaOcjena > 0 ? $"{prosjecnaOcjena:0.00} / 5" : "Nema ocjena";
            Console.WriteLine($"{recept} | Prosječna ocjena: {ocjenaText}");
        }
    }
}
