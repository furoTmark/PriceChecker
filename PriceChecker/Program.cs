using Microsoft.EntityFrameworkCore;
using PriceChecker;
using PriceChecker.Database;
using PriceChecker.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PriceDb>(opt => opt.UseInMemoryDatabase("PriceDb"));

builder.Services.AddHttpClient<IPriceService, BitstampService>("Bitstamp",
    client => { client.BaseAddress = new Uri("https://www.bitstamp.net");});
builder.Services.AddHttpClient<IPriceService, BitfinexService>("Bitfinex",
    client => { client.BaseAddress = new Uri("https://api-pub.bitfinex.com");});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGroup("/BitcoinPrice/v1").MapBitcoinPriceV1().WithOpenApi().WithTags("Bitcoin Price Checker");

app.Run();