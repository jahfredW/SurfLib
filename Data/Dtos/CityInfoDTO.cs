namespace SurfLib.Data.Dtos
{
    public class CityInfoDTO
    {
        public string CityName { get; set; } = string.Empty;

        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        public DateTime CreatedAt {  get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
