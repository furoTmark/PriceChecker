using PriceChecker;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapGroup("/BitcoinPrice/v1").MapBitcoinPriceV1().WithOpenApi().WithTags("Bitcoin Price Checker");

app.Run();