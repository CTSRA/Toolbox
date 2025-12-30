using SQLitePCL;

// ------------------------------------------------------------
// Initialisation de la variable d’environnement DOTNET_ENVIRONMENT
// ------------------------------------------------------------
// Si la variable n’est pas définie (ex : lancement hors IIS, console, service),
// on force le mode Development par défaut.
if (string.IsNullOrEmpty(
        Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")))
{
    Environment.SetEnvironmentVariable(
        "DOTNET_ENVIRONMENT",
        Environments.Development);
}

// ------------------------------------------------------------
// Initialisation de SQLitePCL
// ------------------------------------------------------------
// Nécessaire pour éviter des erreurs de type
// "SQLitePCL.raw.SetProvider() was not called"
// lors de l’utilisation de SQLite (notamment sous Windows / Linux)
Batteries.Init();

// ------------------------------------------------------------
// Création du builder ASP.NET Core
// ------------------------------------------------------------
var builder = WebApplication.CreateBuilder(args);

// ------------------------------------------------------------
// Enregistrement des services (Dependency Injection)
// ------------------------------------------------------------

// Ajout du support des contrôleurs API (MVC minimal)
builder.Services.AddControllers();

// Génération automatique de la documentation OpenAPI
builder.Services.AddEndpointsApiExplorer();

// Ajout de Swagger (UI + JSON OpenAPI)
builder.Services.AddSwaggerGen();

// ------------------------------------------------------------
// Construction de l'application
// ------------------------------------------------------------
var app = builder.Build();

// ------------------------------------------------------------
// Configuration du pipeline HTTP selon l’environnement
// ------------------------------------------------------------
if (app.Environment.IsDevelopment())
{
    // Redirige la racine "/" vers l’interface Swagger
    app.MapGet("/", () => Results.Redirect("/swagger"));

    // Active la génération du JSON Swagger
    app.UseSwagger();

    // Active l’interface graphique Swagger UI
    app.UseSwaggerUI();

    // Gestion des exceptions détaillées en développement
    app.UseExceptionHandler("/error-development");
}
else
{
    // Gestion des exceptions générique en production
    app.UseExceptionHandler("/error");
}

// ------------------------------------------------------------
// Middleware communs
// ------------------------------------------------------------

// Autorise le service de fichiers statiques (wwwroot)
app.UseStaticFiles();

// Politique CORS (doit être configurée via AddCors si nécessaire)
app.UseCors();

// Redirection HTTP → HTTPS
app.UseHttpsRedirection();

// ------------------------------------------------------------
// Routing et sécurité
// ------------------------------------------------------------

// Mappe les contrôleurs API (attributs [ApiController], [Route], etc.)
app.MapControllers();

// Gestion de l’autorisation (JWT, policies, etc.)
app.UseAuthorization();

// ------------------------------------------------------------
// Lancement de l’application
// ------------------------------------------------------------
app.Run();
