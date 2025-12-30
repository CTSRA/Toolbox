using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace SQLLiteRollArtAPI.API.Controllers
{
    /// <summary>
    /// Contrôleur API permettant :
    /// - l’upload d’un fichier CSV,
    /// - la transformation des données CSV en JSON,
    /// - l’injection de ces données dans une base SQLite,
    /// - puis le retour au client d’un fichier SQLite (.s3db) généré.
    ///
    /// Le traitement s’appuie sur une base SQLite template
    /// copiée dynamiquement afin d’éviter tout accès concurrent.
    /// </summary>
    [ApiController]
    [Route("SQLLiteRollArtAPI/[controller]")]
    public class API20242025 : ControllerBase
    {
        /// <summary>
        /// Fournit l’accès aux chemins physiques de l’application
        /// (ContentRootPath, WebRootPath, etc.)
        /// </summary>
        private readonly IWebHostEnvironment _env;

        /// <summary>
        /// Constructeur avec injection de l’environnement ASP.NET Core
        /// </summary>
        /// <param name="env">Environnement d’hébergement (Dev / Prod)</param>
        public API20242025(IWebHostEnvironment env)
        {
            _env = env;
        }

        /// <summary>
        /// Endpoint d’upload d’un fichier CSV.
        ///
        /// Étapes :
        /// 1. Validation du fichier CSV
        /// 2. Lecture du contenu CSV
        /// 3. Conversion CSV → JSON
        /// 4. Copie d’une base SQLite template
        /// 5. Injection des données JSON dans la base
        /// 6. Exécution d’un script SQL d’initialisation
        /// 7. Retour du fichier SQLite généré au client
        /// </summary>
        /// <param name="request">Formulaire contenant le fichier CSV</param>
        /// <returns>Fichier SQLite (.s3db)</returns>
        [HttpPost("init")]
        [Consumes("multipart/form-data")]
        [Produces("application/x-sqlite3")]
        public async Task<IActionResult> CSVToSQLite([FromForm] CsvUploadRequest request)
        {
            // ------------------------------------------------------------
            // 1. Validation du fichier CSV
            // ------------------------------------------------------------

            var csvFile = request.CsvFile;

            // Fichier manquant ou vide
            if (csvFile == null || csvFile.Length == 0)
                return BadRequest("Fichier CSV manquant");

            // Vérification de l’extension
            if (!csvFile.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                return BadRequest("Le fichier doit être un CSV");

            // ------------------------------------------------------------
            // 2. Lecture du contenu CSV
            // ------------------------------------------------------------

            using var reader = new StreamReader(
                csvFile.OpenReadStream(),
                Encoding.UTF8,
                detectEncodingFromByteOrderMarks: true
            );

            var content = await reader.ReadToEndAsync();

            // Contenu vide
            if (string.IsNullOrEmpty(content))
                return BadRequest("Le fichier CSV est vide");

            // ------------------------------------------------------------
            // 3. Conversion CSV → JSON
            // ------------------------------------------------------------
            // Conversion réalisée via CsvHelper + Newtonsoft
            var jsonContent = Helpers.NewtonSoft.CsvToJsonHelper(content);

            // ------------------------------------------------------------
            // 4. Préparation des chemins SQLite
            // ------------------------------------------------------------

            // Base SQLite TEMPLATE (lecture seule)
            var templateDbPath = Path.Combine(
                _env.ContentRootPath,
                "API",
                "Templates",
                "20242025.s3db"
            );

            // Base SQLite de travail (copie unique)
            var workingDbPath = Path.Combine(
                Path.GetTempPath(),
                $"20242025_{Guid.NewGuid():N}.s3db"
            );

            // ------------------------------------------------------------
            // 5. Copie de la base template
            // ------------------------------------------------------------

            System.IO.File.Copy(templateDbPath, workingDbPath, overwrite: false);

            if (!System.IO.File.Exists(workingDbPath))
                return NotFound("Base SQLite introuvable");

            // ------------------------------------------------------------
            // 6. Injection des données dans SQLite
            // ------------------------------------------------------------

            var array = JArray.Parse(jsonContent);

            // Nettoyage de la table cible
            Helpers.SqliteHelper.ExecuteRaw(
                workingDbPath,
                "DELETE FROM Rolskanet;"
            );

            // Insertion JSON → table SQLite
            Helpers.SqliteHelper.InsertJsonIntoTable(
                workingDbPath,
                "Rolskanet",
                array
            );

            // ------------------------------------------------------------
            // 7. Exécution du script SQL d’initialisation
            // ------------------------------------------------------------

            var sqlFilePath = Path.Combine(
                _env.ContentRootPath,
                "API",
                "Templates",
                "20242025Init.sql"
            );

            var sqlScript = System.IO.File.ReadAllText(
                sqlFilePath,
                new UTF8Encoding()
            );

            Helpers.SqliteHelper.ExecuteSqlScript(
                workingDbPath,
                sqlScript
            );

            // ------------------------------------------------------------
            // 8. Récupération du nom de la manifestation
            // ------------------------------------------------------------

            var manifestation = array[0]?["Manifestation"]?.ToString();

            // ------------------------------------------------------------
            // 9. Préparation du flux de retour
            // ------------------------------------------------------------

            // Sécurité mémoire (verrou SQLite / GC)
            GC.Collect();
            GC.WaitForPendingFinalizers();

            var stream = new FileStream(
                workingDbPath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.ReadWrite
            );

            // ------------------------------------------------------------
            // 10. Nettoyage du fichier temporaire après réponse HTTP
            // ------------------------------------------------------------

            HttpContext.Response.OnCompleted(async () =>
            {
                for (var i = 0; i < 5; i++)
                {
                    try
                    {
                        if (System.IO.File.Exists(workingDbPath))
                            System.IO.File.Delete(workingDbPath);

                        break;
                    }
                    catch (IOException)
                    {
                        await Task.Delay(500);
                    }
                }
            });

            // ------------------------------------------------------------
            // 11. Retour du fichier SQLite au client
            // ------------------------------------------------------------

            return File(
                stream,
                "application/x-sqlite3",
                $"{manifestation}.s3db"
            );
        }
    }

    /// <summary>
    /// Modèle utilisé pour l’upload du fichier CSV via multipart/form-data.
    /// Nécessaire pour Swagger et le binding ASP.NET Core.
    /// </summary>
    public class CsvUploadRequest
    {
        /// <summary>
        /// Fichier CSV envoyé par le client
        /// </summary>
        [FromForm(Name = "csvFile")]
        public IFormFile CsvFile { get; set; } = default!;
    }
}
