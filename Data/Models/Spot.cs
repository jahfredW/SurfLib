using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SurfLib.Data.Models;

[Table("spot")]
[Index("SpotName", "SpotLat", "SpotLon", Name = "UQ_spot_name_lat_lon", IsUnique = true)]
public partial class Spot
{
    [Key]
    [Column("spot_id")]
    public int SpotId { get; set; }

    [Column("spot_name")]
    [StringLength(50)]
    public string SpotName { get; set; } = null!;

    [Column("spot_lat", TypeName = "decimal(15, 7)")]
    public decimal SpotLat { get; set; }

    [Column("spot_lon", TypeName = "decimal(15, 7)")]
    public decimal SpotLon { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [InverseProperty("Spot")]
    public virtual ICollection<Maree> Marees { get; set; } = new List<Maree>();
}
