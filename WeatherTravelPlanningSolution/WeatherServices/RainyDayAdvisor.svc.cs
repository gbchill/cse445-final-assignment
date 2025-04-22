using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;

namespace WeatherServices
{
    public class RainyDayAdvisor : IRainyDayAdvisor
    {
        public string[] GetRainyDayAdvice(string location, DateTime date)
        {
            if (string.IsNullOrEmpty(location))
            {
                throw new ArgumentException("Location cannot be empty.");
            }

            List<string> advice = new List<string>();

            //check if it's rainy
            bool isRainy = IsItRainy(location, date);

            if (isRainy)
            {
                advice.Add("It's going to be rainy! Here are some suggestions:");
                advice.AddRange(GetIndoorActivities(location));
                advice.Add("Remember to take an umbrella or raincoat.");
                advice.Add("Consider waterproof shoes or boots.");
                advice.Add("Expect wet conditions - drive carefully if on the road.");
            }
            else
            {
                advice.Add("Good news! It's not expected to rain on this day.");
                advice.Add("Perfect day for outdoor activities!");
                advice.Add("Consider visiting local parks or hiking trails.");
                advice.Add("Don't forget sunscreen if it's sunny.");
                advice.Add("Great weather for picnics or outdoor sports.");
            }

            return advice.ToArray();
        }

        public string[] GetIndoorActivities(string location)
        {
            List<string> activities = new List<string>();

            //generic activities
            activities.Add("Visit a local museum or art gallery");
            activities.Add("Go to a shopping mall");
            activities.Add("Watch a movie at the cinema");
            activities.Add("Try a new restaurant or café");
            activities.Add("Visit an indoor sports facility");
            activities.Add("Go bowling");
            activities.Add("Visit a library");
            activities.Add("Attend a cooking class");
            activities.Add("Try indoor rock climbing");
            activities.Add("Visit a spa or wellness center");

            //city and state from location
            var coordinates = GetCoordinatesFromZipCode(location);

            if (coordinates != null)
            {
                //location specific suggestions based on actual city
                if (coordinates.City.ToLower().Contains("phoenix") || coordinates.State.ToLower() == "az")
                {
                    activities.Add("Visit the Musical Instrument Museum");
                    activities.Add("Explore the Arizona Science Center");
                    activities.Add("Check out the Phoenix Art Museum");
                }
                else if (coordinates.City.ToLower().Contains("new york") || coordinates.State.ToLower() == "ny")
                {
                    activities.Add("Visit the Metropolitan Museum of Art");
                    activities.Add("Explore the American Museum of Natural History");
                    activities.Add("See a Broadway show");
                }
                else if (coordinates.City.ToLower().Contains("chicago") || coordinates.State.ToLower() == "il")
                {
                    activities.Add("Visit the Art Institute of Chicago");
                    activities.Add("Explore the Museum of Science and Industry");
                    activities.Add("Check out the Field Museum");
                }
                else if (coordinates.City.ToLower().Contains("los angeles") || coordinates.State.ToLower() == "ca")
                {
                    activities.Add("Visit the Getty Center");
                    activities.Add("Explore the California Science Center");
                    activities.Add("Check out the Griffith Observatory");
                }
            }

            return activities.ToArray();
        }

        public bool IsItRainy(string location, DateTime date)
        {
            try
            {
                var coordinates = GetCoordinatesFromZipCode(location);
                if (coordinates == null)
                {
                    throw new Exception("Unable to determine location from zip code.");
                }

                //next 7 day
                if (date.Date > DateTime.Now.Date.AddDays(7))
                {
                    throw new Exception("Weather forecast is only available for the next 7 days.");
                }

          
                var (isRaining, probability) = CheckRainInForecast(coordinates.Latitude, coordinates.Longitude, date);
                return isRaining;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking weather: {ex.Message}");
            }
        }

        private (bool IsRaining, int Probability) CheckRainInForecast(double latitude, double longitude, DateTime date)
        {
            try
            {
                string pointsUrl = $"https://api.weather.gov/points/{latitude},{longitude}";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(pointsUrl);
                request.Method = "GET";
                request.UserAgent = "TravelPlanningServices/1.0 (student@example.com)";
                request.Accept = "application/json";
                request.Headers.Add("Accept-Language", "en-US");
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
                string forecastUrl = pointsData["properties"]["forecast"];

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
                var periods = forecastData["properties"]["periods"];

                //remoced maybe addi n future
                foreach (var period in periods)
                {
                    DateTime periodStartTime = DateTime.Parse(period["startTime"].ToString());
                    DateTime periodEndTime = DateTime.Parse(period["endTime"].ToString());

                    if (date.Date >= periodStartTime.Date && date.Date <= periodEndTime.Date)
                    {
                        string detailedForecast = period["detailedForecast"].ToString().ToLower();
                        string shortForecast = period["shortForecast"].ToString().ToLower();
                        int probabilityOfPrecipitation = 0;

                        // Check if precipitation probability is provided
                        if (period["probabilityOfPrecipitation"] != null &&
                            period["probabilityOfPrecipitation"]["value"] != null)
                        {
                            probabilityOfPrecipitation = Convert.ToInt32(period["probabilityOfPrecipitation"]["value"]);
                        }

                        // Check for rain indicators in the forecast
                        bool isRaining = detailedForecast.Contains("rain") ||
                                        detailedForecast.Contains("shower") ||
                                        detailedForecast.Contains("storm") ||
                                        detailedForecast.Contains("precipitation") ||
                                        shortForecast.Contains("rain") ||
                                        shortForecast.Contains("shower") ||
                                        shortForecast.Contains("storm") ||
                                        probabilityOfPrecipitation > 30;

                        return (isRaining, probabilityOfPrecipitation);
                    }
                }


                throw new Exception("No weather forecast available for the specified date.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching weather data: {ex.Message}");
            }
        }

        private Coordinates GetCoordinatesFromZipCode(string zipCode)
        {
            try
            {
                //use Zippopotam.us weather pai
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

                throw new Exception("No data found for the provided zip code.");
            }
            catch (Exception ex)
            {
                //OpenDataSoft API as backup if fials
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

                    throw new Exception("No data found for the provided zip code in backup API.");
                }
                catch (Exception backupEx)
                {
                    throw new Exception($"Error looking up coordinates for zip code: {ex.Message}. Backup API also failed: {backupEx.Message}");
                }
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