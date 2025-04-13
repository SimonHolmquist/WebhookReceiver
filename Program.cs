var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Ruta que recibe el webhook
app.MapPost("/github-webhook", async (HttpRequest request) =>
{
    using var reader = new StreamReader(request.Body);
    var body = await reader.ReadToEndAsync();

    Console.WriteLine("=== Webhook recibido ===");
    Console.WriteLine(body);

    return Results.Ok();
});

app.Run();
