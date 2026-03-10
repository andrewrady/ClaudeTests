var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

var tickers = new[]
{
    "AAPL", "GOOGL", "MSFT", "AMZN", "TSLA", "META", "NVDA", "NFLX", "JPM", "V"
};

app.MapGet("/stockquotes", () =>
{
    var quotes = Enumerable.Range(0, 5).Select(index =>
        new StockQuote
        (
            tickers[Random.Shared.Next(tickers.Length)],
            Math.Round(Random.Shared.NextDouble() * 500 + 50, 2),
            Math.Round((Random.Shared.NextDouble() - 0.5) * 10, 2),
            DateTime.Now
        ))
        .ToArray();
    return quotes;
})
.WithName("GetStockQuotes");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

record StockQuote(string Ticker, double Price, double Change, DateTime Timestamp)
{
    public double ChangePercent => Math.Round(Change / Price * 100, 2);
}
