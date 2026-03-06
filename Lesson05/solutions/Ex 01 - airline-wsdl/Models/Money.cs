using System.Runtime.Serialization;

namespace AirlineWsdl.Models;

[DataContract]
public class Money
{
    [DataMember]
    public decimal Amount { get; set; }

    [DataMember]
    public string Currency { get; set; } = string.Empty;
}
