using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Services;
using System;
using System.Collections.Generic;
using System.IO;

namespace CookBookTests
{
    [TestClass]
    public class DataExportImportServiceTest
    {
        private DataExportImportService _dataExportImportService;
        private FakeIngredientService _ingredientService;
        private string _tempDirectory;

        [TestInitialize]
        public void TestInitialize()
        {
            _ingredientService = new FakeIngredientService();
            _dataExportImportService = new DataExportImportService(_ingredientService);

            _tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(_tempDirectory);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (Directory.Exists(_tempDirectory))
            {
                Directory.Delete(_tempDirectory, true);
            }
        }

        [TestMethod]
        public void EksportujRecepte_CreatesJsonFile()
        {
            // Arrange
            var recepti = new List<Recipe>
            {
                new Recipe { Id = 1, Naziv = "Recipe1" }
            };

            // Act
            _dataExportImportService.EksportujRecepte("JSON", recepti);

            // Assert
            Assert.IsTrue(File.Exists("recepti.json"));
        }

        [TestMethod]
        public void ImportujRecepte_ReadsJsonFile()
        {
            // Arrange
            string jsonContent = "[{\"Id\":1,\"Naziv\":\"Recipe1\"}]";
            File.WriteAllText("recepti.json", jsonContent);

            // Act
            var recepti = _dataExportImportService.ImportujRecepte("JSON");

            // Assert
            Assert.IsNotNull(recepti);
            Assert.AreEqual(1, recepti.Count);
            Assert.AreEqual("Recipe1", recepti[0].Naziv);
        }

        [TestMethod]
        public void EksportujSastojke_CreatesXmlFile()
        {
            var sastojci = new List<Ingredient>
            {
                new Ingredient { Id = 1, Naziv = "Mlijeko" }
            };

            _dataExportImportService.EksportujSastojke("XML", sastojci);

            Assert.IsTrue(File.Exists("ingredients.xml"));
        }


    }
}
