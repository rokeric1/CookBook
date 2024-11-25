using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class MeasurementConverterService : IMeasurementConverterService
    {
        public double CupsToMilliliters(double cups)
        {
            if (cups < 0)
                throw new Exception("Unesena količina nije moguća.");
            return cups * 236.588;
        }


        public double MillilitersToCups(double milliliters)
        {
            if (milliliters < 0)
                throw new Exception("Unesena količina nije moguća.");
            return milliliters / 236.588;
        }

         
        public double TeaspoonsToMilliliters(double teaspoons)
        {
            if (teaspoons < 0)
                throw new Exception("Unesena količina nije moguća.");
            return teaspoons * 4.92892;
        }


        public double MillilitersToTeaspoons(double milliliters)
        {
            if (milliliters < 0)
                throw new Exception("Unesena količina nije moguća.");
            return milliliters / 4.92892;
        }


        public double OuncesToGrams(double ounces)
        {
            if (ounces < 0)
                throw new Exception("Unesena količina nije moguća.");
            return ounces * 28.3495;
        }


        public double GramsToOunces(double grams)
        {
            if (grams < 0)
                throw new Exception("Unesena količina nije moguća.");
            return grams / 28.3495;
        }


        public double PoundsToKilograms(double pounds)
        {
            if (pounds < 0)
                throw new Exception("Unesena količina nije moguća.");
            return pounds * 0.453592;
        }


        public double KilogramsToPounds(double kilograms)
        {
            if (kilograms < 0)
                throw new Exception("Unesena količina nije moguća.");
            return kilograms / 0.453592;
        }


        public double FahrenheitToCelsius(double fahrenheit)
        {
            return (fahrenheit - 32) * 5 / 9;
        }
    }
}
