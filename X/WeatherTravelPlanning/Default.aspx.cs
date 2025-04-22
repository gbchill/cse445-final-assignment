using System;
using System.Web.UI;

namespace WeatherTravelPlanning
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // show the “default” TryIt view
                mvTryIt.ActiveViewIndex = 0;
            }
        }

        // “Try It” buttons switch the MultiView
        protected void btnTryForecast_Click(object sender, EventArgs e) => mvTryIt.ActiveViewIndex = 1;
        protected void btnTryConverter_Click(object sender, EventArgs e) => mvTryIt.ActiveViewIndex = 2;
        protected void btnTryRainy_Click(object sender, EventArgs e) => mvTryIt.ActiveViewIndex = 3;
        protected void btnTryEncryption_Click(object sender, EventArgs e) => mvTryIt.ActiveViewIndex = 4;
        protected void btnTryControl_Click(object sender, EventArgs e) => mvTryIt.ActiveViewIndex = 5;
        protected void btnTryCookie_Click(object sender, EventArgs e) => mvTryIt.ActiveViewIndex = 6;

        // Inside each view, the action buttons need handlers too:
        protected void btnGetForecast_Click(object sender, EventArgs e)
        {
            // TODO: call WeatherForecast WCF, bind to lblForecastResult
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
