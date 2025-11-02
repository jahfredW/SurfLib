using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SurfLib.Data.Models;

[PrimaryKey("SpotId", "MareeId", "PrevisionHeure", "PrevisionDate")]
[Table("previsions")]
public partial class Prevision
{
    [Key]
    [Column("spot_id")]
    public int SpotId { get; set; }

    [Key]
    [Column("maree_id")]
    public int MareeId { get; set; }

    [Key]
    [Column("prevision_heure")]
    public TimeOnly PrevisionHeure { get; set; }

    [Key]
    [Column("prevision_date")]
    public DateOnly PrevisionDate { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [ForeignKey("MareeId")]
    [InverseProperty("Previsions")]
    public virtual Maree Maree { get; set; } = null!;

    [ForeignKey("SpotId")]
    [InverseProperty("Previsions")]
    public virtual Spot Spot { get; set; } = null!;
}
