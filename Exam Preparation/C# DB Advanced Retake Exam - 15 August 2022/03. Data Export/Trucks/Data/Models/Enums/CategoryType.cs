using System.Xml.Serialization;

namespace Trucks.Data.Models.Enums;

public enum CategoryType
{
    [XmlEnum("0")]
    Flatbed = 0,

    [XmlEnum("1")]
    Jumbo = 1,

    [XmlEnum("2")]
    Refrigerated = 2,

    [XmlEnum("3")]
    Semi = 3
}
