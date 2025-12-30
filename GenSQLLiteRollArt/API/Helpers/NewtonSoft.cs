using Newtonsoft.Json;
using System.Formats.Asn1;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace SQLLiteRollArtAPI.API.Helpers
{
    public static class NewtonSoft
    {
        public static string CsvToJsonHelper(string csvContent, char separator = ';')
        {
            var lines = csvContent
                .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length < 2)
                throw new Exception("CSV invalide");

            var headers = lines[0].Split(separator);

            var data = new List<Dictionary<string, string>>();

            for (var i = 1; i < lines.Length; i++)
            {
                var values = lines[i].Split(separator);
                var row = new Dictionary<string, string>();

                for (var j = 0; j < headers.Length && j < values.Length; j++)
                {
                    row[headers[j]] = values[j];
                }

                data.Add(row);
            }

            return JsonConvert.SerializeObject(data, Formatting.Indented);
        }

        public static string CsvToJsonHelper(string csvContent)
        {
            using var reader = new StringReader(csvContent);

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HasHeaderRecord = true,

                // 🔑 Gestion correcte des guillemets
                TrimOptions = TrimOptions.Trim,
                IgnoreBlankLines = true,

                // Tolérance CSV réel
                BadDataFound = null,
                MissingFieldFound = null
            };

            using var csv = new CsvReader(reader, config);

            // Lecture dynamique (colonnes inconnues)
            var records = csv.GetRecords<dynamic>().ToList();

            return JsonConvert.SerializeObject(records, Formatting.Indented);
        }
    }

}

