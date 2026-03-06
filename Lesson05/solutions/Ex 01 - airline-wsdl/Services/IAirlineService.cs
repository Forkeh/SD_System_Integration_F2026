using System.ServiceModel;
using AirlineWsdl.Messages;

namespace AirlineWsdl.Services;

[ServiceContract]
public interface IAirlineService
{
    [OperationContract]
    SearchFlightsResponse SearchFlights(SearchFlightsRequest request);
}
