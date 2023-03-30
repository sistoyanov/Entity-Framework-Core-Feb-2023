using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using VaporStore.Data.Models.Enums;
using VaporStore.Data.Models;
using System.Xml.Serialization;

namespace VaporStore.DataProcessor.ImportDto;

[XmlType("Purchase")]
public class ImportPurchaseDTO
{
    [XmlAttribute("title")]
    [Required]
    public string Title { get; set; } = null!;

    [XmlElement("Type")]
    [Required]
    public PurchaseType Type { get; set; }

    [XmlElement("Key")]
    [Required]
    [RegularExpression(@"^[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}$")]
    public string Key { get; set; } = null!;

    [XmlElement("Card")]
    [RegularExpression(@"^\d{4} \d{4} \d{4} \d{4}$")]
    [Required]
    public string Card { get; set; } = null!;

    [XmlElement("Date")]
    [Required]
    public string Date { get; set; } = null!;


}
