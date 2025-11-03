namespace SurfLib.Data.Dtos
{
    public class MareeDTO
    {
        public int MareeId { get; set; }
        public int SpotId { get; set; }
        public bool MareeMoment { get; set; }
        public int MareeCoefficient { get; set; }
        public double MareeHauteur { get; set; }
        public TimeOnly MareeHeure { get; set; }
        public DateOnly MareeDate { get; set; }
    }
}
