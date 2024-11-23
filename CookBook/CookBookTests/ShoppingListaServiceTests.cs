using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Moq;
using Services;
using System;
using System.Collections.Generic;

namespace CookBookTests
{
    [TestClass]
    public class ShoppingListServiceTests
    {
        private Mock<IIngredientService> mockIngredientService;
        private ShoppingListService shoppingListService;

        [TestInitialize]
        public void Setup()
        {
            mockIngredientService = new Mock<IIngredientService>();
            shoppingListService = new ShoppingListService(mockIngredientService.Object);
        }

        [TestMethod]
        public void DodajNaListu_NewIngredient_ShouldAddToDictionary()
        {
            // Arrange
            int ingredientId = 1;
            double kolicina = 2.5;

            var ingredient = new Ingredient { Id = ingredientId, Naziv = "Jabuka", Cijena = 3.0M };
            mockIngredientService.Setup(s => s.GetIngredientById(ingredientId)).Returns(ingredient);

            // Act
            shoppingListService.DodajNaListu(ingredientId, kolicina);

            // Assert
            var lista = shoppingListService.GetLista();
            Assert.AreEqual(1, lista.Count);
            Assert.IsTrue(lista.ContainsKey(ingredient));
            Assert.AreEqual(kolicina, lista[ingredient]);
        }

        [TestMethod]
        public void DodajNaListu_ExistingIngredient_ShouldUpdateQuantity()
        {
            // Arrange
            int ingredientId = 1;
            double initialKolicina = 2.5;
            double additionalKolicina = 1.5;

            var ingredient = new Ingredient { Id = ingredientId, Naziv = "Jabuka", Cijena = 3.0M };
            mockIngredientService.Setup(s => s.GetIngredientById(ingredientId)).Returns(ingredient);

            shoppingListService.DodajNaListu(ingredientId, initialKolicina);

            // Act
            shoppingListService.DodajNaListu(ingredientId, additionalKolicina);

            // Assert
            var lista = shoppingListService.GetLista();
            Assert.AreEqual(1, lista.Count);
            Assert.AreEqual(initialKolicina + additionalKolicina, lista[ingredient]);
        }

        [TestMethod]
        public void IzracunajUkupnuCijenu_WithValidIngredients_ShouldReturnCorrectSum()
        {
            // Arrange
            var ingredient1 = new Ingredient { Id = 1, Naziv = "Jabuka", Cijena = 3.0M };
            var ingredient2 = new Ingredient { Id = 2, Naziv = "Mlijeko", Cijena = 1.5M };

            mockIngredientService.Setup(s => s.GetIngredientById(1)).Returns(ingredient1);
            mockIngredientService.Setup(s => s.GetIngredientById(2)).Returns(ingredient2);

            shoppingListService.DodajNaListu(1, 2); 
            shoppingListService.DodajNaListu(2, 3); 

            // Act
            decimal ukupnaCijena = shoppingListService.IzracunajUkupnuCijenu();

            // Assert
            Assert.AreEqual(10.5M, ukupnaCijena);
        }

        [TestMethod]
        public void GetLista_ShouldReturnIngredientListWithQuantities()
        {
            // Arrange
            var ingredient = new Ingredient { Id = 1, Naziv = "Jabuka", Cijena = 3.0M };
            mockIngredientService.Setup(s => s.GetIngredientById(1)).Returns(ingredient);

            shoppingListService.DodajNaListu(1, 2);

            // Act
            var lista = shoppingListService.GetLista();

            // Assert
            Assert.AreEqual(1, lista.Count);
            Assert.AreEqual(ingredient, lista.Keys.First());
            Assert.AreEqual(2, lista.Values.First());
        }

        [TestMethod]
        public void ToString_ShouldReturnFormattedString()
        {
            // Arrange
            var ingredient = new Ingredient
            {
                Id = 1,
                Naziv = "Jabuka",
                Cijena = 3.0M,
                Nutrijenti = "Vitamini A, B, C",
                EkoUtjecaj = 0.75,
                Dostupan = true
            };
            mockIngredientService.Setup(s => s.GetIngredientById(1)).Returns(ingredient);

            shoppingListService.DodajNaListu(1, 2);

            // Act
            string result = shoppingListService.ToString();

            // Assert
            StringAssert.Contains(result, "Jabuka");
            StringAssert.Contains(result, "Količina: 2");
            StringAssert.Contains(result, "Ukupno: £6.00");
        }

        [TestMethod]
        public void IzracunajUkupnuCijenu_WithNullIngredient_ShouldSkipAndReturnCorrectSum()
        {
            // Arrange
            var validIngredient = new Ingredient { Id = 1, Naziv = "Jabuka", Cijena = 3.0M };

            mockIngredientService.Setup(s => s.GetIngredientById(1)).Returns(validIngredient);
            mockIngredientService.Setup(s => s.GetIngredientById(2)).Returns((Ingredient)null); 

            shoppingListService.DodajNaListu(1, 2); 
            shoppingListService.DodajNaListu(2, 3); 

            // Act
            decimal ukupnaCijena = shoppingListService.IzracunajUkupnuCijenu();

            // Assert
            Assert.AreEqual(6.0M, ukupnaCijena); 
        }

        [TestMethod]
        public void ToString_WithNullIngredient_ShouldHandleGracefully()
        {
            // Arrange
            var validIngredient = new Ingredient
            {
                Id = 1,
                Naziv = "Jabuka",
                Cijena = 3.0M,
                Nutrijenti = "Vitamini A, B, C",
                EkoUtjecaj = 0.75,
                Dostupan = true
            };

            mockIngredientService.Setup(s => s.GetIngredientById(1)).Returns(validIngredient);
            mockIngredientService.Setup(s => s.GetIngredientById(2)).Returns((Ingredient)null); 

            shoppingListService.DodajNaListu(1, 2); 
            shoppingListService.DodajNaListu(2, 3); 

            // Act
            string result = shoppingListService.ToString();

            // Assert
            StringAssert.Contains(result, "Jabuka");
            StringAssert.Contains(result, "Količina: 2");
            StringAssert.Contains(result, "Ukupno: £6.00");
            StringAssert.Contains(result, "Id: 2 nije pronađen."); 
        }

        [TestMethod]
        public void ToString_WithUnavailableIngredient_ShouldDisplayCorrectMessage()
        {
            // Arrange
            var ingredient = new Ingredient
            {
                Id = 1,
                Naziv = "Mlijeko",
                Cijena = 4.0M,
                Nutrijenti = "Kalij",
                EkoUtjecaj = 1.25,
                Dostupan = false 
            };

            mockIngredientService.Setup(s => s.GetIngredientById(1)).Returns(ingredient);

            shoppingListService.DodajNaListu(1, 1);

            // Act
            string result = shoppingListService.ToString();

            // Assert
            StringAssert.Contains(result, "Mlijeko");
            StringAssert.Contains(result, "Dostupnost: Nije Dostupan"); 
        }

        [TestMethod]
        public void ToString_WithMultipleIngredients_ShouldHandleMixedAvailability()
        {
            // Arrange
            var ingredient1 = new Ingredient
            {
                Id = 1,
                Naziv = "Kruh",
                Cijena = 2.5M,
                Nutrijenti = "Vlakna",
                EkoUtjecaj = 0.5,
                Dostupan = true 
            };

            var ingredient2 = new Ingredient
            {
                Id = 2,
                Naziv = "Jogurt",
                Cijena = 1.8M,
                Nutrijenti = "Probiotici",
                EkoUtjecaj = 0.9,
                Dostupan = false 
            };

            mockIngredientService.Setup(s => s.GetIngredientById(1)).Returns(ingredient1);
            mockIngredientService.Setup(s => s.GetIngredientById(2)).Returns(ingredient2);

            shoppingListService.DodajNaListu(1, 2); 
            shoppingListService.DodajNaListu(2, 1); 

            // Act
            string result = shoppingListService.ToString();

            // Assert
            StringAssert.Contains(result, "Kruh");
            StringAssert.Contains(result, "Dostupnost: Dostupan");
            StringAssert.Contains(result, "Jogurt");
            StringAssert.Contains(result, "Dostupnost: Nije Dostupan");
        }

        [TestMethod]
        public void ToString_WithMultipleNullIngredients_ShouldHandleGracefully()
        {
            // Arrange
            mockIngredientService.Setup(s => s.GetIngredientById(It.IsAny<int>())).Returns((Ingredient)null);

            shoppingListService.DodajNaListu(1, 1); 
            shoppingListService.DodajNaListu(2, 2); 

            // Act
            string result = shoppingListService.ToString();

            // Assert
            StringAssert.Contains(result, "Id: 1 nije pronađen.");
            StringAssert.Contains(result, "Id: 2 nije pronađen.");
        }




    }
}