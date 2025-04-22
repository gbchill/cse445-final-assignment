using System;
using System.Web.UI;

namespace WeatherTravelPlanning.UserControls
{
    public partial class WeatherDisplay : System.Web.UI.UserControl
    {

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
          
                SetWeatherData(
                    location: "Phoenix, AZ",
                    currentTemp: 75.5,
                    highTemp: 82.0,
                    lowTemp: 68.3,
                    conditions: "Partly Cloudy",
                    rainChance: 10.0,
                    windSpeed: 5.5,
                    humidity: 45.0
                );
            }
        }

        public void UpdateDisplay()
        {
            lblLocation.Text = string.IsNullOrEmpty(Location) ? "Weather" : $"Weather for {Location}";
            lblTemperature.Text = $"{CurrentTemperature:0.0}°F";
            lblConditions.Text = Conditions;
            lblHighLow.Text = $"{HighTemperature:0.0}°/{LowTemperature:0.0}°";
            lblRainChance.Text = $"{RainChance:0.0}%";
            lblWind.Text = $"{WindSpeed:0.0} mph";
            lblHumidity.Text = $"{Humidity:0.0}%";
        }

        //update the weather data
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