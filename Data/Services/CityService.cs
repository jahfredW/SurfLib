using SurfLib.Data.Dtos;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SurfLib.Data.Services
{
    public class CityService
    {
        private readonly HttpClient _httpClient;

        public CityService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<CityInfoDTO?> GetCityInfoAsync(string cityName)
        {
            // EndPoint de l'API 
            var url = $"https://nominatim.openstreetmap.org/search?city={cityName}&format=json&limit=1";


            // L'API a besoin d'un user Agent dans le header pour fonctionner correctement 
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("User-Agent", "MySurfApp/1.0 (+https://example.com)");

            // récupération de la réponse -> SendAsync permet d'inclure le header dans la réponse != GetAsync
            var response = await _httpClient.SendAsync(request);

            // code http erreur -> on ne renvoie rien 
            if (!response.IsSuccessStatusCode) return null;

            // lecture des datas renvoyéées 
            var content = await response.Content.ReadAsStringAsync();

            // Désérialisation -> format sérialisé vers objet 
            var results = JsonSerializer.Deserialize<List<CityCoordinatesJson>>(content);

            // on teste si result != null et si liste n'est pas == 0 
            if(results == null ||  results.Count == 0) return null;

            var result = results.First();

            return new CityInfoDTO
            {
                CityName = cityName,
                Latitude = decimal.TryParse(result.Latitude, CultureInfo.InvariantCulture, out var lat) ? lat : 0,
                Longitude = decimal.TryParse(result.Longitude, CultureInfo.InvariantCulture, out var lon) ? lon : 0,
            };
            
        }

        // classe privée permettant de récupérer les infos de l'API. 
        private class CityCoordinatesJson
        {
            // JsonPropertyName indique que la valeur JSON dans la réponse est lat 
            [JsonPropertyName("lat")]
            public string Latitude { get; set; } = string.Empty;

            [JsonPropertyName("lon")]
            public string Longitude { get; set; } = string.Empty;
        }

    }
}
