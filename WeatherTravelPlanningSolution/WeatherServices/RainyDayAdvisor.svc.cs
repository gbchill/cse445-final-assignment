using System;
using System.Collections.Generic;

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

            // Check if it's rainy
            if (IsItRainy(location, date))
            {
                advice.Add("It's going to be rainy! Here are some suggestions:");
                advice.AddRange(GetIndoorActivities(location));
                advice.Add("Remember to take an umbrella or raincoat.");
                advice.Add("Consider waterproof shoes or boots.");
            }
            else
            {
                advice.Add("Good news! It's not expected to rain on this day.");
                advice.Add("Perfect day for outdoor activities!");
                advice.Add("Consider visiting local parks or hiking trails.");
                advice.Add("Don't forget sunscreen if it's sunny.");
            }

            return advice.ToArray();
        }

        public string[] GetIndoorActivities(string location)
        {
            // Simulate location-based indoor activities
            List<string> activities = new List<string>();

            // Generic activities available in most locations
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

            // Add some location-specific suggestions (simulated)
            if (location.ToLower().Contains("phoenix") || location.ToLower().Contains("arizona"))
            {
                activities.Add("Visit the Musical Instrument Museum");
                activities.Add("Explore the Arizona Science Center");
                activities.Add("Check out the Phoenix Art Museum");
            }
            else if (location.ToLower().Contains("new york"))
            {
                activities.Add("Visit the Metropolitan Museum of Art");
                activities.Add("Explore the American Museum of Natural History");
                activities.Add("See a Broadway show");
            }

            return activities.ToArray();
        }

        public bool IsItRainy(string location, DateTime date)
        {
            // For this assignment, we'll simulate rain predictions
            // In a real implementation, you would check a weather service

            Random random = new Random(location.GetHashCode() + date.DayOfYear);

            // Simulate a 30% chance of rain
            return random.Next(100) < 30;
        }
    }
}