using System;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace TravelPlanningServices
{
    [ServiceContract]
    public interface ICurrencyConverter
    {
        [OperationContract]
        [WebGet(UriTemplate = "Convert?amount={amount}&from={fromCurrency}&to={toCurrency}",
                ResponseFormat = WebMessageFormat.Json)]
        decimal ConvertCurrency(decimal amount, string fromCurrency, string toCurrency);

        [OperationContract]
        [WebGet(UriTemplate = "GetAvailableCurrencies",
                ResponseFormat = WebMessageFormat.Json)]
        string[] GetAvailableCurrencies();
    }
}