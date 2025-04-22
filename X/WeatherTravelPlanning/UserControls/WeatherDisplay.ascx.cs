using System;
using System.Web.UI;

namespace WeatherTravelPlanning.UserControls
{
    public partial class WeatherDisplay : System.Web.UI.UserControl
    {
        // Properties to set the weather data
        public string Location { get; set; }
        public double CurrentTemperature { get; set; }
        public double HighTemperature { get; set; }
        public double LowTemperature { get; set; }
        public string Conditions { get; set; }
        public double RainChance { get; set; }
        public double WindSpeed { get; set; }
        public double Humidity { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UpdateDisplay();
            }
        }

        public void UpdateDisplay()
        {
            lblLocation.Text = string.IsNullOrEmpty(Location) ? "Weather" : $"Weather for {Location}";
            lblTemperature.Text = $"{CurrentTemperature}°F";
            lblConditions.Text = Conditions;
            lblHighLow.Text = $"{HighTemperature}°/{LowTemperature}°";
            lblRainChance.Text = $"{RainChance}%";
            lblWind.Text = $"{WindSpeed} mph";
            lblHumidity.Text = $"{Humidity}%";
        }

        // Method to update the weather data
        public void SetWeatherData(string location, double currentTemp, double highTemp,
                                  double lowTemp, string conditions, double rainChance,
                                  double windSpeed, double humidity)
        {
            Location = location;
            CurrentTemperature = currentTemp;
            HighTemperature = highTemp;
            LowTemperature = lowTemp;
            Conditions = conditions;
            RainChance = rainChance;
            WindSpeed = windSpeed;
            Humidity = humidity;

            UpdateDisplay();
        }
    }
}