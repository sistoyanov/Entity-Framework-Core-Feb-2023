using System.Xml.Serialization;

namespace SoftJail.Data.Models.Enums;

public enum Position
{
    //[XmlEnum("0")]
    Overseer = 0,

    //[XmlEnum("1")]
    Guard = 1,

    //[XmlEnum("2")]
    Watcher = 2,

    //[XmlEnum("3")]
    Labour = 3
}
