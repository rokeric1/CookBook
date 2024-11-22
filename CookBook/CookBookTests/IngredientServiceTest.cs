using Models;
using Services;

namespace CookBookTests
{
    [TestClass]
    public class IngredientServiceTest
    {
        private IngredientService _ingredientService;

        [TestInitialize]
        public void TestInitialize()
        {
            _ingredientService = new IngredientService();
        }

        [TestMethod]
        public void DodajIngredient_AddsIngredient_WhenIngredientIsUnique()
        {
            var ingredient = new Ingredient
            {
                Id = 1,
                Naziv = "Mlijeko",
                Nutrijenti = "10g proteina, 5 grama masti",
                EkoUtjecaj = 1.2,
                Cijena = 2,
                Dostupan = true
            };

            _ingredientService.DodajIngredient(ingredient);

            var result = _ingredientService.GetIngredientById(1);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DodajIngredient_ThrowsArgumentException_WhenIdAlreadyExistsAndPreserveIdIsTrue()
        {
            var ingredient1 = new Ingredient
            {
                Id = 1,
                Naziv = "Mlijeko",
                Nutrijenti = "10g proteina, 5 grama masti",
                EkoUtjecaj = 1.2,
                Cijena = 2,
                Dostupan = true
            };

            var ingredient2 = new Ingredient
            {
                Id = 1,
                Naziv = "Jogurt",
                Nutrijenti = "20g proteina, 10 grama masti",
                EkoUtjecaj = 1.0,
                Cijena = 3,
                Dostupan = true
            };

            _ingredientService.DodajIngredient(ingredient1);
            _ingredientService.DodajIngredient(ingredient2, true);
        }

        [TestMethod]
        public void DodajIngredient_AddsIngredientWithNewId_WhenIdAlreadyExistsAndPreserveIdIsFalse()
        {
            var ingredient1 = new Ingredient
            {
                Id = 1,
                Naziv = "Mlijeko",
                Nutrijenti = "10g proteina, 5 grama masti",
                EkoUtjecaj = 1.2,
                Cijena = 2,
                Dostupan = true
            };

            var ingredient2 = new Ingredient
            {
                Id = 1,
                Naziv = "Jogurt",
                Nutrijenti = "20g proteina, 10 grama masti",
                EkoUtjecaj = 1.0,
                Cijena = 3,
                Dostupan = true
            };

            _ingredientService.DodajIngredient(ingredient1);
            _ingredientService.DodajIngredient(ingredient2);

            var result1 = _ingredientService.GetIngredientById(1);
            var result2 = _ingredientService.GetIngredientById(2);
            Assert.AreEqual(result1, ingredient1);
            Assert.AreEqual(result2, ingredient2);
        }

        [TestMethod]
        public void DodajIngredient_UpdatesCurrentId_WhenPreserveIdIsTrueAndIdIsHigh()
        {
            var ingredient = new Ingredient
            {
                Id = 10,
                Naziv = "Mlijeko",
                Nutrijenti = "10g proteina, 5 grama masti",
                EkoUtjecaj = 1.2,
                Cijena = 2,
                Dostupan = true
            };

            _ingredientService.DodajIngredient(ingredient, true);
            Assert.AreEqual(11, _ingredientService.GetNextId());
        }

        [TestMethod]
        public void DodajIngredient_InitializesCurrentIdCorrectly_WhenListIsEmpty()
        {
            var ingredient = new Ingredient { Naziv = "Mlijeko" };
            _ingredientService.DodajIngredient(ingredient);

            var result = _ingredientService.GetIngredientById(1);
            Assert.AreEqual(1, result.Id);
        }

        [TestMethod]
        public void GetIngredientById_ReturnsIngredientWithId()
        {
            var ingredient = new Ingredient
            {
                Id = 5,
                Naziv = "Mlijeko",
            };

            _ingredientService.DodajIngredient(ingredient, true);
            
            var result = _ingredientService.GetIngredientById(5);
            Assert.AreEqual(result, ingredient);
        }


        [TestMethod]
        public void GetIngredientByName_ReturnsIngredientWithName()
        {
            var ingredient = new Ingredient
            {
                Id = 5,
                Naziv = "Mlijeko",
            };

            _ingredientService.DodajIngredient(ingredient, true);

            var result = _ingredientService.GetIngredientByName("Mlijeko");
            Assert.AreEqual(result, ingredient);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetIngredientByName_ThrowsArgumentException_WhenNameIsNotFound()
        {
            _ingredientService.GetIngredientByName("Jogurt");
        }

        [TestMethod]
        public void GetNextId_ReturnsIncrementedId_WhenPreserveIdIsTrue()
        {
            var ingredient = new Ingredient
            {
                Id = 5,
                Naziv = "Mlijeko"
            };

            
            var id1 = _ingredientService.GetNextId();
            _ingredientService.DodajIngredient(ingredient, true);
            var result = _ingredientService.GetIngredientByName("Mlijeko");
            var id2 = _ingredientService.GetNextId();

            Assert.AreEqual(1, id1);
            Assert.AreEqual(5, result.Id);
            Assert.AreEqual(6, id2);
        }
        public void GetNextId_ReturnsIncrementedId_WhenPreserveIdIsFalse()
        {
            var ingredient = new Ingredient
            {
                Id = 5,
                Naziv = "Mlijeko"
            };


            var id1 = _ingredientService.GetNextId();
            _ingredientService.DodajIngredient(ingredient);
            var result = _ingredientService.GetIngredientByName("Mlijeko");
            var id2 = _ingredientService.GetNextId();

            Assert.AreEqual(1, id1);
            Assert.AreEqual(2, result.Id);
            Assert.AreEqual(3, id2);
        }

        [TestMethod]
        public void GetAllIngredients_ReturnsAllIngredients_WhenIngredientsExist()
        {
            _ingredientService.DodajIngredient(new Ingredient { Naziv = "Mlijeko" });
            _ingredientService.DodajIngredient(new Ingredient { Naziv = "Jogurt" });

            var result = _ingredientService.GetAllIngredients();

            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void GetAllIngredient_ReturnsEmpty_WhenIngredientsDoNotExist()
        {
            var result = _ingredientService.GetAllIngredients();

            Assert.AreEqual(0, result.Count);
        }


    }
}
