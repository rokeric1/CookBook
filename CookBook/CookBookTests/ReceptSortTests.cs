using Models;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBookTests
{
    [TestClass]
    public class ReceptSortTests
    {
        [TestMethod]
        public void SortirajPoKategoriji_ShouldSortRecipes_WhenCategoriesAreAlphabetical()
        {
            // Arrange
            var recipes = new List<Recipe>
            {
                new Recipe { Naziv = "Torta", Kategorija = "Desert" },
                new Recipe { Naziv = "Čorba", Kategorija = "Predjelo" },
                new Recipe { Naziv = "Pizza", Kategorija = "Glavno jelo" }
            };

            // Act
            ReceptSort.SortirajPoKategoriji(recipes);

            // Assert
            Assert.AreEqual("Desert", recipes[0].Kategorija);
            Assert.AreEqual("Glavno jelo", recipes[1].Kategorija);
            Assert.AreEqual("Predjelo", recipes[2].Kategorija);
        }

        [TestMethod]
        public void SortirajPoKategoriji_ShouldHandleEmptyList()
        {
            // Arrange
            var recipes = new List<Recipe>();

            // Act
            ReceptSort.SortirajPoKategoriji(recipes);

            // Assert
            Assert.AreEqual(0, recipes.Count); 
        }

        [TestMethod]
        public void SortirajPoKategoriji_ShouldHandleSingleElementList()
        {
            // Arrange
            var recipes = new List<Recipe>
            {
                new Recipe { Naziv = "Torta", Kategorija = "Desert" }
            };

            // Act
            ReceptSort.SortirajPoKategoriji(recipes);

            // Assert
            Assert.AreEqual(1, recipes.Count);
            Assert.AreEqual("Desert", recipes[0].Kategorija);
        }

        [TestMethod]
        public void SortirajPoKategoriji_ShouldSortRecipes_CaseInsensitive()
        {
            // Arrange
            var recipes = new List<Recipe>
            {
                new Recipe { Naziv = "Torta", Kategorija = "desert" },
                new Recipe { Naziv = "Čorba", Kategorija = "Predjelo" },
                new Recipe { Naziv = "Pizza", Kategorija = "GLAVNO JELO" }
            };

            // Act
            ReceptSort.SortirajPoKategoriji(recipes);

            // Assert
            Assert.AreEqual("desert", recipes[0].Kategorija);
            Assert.AreEqual("GLAVNO JELO", recipes[1].Kategorija);
            Assert.AreEqual("Predjelo", recipes[2].Kategorija);
        }

        [TestMethod]
        public void SortirajPoKategoriji_ShouldSortRecipes_WithDuplicateCategories()
        {
            // Arrange
            var recipes = new List<Recipe>
            {
                new Recipe { Naziv = "Torta", Kategorija = "Desert" },
                new Recipe { Naziv = "Čokoladni kolač", Kategorija = "Desert" },
                new Recipe { Naziv = "Pizza", Kategorija = "Glavno jelo" }
            };

            // Act
            ReceptSort.SortirajPoKategoriji(recipes);

            // Assert
            Assert.AreEqual("Desert", recipes[0].Kategorija);
            Assert.AreEqual("Desert", recipes[1].Kategorija);
            Assert.AreEqual("Glavno jelo", recipes[2].Kategorija);
        }
    }
}
