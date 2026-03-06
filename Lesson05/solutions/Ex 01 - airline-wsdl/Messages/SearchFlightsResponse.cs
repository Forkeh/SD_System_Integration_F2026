using System.Runtime.Serialization;
using AirlineWsdl.Models;

namespace AirlineWsdl.Messages;

[DataContract]
public class SearchFlightsResponse
{
    [DataMember]
    public List<Flight> Flights { get; set; } = new();
}
