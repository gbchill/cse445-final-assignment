using System;
using System.ServiceModel;

namespace WeatherServices
{
    [ServiceContract]
    public interface IRainyDayAdvisor
    {
        [OperationContract]
        string[] GetRainyDayAdvice(string location, DateTime date);

        [OperationContract]
        string[] GetIndoorActivities(string location);

        [OperationContract]
        bool IsItRainy(string location, DateTime date);
    }
}