using SurfLib.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurfLib.Data.Dtos
{
    public class SpotsDTO
    {

        public int SpotId { get; set; }

        public string SpotName { get; set; } = null!;

        public decimal SpotLat { get; set; }
        public decimal SpotLon { get; set; }

        public ICollection<Prevision> Previsions { get; set; } = new List<Prevision>();
    }
}
