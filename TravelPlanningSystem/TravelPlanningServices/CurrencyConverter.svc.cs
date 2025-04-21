using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace TravelPlanningServices
{
    public class CurrencyConverter : ICurrencyConverter
    {
        // dictionary to store exchange rates (currency code to USD)
        private Dictionary<string, decimal> _exchangeRates;

        // Get absolute path to the rates file in App_Data folder
        private string RatesFilePath
        {
            get
            {
                string appDataPath = HttpContext.Current.Server.MapPath("~/App_Data");
                // Create the directory if it doesn't exist
                if (!Directory.Exists(appDataPath))
                {
                    Directory.CreateDirectory(appDataPath);
                }
                return Path.Combine(appDataPath, "ExchangeRates.txt");
            }
        }

        public CurrencyConverter()
        {
            // initialize exchange rates
            LoadExchangeRates();
        }

        public decimal ConvertCurrency(decimal amount, string fromCurrency, string toCurrency)
        {
            try
            {
                // validate input
                if (amount < 0)
                {
                    throw new ArgumentException("Amount must be non-negative");
                }

                fromCurrency = fromCurrency.ToUpper();
                toCurrency = toCurrency.ToUpper();

                // check if currencies are supported
                if (!_exchangeRates.ContainsKey(fromCurrency) || !_exchangeRates.ContainsKey(toCurrency))
                {
                    throw new ArgumentException("One or both currencies are not supported");
                }


                //convert from source currency to USD
                //convert from USD to target currency
                decimal amountInUsd = amount / _exchangeRates[fromCurrency];
                decimal result = amountInUsd * _exchangeRates[toCurrency];

                // round to 2 decimal places
                return Math.Round(result, 2);
            }
            catch (Exception ex)
            {
                // log error
                LogError(ex);
                throw;
            }
        }

        public string[] GetAvailableCurrencies()
        {
            return _exchangeRates.Keys.ToArray();
        }

        private void LoadExchangeRates()
        {
            _exchangeRates = new Dictionary<string, decimal>();

            // check if rates file exists, if not create it with default values
            if (!File.Exists(RatesFilePath))
            {
                CreateDefaultRatesFile();
            }

            // read rates from file
            string[] lines = File.ReadAllLines(RatesFilePath);

            foreach (string line in lines)
            {
                // skip empty lines
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                // parse currency code and rate
                string[] parts = line.Split(':');
                if (parts.Length == 2)
                {
                    string currencyCode = parts[0].Trim();

                    // try to parse rate
                    if (decimal.TryParse(parts[1].Trim(), out decimal rate))
                    {
                        _exchangeRates[currencyCode] = rate;
                    }
                }
            }
        }

        private void CreateDefaultRatesFile()
        {
            // create default exchange rates (relative to 1 USD)
            string[] defaultRates = new string[]
            {
                "USD:1.00",    // US Dollar
                "EUR:0.85",    // Euro
                "GBP:0.75",    // British Pound
                "JPY:110.28",  // Japanese Yen
                "CAD:1.25",    // Canadian Dollar
                "AUD:1.35",    // Australian Dollar
                "CHF:0.92",    // Swiss Franc
                "CNY:6.45",    // Chinese Yuan
                "INR:74.50",   // Indian Rupee
                "MXN:20.15"    // Mexican Peso
            };

            // write default rates to file
            File.WriteAllLines(RatesFilePath, defaultRates);
        }

        private void LogError(Exception ex)
        {
            // simple error logging
            try
            {
                string logPath = Path.Combine(HttpContext.Current.Server.MapPath("~/App_Data"), "error_log.txt");
                File.AppendAllText(logPath, $"{DateTime.Now}: {ex.Message}\n{ex.StackTrace}\n\n");
            }
            catch
            {
                // ignore errors when logging
            }
        }
    }
}