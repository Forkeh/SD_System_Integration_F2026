using System.Runtime.Serialization;

namespace AirlineWsdl.Models;

[DataContract]
public class Flight
{
    [DataMember]
    public string Carrier { get; set; } = string.Empty;

    [DataMember]
    public string FlightNumber { get; set; } = string.Empty;

    [DataMember]
    public DateTime DepartDateTime { get; set; }

    [DataMember]
    public DateTime ArriveDateTime { get; set; }

    /// <summary>IATA 3-letter airport code (origin)</summary>
    [DataMember]
    public string Origin { get; set; } = string.Empty;

    /// <summary>IATA 3-letter airport code (destination)</summary>
    [DataMember]
    public string Destination { get; set; } = string.Empty;

    [DataMember]
    public Money Price { get; set; } = new();
}
