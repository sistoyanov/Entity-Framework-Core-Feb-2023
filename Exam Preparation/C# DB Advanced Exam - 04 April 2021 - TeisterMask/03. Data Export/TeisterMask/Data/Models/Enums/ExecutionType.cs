using System.Xml.Serialization;

namespace TeisterMask.Data.Models.Enums;

public enum ExecutionType
{
    [XmlEnum("0")]
    ProductBacklog = 0,

    [XmlEnum("1")]
    SprintBacklog = 1,

    [XmlEnum("2")]
    InProgress = 2,

    [XmlEnum("3")]
    Finished = 3
}
