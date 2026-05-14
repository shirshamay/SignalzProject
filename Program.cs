var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy => {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddOpenApi();

var app = builder.Build();

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[] { "SIGNALZ_BUY", "SIGNALZ_SELL" };

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)],
            (decimal)Random.Shared.Next(50000, 60000) + (decimal)Random.Shared.NextDouble()
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary, decimal Price);