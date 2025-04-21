using System;
using System.ServiceModel;

namespace TravelPlanningServices
{
    [ServiceContract]
    public interface IWordFilter
    {
        [OperationContract]
        string FilterStopWords(string inputText);

        [OperationContract]
        string[] GetTopContentWords(string inputText, int count);

        [OperationContract]
        int CountContentWords(string inputText);
    }
}