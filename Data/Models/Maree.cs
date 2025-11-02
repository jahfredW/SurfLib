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

    [Column("maree_moment")]
    public bool MareeMoment { get; set; }

    [InverseProperty("Maree")]
    public virtual ICollection<Prevision> Previsions { get; set; } = new List<Prevision>();
}
