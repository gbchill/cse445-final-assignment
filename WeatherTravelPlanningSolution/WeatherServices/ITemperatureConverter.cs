using System;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;

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

        [OperationContract]
        TemperatureData GetTemperatureByZipCode(string zipCode);
    }

    [DataContract]
    public class TemperatureData
    {
        [DataMember]
        public string ZipCode { get; set; }

        [DataMember]
        public double TemperatureFahrenheit { get; set; }

        [DataMember]
        public double TemperatureCelsius { get; set; }

        [DataMember]
        public string Location { get; set; }

        [DataMember]
        public DateTime RetrievedAt { get; set; }
    }
}