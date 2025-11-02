using SurfLib.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurfLib.Data.Dtos
{
    public class PrevisionsDTO
    {
        public int SpotId { get; set; }
        public int MareeId { get; set; }

        public TimeOnly PrevisionHeure { get; set; }

 
        public DateOnly PrevisionDate { get; set; }

        public DateTime CreatedAt { get; set; }

   
        public DateTime UpdatedAt { get; set; }

        public Maree Maree { get; set; } = null!;

        public Spot Spot { get; set; } = null!;
    }
}
