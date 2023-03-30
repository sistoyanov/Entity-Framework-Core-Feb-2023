using System.Xml.Serialization;

namespace VaporStore.Data.Models.Enums;

public enum CardType
{
    [XmlEnum("0")]
    Debit = 0,

    [XmlEnum("1")]
    Credit = 1
}
