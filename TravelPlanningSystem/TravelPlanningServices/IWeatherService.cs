using System;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace TravelPlanningServices
{
    [ServiceContract]
    public interface IWeatherService
    {
        [OperationContract]
        string GetWeatherForecast(string zipCode);
    }
}