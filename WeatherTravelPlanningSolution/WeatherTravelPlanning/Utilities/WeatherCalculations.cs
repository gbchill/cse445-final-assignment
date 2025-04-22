using System;

namespace WeatherTravelPlanning.Utilities
{
    public class WeatherCalculations
    {
        // Temperature conversion methods
        public double CelsiusToFahrenheit(double celsius)
        {
            return (celsius * 9 / 5) + 32;
        }

        public double FahrenheitToCelsius(double fahrenheit)
        {
            return (fahrenheit - 32) * 5 / 9;
        }

        // Wind speed conversions
        public double MphToKmh(double mph)
        {
            return mph * 1.60934;
        }

        public double KmhToMph(double kmh)
        {
            return kmh / 1.60934;
        }

        // Calculate heat index (feels‑like temperature)
        public double CalculateHeatIndex(double temperatureF, double relativeHumidity)
        {
            if (temperatureF < 80)
                return temperatureF;  // Only applies when T ≥ 80°F

            double HI = -42.379
                      + 2.04901523 * temperatureF
                      + 10.14333127 * relativeHumidity
                      - 0.22475541 * temperatureF * relativeHumidity
                      - 0.00683783 * temperatureF * temperatureF
                      - 0.05481717 * relativeHumidity * relativeHumidity
                      + 0.00122874 * temperatureF * temperatureF * relativeHumidity
                      + 0.00085282 * temperatureF * relativeHumidity * relativeHumidity
                      - 0.00000199 * temperatureF * temperatureF * relativeHumidity * relativeHumidity;

            return Math.Round(HI, 1);
        }

        // Calculate wind chill
        public double CalculateWindChill(double temperatureF, double windSpeedMph)
        {
            if (temperatureF > 50 || windSpeedMph < 3)
                return temperatureF;  // Only applies when T ≤ 50°F and wind ≥ 3 mph

            double windChill = 35.74
                             + 0.6215 * temperatureF
                             - 35.75 * Math.Pow(windSpeedMph, 0.16)
                             + 0.4275 * temperatureF * Math.Pow(windSpeedMph, 0.16);

            return Math.Round(windChill, 1);
        }

        // Calculate dew point
        public double CalculateDewPoint(double temperatureC, double relativeHumidity)
        {
            const double a = 17.27;
            const double b = 237.7;

            double alpha = (a * temperatureC) / (b + temperatureC)
                         + Math.Log(relativeHumidity / 100.0);
            double dewPoint = (b * alpha) / (a - alpha);

            return Math.Round(dewPoint, 1);
        }

        // Pressure conversions
        public double InHgToMbar(double inHg)
        {
            return inHg * 33.8639;
        }

        public double MbarToInHg(double mbar)
        {
            return mbar / 33.8639;
        }
    }
}
