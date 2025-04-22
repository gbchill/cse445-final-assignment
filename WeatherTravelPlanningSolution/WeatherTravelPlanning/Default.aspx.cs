using System;
using System.Web.UI;
using System.ServiceModel;

namespace WeatherTravelPlanning
{
    // Define a simple class for temperature data to avoid serialization issues
    [Serializable]
    public class SimpleTemperatureData
    {
        public string Location { get; set; }
        public double TemperatureFahrenheit { get; set; }
        public double TemperatureCelsius { get; set; }
        public DateTime RetrievedAt { get; set; }
    }

    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // show the "default" TryIt view
                mvTryIt.ActiveViewIndex = 0;
            }
        }

        // "Try It" buttons switch the MultiView
        protected void btnTryForecast_Click(object sender, EventArgs e) => mvTryIt.ActiveViewIndex = 1;
        protected void btnTryConverter_Click(object sender, EventArgs e) => mvTryIt.ActiveViewIndex = 2;
        protected void btnTryRainy_Click(object sender, EventArgs e) => mvTryIt.ActiveViewIndex = 3;
        protected void btnTryEncryption_Click(object sender, EventArgs e) => mvTryIt.ActiveViewIndex = 4;
        protected void btnTryControl_Click(object sender, EventArgs e) => mvTryIt.ActiveViewIndex = 5;
        protected void btnTryCookie_Click(object sender, EventArgs e) => mvTryIt.ActiveViewIndex = 6;

        protected void btnGetForecast_Click(object sender, EventArgs e)
        {
            try
            {
                string zipCode = txtZipCode.Text.Trim();

                if (string.IsNullOrEmpty(zipCode) || zipCode.Length != 5)
                {
                    lblForecastResult.Text = "Please enter a valid 5-digit US zip code.";
                    return;
                }

                WeatherServiceReference.WeatherForecastClient client =
                    new WeatherServiceReference.WeatherForecastClient("BasicHttpBinding_IWeatherForecast");

                string forecast = client.GetWeatherForecast(zipCode);
                lblForecastResult.Text = forecast.Replace("\n", "<br/>");

                client.Close();
            }
            catch (Exception ex)
            {
                lblForecastResult.Text = "Error: " + ex.Message;
            }
        }

        protected void btnConvert_Click(object sender, EventArgs e)
        {
            try
            {
                string zipCode = txtConverterZipCode.Text.Trim();

                if (string.IsNullOrEmpty(zipCode) || zipCode.Length != 5)
                {
                    lblConverterResult.Text = "Please enter a valid 5-digit US zip code.";
                    return;
                }

                // Use the TemperatureConverterClient with existing methods
                TemperatureServiceReference.TemperatureConverterClient client =
                    new TemperatureServiceReference.TemperatureConverterClient("BasicHttpsBinding_ITemperatureConverter");

                // For now, simulate temperature data as the service doesn't expose GetTemperatureByZipCode
                double tempFahrenheit = 75.0; // Simulated temperature
                double tempCelsius = client.FahrenheitToCelsius(tempFahrenheit);

                // Create a simple data object that can be serialized
                SimpleTemperatureData tempData = new SimpleTemperatureData
                {
                    Location = zipCode,
                    TemperatureFahrenheit = tempFahrenheit,
                    TemperatureCelsius = tempCelsius,
                    RetrievedAt = DateTime.Now
                };

                // Display results
                string result = $"<strong>Location:</strong> {tempData.Location}<br/>";
                result += $"<strong>Temperature:</strong> {tempData.TemperatureFahrenheit}°F / {tempData.TemperatureCelsius}°C<br/>";
                result += $"<strong>Retrieved at:</strong> {tempData.RetrievedAt:g}<br/>";

                // Store the serializable object in ViewState
                ViewState["TempData"] = tempData;
                ViewState["DisplayInF"] = true;

                // Show toggle button
                btnToggleTemp.Visible = true;
                lblConverterResult.Text = result;

                client.Close();
            }
            catch (Exception ex)
            {
                lblConverterResult.Text = "Error: " + ex.Message;
                btnToggleTemp.Visible = false;
            }
        }

        protected void btnToggleTemp_Click(object sender, EventArgs e)
        {
            if (ViewState["TempData"] != null)
            {
                SimpleTemperatureData tempData = (SimpleTemperatureData)ViewState["TempData"];
                bool displayInF = (bool)ViewState["DisplayInF"];

                string result = $"<strong>Location:</strong> {tempData.Location}<br/>";

                if (displayInF)
                {
                    result += $"<strong>Temperature:</strong> {tempData.TemperatureCelsius}°C ({tempData.TemperatureFahrenheit}°F)<br/>";
                    ViewState["DisplayInF"] = false;
                }
                else
                {
                    result += $"<strong>Temperature:</strong> {tempData.TemperatureFahrenheit}°F ({tempData.TemperatureCelsius}°C)<br/>";
                    ViewState["DisplayInF"] = true;
                }

                result += $"<strong>Retrieved at:</strong> {tempData.RetrievedAt:g}<br/>";
                lblConverterResult.Text = result;
            }
        }

        protected void btnGetAdvice_Click(object sender, EventArgs e)
        {
            try
            {
                string location = txtRainLocation.Text.Trim();
                DateTime selectedDate;

                if (string.IsNullOrEmpty(location) || location.Length != 5)
                {
                    lblRainyResult.Text = "Please enter a valid 5-digit US zip code.";
                    return;
                }

                if (!DateTime.TryParse(txtRainDate.Text, out selectedDate))
                {
                    lblRainyResult.Text = "Please enter a valid date.";
                    return;
                }

                RainyDayServiceReference.RainyDayAdvisorClient client =
                    new RainyDayServiceReference.RainyDayAdvisorClient("BasicHttpsBinding_IRainyDayAdvisor");

                bool isRainy = client.IsItRainy(location, selectedDate);
                string[] advice = client.GetRainyDayAdvice(location, selectedDate);

                string result = string.Format("Weather forecast for {0} on {1}:<br/>",
                    location, selectedDate.ToShortDateString());
                result += isRainy ? "<strong>It is expected to be rainy!</strong><br/><br/>"
                                  : "<strong>It is not expected to rain.</strong><br/><br/>";

                foreach (string suggestion in advice)
                {
                    result += "• " + suggestion + "<br/>";
                }

                lblRainyResult.Text = result;
                client.Close();
            }
            catch (Exception ex)
            {
                lblRainyResult.Text = "Error: " + ex.Message;
            }
        }

        protected void btnEncrypt_Click(object sender, EventArgs e)
        {
            // TODO: encrypt txtPlainText.Text, display in lblEncrypted
        }

        protected void btnDecrypt_Click(object sender, EventArgs e)
        {
            // TODO: decrypt lblEncrypted.Text, display in lblDecrypted
        }

        protected void btnSavePreferences_Click(object sender, EventArgs e)
        {
            // TODO: write a cookie using ddlTempUnit.SelectedValue & txtPrefLocation.Text
        }

        protected void btnLoadPreferences_Click(object sender, EventArgs e)
        {
            // TODO: read your WeatherPreferences cookie, show results in lblCookieResult
        }
    }
}