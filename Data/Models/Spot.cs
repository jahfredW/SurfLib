using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SurfLib.Data.Models;

[Table("spot")]
[Index("SpotName", Name = "UQ__spot__8BF62491B7D336C7", IsUnique = true)]
public partial class Spot
{
    [Key]
    [Column("spot_id")]
    public int SpotId { get; set; }

    [Column("spot_name")]
    [StringLength(50)]
    public string SpotName { get; set; } = null!;

    [Column("spot_lat", TypeName = "decimal(15, 6)")]
    public decimal SpotLat { get; set; }

    [Column("spot_lon", TypeName = "decimal(15, 6)")]
    public decimal SpotLon { get; set; }

    [InverseProperty("Spot")]
    public virtual ICollection<Prevision> Previsions { get; set; } = new List<Prevision>();
}
