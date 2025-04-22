using System;
using System.Web.UI;
using System.ServiceModel;

namespace WeatherTravelPlanning
{
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

        // Inside each view, the action buttons need handlers too:
        protected void btnGetForecast_Click(object sender, EventArgs e)
        {
            try
            {
                // get the zip code from the input field
                string zipCode = txtZipCode.Text.Trim();

                // validate zip code
                if (string.IsNullOrEmpty(zipCode) || zipCode.Length != 5)
                {
                    lblForecastResult.Text = "Please enter a valid 5-digit US zip code.";
                    return;
                }

                // create a service client - you'll need to add a service reference to WeatherForecast service
                // Right-click References > Add Service Reference > set the URL to your WeatherForecast service endpoint
                WeatherServiceReference.WeatherForecastClient client =
                    new WeatherServiceReference.WeatherForecastClient();

                // call the service
                string forecast = client.GetWeatherForecast(zipCode);

                // display the result - replace newlines with <br/> for HTML display
                lblForecastResult.Text = forecast.Replace("\n", "<br/>");

                // close the client
                client.Close();
            }
            catch (Exception ex)
            {
                // display error message
                lblForecastResult.Text = "Error: " + ex.Message;
            }
        }

        protected void btnConvert_Click(object sender, EventArgs e)
        {
            // TODO: read txtTemperature & dropdowns, call converter service, set lblConverterResult.Text
        }

        protected void btnGetAdvice_Click(object sender, EventArgs e)
        {
            // TODO: call RainyDayAdvisor service, format suggestions into lblRainyResult.Text
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