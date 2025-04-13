using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Ruta que recibe el webhook
app.MapPost("/github-webhook", async (HttpRequest request) =>
{
    using var reader = new StreamReader(request.Body);
    var body = await reader.ReadToEndAsync();

    // Parsear el JSON manualmente
    var json = JsonDocument.Parse(body);
    var repo = json.RootElement.GetProperty("repository").GetProperty("name").GetString();
    var pusher = json.RootElement.GetProperty("pusher").GetProperty("name").GetString();
    var branch = json.RootElement.GetProperty("ref").GetString()?.Split('/').Last();

    Console.WriteLine($"\nRepositorio: {repo}");
    Console.WriteLine($"Push por: {pusher}");
    Console.WriteLine($"Branch: {branch}");

    var commits = json.RootElement.GetProperty("commits");
    foreach (var commit in commits.EnumerateArray())
    {
        var id = commit.GetProperty("id").GetString()?.Substring(0, 7);
        var message = commit.GetProperty("message").GetString();
        var timestamp = commit.GetProperty("timestamp").GetString();

        Console.WriteLine($"\nCommit: {id}");
        Console.WriteLine($"Fecha: {timestamp}");
        Console.WriteLine($"Mensaje: {message}");
    }

    return Results.Ok();
});


app.Run();
