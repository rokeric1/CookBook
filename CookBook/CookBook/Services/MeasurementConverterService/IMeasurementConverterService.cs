using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IMeasurementConverterService
    {
        double CupsToMilliliters(double cups);
        double MillilitersToCups(double milliliters);
        double TeaspoonsToMilliliters(double teaspoons);
        double MillilitersToTeaspoons(double milliliters);
        double OuncesToGrams(double ounces);
        double GramsToOunces(double grams);
        double PoundsToKilograms(double pounds);
        double KilogramsToPounds(double kilograms);
        double FahrenheitToCelsius(double fahrenheit);
    }
}
