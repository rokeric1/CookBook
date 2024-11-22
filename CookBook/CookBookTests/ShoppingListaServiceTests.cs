using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Services;
using System;
using System.Collections.Generic;

namespace CookBookTests
{
    [TestClass]
    public class ShoppingListServiceTest
    {
        private ShoppingListService? _shoppingListService;
        private FakeIngredientService? _ingredientService;

        [TestInitialize]
        public void TestInitialize()
        {
            _ingredientService = new FakeIngredientService();
            _shoppingListService = new ShoppingListService(_ingredientService);

            // Adding sample ingredients
            _ingredientService.AddIngredient(new Ingredient { Id = 1, Naziv = "Mlijeko", Cijena = 1.5M });
            _ingredientService.AddIngredient(new Ingredient { Id = 2, Naziv = "Hljeb", Cijena = 0.8M });
            _ingredientService.AddIngredient(new Ingredient { Id = 3, Naziv = "Jaja", Cijena = 0.2M });
        }

        [TestMethod]
        public void DodajNaListu_AddsIngredient_WhenNotAlreadyInList()
        {
            _shoppingListService?.DodajNaListu(1, 2.0);

            var lista = _shoppingListService?.GetLista();
            Assert.IsNotNull(lista);
            Assert.AreEqual(1, lista.Count);
            Assert.AreEqual(2.0, lista[key: _ingredientService.GetIngredientById(1)]);
        }

        [TestMethod]
        public void DodajNaListu_IncreasesQuantity_WhenIngredientAlreadyExists()
        {
            _shoppingListService?.DodajNaListu(1, 1.0);
            _shoppingListService?.DodajNaListu(1, 1.5);

            var lista = _shoppingListService?.GetLista();
            Assert.AreEqual(1, lista?.Count);
            Assert.AreEqual(2.5, lista?[_ingredientService.GetIngredientById(1)]);
        }

        [TestMethod]
        public void GetLista_ReturnsEmpty_WhenNoIngredientsAdded()
        {
            var lista = _shoppingListService?.GetLista();

            Assert.IsNotNull(lista);
            Assert.AreEqual(0, lista.Count);
        }

        [TestMethod]
        public void IzracunajUkupnuCijenu_ReturnsZero_WhenListIsEmpty()
        {
            var ukupnaCijena = _shoppingListService?.IzracunajUkupnuCijenu();

            Assert.AreEqual(0.0M, ukupnaCijena);
        }

        [TestMethod]
        public void IzracunajUkupnuCijenu_CalculatesCorrectTotal_WhenIngredientsAdded()
        {
            _shoppingListService?.DodajNaListu(1, 2.0); // 2 * 1.5 = 3.0
            _shoppingListService?.DodajNaListu(2, 3.0); // 3 * 0.8 = 2.4

            var ukupnaCijena = _shoppingListService?.IzracunajUkupnuCijenu();

            Assert.AreEqual(5.4M, ukupnaCijena);
        }

        [TestMethod]
        public void GetLista_ReturnsCorrectItems_WhenIngredientsAdded()
        {
            _shoppingListService?.DodajNaListu(1, 2.0);
            _shoppingListService?.DodajNaListu(2, 1.0);

            var lista = _shoppingListService?.GetLista();

            Assert.AreEqual(2, lista?.Count);
            Assert.AreEqual(2.0, lista?[_ingredientService.GetIngredientById(1)]);
            Assert.AreEqual(1.0, lista?[_ingredientService.GetIngredientById(2)]);
        }

        // Data-driven tests for better coverage
        [DataTestMethod]
        [DataRow(1, 1.0, 1.0)] 
        [DataRow(1, 2.5, 2.5)] 
        [DataRow(2, 3.0, 3.0)] 
        public void DodajNaListu_AddsCorrectQuantity(int ingredientId, double quantity, double expectedQuantity)
        {
            
            _shoppingListService?.DodajNaListu(ingredientId, quantity);

            var lista = _shoppingListService?.GetLista();
            var ingredient = _ingredientService?.GetIngredientById(ingredientId);

            Assert.IsNotNull(ingredient, "Sastojak ne treba da postoji u listi.");
            Assert.AreEqual(expectedQuantity, lista?[ingredient], $"Očekivana količina {expectedQuantity} nije ista kao dobijena.");
        }

        [DataTestMethod]
        [DataRow(1, 2.0, -1.0, 1.0)] 
        [DataRow(2, 3.5, -3.5, 0.0)] 
        public void DodajNaListu_HandlesQuantityChanges(int ingredientId, double addQuantity, double subtractQuantity, double expectedQuantity)
        {
            _shoppingListService?.DodajNaListu(ingredientId, addQuantity);
            _shoppingListService?.DodajNaListu(ingredientId, subtractQuantity);

            // Assert
            var lista = _shoppingListService?.GetLista();
            var ingredient = _ingredientService?.GetIngredientById(ingredientId);

            if (expectedQuantity == 0.0)
            {
                Assert.IsFalse(lista?.ContainsKey(ingredient), "Sastojak treba ukloniti sa liste.");
            }
            else
            {
                Assert.AreEqual(expectedQuantity, lista?[ingredient], $"Očekivana količina {expectedQuantity} nije ista kao dobijena.");
            }
        }

        [DataTestMethod]
        [DataRow(1, 2.0, 1.5, 3.0)]
        [DataRow(2, 3.0, 0.8, 2.4)] 
        [DataRow(1, 0.0, 1.5, 0.0)] 
        public void IzracunajUkupnuCijenu_CalculatesCorrectly(int ingredientId, double quantity, double price, double expectedTotal)
        {
            var expectedPrice = (decimal)price;        
            var expectedTotalPrice = (decimal)expectedTotal; 
            _shoppingListService?.DodajNaListu(ingredientId, quantity);

            var totalPrice = _shoppingListService?.IzracunajUkupnuCijenu();

            Assert.AreEqual(expectedTotalPrice, totalPrice, $"Očekivana cijena{expectedTotalPrice:C} nije ista kao dobijena.");
        }

    }

    public class FakeIngredientService : IIngredientService
    {
        private readonly Dictionary<int, Ingredient> _ingredients = new Dictionary<int, Ingredient>();

        public void AddIngredient(Ingredient ingredient)
        {
            _ingredients[ingredient.Id] = ingredient;
        }

        public Ingredient? GetIngredientById(int id)
        {
            return _ingredients.ContainsKey(id) ? _ingredients[id] : null;
        }

        public List<Ingredient> GetAllIngredients()
        {
            return new List<Ingredient>(_ingredients.Values);
        }

        public void DodajIngredient(Ingredient sastojak, bool preserveId = false)
        {
            throw new NotImplementedException();
        }

        public Ingredient GetIngredientByName(string naziv)
        {
            throw new NotImplementedException();
        }

        public int GetNextId()
        {
            throw new NotImplementedException();
        }
    }
}