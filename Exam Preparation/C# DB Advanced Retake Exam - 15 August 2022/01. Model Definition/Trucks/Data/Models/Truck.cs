using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using Trucks.Data.Models.Enums;

namespace Trucks.Data.Models;

public class Truck
{
    public Truck()
    {
        this.ClientsTrucks = new HashSet<ClientTruck>();
    }

    [Key]
    public int Id { get; set; }

    [MaxLength(8)]
    [MinLength(8)]
    [RegularExpression("^[A-Z]{2}\\d{4}[A-Z]{2}$")]
    public string RegistrationNumber { get; set; } = null!;

    [MaxLength(17)]
    [MinLength(17)]
    public string VinNumber { get; set; } = null!;

    public int TankCapacity  { get; set; }

    public int CargoCapacity  { get; set; }

    public CategoryType CategoryType  { get; set; }

    public MakeType MakeType  { get; set; }

    [ForeignKey(nameof(Despatcher))]
    public int DespatcherId { get; set; }
    public virtual Despatcher Despatcher { get; set; } = null!;

    public virtual ICollection<ClientTruck> ClientsTrucks { get; set; }
}
