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
    public class RecipeServiceTests
    {
        private RecipeService _recipeService;
        private IngredientService _ingredientService;

        [TestInitialize]
        public void TestInitialize()
        {
            _ingredientService = new IngredientService();
            _recipeService = new RecipeService(_ingredientService);
        }

        [TestMethod]
        public void DodajRecept_ShouldAddRecipe_WhenRecipeIsUnique()
        {
            // Arrange
            var recipe = new Recipe
            {
                Naziv = "Torta",
                Sastojci = new Dictionary<int, double> { { 1, 200 } },
                Kategorija = "Desert",
                VrijemePripreme = 60,
                Upute = "Peći 40 minuta.",
                Popularnost = 5
            };

            // Act
            _recipeService.DodajRecept(recipe);

            // Assert
            var result = _recipeService.GetReceptPoId(1);
            Assert.IsNotNull(result);
            Assert.AreEqual("Torta", result.Naziv);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DodajRecept_ShouldThrowException_WhenRecipeNameAlreadyExists()
        {
            // Arrange
            var recipe1 = new Recipe { Naziv = "Torta" };
            var recipe2 = new Recipe { Naziv = "Torta" };

            _recipeService.DodajRecept(recipe1);

            // Act
            _recipeService.DodajRecept(recipe2);

            // Assert - Expect exception
        }

        [TestMethod]
        public void AzurirajRecept_ShouldUpdateRecipe_WhenRecipeExists()
        {
            // Arrange
            var recipe = new Recipe { Naziv = "Torta" };
            _recipeService.DodajRecept(recipe);

            var updatedRecipe = new Recipe
            {
                Naziv = "Nova Torta",
                Sastojci = new Dictionary<int, double> { { 2, 100 } },
                Kategorija = "Desert",
                VrijemePripreme = 45,
                Upute = "Nove upute.",
                Popularnost = 10
            };

            // Act
            _recipeService.AzurirajRecept(1, updatedRecipe);

            // Assert
            var result = _recipeService.GetReceptPoId(1);
            Assert.IsNotNull(result);
            Assert.AreEqual("Nova Torta", result.Naziv);
            Assert.AreEqual(45, result.VrijemePripreme);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AzurirajRecept_ShouldThrowException_WhenRecipeDoesNotExist()
        {
            // Arrange
            var updatedRecipe = new Recipe { Naziv = "Nova Torta" };

            // Act
            _recipeService.AzurirajRecept(1, updatedRecipe);

            // Assert - Expect exception
        }

        [TestMethod]
        public void ObrisiRecept_ShouldRemoveRecipe_WhenRecipeExists()
        {
            // Arrange
            var recipe = new Recipe { Naziv = "Torta" };
            _recipeService.DodajRecept(recipe);

            // Act
            _recipeService.ObrisiRecept(1);

            // Assert
            var result = _recipeService.GetReceptPoId(1);
            Assert.IsNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ObrisiRecept_ShouldThrowException_WhenRecipeDoesNotExist()
        {
            // Act
            _recipeService.ObrisiRecept(1);

            // Assert - Expect exception
        }

        [TestMethod]
        public void PretraziPoNazivu_ShouldReturnMatchingRecipes()
        {
            // Arrange
            _recipeService.DodajRecept(new Recipe { Naziv = "Torta" });
            _recipeService.DodajRecept(new Recipe { Naziv = "Kolač" });

            // Act
            var result = _recipeService.PretraziPoNazivu("Torta");

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Torta", result[0].Naziv);
        }

        [TestMethod]
        public void GetSviRecepti_ShouldReturnAllRecipes()
        {
            // Arrange
            _recipeService.DodajRecept(new Recipe { Naziv = "Torta" });
            _recipeService.DodajRecept(new Recipe { Naziv = "Kolač" });

            // Act
            var result = _recipeService.GetSviRecepti();

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void GetNextId_ShouldIncrementIdProperly()
        {
            // Arrange
            var id1 = _recipeService.GetNextId();
            var id2 = _recipeService.GetNextId();

            // Assert
            Assert.AreEqual(1, id1);
            Assert.AreEqual(2, id2);
        }

        [TestMethod]
        public void PretraziPoAtributima_ShouldReturnRecipes_WhenMatchingMaxVrijemePripreme()
        {
            // Arrange
            _recipeService.DodajRecept(new Recipe
            {
                Naziv = "Torta",
                VrijemePripreme = 60,
                Sastojci = new Dictionary<int, double>(),
                Popularnost = 10
            });

            _recipeService.DodajRecept(new Recipe
            {
                Naziv = "Kolač",
                VrijemePripreme = 30,
                Sastojci = new Dictionary<int, double>(),
                Popularnost = 5
            });

            // Act
            var result = _recipeService.PretraziPoAtributima(maxVrijemePripreme: 40);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Kolač", result[0].Naziv);
        }

        [TestMethod]
        public void PretraziPoAtributima_ShouldReturnRecipes_WhenMatchingSastojak()
        {
            // Arrange
            var sastojakId = 1;
            var ingredient = new Ingredient { Id = sastojakId, Naziv = "Čokolada" };
            _ingredientService.DodajIngredient(ingredient);

            _recipeService.DodajRecept(new Recipe
            {
                Naziv = "Torta",
                VrijemePripreme = 60,
                Sastojci = new Dictionary<int, double> { { sastojakId, 200 } },
                Popularnost = 10
            });

            _recipeService.DodajRecept(new Recipe
            {
                Naziv = "Kolač",
                VrijemePripreme = 30,
                Sastojci = new Dictionary<int, double>(),
                Popularnost = 5
            });

            // Act
            var result = _recipeService.PretraziPoAtributima(sastojak: "Čokolada");

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Torta", result[0].Naziv);
        }

        [TestMethod]
        public void PretraziPoAtributima_ShouldReturnRecipes_WhenMatchingMinPopularnost()
        {
            // Arrange
            _recipeService.DodajRecept(new Recipe
            {
                Naziv = "Torta",
                VrijemePripreme = 60,
                Sastojci = new Dictionary<int, double>(),
                Popularnost = 10
            });

            _recipeService.DodajRecept(new Recipe
            {
                Naziv = "Kolač",
                VrijemePripreme = 30,
                Sastojci = new Dictionary<int, double>(),
                Popularnost = 5
            });

            // Act
            var result = _recipeService.PretraziPoAtributima(minPopularnost: 6);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Torta", result[0].Naziv);
        }

        [TestMethod]
        public void PretraziPoAtributima_ShouldReturnAllMatchingRecipes_WhenMultipleCriteria()
        {
            // Arrange
            var sastojakId = 1;
            var ingredient = new Ingredient { Id = sastojakId, Naziv = "Čokolada" };
            _ingredientService.DodajIngredient(ingredient);

            _recipeService.DodajRecept(new Recipe
            {
                Naziv = "Torta",
                VrijemePripreme = 60,
                Sastojci = new Dictionary<int, double> { { sastojakId, 200 } },
                Popularnost = 10
            });

            _recipeService.DodajRecept(new Recipe
            {
                Naziv = "Kolač",
                VrijemePripreme = 30,
                Sastojci = new Dictionary<int, double> { { sastojakId, 100 } },
                Popularnost = 8
            });

            _recipeService.DodajRecept(new Recipe
            {
                Naziv = "Pita",
                VrijemePripreme = 90,
                Sastojci = new Dictionary<int, double>(),
                Popularnost = 5
            });

            // Act
            var result = _recipeService.PretraziPoAtributima(maxVrijemePripreme: 60, sastojak: "Čokolada", minPopularnost: 9);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Torta", result[0].Naziv);
        }

    }
}
