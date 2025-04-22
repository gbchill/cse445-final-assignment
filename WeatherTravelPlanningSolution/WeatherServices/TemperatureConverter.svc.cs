using System;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using System.Collections.Generic;

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

            fromUnit = fromUnit.ToUpper();
            toUnit = toUnit.ToUpper();


            if (fromUnit == toUnit)
            {
                return temperature;
            }

            // Convert f to c
            if (fromUnit == "F" && toUnit == "C")
            {
                return FahrenheitToCelsius(temperature);
            }
            //covner c to f
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
            //F - 32) × 5/9 = °C
            return Math.Round((fahrenheit - 32) * 5 / 9, 2);
        }

        public double CelsiusToFahrenheit(double celsius)
        {
            //C × 9/5) + 32 = °F
            return Math.Round((celsius * 9 / 5) + 32, 2);
        }

        public TemperatureData GetTemperatureByZipCode(string zipCode)
        {
            try
            {
                // Validate zip code
                if (string.IsNullOrEmpty(zipCode) || zipCode.Length != 5)
                {
                    throw new ArgumentException("Please provide a valid 5-digit US zip code.");
                }

                // First, get coordinates from zip code
                var coordinates = GetCoordinatesFromZipCode(zipCode);
                if (coordinates == null)
                {
                    throw new Exception("Could not determine location for the provided zip code.");
                }

                // Then get weather data
                var weatherData = GetCurrentWeatherFromNWS(coordinates.Latitude, coordinates.Longitude);

                // Extract temperature
                double tempFahrenheit = ExtractTemperature(weatherData);
                double tempCelsius = FahrenheitToCelsius(tempFahrenheit);

                return new TemperatureData
                {
                    ZipCode = zipCode,
                    TemperatureFahrenheit = tempFahrenheit,
                    TemperatureCelsius = tempCelsius,
                    Location = $"{coordinates.City}, {coordinates.State}",
                    RetrievedAt = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving temperature: {ex.Message}");
            }
        }

        private Coordinates GetCoordinatesFromZipCode(string zipCode)
        {
            try
            {
                // Zippopotam.us
                string apiUrl = $"https://api.zippopotam.us/us/{zipCode}";

                using (WebClient client = new WebClient())
                {
                    client.Headers.Add("User-Agent", "TemperatureConverter/1.0");
                    client.Headers.Add("Accept", "application/json");

                    string response = client.DownloadString(apiUrl);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    dynamic zipData = serializer.Deserialize<dynamic>(response);

                    if (zipData != null && zipData["places"] != null && zipData["places"].Length > 0)
                    {
                        var place = zipData["places"][0];
                        return new Coordinates
                        {
                            Latitude = Convert.ToDouble(place["latitude"]),
                            Longitude = Convert.ToDouble(place["longitude"]),
                            City = place["place name"].ToString(),
                            State = place["state"].ToString()
                        };
                    }
                }

                return null;
            }
            catch
            {
                //backup API 
                try
                {
                    string odApiUrl = $"https://public.opendatasoft.com/api/records/1.0/search/?dataset=us-zip-code-latitude-and-longitude&q={zipCode}&facet=state&facet=timezone&facet=dst";

                    using (WebClient client = new WebClient())
                    {
                        client.Headers.Add("User-Agent", "TemperatureConverter/1.0");
                        client.Headers.Add("Accept", "application/json");

                        string response = client.DownloadString(odApiUrl);

                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        dynamic odData = serializer.Deserialize<dynamic>(response);

                        if (odData != null && odData["records"] != null && odData["records"].Length > 0)
                        {
                            var record = odData["records"][0]["fields"];
                            return new Coordinates
                            {
                                Latitude = Convert.ToDouble(record["latitude"]),
                                Longitude = Convert.ToDouble(record["longitude"]),
                                City = record["city"].ToString(),
                                State = record["state"].ToString()
                            };
                        }
                    }
                }
                catch
                {
                    return null;
                }

                return null;
            }
        }

        private string GetCurrentWeatherFromNWS(double latitude, double longitude)
        {
            try
            {
               
                string pointsUrl = $"https://api.weather.gov/points/{latitude},{longitude}";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(pointsUrl);
                request.Method = "GET";
                request.UserAgent = "TemperatureConverter/1.0 (student@example.com)";
                request.Accept = "application/json";
                request.Timeout = 10000;

                string pointsResponse = "";
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        pointsResponse = reader.ReadToEnd();
                    }
                }

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                dynamic pointsData = serializer.Deserialize<dynamic>(pointsResponse);

                // Get the forecast URL
                string forecastUrl = pointsData["properties"]["forecast"];

                // Get the forecast data
                HttpWebRequest forecastRequest = (HttpWebRequest)WebRequest.Create(forecastUrl);
                forecastRequest.Method = "GET";
                forecastRequest.UserAgent = "TemperatureConverter/1.0 (student@example.com)";
                forecastRequest.Accept = "application/json";
                forecastRequest.Timeout = 10000;

                string forecastResponse = "";
                using (HttpWebResponse response = (HttpWebResponse)forecastRequest.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        forecastResponse = reader.ReadToEnd();
                    }
                }

                return forecastResponse;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting weather data: {ex.Message}");
            }
        }

        private double ExtractTemperature(string weatherData)
        {
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                dynamic forecastData = serializer.Deserialize<dynamic>(weatherData);

                // Get the first period (current conditions)
                var firstPeriod = forecastData["properties"]["periods"][0];
                int temperature = Convert.ToInt32(firstPeriod["temperature"]);

                return temperature;
            }
            catch
            {
                throw new Exception("Could not extract temperature from weather data.");
            }
        }

        private class Coordinates
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public string City { get; set; }
            public string State { get; set; }
        }
    }
}