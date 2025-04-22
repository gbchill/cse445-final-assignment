using System;
using System.Web.UI;
using System.ServiceModel;
using System.Web;
using WeatherTravelPlanning.Utilities;


namespace WeatherTravelPlanning
{
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
                mvTryIt.ActiveViewIndex = 0;
        }

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
                string zip = txtZipCode.Text.Trim();
                if (zip.Length != 5)
                {
                    lblForecastResult.Text = "Please enter a valid 5‑digit ZIP.";
                    return;
                }

                var client = new WeatherServiceReference.WeatherForecastClient("BasicHttpBinding_IWeatherForecast");
                lblForecastResult.Text = client.GetWeatherForecast(zip).Replace("\n", "<br/>");
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
                string zip = txtConverterZipCode.Text.Trim();
                if (zip.Length != 5)
                {
                    lblConverterResult.Text = "Please enter a valid 5‑digit ZIP.";
                    return;
                }

                var client = new TemperatureServiceReference.TemperatureConverterClient("BasicHttpsBinding_ITemperatureConverter");

                //use the new  method
                var data = client.GetTemperatureByZipCode(zip);

                var simpleData = new SimpleTemperatureData
                {
                    Location = data.Location,
                    TemperatureFahrenheit = data.TemperatureFahrenheit,
                    TemperatureCelsius = data.TemperatureCelsius,
                    RetrievedAt = data.RetrievedAt
                };

                string html =
                    $"<strong>Location:</strong> {simpleData.Location}<br/>" +
                    $"<strong>Temperature:</strong> {simpleData.TemperatureFahrenheit:0}°F / {simpleData.TemperatureCelsius:0.0}°C<br/>" +
                    $"<strong>Retrieved at:</strong> {simpleData.RetrievedAt:g}<br/>";

                ViewState["TempData"] = simpleData;
                ViewState["DisplayInF"] = true;
                btnToggleTemp.Visible = true;
                lblConverterResult.Text = html;

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
            if (ViewState["TempData"] is SimpleTemperatureData data)
            {
                bool inF = (bool)ViewState["DisplayInF"];
                string html = $"<strong>Location:</strong> {data.Location}<br/>";

                if (inF)
                {
                    html += $"<strong>Temperature:</strong> {data.TemperatureCelsius:0.0}°C ({data.TemperatureFahrenheit:0}°F)<br/>";
                    ViewState["DisplayInF"] = false;
                }
                else
                {
                    html += $"<strong>Temperature:</strong> {data.TemperatureFahrenheit:0}°F ({data.TemperatureCelsius:0.0}°C)<br/>";
                    ViewState["DisplayInF"] = true;
                }

                html += $"<strong>Retrieved at:</strong> {data.RetrievedAt:g}<br/>";
                lblConverterResult.Text = html;
            }
        }

        protected void btnGetAdvice_Click(object sender, EventArgs e)
        {
            try
            {
                string zip = txtRainLocation.Text.Trim();
                if (zip.Length != 5)
                {
                    lblRainyResult.Text = "Please enter a valid 5‑digit ZIP.";
                    return;
                }

                //change to useuse today's date:
                DateTime today = DateTime.Now.Date;
                var client = new RainyDayServiceReference.RainyDayAdvisorClient("BasicHttpsBinding_IRainyDayAdvisor");
                bool rainy = client.IsItRainy(zip, today);
                string[] tips = client.GetRainyDayAdvice(zip, today);

                string html = rainy
                    ? "<strong>It's rainy today! Suggestions:</strong><br/>"
                    : "<strong>No rain expected today. Suggestions:</strong><br/>";

                foreach (var t in tips)
                    html += "• " + t + "<br/>";

                lblRainyResult.Text = html;
                client.Close();
            }
            catch (Exception ex)
            {
                lblRainyResult.Text = "Error: " + ex.Message;
            }
        }

        protected void btnEncrypt_Click(object sender, EventArgs e)
        {
            try
            {
                string txt = txtPlainText.Text;
                if (string.IsNullOrEmpty(txt))
                {
                    lblEncrypted.Text = "Enter text to encrypt.";
                    return;
                }

                var enc = new WeatherEncryption();
                lblEncrypted.Text = enc.Encrypt(txt);
                ViewState["EncryptedText"] = lblEncrypted.Text;
            }
            catch (Exception ex)
            {
                lblEncrypted.Text = "Error encrypting: " + ex.Message;
            }
        }

        protected void btnDecrypt_Click(object sender, EventArgs e)
        {
            try
            {
                string cipher = ViewState["EncryptedText"] as string;
                if (string.IsNullOrEmpty(cipher))
                {
                    lblDecrypted.Text = "No encrypted text to decrypt.";
                    return;
                }

                var enc = new WeatherEncryption();
                lblDecrypted.Text = enc.Decrypt(cipher);
            }
            catch (Exception ex)
            {
                lblDecrypted.Text = "Error decrypting: " + ex.Message;
            }
        }

        protected void btnSavePreferences_Click(object sender, EventArgs e)
        {
            string unit = ddlTempUnit.SelectedValue;
            string loc = txtPrefLocation.Text.Trim();
            if (string.IsNullOrEmpty(loc))
            {
                lblCookieResult.Text = "Enter a location.";
                return;
            }

            try
            {
                var ck = new HttpCookie("WeatherPreferences")
                {
                    ["TempUnit"] = unit,
                    ["Location"] = loc,
                    Expires = DateTime.Now.AddDays(30)
                };
                Response.Cookies.Add(ck);
                Session["TemperatureUnit"] = unit;
                Session["PreferredLocation"] = loc;
                lblCookieResult.Text = "Preferences saved!";
            }
            catch (Exception ex)
            {
                lblCookieResult.Text = "Error saving preferences: " + ex.Message;
            }
        }

        protected void btnLoadPreferences_Click(object sender, EventArgs e)
        {
            try
            {
                var ck = Request.Cookies["WeatherPreferences"];
                if (ck != null)
                {
                    ddlTempUnit.SelectedValue = ck["TempUnit"] ?? ddlTempUnit.SelectedValue;
                    txtPrefLocation.Text = ck["Location"];
                    lblCookieResult.Text = $"Loaded: {ddlTempUnit.SelectedValue} – {txtPrefLocation.Text}";
                }
                else
                {
                    lblCookieResult.Text = "No preferences found.";
                }
            }
            catch (Exception ex)
            {
                lblCookieResult.Text = "Error loading preferences: " + ex.Message;
            }
        }
        protected void btnDemoWeatherControl_Click(object sender, EventArgs e)
        {
            var control = (WeatherTravelPlanning.UserControls.WeatherDisplay)weatherDisplayDemo;
            control.SetWeatherData(
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
        protected void btnMember_Click(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("Login.aspx?ReturnUrl=Member.aspx");
            else
                Response.Redirect("Member.aspx");
        }

        protected void btnStaff_Click(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("Login.aspx?ReturnUrl=Staff.aspx");
            else
                Response.Redirect("Staff.aspx");
        }


    }
}