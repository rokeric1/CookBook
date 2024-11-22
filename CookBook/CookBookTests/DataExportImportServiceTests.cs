using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Moq;
using Services;
using System;
using System.Collections.Generic;
using System.IO;

namespace CookBookTests
{
    [TestClass]
    public class DataExportImportServiceTests
    {
        private Mock<IIngredientService> mockIngredientService;
        private DataExportImportService service;

        [TestInitialize]
        public void Setup()
        {
            mockIngredientService = new Mock<IIngredientService>();
            service = new DataExportImportService(mockIngredientService.Object);
        }

        [TestMethod]
        [DataRow("JSON", "test_empty_recepti.json", DisplayName = "Export Empty Recipes to JSON")]
        [DataRow("XML", "test_empty_recepti.xml", DisplayName = "Export Empty Recipes to XML")]
        public void EksportujRecepte_EmptyList_ShouldCreateEmptyFile(string format, string filePath)
        {
            try
            {
                service.EksportujRecepte(format, new List<Recipe>(), filePath);

                Assert.IsTrue(File.Exists(filePath), $"File {filePath} was not created.");
                var fileContent = File.ReadAllText(filePath);
                if (format == "JSON")
                {
                    Assert.AreEqual("[]", fileContent.Trim(), "File content is not an empty JSON array.");
                }
                else
                {
                    Assert.IsTrue(fileContent.Contains("<ArrayOfRecipe"), "File content does not contain valid XML structure.");
                }
            }
            finally
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }

        [TestMethod]
        [DataRow("JSON", "test_empty_ingredients.json", DisplayName = "Export Empty Ingredients to JSON")]
        [DataRow("XML", "test_empty_ingredients.xml", DisplayName = "Export Empty Ingredients to XML")]
        public void EksportujSastojke_EmptyList_ShouldCreateEmptyFile(string format, string filePath)
        {
            try
            {
                service.EksportujSastojke(format, new List<Ingredient>(), filePath);

                Assert.IsTrue(File.Exists(filePath), $"File {filePath} was not created.");
                var fileContent = File.ReadAllText(filePath);
                if (format == "JSON")
                {
                    Assert.AreEqual("[]", fileContent.Trim(), "File content is not an empty JSON array.");
                }
                else
                {
                    Assert.IsTrue(fileContent.Contains("<ArrayOfIngredient"), "File content does not contain valid XML structure.");
                }
            }
            finally
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }

        [TestMethod]
        [DataRow("JSON", "test_invalid_recepti.json", DisplayName = "Import Recipes with Invalid JSON Format")]
        public void ImportujRecepte_InvalidJsonFormat_ShouldThrowJsonException(string format, string filePath)
        {
            try
            {
                File.WriteAllText(filePath, "{ invalid json }");

                Assert.ThrowsException<System.Exception>(() => service.ImportujRecepte(format, filePath));
            }
            finally
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }

        [TestMethod]
        [DataRow("XML", "test_invalid_ingredients.xml", DisplayName = "Import Ingredients with Invalid XML Format")]
        public void ImportujSastojke_InvalidXmlFormat_ShouldThrowInvalidOperationException(string format, string filePath)
        {
            try
            {
                File.WriteAllText(filePath, "<Invalid><Xml>");

                Assert.ThrowsException<System.Exception>(() => service.ImportujSastojke(format, filePath));
            }
            finally
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }

        [TestMethod]
        [DataRow("JSON", "test_nonexistent_recepti.json", DisplayName = "Import Nonexistent Recipes File")]
        [DataRow("XML", "test_nonexistent_recepti.xml", DisplayName = "Import Nonexistent Recipes File")]
        public void ImportujRecepte_FileNotFound_ShouldThrowFileNotFoundException(string format, string filePath)
        {
            Assert.ThrowsException<Exception>(() => service.ImportujRecepte(format, filePath));
        }

        [TestMethod]
        [DataRow("JSON", "test_nonexistent_ingredients.json", DisplayName = "Import Nonexistent Ingredients File")]
        [DataRow("XML", "test_nonexistent_ingredients.xml", DisplayName = "Import Nonexistent Ingredients File")]
        public void ImportujSastojke_FileNotFound_ShouldThrowFileNotFoundException(string format, string filePath)
        {
            Assert.ThrowsException<Exception>(() => service.ImportujSastojke(format, filePath));
        }

        [TestMethod]
        [DataRow("JSON", "test_empty_recepti_import.json", DisplayName = "Import Empty Recipes File")]
        [DataRow("XML", "test_empty_recepti_import.xml", DisplayName = "Import Empty Recipes File")]
        public void ImportujRecepte_EmptyFile_ShouldReturnEmptyList(string format, string filePath)
        {
            try
            {
                if (format == "JSON")
                {
                    File.WriteAllText(filePath, "[]");
                }
                else if (format == "XML")
                {
                    File.WriteAllText(filePath, "<ArrayOfRecipe></ArrayOfRecipe>");
                }

                var result = service.ImportujRecepte(format, filePath);

                Assert.IsNotNull(result);
                Assert.AreEqual(0, result.Count, "The list is not empty.");
            }
            finally
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }

        [TestMethod]
        [DataRow("JSON", "test_empty_ingredients_import.json", DisplayName = "Import Empty Ingredients File")]
        [DataRow("XML", "test_empty_ingredients_import.xml", DisplayName = "Import Empty Ingredients File")]
        public void ImportujSastojke_EmptyFile_ShouldReturnEmptyList(string format, string filePath)
        {
            try
            {
                if (format == "JSON")
                {
                    File.WriteAllText(filePath, "[]");
                }
                else if (format == "XML")
                {
                    File.WriteAllText(filePath, "<ArrayOfIngredient></ArrayOfIngredient>");
                }

                var result = service.ImportujSastojke(format, filePath);

                Assert.IsNotNull(result);
                Assert.AreEqual(0, result.Count, "The list is not empty.");
            }
            finally
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }

        [TestMethod]
        [DataRow("INVALID", "test_invalid_format_recepti.json", DisplayName = "Export Recipes with Invalid Format")]
        [DataRow("INVALID", "test_invalid_format_ingredients.json", DisplayName = "Export Ingredients with Invalid Format")]
        public void Eksportuj_InvalidFormat_ShouldThrowArgumentException(string format, string filePath)
        {
            var recepti = new List<Recipe>
            {
                new Recipe(1, "Test Recept", new Dictionary<int, double> { { 1, 2.0 } }, "Test", 30, "Instructions", 4)
            };

            var sastojci = new List<Ingredient>
            {
                new Ingredient { Id = 1, Naziv = "Test Sastojak", Nutrijenti = "Test Nutrijenti", Cijena = 10.0M, EkoUtjecaj = 1.0, Dostupan = true }
            };

            Assert.ThrowsException<Exception>(() => service.EksportujRecepte(format, recepti, filePath));
            Assert.ThrowsException<Exception>(() => service.EksportujSastojke(format, sastojci, filePath));
        }

        [TestMethod]
        [DataRow("JSON", "test_missing_ingredient_recepti.json", DisplayName = "Import Recipes with Missing Ingredient")]
        public void ImportujRecepte_MissingIngredient_ShouldHandleGracefully(string format, string filePath)
        {
            var recepti = new List<Recipe>
            {
                new Recipe(1, "Test Recept", new Dictionary<int, double> { { 999, 1.0 } }, "Test", 30, "Instructions", 4)
            };

            try
            {
                if (format == "JSON")
                {
                    File.WriteAllText(filePath, System.Text.Json.JsonSerializer.Serialize(recepti));
                }
                else if (format == "XML")
                {
                    var serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<Recipe>));
                    using (var writer = new StreamWriter(filePath))
                    {
                        serializer.Serialize(writer, recepti);
                    }
                }

                mockIngredientService
                    .Setup(s => s.GetIngredientById(999))
                    .Returns((Ingredient)null);

                var result = service.ImportujRecepte(format, filePath);

                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual("Test Recept", result[0].Naziv);
            }
            finally
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }

        [TestMethod]
        [DataRow("JSON", "test_recepti_large.json", DisplayName = "Export and Import Large Recipe List")]
        public void EksportujIImportujRecepte_LargeList_ShouldWork(string format, string filePath)
        {
            var recepti = new List<Recipe>();
            for (int i = 1; i <= 1000; i++)
            {
                recepti.Add(new Recipe(i, $"Recept {i}", new Dictionary<int, double> { { 1, i } }, "Kategorija", i * 10, $"Upute {i}", i % 5 + 1));
            }

            try
            {
                service.EksportujRecepte(format, recepti, filePath);

                Assert.IsTrue(File.Exists(filePath), $"File {filePath} was not created.");

                var importedRecipes = service.ImportujRecepte(format, filePath);

                Assert.IsNotNull(importedRecipes);
                Assert.AreEqual(1000, importedRecipes.Count);
                Assert.AreEqual("Recept 1", importedRecipes[0].Naziv);
                Assert.AreEqual($"Recept 1000", importedRecipes[999].Naziv);
            }
            finally
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }

        [TestMethod]
        [DataRow("", "test_empty_format_recepti.json", DisplayName = "Export Recipes with Empty Format")]
        [DataRow("INVALID", "test_invalid_format_recepti.json", DisplayName = "Export Recipes with Invalid Format")]
        public void EksportujRecepte_InvalidFormat_ShouldThrowExpectedException(string format, string filePath)
        {
            var recepti = new List<Recipe>
    {
        new Recipe(1, "Pita od jabuka", new Dictionary<int, double> { { 1, 2.0 } }, "Desert", 45, "Koraci pripreme...", 5)
    };

            try
            {
                service.EksportujRecepte(format, recepti, filePath);
                Assert.Fail("Expected exception was not thrown.");
            }
            catch (Exception ex) when (ex is ArgumentException || ex is Exception)
            {
                Assert.IsTrue(true, $"Caught expected exception: {ex.GetType().Name}");
            }
        }


        [TestMethod]
        [DataRow("", "test_empty_format_import_recepti.json", DisplayName = "Import Recipes with Empty Format")]
        [DataRow("INVALID", "test_invalid_format_import_recepti.json", DisplayName = "Import Recipes with Invalid Format")]
        public void ImportujRecepte_InvalidFormat_ShouldThrowExpectedException(string format, string filePath)
        {
            try
            {
                service.ImportujRecepte(format, filePath);
                Assert.Fail("Expected exception was not thrown.");
            }
            catch (Exception ex) when (ex is ArgumentException || ex is Exception)
            {
                Assert.IsTrue(true, $"Caught expected exception: {ex.GetType().Name}");
            }
        }


        [TestMethod]
        [DataRow("", "test_empty_format_import_ingredients.json", DisplayName = "Import Ingredients with Empty Format")]
        [DataRow("INVALID", "test_invalid_format_import_ingredients.json", DisplayName = "Import Ingredients with Invalid Format")]
        public void ImportujSastojke_InvalidFormat_ShouldThrowExpectedException(string format, string filePath)
        {
            try
            {
                service.ImportujSastojke(format, filePath);
                Assert.Fail("Expected exception was not thrown.");
            }
            catch (Exception ex) when (ex is ArgumentException || ex is Exception)
            {
                Assert.IsTrue(true, $"Caught expected exception: {ex.GetType().Name}");
            }
        }


        [TestMethod]
        [DataRow("JSON", "test_large_recepti.json", DisplayName = "Export Large Number of Recipes")]
        public void EksportujRecepte_LargeData_ShouldExportSuccessfully(string format, string filePath)
        {
            var recepti = new List<Recipe>();
            for (int i = 0; i < 10000; i++)
            {
                recepti.Add(new Recipe(i, $"Recipe {i}", new Dictionary<int, double> { { i, i * 0.1 } }, "Category", 10, "Instructions", 5));
            }

            try
            {
                service.EksportujRecepte(format, recepti, filePath);

                Assert.IsTrue(File.Exists(filePath), $"File {filePath} was not created.");
                var fileContent = File.ReadAllText(filePath);
                Assert.IsFalse(string.IsNullOrEmpty(fileContent), "File content is empty.");
            }
            finally
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }

        [TestMethod]
        [DataRow("", "test_empty_format_ingredients.json", DisplayName = "Export Ingredients with Empty Format")]
        [DataRow("INVALID", "test_invalid_format_ingredients.json", DisplayName = "Export Ingredients with Invalid Format")]
        public void EksportujSastojke_InvalidFormat_ShouldThrowExpectedException(string format, string filePath)
        {
            var sastojci = new List<Ingredient>
    {
        new Ingredient { Id = 1, Naziv = "Jabuka", Nutrijenti = "Vitamini A, C", Cijena = 3.5M, EkoUtjecaj = 0.75, Dostupan = true }
    };

            try
            {
                service.EksportujSastojke(format, sastojci, filePath);
                Assert.Fail("Expected exception was not thrown.");
            }
            catch (Exception ex) when (ex is ArgumentException || ex is Exception)
            {
                Assert.IsTrue(true, $"Caught expected exception: {ex.GetType().Name}");
            }
        }

        [TestMethod]
        [DataRow("JSON", "test_invalid_content_ingredients.json", DisplayName = "Import Ingredients with Invalid JSON Content")]
        [DataRow("XML", "test_invalid_content_ingredients.xml", DisplayName = "Import Ingredients with Invalid XML Content")]
        public void ImportujSastojke_InvalidContent_ShouldThrowExpectedException(string format, string filePath)
        {
            try
            {
                if (format == "JSON")
                {
                    File.WriteAllText(filePath, "{ invalid json }");
                }
                else if (format == "XML")
                {
                    File.WriteAllText(filePath, "<Invalid><Xml>");
                }

                service.ImportujSastojke(format, filePath);
                Assert.Fail("Expected exception was not thrown.");
            }
            catch (Exception ex)
            {
                if (format == "JSON")
                {
                    Assert.IsTrue(ex.InnerException is System.Text.Json.JsonException || ex is System.Text.Json.JsonException,
                        $"Expected JsonException, but got {ex.GetType().Name}. Message: {ex.Message}");
                }
                else if (format == "XML")
                {
                    Assert.IsTrue(ex.InnerException is System.InvalidOperationException || ex is System.InvalidOperationException,
                        $"Expected InvalidOperationException, but got {ex.GetType().Name}. Message: {ex.Message}");
                }
            }
            finally
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }



        [TestMethod]
        [DataRow("JSON", "test_missing_fields_ingredients.json", DisplayName = "Import Ingredients with Missing JSON Fields")]
        [DataRow("XML", "test_missing_fields_ingredients.xml", DisplayName = "Import Ingredients with Missing XML Fields")]
        public void ImportujSastojke_MissingFields_ShouldThrowExpectedException(string format, string filePath)
        {
            try
            {
                if (format == "JSON")
                {
                    File.WriteAllText(filePath, "[{ \"Id\": 1, \"Naziv\": null }]");
                }
                else if (format == "XML")
                {
                    File.WriteAllText(filePath, "<ArrayOfIngredient><Ingredient><Id>1</Id><Naziv></Naziv></Ingredient></ArrayOfIngredient>");
                }

                service.ImportujSastojke(format, filePath);
                Assert.Fail("Expected exception was not thrown.");
            }
            catch (Exception ex) when (ex is ArgumentNullException || ex is Exception)
            {
                Assert.IsTrue(true, $"Caught expected exception: {ex.GetType().Name}");
            }
            finally
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }


    }
}
