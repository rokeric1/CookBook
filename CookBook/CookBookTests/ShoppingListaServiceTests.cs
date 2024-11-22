using Models;
using Services;

namespace CookBookTests
{
    [TestClass]
    public class ShoppingListServiceTest
    {
        private ShoppingListService _shoppingListService;
        private FakeIngredientService _ingredientService;

        [TestInitialize]
        public void TestInitialize()
        {
            _ingredientService = new FakeIngredientService();
            _shoppingListService = new ShoppingListService(_ingredientService);

            _ingredientService.AddIngredient(new Ingredient { Id = 1, Naziv = "Mlijeko", Cijena = 1.5M });
            _ingredientService.AddIngredient(new Ingredient { Id = 2, Naziv = "Hljeb", Cijena = 0.8M });
        }

        [TestMethod]
        public void DodajNaListu_AddsIngredient_WhenNotAlreadyInList()
        {
            _shoppingListService.DodajNaListu(1, 2.0);

            var lista = _shoppingListService.GetLista();
            Assert.IsNotNull(lista);
            Assert.AreEqual(1, lista.Count);
            Assert.AreEqual(2.0, lista[_ingredientService.GetIngredientById(1)]);
        }

        [TestMethod]
        public void DodajNaListu_IncreasesQuantity_WhenIngredientAlreadyExists()
        {
            _shoppingListService.DodajNaListu(1, 1.0);
            _shoppingListService.DodajNaListu(1, 1.5);

            var lista = _shoppingListService.GetLista();
            Assert.AreEqual(1, lista.Count);
            Assert.AreEqual(2.5, lista[_ingredientService.GetIngredientById(1)]);
        }

        [TestMethod]
        public void GetLista_ReturnsEmpty_WhenNoIngredientsAdded()
        {
            var lista = _shoppingListService.GetLista();

            Assert.IsNotNull(lista);
            Assert.AreEqual(0, lista.Count);
        }

        [TestMethod]
        public void IzracunajUkupnuCijenu_ReturnsZero_WhenListIsEmpty()
        {
            var ukupnaCijena = _shoppingListService.IzracunajUkupnuCijenu();

            Assert.AreEqual(0.0M, ukupnaCijena);
        }

        [TestMethod]
        public void IzracunajUkupnuCijenu_CalculatesCorrectTotal_WhenIngredientsAdded()
        {
            _shoppingListService.DodajNaListu(1, 2.0); // 2 * 1.5 = 3.0
            _shoppingListService.DodajNaListu(2, 3.0); // 3 * 0.8 = 2.4

            var ukupnaCijena = _shoppingListService.IzracunajUkupnuCijenu();

            Assert.AreEqual(5.4M, ukupnaCijena);
        }

        [TestMethod]
        public void GetLista_ReturnsCorrectItems_WhenIngredientsAdded()
        {
            _shoppingListService.DodajNaListu(1, 2.0);
            _shoppingListService.DodajNaListu(2, 1.0);

            var lista = _shoppingListService.GetLista();

            Assert.AreEqual(2, lista.Count);
            Assert.AreEqual(2.0, lista[_ingredientService.GetIngredientById(1)]);
            Assert.AreEqual(1.0, lista[_ingredientService.GetIngredientById(2)]);
        }
    }

    public class FakeIngredientService : IIngredientService
    {
        private readonly Dictionary<int, Ingredient> _ingredients = new Dictionary<int, Ingredient>();

        public void AddIngredient(Ingredient ingredient)
        {
            _ingredients[ingredient.Id] = ingredient;
        }

        public Ingredient GetIngredientById(int id)
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
