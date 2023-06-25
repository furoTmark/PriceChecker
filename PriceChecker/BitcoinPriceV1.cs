using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using PriceChecker.Database;
using PriceChecker.Services;

namespace PriceChecker;

public static class BitcoinPriceV1
{
    public static RouteGroupBuilder MapBitcoinPriceV1(this RouteGroupBuilder group)
    {
        group.MapGet("/aggregated/{date:datetime}/{hour:range(0,23)}", GetAggregated).WithOpenApi(op =>
        {
            op.OperationId = "Aggregated";
            op.Description =
                "Provide API Endpoint to request the aggregated bitcoin price at a specific time point with hour accuracy.";
            op.Parameters = new List<OpenApiParameter>
            {
                new OpenApiParameter { Name = "date", Description = "Date", Required = true, Example = new OpenApiString("2023-06-23"), In = ParameterLocation.Path},
                new OpenApiParameter { Name = "hour", Description = "Hour", Required = true, Example = new OpenApiString("14"), In = ParameterLocation.Path}
            };
            return op;
        });
        group.MapGet("/persisted", GetPersisted).WithOpenApi(op =>
        {
            op.OperationId = "Persisted";
            op.Description =
                "Provide API Endpoint that fetches the persisted bitcoin prices from the datastore during a user-specified time range.";
            op.Parameters = new List<OpenApiParameter>
            {
                new OpenApiParameter { Name = "start", Description = "Start time range", Required = true, Example = new OpenApiString("2023-06-23 00:00"), In = ParameterLocation.Query},
                new OpenApiParameter { Name = "end", Description = "End time range", Required = true, Example = new OpenApiString("2023-06-23 23:00"), In = ParameterLocation.Query}
            };
            return op;
        });
        return group;
    }

    private static async Task<IResult> GetAggregated(DateOnly date, int hour, CancellationToken cancellationToken,
        IEnumerable<IPriceService> priceServices, PriceDb db)
    {
        var finalDateTime = date.ToDateTime(new TimeOnly(hour, 0));
        var aggregatedPrice = GetPriceFromDb(finalDateTime, db);
        if (aggregatedPrice == null)
        {
            aggregatedPrice = await AggregatePrices(finalDateTime, cancellationToken, priceServices);
            if (aggregatedPrice == null)
            {
                return Results.Problem("Error while getting the price");
            }

            await PersistResult(finalDateTime, (double)aggregatedPrice, cancellationToken, db);
        }

        return Results.Ok(aggregatedPrice);
    }

    private static double? GetPriceFromDb(DateTime finalDateTime, PriceDb db)
    {
        return db.Prices.FirstOrDefault(p => p.DateId == finalDateTime)?.Close;
    }

    private static async Task PersistResult(DateTime date, double aggregatedPrice, CancellationToken cancellationToken,
        PriceDb db)
    {
        var newPrice = new Price { DateId = date, Name = "BTC/USD", Close = aggregatedPrice };
        db.Prices.Add(newPrice);
        await db.SaveChangesAsync(cancellationToken);
    }

    private static async Task<double?> AggregatePrices(DateTime date, CancellationToken cancellationToken,
        IEnumerable<IPriceService> priceServices)
    {
        List<Task<double>> tasks = new List<Task<double>>();
        foreach (var priceService in priceServices)
        {
            tasks.Add(priceService.GetPrice(date, cancellationToken));
        }

        var results = await Task.WhenAll(tasks);

        double finalResult = results.Sum() / results.Length;
        return finalResult;
    }

    private static IResult GetPersisted(DateTime start, DateTime end, PriceDb db)
    {
        var list = db.Prices.Where(p => p.DateId >= start && p.DateId <= end).OrderBy(p => p.DateId).ToList();
        return Results.Ok(list);
    }
}