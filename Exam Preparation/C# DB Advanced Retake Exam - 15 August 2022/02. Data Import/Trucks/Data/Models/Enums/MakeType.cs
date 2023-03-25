using System.Xml.Serialization;

namespace Trucks.Data.Models.Enums;

public enum MakeType
{
    [XmlEnum("0")]
    Daf = 0,

    [XmlEnum("1")]
    Man = 1,

    [XmlEnum("2")]
    Mercedes = 2,

    [XmlEnum("3")]
    Scania = 3,

    [XmlEnum("4")]
    Volvo = 4
}
