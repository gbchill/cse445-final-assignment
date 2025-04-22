using System;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using System.Collections.Generic;

namespace WeatherServices
{
    public class WeatherForecast : IWeatherForecast
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

                // First convert zip code to coordinates using free API
                var coordinates = GetCoordinatesFromZipCode(zipCode);
                if (coordinates == null)
                {
                    return "Error: Could not determine location for the provided zip code.";
                }

                // Get the forecast data
                var forecast = GetForecastFromNWS(coordinates.Latitude, coordinates.Longitude, zipCode);
                return forecast;
            }
            catch (Exception ex)
            {
                return $"Sorry, an error occurred: {ex.Message}";
            }
        }

        private Coordinates GetCoordinatesFromZipCode(string zipCode)
        {
            try
            {
                // Use Zippopotam.us API for free ZIP code lookup
                string apiUrl = $"https://api.zippopotam.us/us/{zipCode}";

                using (WebClient client = new WebClient())
                {
                    client.Headers.Add("User-Agent", "TravelPlanningServices/1.0");
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
            catch (Exception)
            {
                // If Zippopotam fails, try OpenDataSoft API as backup
                try
                {
                    string odApiUrl = $"https://public.opendatasoft.com/api/records/1.0/search/?dataset=us-zip-code-latitude-and-longitude&q={zipCode}&facet=state&facet=timezone&facet=dst";

                    using (WebClient client = new WebClient())
                    {
                        client.Headers.Add("User-Agent", "TravelPlanningServices/1.0");
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
                catch (Exception)
                {
                    return null;
                }

                return null;
            }
        }

        private string GetForecastFromNWS(double latitude, double longitude, string zipCode)
        {
            try
            {
                // Using HttpWebRequest for more control over headers
                string pointsUrl = $"https://api.weather.gov/points/{latitude},{longitude}";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(pointsUrl);
                request.Method = "GET";
                request.UserAgent = "TravelPlanningServices/1.0 (student@example.com)";
                request.Accept = "application/json";
                request.Headers.Add("Accept-Language", "en-US");
                request.Timeout = 10000; // 10 second timeout

                string pointsResponse = "";
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        pointsResponse = reader.ReadToEnd();
                    }
                }

                // Parse the JSON using JavaScriptSerializer
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                dynamic pointsData = serializer.Deserialize<dynamic>(pointsResponse);

                // Extract forecast URL from the response
                string forecastUrl = pointsData["properties"]["forecast"];

                // Now get the actual forecast
                HttpWebRequest forecastRequest = (HttpWebRequest)WebRequest.Create(forecastUrl);
                forecastRequest.Method = "GET";
                forecastRequest.UserAgent = "TravelPlanningServices/1.0 (student@example.com)";
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

                dynamic forecastData = serializer.Deserialize<dynamic>(forecastResponse);

                // Parse and format the forecast with better formatting
                var periods = forecastData["properties"]["periods"];
                var coordinates = GetCoordinatesFromZipCode(zipCode);

                string forecastResult = "";

                // Add location header
                if (coordinates != null && !string.IsNullOrEmpty(coordinates.City))
                {
                    forecastResult += $"7-Day Weather Forecast for {coordinates.City}, {coordinates.State} ({zipCode})\n";
                }
                else
                {
                    forecastResult += $"7-Day Weather Forecast for ZIP Code {zipCode}\n";
                }

                forecastResult += $"Coordinates: {latitude:F4}°, {longitude:F4}°\n";
                forecastResult += new string('=', 60) + "\n\n";

                foreach (var period in periods)
                {
                    string name = period["name"].ToString();
                    int temperature = Convert.ToInt32(period["temperature"]);
                    string temperatureUnit = period["temperatureUnit"].ToString();
                    string shortForecast = period["shortForecast"].ToString();
                    string windSpeed = period["windSpeed"].ToString();
                    string windDirection = period["windDirection"].ToString();

                    forecastResult += $"{name.ToUpper()}\n";
                    forecastResult += new string('-', 40) + "\n";
                    forecastResult += $"Temperature: {temperature}°{temperatureUnit}\n";
                    forecastResult += $"Conditions:  {shortForecast}\n";
                    forecastResult += $"Wind:        {windSpeed} {windDirection}\n\n";
                }

                return forecastResult;
            }
            catch (WebException ex)
            {
                if (ex.Response is HttpWebResponse response)
                {
                    if (response.StatusCode == HttpStatusCode.Forbidden)
                    {
                        return "Error: Access forbidden (403). This might be due to IP restrictions or User-Agent issues.";
                    }
                    return $"Error: Web service returned status code {response.StatusCode}";
                }
                return $"Error getting forecast: {ex.Message}";
            }
            catch (Exception ex)
            {
                return $"Error getting forecast: {ex.Message}";
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