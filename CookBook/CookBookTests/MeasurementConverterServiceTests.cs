using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBookTests
{
    namespace CookBook.Tests
    {
        [TestClass]
        public class MeasurementConverterServiceTests
        {
            private MeasurementConverterService converter;

            [TestInitialize]
            public void SetUp()
            {
                converter = new MeasurementConverterService();
            }

            [TestMethod]
            public void CupsToMilliliters_ShouldConvertCorrectly()
            {
                
                int cups = 2;
                double expected = 473.176;
                double result = converter.CupsToMilliliters(cups);

                Assert.AreEqual(expected, result, 0.001);
            }

            [TestMethod]
            public void MillilitersToCups_ShouldConvertCorrectly()
            {
                
                int milliliters = 500;
                double expected = 2.11338; 
                double result = converter.MillilitersToCups(milliliters);

                
                Assert.AreEqual(expected, result, 0.00001);
            }

            [TestMethod]
            public void TeaspoonsToMilliliters_ShouldConvertCorrectly()
            {
                
                int teaspoons = 3;
                double expected = 14.7868; 
                double result = converter.TeaspoonsToMilliliters(teaspoons);

                
                Assert.AreEqual(expected, result, 0.0001);
            }

            [TestMethod]
            public void MillilitersToTeaspoons_ShouldConvertCorrectly()
            {
                
                int milliliters = 10;
                double expected = 2.02884; 
                double result = converter.MillilitersToTeaspoons(milliliters);

                
                Assert.AreEqual(expected, result, 0.00001);
            }

            [TestMethod]
            public void OuncesToGrams_ShouldConvertCorrectly()
            {
                
                int ounces = 5;
                double expected = 141.7475; 
                double result = converter.OuncesToGrams(ounces);

                
                Assert.AreEqual(expected, result, 0.0001);
            }

            [TestMethod]
            public void GramsToOunces_ShouldConvertCorrectly()
            {
                
                int grams = 100;
                double expected = 3.5274; 
                double result = converter.GramsToOunces(grams);

                
                Assert.AreEqual(expected, result, 0.0001);
            }

            [TestMethod]
            public void PoundsToKilograms_ShouldConvertCorrectly()
            {
                
                int pounds = 10;
                double expected = 4.53592;
                double result = converter.PoundsToKilograms(pounds);

                
                Assert.AreEqual(expected, result, 0.00001);
            }

            [TestMethod]
            public void KilogramsToPounds_ShouldConvertCorrectly()
            {
                
                int kilograms = 5;
                double expected = 11.0231; 
                double result = converter.KilogramsToPounds(kilograms);

                
                Assert.AreEqual(expected, result, 0.0001);
            }

            [TestMethod]
            public void FahrenheitToCelsius_ShouldConvertCorrectly()
            {
                
                int fahrenheit = 98;
                double expected = 36.6667;
                double result = converter.FahrenheitToCelsius(fahrenheit);

                
                Assert.AreEqual(expected, result, 0.0001);
            }

            [TestMethod]
            [ExpectedException(typeof(Exception))]
            public void CupsToMilliliters_ShouldThrowException_ForNegativeInput()
            {
                
                converter.CupsToMilliliters(-1);
            }

            [TestMethod]
            [ExpectedException(typeof(Exception))]
            public void MillilitersToCups_ShouldThrowException_ForNegativeInput()
            {
                
                converter.MillilitersToCups(-1);
            }

            [TestMethod]
            [ExpectedException(typeof(Exception))]
            public void TeaspoonsToMilliliters_ShouldThrowException_ForNegativeInput()
            {
                converter.TeaspoonsToMilliliters(-1);
            }

            [TestMethod]
            [ExpectedException(typeof(Exception))]
            public void MillilitersToTeaspoons_ShouldThrowException_ForNegativeInput()
            {
                converter.MillilitersToTeaspoons(-1);
            }

            [TestMethod]
            [ExpectedException(typeof(Exception))]
            public void OuncesToGrams_ShouldThrowException_ForNegativeInput()
            {
                converter.OuncesToGrams(-1);
            }

            [TestMethod]
            [ExpectedException(typeof(Exception))]
            public void PoundsToKilograms_ShouldThrowException_ForNegativeInput()
            {
                converter.PoundsToKilograms(-1);
            }

            [TestMethod]
            [ExpectedException(typeof(Exception))]
            public void GramsToOunces_ShouldThrowException_ForNegativeInput()
            {
                converter.GramsToOunces(-1);
            }

            [TestMethod]
            [ExpectedException(typeof(Exception))]
            public void KilogramsToPounds_ShouldThrowException_ForNegativeInput()
            {
                converter.KilogramsToPounds(-1);
            }




        }
    }
}
