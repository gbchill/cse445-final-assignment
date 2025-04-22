using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace WeatherServices
{
    [ServiceContract]
    public interface IWeatherForecast
    {
        [OperationContract]
        string GetWeatherForecast(string zipCode);
    }
    }
}