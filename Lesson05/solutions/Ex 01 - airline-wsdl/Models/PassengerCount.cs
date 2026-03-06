using System.Runtime.Serialization;

namespace AirlineWsdl.Models;

[DataContract]
public class PassengerCount
{
    [DataMember]
    public int Adults { get; set; }

    [DataMember]
    public int Children { get; set; }

    [DataMember]
    public int Infants { get; set; }
}
