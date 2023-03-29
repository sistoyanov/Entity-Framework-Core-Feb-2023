using System.Xml.Serialization;

namespace TeisterMask.Data.Models.Enums;

public enum LabelType
{
    [XmlEnum("0")]
    Priority = 0,

    [XmlEnum("1")]
    CSharpAdvanced = 1,

    [XmlEnum("2")]
    JavaAdvanced = 2,

    [XmlEnum("3")]
    EntityFramework = 3,

    [XmlEnum("4")]
    Hibernate = 4
}
