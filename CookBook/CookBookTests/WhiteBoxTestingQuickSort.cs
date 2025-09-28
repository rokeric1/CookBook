using Models;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBookTests
{
    internal class WhiteBoxTestingQuickSort
    {
        [TestClass]
        public class BadQuickSortTests
        {
            [TestMethod]
            public void BadQuickSort_EmptyList()
            {
                
                var recipes = new List<Recipe>();

                
                ReceptSort.BadQuickSort(recipes);

                
                Assert.AreEqual(0, recipes.Count);
            }
        }

            [TestMethod]
            public void BadQuickSort_SingleItem()
            {
                
                var recipes = new List<Recipe>
        {
            new Recipe { Kategorija = "Dessert" }
        };

                
               ReceptSort.BadQuickSort(recipes);

                
                Assert.AreEqual(1, recipes.Count);
                Assert.AreEqual("Dessert", recipes[0].Kategorija);

            }
        [TestMethod]
        public void BadQuickSort_SmallList()
        {
            
            var recipes = new List<Recipe>
    {
        new Recipe { Kategorija = "Dessert" },
        new Recipe { Kategorija = "Appetizer" },
        new Recipe { Kategorija = "Main Course" }
    };

            
            ReceptSort.BadQuickSort(recipes);

            
            Assert.AreEqual("Dessert", recipes[0].Kategorija); 
        }

        [TestMethod]
        public void BadQuickSort_MediumList()
        {
            
            var recipes = new List<Recipe>
    {
        new Recipe { Kategorija = "Dessert" },
        new Recipe { Kategorija = "Appetizer" },
        new Recipe { Kategorija = "Soup" },
        new Recipe { Kategorija = "Salad" },
        new Recipe { Kategorija = "Beverage" },
        new Recipe { Kategorija = "Main Course" }
    };

            
            ReceptSort.BadQuickSort(recipes);

            
            var expectedOrder = new[] { "Appetizer", "Beverage", "Dessert", "Main Course", "Salad", "Soup" };
            for (int i = 0; i < recipes.Count; i++)
            {
                Assert.AreEqual(expectedOrder[i], recipes[i].Kategorija);
            }
        }

        [TestMethod]
        public void BadQuickSort_CaseInsensitive()
        {
            
            var recipes = new List<Recipe>
    {
        new Recipe { Kategorija = "dessert" },
        new Recipe { Kategorija = "Appetizer" },
        new Recipe { Kategorija = "Soup" },
        new Recipe { Kategorija = "Salad" },
        new Recipe { Kategorija = "beverage" }
    };

            
            ReceptSort.BadQuickSort(recipes);

            
            var expectedOrder = new[] { "Appetizer", "beverage", "dessert", "Salad", "Soup" };
            for (int i = 0; i < recipes.Count; i++)
            {
                Assert.AreEqual(expectedOrder[i], recipes[i].Kategorija);
            }
        }

        [TestMethod]
        public void BadQuickSort_ListWithSwaps()
        {
            
            var recipes = new List<Recipe>
    {
        new Recipe { Kategorija = "Soup" },
        new Recipe { Kategorija = "Salad" },
        new Recipe { Kategorija = "Dessert" },
        new Recipe { Kategorija = "Appetizer" },
        new Recipe { Kategorija = "Main Course" }
    };

            
            ReceptSort.BadQuickSort(recipes);

            
            var expectedOrder = new[] { "Appetizer", "Dessert", "Main Course", "Salad", "Soup" };
            for (int i = 0; i < recipes.Count; i++)
            {
                Assert.AreEqual(expectedOrder[i], recipes[i].Kategorija);
            }
        }

        [TestMethod]
        public void BadQuickSort_DuplicateCategories()
        {
            
            var recipes = new List<Recipe>
    {
        new Recipe { Kategorija = "Soup" },
        new Recipe { Kategorija = "Soup" },
        new Recipe { Kategorija = "Salad" },
        new Recipe { Kategorija = "Dessert" },
        new Recipe { Kategorija = "Appetizer" },
        new Recipe { Kategorija = "Soup" }
    };

            
            ReceptSort.BadQuickSort(recipes);

            
            var expectedOrder = new[] { "Appetizer", "Dessert", "Salad", "Soup", "Soup", "Soup" };
            for (int i = 0; i < recipes.Count; i++)
            {
                Assert.AreEqual(expectedOrder[i], recipes[i].Kategorija);
            }
        }
    }
}
