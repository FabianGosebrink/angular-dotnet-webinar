namespace ExpenseTracker.Controller;

public static class WeatherforecastController
{
    public static void UseWeatherforecastEndpoints(this WebApplication app)
    {
        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        
        var group = app
            .MapGroup("weatherforecast")
            .WithOpenApi();

        group.MapPost("", () => { });
        
        group.MapGet("", () =>
        {
            var rng = new Random();
            var forecasts = Enumerable.Range(1, 5)
                .Select(index => new WeatherForecast(DateTime.Now.AddDays(index), rng.Next(-20, 55), summaries[rng.Next(summaries.Length)]))
            .ToArray();
            return forecasts;
        });
    }
    
    record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}