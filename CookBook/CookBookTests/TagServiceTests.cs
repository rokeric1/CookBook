using Models;
using Services;
using System.Collections.Generic;

namespace CookBook.Tests
{
    [TestClass]
    public class TagServiceTests
    {
        private TagService tagService;
        private RecipeService recipeService;

        [TestInitialize]
        public void SetUp()
        {
            var ingredientService = new IngredientService(); 
            recipeService = new RecipeService(ingredientService);

            // Dodajemo recepte u RecipeService
            recipeService.DodajRecept(new Recipe(1, "Pasta Carbonara", new Dictionary<int, double>
            {
                { 1, 200 }, // Pasta - 200g
                { 2, 2 },   // Jaja - 2 komada
                { 3, 50 }   // Parmezan - 50g
            }, "Glavno jelo", 20, "Skuhaj pastu i sjedini.", 5));

            recipeService.DodajRecept(new Recipe(2, "Grčka Salata", new Dictionary<int, double>
            {
                { 4, 100 }, // Maslinovo ulje - 100ml
                { 5, 50 },  // Feta sir - 50g
                { 6, 20 }   // Krastavci - 20g
            }, "Prilog", 10, "Izmešaj sve sastojke.", 4));

            // Kreiranje TagService
            tagService = new TagService();
        }

        [TestMethod]
        public void KreirajTag_ShouldCreateNewTag()
        {
            var tag = tagService.KreirajTag("Quick");
            Assert.AreEqual("Quick", tag.Naziv);
            Assert.AreEqual(1, tag.Id);
        }

        [TestMethod]
        public void DodajReceptUTag_ShouldAddRecipeToTag()
        {
            tagService.KreirajTag("Healthy");
            tagService.DodajReceptUTag("Healthy", 1);

            var tagovi = tagService.PrikaziSveTagove();
            Assert.AreEqual(1, tagovi[0].Recepti.Count);
            Assert.AreEqual(1, tagovi[0].Recepti[0]);
        }

        [TestMethod]
        public void ObrisiTag_ShouldDeleteTag()
        {
            tagService.KreirajTag("Quick");
            tagService.ObrisiTag("Quick");

            Assert.AreEqual(0, tagService.PrikaziSveTagove().Count);
        }

        [TestMethod]
        [ExpectedException(typeof(System.Exception))]
        public void KreirajTag_ShouldThrowException_WhenTagAlreadyExists()
        {
            // Arrange
            tagService.KreirajTag("Quick");

            // Act
            tagService.KreirajTag("Quick");
        }


        [TestMethod]
        [ExpectedException(typeof(System.Exception))]
        public void DodajReceptUTag_ShouldThrowException_WhenTagDoesNotExist()
        {
            // Act
            tagService.DodajReceptUTag("Nonexistent", 1);
        }

        [TestMethod]
        public void PrikaziRecepteZaTag_ShouldReturnRecipesForTag()
        {
            // Arrange
            tagService.KreirajTag("Healthy");
            tagService.DodajReceptUTag("Healthy", 1);
            tagService.DodajReceptUTag("Healthy", 2);

            // Act
            var recepti = tagService.PrikaziRecepteZaTag("Healthy", recipeService);

            // Assert
            Assert.AreEqual(2, recepti.Count);
            Assert.AreEqual("Pasta Carbonara", recepti[0].Naziv);
            Assert.AreEqual("Grčka Salata", recepti[1].Naziv);
        }

        [TestMethod]
        public void PrikaziRecepteZaTag_ShouldReturnEmptyListForTagWithoutRecipes()
        {
            // Arrange
            tagService.KreirajTag("EmptyTag");

            // Act
            var recepti = tagService.PrikaziRecepteZaTag("EmptyTag", recipeService);

            // Assert
            Assert.AreEqual(0, recepti.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(System.Exception))]
        public void PrikaziRecepteZaTag_ShouldThrowExceptionForNonExistingTag()
        {
            // Act
            tagService.PrikaziRecepteZaTag("NonexistentTag", recipeService);
        }
    }
}
