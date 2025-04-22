using System;

namespace WeatherServices
{
    public class TemperatureConverter : ITemperatureConverter
    {
        public double ConvertTemperature(double temperature, string fromUnit, string toUnit)
        {
            if (string.IsNullOrEmpty(fromUnit) || string.IsNullOrEmpty(toUnit))
            {
                throw new ArgumentException("Both fromUnit and toUnit must be specified.");
            }

            // Convert to uppercase for case-insensitive comparison
            fromUnit = fromUnit.ToUpper();
            toUnit = toUnit.ToUpper();

            // If units are the same, return original temperature
            if (fromUnit == toUnit)
            {
                return temperature;
            }

            // Convert Fahrenheit to Celsius
            if (fromUnit == "F" && toUnit == "C")
            {
                return FahrenheitToCelsius(temperature);
            }
            // Convert Celsius to Fahrenheit
            else if (fromUnit == "C" && toUnit == "F")
            {
                return CelsiusToFahrenheit(temperature);
            }
            else
            {
                throw new ArgumentException("Invalid temperature units. Use 'F' for Fahrenheit or 'C' for Celsius.");
            }
        }

        public double FahrenheitToCelsius(double fahrenheit)
        {
            // Formula: (°F - 32) × 5/9 = °C
            return Math.Round((fahrenheit - 32) * 5 / 9, 2);
        }

        public double CelsiusToFahrenheit(double celsius)
        {
            // Formula: (°C × 9/5) + 32 = °F
            return Math.Round((celsius * 9 / 5) + 32, 2);
        }
    }
}