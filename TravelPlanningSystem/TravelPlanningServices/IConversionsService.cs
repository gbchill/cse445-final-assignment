using System.ServiceModel;

// We define our contract i.e. what method can the server execute
namespace TravelPlanningServices
{
    [ServiceContract]
    public interface IConversionsService
    {

        [OperationContract]
        int c2f(int c);

        [OperationContract]
        int f2c(int f);
    }
}