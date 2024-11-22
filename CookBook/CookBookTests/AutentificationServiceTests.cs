using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services;

namespace CookBookTests
{
    [TestClass]
    public class AuthenticationServiceTests
    {
        private AuthenticationService _authService;

        [TestInitialize]
        public void Setup()
        {
            _authService = new AuthenticationService();
        }

        [TestMethod]
        public void RegistrujKorisnika_NoviKorisnik_RegistracijaUspjesna()
        {
            string korisnickoIme = "testKorisnik";
            string lozinka = "testLozinka";

            bool rezultat = _authService.RegistrujKorisnika(korisnickoIme, lozinka);

            Assert.IsTrue(rezultat, "Korisnik nije uspješno registrovan.");
        }

        [TestMethod]
        public void RegistrujKorisnika_PostojeceKorisnickoIme_RegistracijaNeuspjesna()
        {

            string korisnickoIme = "testKorisnik";
            string lozinka = "testLozinka";
            _authService.RegistrujKorisnika(korisnickoIme, lozinka);

            bool rezultat = _authService.RegistrujKorisnika(korisnickoIme, lozinka);

            Assert.IsFalse(rezultat, "Korisnik je registrovan sa postojećim korisničkim imenom.");
        }

        [TestMethod]
        public void AutentifikujKorisnika_ValidniPodaci_AutentifikacijaUspjesna()
        {
            string korisnickoIme = "testKorisnik";
            string lozinka = "testLozinka";
            _authService.RegistrujKorisnika(korisnickoIme, lozinka);

            bool rezultat = _authService.AutentifikujKorisnika(korisnickoIme, lozinka);

            Assert.IsTrue(rezultat, "Autentifikacija nije uspjela za validne podatke.");
        }

        [TestMethod]
        public void AutentifikujKorisnika_NevalidniPodaci_AutentifikacijaNeuspjesna()
        {
            string korisnickoIme = "testKorisnik";
            string lozinka = "testLozinka";
            string pogresnaLozinka = "pogresnaLozinka";
            _authService.RegistrujKorisnika(korisnickoIme, lozinka);

            bool rezultat = _authService.AutentifikujKorisnika(korisnickoIme, pogresnaLozinka);

            Assert.IsFalse(rezultat, "Autentifikacija je uspjela sa pogrešnim podacima.");
        }


        [TestMethod]
        public void AutentifikujKorisnika_KorisnikNePostoji_AutentifikacijaNeuspjesna()
        {
            // Arrange
            string korisnickoIme = "nepostojeciKorisnik";
            string lozinka = "nekaLozinka";

            // Act
            bool rezultat = _authService.AutentifikujKorisnika(korisnickoIme, lozinka);

            // Assert
            Assert.IsFalse(rezultat, "Autentifikacija je uspela za nepostojećeg korisnika.");
        }


        [TestMethod]
        public void HashLozinka_ReflectionTest()
        {
            Type type = typeof(AuthenticationService); 
            object authServiceInstance = Activator.CreateInstance(type); 
            MethodInfo metoda = type.GetMethod("HashLozinka", BindingFlags.NonPublic | BindingFlags.Instance); 

            string lozinka = "testLozinka"; 
            object[] parametri = { lozinka }; 

            string hash = (string)metoda.Invoke(authServiceInstance, parametri); 

            Assert.IsNotNull(hash, "Hash nije generisan.");
            Assert.AreNotEqual(lozinka, hash, "Hash vrednost ne sme biti ista kao lozinka.");
        }
    }
}
