using System;
using System.ServiceModel;
using System.Net;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TravelPlanningServicesTryIt
{
    public partial class TryIt : System.Web.UI.Page
    {

        private string WeatherServiceUrl => "http://localhost:52780/WeatherService.svc";
        private string CurrencyServiceUrl => "http://localhost:52780/CurrencyConverter.svc";
        private string WordFilterServiceUrl => "http://localhost:52780/WordFilter.svc";

       

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // set the service URLs in the UI
                lblWeatherServiceUrl.Text = WeatherServiceUrl;
                lblCurrencyServiceUrl.Text = CurrencyServiceUrl;
                lblWordFilterServiceUrl.Text = WordFilterServiceUrl;

                // initialize with some sample text for testing
                if (string.IsNullOrEmpty(txtInputText.Text))
                {
                    txtInputText.Text = "The quick brown fox jumps over the lazy dog. The fox was very quick and the dog was extremely lazy.";
                }
            }
        }

        protected void btnGetWeather_Click(object sender, EventArgs e)
        {
            try
            {
                // get the zip code from the input field
                string zipCode = txtZipCode.Text.Trim();

                // validate zip code
                if (string.IsNullOrEmpty(zipCode) || zipCode.Length != 5)
                {
                    lblWeatherResult.Text = "Please enter a valid 5-digit US zip code.";
                    return;
                }

                // create a service client
                WeatherServiceReference.WeatherServiceClient client =
                    new WeatherServiceReference.WeatherServiceClient();

                // call the service
                string forecast = client.GetWeatherForecast(zipCode);

                // display the result
                lblWeatherResult.Text = forecast;

                // close the client
                client.Close();
            }
            catch (Exception ex)
            {
                // display error message
                lblWeatherResult.Text = "Error: " + ex.Message;
            }
        }

        protected void btnConvertCurrency_Click(object sender, EventArgs e)
        {
            try
            {
                // get values from the form
                string amountText = txtAmount.Text.Trim();
                string fromCurrency = ddlFromCurrency.SelectedValue;
                string toCurrency = ddlToCurrency.SelectedValue;

                // validate amount
                if (!decimal.TryParse(amountText, out decimal amount))
                {
                    lblCurrencyResult.Text = "Please enter a valid amount.";
                    return;
                }

                // use the service reference client
                CurrencyServiceReference.CurrencyConverterClient client =
                    new CurrencyServiceReference.CurrencyConverterClient();

                // call the service
                decimal result = client.ConvertCurrency(amount, fromCurrency, toCurrency);

                // display the result
                lblCurrencyResult.Text = $"{amount} {fromCurrency} = {result} {toCurrency}";

                // close the client
                client.Close();
            }
            catch (Exception ex)
            {
                // display error message
                lblCurrencyResult.Text = "Error: " + ex.Message;
            }
        }

        protected void btnFilterStopWords_Click(object sender, EventArgs e)
        {
            try
            {
                // get input text
                string inputText = txtInputText.Text;

                // validate input
                if (string.IsNullOrWhiteSpace(inputText))
                {
                    lblWordFilterResult.Text = "Please enter some text to filter.";
                    return;
                }

                // create service client
                WordFilterServiceReference.WordFilterClient client =
                    new WordFilterServiceReference.WordFilterClient();

                // call the service
                string filteredText = client.FilterStopWords(inputText);

                // display the result
                lblWordFilterResult.Text = "Filtered Text: " + filteredText;

                // close the client
                client.Close();
            }
            catch (Exception ex)
            {
                // display error message
                lblWordFilterResult.Text = "Error: " + ex.Message;
            }
        }

        protected void btnGetTopWords_Click(object sender, EventArgs e)
        {
            try
            {
                // get input text and count
                string inputText = txtInputText.Text;
                string countText = txtTopWordCount.Text.Trim();

                // validate input
                if (string.IsNullOrWhiteSpace(inputText))
                {
                    lblWordFilterResult.Text = "Please enter some text to analyze.";
                    return;
                }

                if (!int.TryParse(countText, out int count) || count <= 0)
                {
                    lblWordFilterResult.Text = "Please enter a valid positive number for top word count.";
                    return;
                }

                // create service client
                WordFilterServiceReference.WordFilterClient client =
                    new WordFilterServiceReference.WordFilterClient();

                // call the service
                string[] topWords = client.GetTopContentWords(inputText, count);

                // display the result
                if (topWords.Length > 0)
                {
                    // build a formatted result with word numbers
                    StringBuilder sb = new StringBuilder("Top " + count + " words:\n");
                    for (int i = 0; i < topWords.Length; i++)
                    {
                        sb.AppendLine($"{i + 1}. {topWords[i]}");
                    }
                    lblWordFilterResult.Text = sb.ToString();
                }
                else
                {
                    lblWordFilterResult.Text = "No content words found.";
                }

                // close the client
                client.Close();
            }
            catch (Exception ex)
            {
                // display error message
                lblWordFilterResult.Text = "Error: " + ex.Message;
            }
        }

        protected void btnCountWords_Click(object sender, EventArgs e)
        {
            try
            {
                // get input text
                string inputText = txtInputText.Text;

                // validate input
                if (string.IsNullOrWhiteSpace(inputText))
                {
                    lblWordFilterResult.Text = "Please enter some text to count.";
                    return;
                }

                // create service client
                WordFilterServiceReference.WordFilterClient client =
                    new WordFilterServiceReference.WordFilterClient();

                // call the service
                int wordCount = client.CountContentWords(inputText);

                // display the result
                lblWordFilterResult.Text = "Content word count: " + wordCount;

                // close the client
                client.Close();
            }
            catch (Exception ex)
            {
                // display error message
                lblWordFilterResult.Text = "Error: " + ex.Message;
            }
        }
    }
}