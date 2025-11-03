using HtmlAgilityPack;
using NuGet.Packaging;
using SurfLib.Data.Models;
using System.Globalization;
using System.Net.Http;
using System.Security.Principal;

namespace SurfLib.Utils
{
    public class MareeScrapper
    {
        public readonly HttpClient _httpClient;
        static string url = "https://www.horaire-maree.fr/maree/";

        public MareeScrapper(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        /// <summary>
        /// Récupération basiqeu dans une balise strong 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public string ParseText(int number, HtmlNodeCollection node)
        {
            try
            {
                string text = node[number].SelectSingleNode("strong").InnerText.Trim();
                return text;
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        /// <summary>
        /// Récupération de la hauteur d'eau
        /// </summary>
        /// <param name="number"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public double ParseHauteurEau(int number, HtmlNodeCollection node)
        {
            string? hauteurEauBasse1 = node[number].InnerText.Trim();
            var parts = hauteurEauBasse1.Split(' ');

            if (double.TryParse(parts[1].Replace(',', '.'), out double result))
            {
                return result;
            }

            return 0;
        }

        /// <summary>
        /// Conversion d'une chaine de caractère en TimeOnly
        /// </summary>
        /// <param name="horaire"></param>
        /// <param name="pattern"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public TimeOnly ConvertStringToTimeOnly(string horaire, string? pattern, CultureInfo? options)
        {
            if (string.IsNullOrEmpty(horaire) || string.IsNullOrEmpty(pattern)) {
                return TimeOnly.MinValue;
            }


            horaire = horaire.Replace("h", ":");
            return TimeOnly.ParseExact(horaire, pattern, options);
        }

        public async Task<IEnumerable<Maree>> GetDocument(Spot spot)
        {
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:110.0) Gecko/20100101 Firefox/110.0");

            string finalUrl = url + spot.SpotName.ToUpper() + "/";
            var html = await _httpClient.GetStringAsync(finalUrl);

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            // Debug
            File.WriteAllText("debug.html", html);

            var marees = new List<Maree>();

            // Extraction de la date du jour depuis le <h3 class="orange">
            var dateNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@id='i_header_tbl_droite']/h3");
            DateTime dateDuJour = DateTime.Now;
            if (dateNode != null)
            {
                // Exemple : "Marée aujourd'hui<br /> samedi 27 septembre 2025"
                var text = dateNode.InnerText.Replace("Marée aujourd'hui", "").Trim();
                if (DateTime.TryParseExact(text.Split(new[] { '\r', '\n', ' ' }, StringSplitOptions.RemoveEmptyEntries).Skip(1).Take(3).Aggregate((a, b) => a + " " + b),
                                           "dd MMMM yyyy", CultureInfo.GetCultureInfo("fr-FR"), DateTimeStyles.None, out DateTime parsedDate))
                {
                    dateDuJour = parsedDate;
                }
            }

            // On récupère les lignes <tr> à partir de la 3e ligne (pour ignorer les en-têtes)
            var previsionRows = htmlDoc.DocumentNode.SelectNodes("//div[@id='i_donnesJour']//table//tr[position()>2]");
            if (previsionRows == null) return marees;

            foreach (var row in previsionRows)
            {
                var tds = row.SelectNodes("td");
                if (tds == null || tds.Count < 6) continue;

                // Coefficients
                int coefBasse = int.TryParse(tds[0].InnerText.Trim(), out int cB) ? cB : 0;
                int coefHaute = int.TryParse(tds[3].InnerText.Trim(), out int cH) ? cH : 0;

                // Horaires
                string horaireBasse1 = tds[1].SelectSingleNode("strong")?.InnerText.Trim() ?? "";
                string horaireHaute1 = tds[2].SelectSingleNode("strong")?.InnerText.Trim() ?? "";
                string horaireBasse2 = tds[4].SelectSingleNode("strong")?.InnerText.Trim() ?? "";
                string horaireHaute2 = tds[5].SelectSingleNode("strong")?.InnerText.Trim() ?? "";

                TimeOnly heureBasse1 = ConvertStringToTimeOnly(horaireBasse1, "HH:mm", CultureInfo.InvariantCulture);
                TimeOnly heureHaute1 = ConvertStringToTimeOnly(horaireHaute1, "HH:mm", CultureInfo.InvariantCulture);
                TimeOnly heureBasse2 = ConvertStringToTimeOnly(horaireBasse2, "HH:mm", CultureInfo.InvariantCulture);
                TimeOnly heureHaute2 = ConvertStringToTimeOnly(horaireHaute2, "HH:mm", CultureInfo.InvariantCulture);

                var test = tds[1].InnerText.Replace(',', '.').Trim().Split(' ')[1].Trim();

                // Hauteurs
                double hauteurBasse1 = double.TryParse(tds[1].InnerText.Replace(',', '.').Trim().Split(' ')[1].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out double hB1) ? hB1 : 0;
                double hauteurHaute1 = double.TryParse(tds[2].InnerText.Replace(',', '.').Trim().Split(' ')[1].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out double hH1) ? hH1 : 0;
                double hauteurBasse2 = double.TryParse(tds[4].InnerText.Replace(',', '.').Trim().Split(' ')[1].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out double hB2) ? hB2 : 0;
                double hauteurHaute2 = double.TryParse(tds[5].InnerText.Replace(',', '.').Trim().Split(' ')[1].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out double hH2) ? hH2 : 0;

                marees.AddRange(new[]
                {
            new Maree
            {
                MareeMoment = false,
                MareeCoefficient = coefBasse,
                MareeHauteur = hauteurBasse1,
                MareeHeure = heureBasse1,
                MareeDate = DateOnly.FromDateTime(dateDuJour),
                SpotId = spot.SpotId
            },
            new Maree
            {
                MareeMoment = true,
                MareeCoefficient = coefBasse,
                MareeHauteur = hauteurHaute1,
                MareeHeure = heureHaute1,
                MareeDate = DateOnly.FromDateTime(dateDuJour),
                SpotId = spot.SpotId
            },
            new Maree
            {
                MareeMoment = false,
                MareeCoefficient = coefHaute,
                MareeHauteur = hauteurBasse2,
                MareeHeure = heureBasse2,
                MareeDate = DateOnly.FromDateTime(dateDuJour),
                SpotId = spot.SpotId
            },
            new Maree
            {
                MareeMoment = true,
                MareeCoefficient = coefHaute,
                MareeHauteur = hauteurHaute2,
                MareeHeure = heureHaute2,
                MareeDate = DateOnly.FromDateTime(dateDuJour),
                SpotId = spot.SpotId
            }
        });
            }

            return marees;
        }
    }
}
