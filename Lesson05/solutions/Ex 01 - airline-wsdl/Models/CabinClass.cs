using System.Runtime.Serialization;

namespace AirlineWsdl.Models;

[DataContract]
public enum CabinClass
{
    [EnumMember]
    ECONOMY,

    [EnumMember]
    PREMIUM_ECONOMY,

    [EnumMember]
    BUSINESS,

    [EnumMember]
    FIRST
}
