﻿using System.Xml.Serialization;

namespace Theatre.DataProcessor.ExportDto
{
    [XmlType("Actor")]
    public class ExportActorDTO
    {
        [XmlAttribute("FullName")]
        public string FullName { get; set; } = null!;

        [XmlAttribute("MainCharacter")]
        public string MainCharacter { get; set; } = null!;
    }
}
