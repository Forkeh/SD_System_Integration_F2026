using System.Runtime.Serialization;
using AirlineWsdl.Models;

namespace AirlineWsdl.Messages;

[DataContract]
public class SearchFlightsRequest
{
    /// <summary>IATA 3-letter airport code (origin)</summary>
    [DataMember]
    public string Origin { get; set; } = string.Empty;

    /// <summary>IATA 3-letter airport code (destination)</summary>
    [DataMember]
    public string Destination { get; set; } = string.Empty;

    [DataMember]
    public DateTime DepartureDate { get; set; }

    [DataMember]
    public DateTime ReturnDate { get; set; }

    [DataMember]
    public PassengerCount Passengers { get; set; } = new();

    [DataMember]
    public CabinClass Cabin { get; set; }
}
