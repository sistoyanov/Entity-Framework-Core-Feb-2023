using System.Xml.Serialization;

namespace Theatre.Data.Models.Enums
{
    public enum Genre
    {
        [XmlEnum("0")]
        Drama = 0,

        [XmlEnum("1")]
        Comedy = 1,

        [XmlEnum("2")]
        Romance = 2,

        [XmlEnum("3")]
        Musical = 3
    }
}
