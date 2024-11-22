using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services;
using Models;
using System.Collections.Generic;

namespace CookBookTests
{
    [TestClass]
    public class OcjenaServiceTests
    {
        private OcjenaService? _ocjenaService;

        [TestInitialize]
        public void SetUp()
        {
            _ocjenaService = new OcjenaService();
        }

        [TestMethod]
        public void DodajOcjenu_NovaOcjena_DodajePravilno()
        {
            int recipeId = 1;
            int ocjena = 5;

            _ocjenaService.DodajOcjenu(recipeId, ocjena);

            var sveOcjene = _ocjenaService.GetSveOcjene(recipeId);
            Assert.AreEqual(1, sveOcjene.Count);
            Assert.AreEqual(ocjena, sveOcjene[0]);
        }

        [TestMethod]
        public void DodajOcjenu_PostojeceOcjene_DodajeNovuOcjenu()
        {
            // Arrange
            int recipeId = 1;
            int prvaOcjena = 5;
            int drugaOcjena = 4;

            // Act
            _ocjenaService.DodajOcjenu(recipeId, prvaOcjena);
            _ocjenaService.DodajOcjenu(recipeId, drugaOcjena);

            // Assert
            var sveOcjene = _ocjenaService.GetSveOcjene(recipeId);
            Assert.AreEqual(2, sveOcjene.Count);
            Assert.AreEqual(prvaOcjena, sveOcjene[0]);
            Assert.AreEqual(drugaOcjena, sveOcjene[1]);
        }

        [TestMethod]
        public void GetProsjecnaOcjena_BezOcjena_VracaNula()
        {
            // Arrange
            int recipeId = 1;

            // Act
            var prosjecnaOcjena = _ocjenaService.GetProsjecnaOcjena(recipeId);

            // Assert
            Assert.AreEqual(0.0, prosjecnaOcjena);
        }

        [TestMethod]
        public void GetProsjecnaOcjena_SaOcjenama_VracaPravilnuVrijednost()
        {
            // Arrange
            int recipeId = 1;
            _ocjenaService.DodajOcjenu(recipeId, 5);
            _ocjenaService.DodajOcjenu(recipeId, 3);
            _ocjenaService.DodajOcjenu(recipeId, 4);

            // Act
            var prosjecnaOcjena = _ocjenaService.GetProsjecnaOcjena(recipeId);

            // Assert
            Assert.AreEqual(4.0, prosjecnaOcjena);
        }

        [TestMethod]
        public void GetSveOcjene_NemaOcjena_VracaPraznuListu()
        {
            // Arrange
            int recipeId = 1;

            // Act
            var sveOcjene = _ocjenaService.GetSveOcjene(recipeId);

            // Assert
            Assert.AreEqual(0, sveOcjene.Count);
        }

        [TestMethod]
        public void GetSveOcjene_SaOcjenama_VracaPravilneOcjene()
        {
            // Arrange
            int recipeId = 1;
            _ocjenaService.DodajOcjenu(recipeId, 5);
            _ocjenaService.DodajOcjenu(recipeId, 4);

            // Act
            var sveOcjene = _ocjenaService.GetSveOcjene(recipeId);

            // Assert
            Assert.AreEqual(2, sveOcjene.Count);
            Assert.AreEqual(5, sveOcjene[0]);
            Assert.AreEqual(4, sveOcjene[1]);
        }
    }
}
