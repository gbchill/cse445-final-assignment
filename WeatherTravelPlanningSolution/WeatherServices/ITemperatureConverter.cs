using System.ServiceModel;

namespace WeatherServices
{
    [ServiceContract]
    public interface ITemperatureConverter
    {
        [OperationContract]
        double ConvertTemperature(double temperature, string fromUnit, string toUnit);

        [OperationContract]
        double FahrenheitToCelsius(double fahrenheit);

        [OperationContract]
        double CelsiusToFahrenheit(double celsius);
    }
}