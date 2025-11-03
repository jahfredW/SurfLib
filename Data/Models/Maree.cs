using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SurfLib.Data.Models;

[Table("maree")]
public partial class Maree
{
    [Key]
    [Column("maree_id")]
    public int MareeId { get; set; }

    [Column("maree_coefficient")]
    public int MareeCoefficient { get; set; }

    [Column("maree_heure")]
    public TimeOnly MareeHeure { get; set; }

    [Column("maree_date")]
    public DateOnly MareeDate { get; set; }

    [Column("maree_moment")]
    public bool MareeMoment { get; set; }

    [Column("maree_hauteur")]
    public double MareeHauteur { get; set; }

    [ForeignKey(nameof(SpotId))]
    public int SpotId { get; set; }

    [InverseProperty("Marees")]
    public virtual Spot Spot { get; set; } = null;
}
