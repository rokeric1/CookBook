using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;

namespace Services
{
    public class DataExportImportService : IDataExportImportService
    {
        private readonly IIngredientService ingredientService;

        public DataExportImportService(IIngredientService ingredientService)
        {
            this.ingredientService = ingredientService;
        }

        public void EksportujRecepte(string format, List<Recipe> recepti)
        {
            if (string.IsNullOrEmpty(format))
            {
                throw new ArgumentException("Format ne može biti prazan.", nameof(format));
            }

            format = format.ToUpper();

            try
            {
                switch (format)
                {
                    case "JSON":
                        ExportToJson(recepti, "recepti.json");
                        break;
                    case "XML":
                        ExportToXml(recepti, "recepti.xml");
                        break;
                    default:
                        throw new ArgumentException("Nepoznat format. Odaberite JSON ili XML.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Greška prilikom izvoza recepata: {ex.Message}", ex);
            }
        }

        public List<Recipe> ImportujRecepte(string format)
        {
            if (string.IsNullOrEmpty(format))
            {
                throw new ArgumentException("Format ne može biti prazan.", nameof(format));
            }

            format = format.ToUpper();

            try
            {
                List<Recipe> recepti;
                switch (format)
                {
                    case "JSON":
                        recepti = ImportFromJson<Recipe>("recepti.json");
                        break;
                    case "XML":
                        recepti = ImportFromXml<Recipe>("recepti.xml");
                        break;
                    default:
                        throw new ArgumentException("Nepoznat format. Odaberite JSON ili XML.");
                }
                foreach (var recept in recepti)
                {
                    foreach (var sastojakId in recept.Sastojci.Keys.ToList())
                    {
                        var ingredient = ingredientService.GetIngredientById(sastojakId);
                        if (ingredient == null)
                        {
                            Console.WriteLine($"Ingredient ID {sastojakId} nije pronađen. Dodajem ga kao nedostupan.");
                            var noviSastojak = new Ingredient(sastojakId, "Unknown", "Unknown", 0.0, 0.0m, false);
                            ingredientService.DodajIngredient(noviSastojak, preserveId: true);
                        }
                    }
                }

                return recepti;
            }
            catch (Exception ex)
            {
                throw new Exception($"Greška prilikom uvoza recepata: {ex.Message}", ex);
            }
        }

        public void EksportujSastojke(string format, List<Ingredient> sastojci)
        {
            if (string.IsNullOrEmpty(format))
            {
                throw new ArgumentException("Format ne može biti prazan.", nameof(format));
            }

            format = format.ToUpper();

            try
            {
                switch (format)
                {
                    case "JSON":
                        ExportToJson(sastojci, "ingredients.json");
                        break;
                    case "XML":
                        ExportToXml(sastojci, "ingredients.xml");
                        break;
                    default:
                        throw new ArgumentException("Nepoznat format. Odaberite JSON ili XML.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Greška prilikom izvoza sastojaka: {ex.Message}", ex);
            }
        }

        public List<Ingredient> ImportujSastojke(string format)
        {
            if (string.IsNullOrEmpty(format))
            {
                throw new ArgumentException("Format ne može biti prazan.", nameof(format));
            }

            format = format.ToUpper();

            try
            {
                List<Ingredient> sastojci;
                switch (format)
                {
                    case "JSON":
                        sastojci = ImportFromJson<Ingredient>("ingredients.json");
                        break;
                    case "XML":
                        sastojci = ImportFromXml<Ingredient>("ingredients.xml");
                        break;
                    default:
                        throw new ArgumentException("Nepoznat format. Odaberite JSON ili XML.");
                }

                foreach (var sastojak in sastojci)
                {
                    var existing = ingredientService.GetIngredientById(sastojak.Id);
                    if (existing == null)
                    {
                        ingredientService.DodajIngredient(sastojak, preserveId: true);
                    }
                    else
                    {
                        Console.WriteLine($"Sastojak sa ID {sastojak.Id} već postoji. Preskačem.");
                    }
                }

                return sastojci;
            }
            catch (Exception ex)
            {
                throw new Exception($"Greška prilikom uvoza sastojaka: {ex.Message}", ex);
            }
        }

        private void ExportToJson<T>(List<T> podaci, string filePath)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(podaci, options);
            File.WriteAllText(filePath, json);
        }

        private void ExportToXml<T>(List<T> podaci, string filePath)
        {
            var serializer = new XmlSerializer(typeof(List<T>));
            using (var writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, podaci);
            }
        }

        private List<T> ImportFromJson<T>(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Datoteka {filePath} nije pronađena.");
            }

            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<T>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        private List<T> ImportFromXml<T>(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Datoteka {filePath} nije pronađena.");
            }

            var serializer = new XmlSerializer(typeof(List<T>));
            using (var reader = new StreamReader(filePath))
            {
                return (List<T>)serializer.Deserialize(reader);
            }
        }
    }
}
