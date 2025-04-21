using System;
using System.IO;
using System.Net;
using System.Xml;

namespace TravelPlanningServices
{
    public class WeatherService : IWeatherService
    {
        public string GetWeatherForecast(string zipCode)
        {
            try
            {
                // validate zip code input (basic validation)
                if (string.IsNullOrEmpty(zipCode) || zipCode.Length != 5)
                {
                    return "Error: Please provide a valid 5-digit US zip code.";
                }

                // call the national weather service to get lat/long for zip code
                string latLongXml = GetLatLongForZipCode(zipCode);

                // parse the lat/long from the response
                string latLong = ParseLatLong(latLongXml);

                if (string.IsNullOrEmpty(latLong))
                {
                    return "Error: Could not determine location for the provided zip code.";
                }

                // split the latLong string into separate lat and long values
                string[] coordinates = latLong.Split(',');
                string latitude = coordinates[0];
                string longitude = coordinates[1];

                // get the weather forecast using the lat/long
                string forecast = GetForecastForLatLong(latitude, longitude);

                return forecast;
            }
            catch (Exception ex)
            {
                // log the error (in a real app you'd use a proper logging framework)
                File.AppendAllText("error_log.txt", $"{DateTime.Now}: {ex.Message}\n{ex.StackTrace}\n\n");

                // return a user-friendly error message
                return "Sorry, an error occurred while retrieving the weather forecast. Please try again later.";
            }
        }

        private string GetLatLongForZipCode(string zipCode)
        {
            // this endpoint converts a zip code to lat/long
            string url = $"http://graphical.weather.gov/xml/SOAP_server/ndfdXMLclient.php?listZipCodeList={zipCode}";

            WebClient client = new WebClient();
            return client.DownloadString(url);
        }

        private string ParseLatLong(string xml)
        {
            try
            {
                // parse the XML response to extract lat/long
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                // get the latLonList element content
                XmlNode latLonNode = doc.SelectSingleNode("//latLonList");

                if (latLonNode != null)
                {
                    return latLonNode.InnerText;
                }

                return string.Empty;
            }
            catch
            {
                // if parsing fails, return empty string
                return string.Empty;
            }
        }

        private string GetForecastForLatLong(string latitude, string longitude)
        {
   
            // replace this with actual API call in a real implementation
            return $"5-Day Forecast for coordinates {latitude}, {longitude}:\n"
                + "Day 1: Sunny, High: 75°F, Low: 60°F\n"
                + "Day 2: Partly Cloudy, High: 72°F, Low: 58°F\n"
                + "Day 3: Rainy, High: 65°F, Low: 55°F\n"
                + "Day 4: Cloudy, High: 68°F, Low: 56°F\n"
                + "Day 5: Sunny, High: 78°F, Low: 62°F";
        }
    }
}