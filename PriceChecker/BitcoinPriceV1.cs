using PriceChecker.Services;

namespace PriceChecker;

public static class BitcoinPriceV1
{
    public static RouteGroupBuilder MapBitcoinPriceV1(this RouteGroupBuilder group)
    {
        group.MapGet("/aggregated/{date:datetime}/{hour:range(0,24)}", GetAggregated).WithOpenApi(op =>
        {
            op.OperationId = "Aggregated";
            op.Description =
                "Provide API Endpoint to request the aggregated bitcoin price at a specific time point with hour accuracy.";
            return op;
        });
        group.MapGet("/persisted/{start:datetime}/{end:datetime}", GetPersisted).WithOpenApi(op =>
        {
            op.OperationId = "Persisted";
            op.Description =
                "Provide API Endpoint that fetches the persisted bitcoin prices from the datastore during a user-specified time range.";
            return op;
        });
        return group;
    }

    private static async Task<IResult> GetAggregated(DateOnly date, int hour, CancellationToken cancellationToken, IEnumerable<IPriceService> priceServices)
    {
        var finalDateTime = date.ToDateTime(new TimeOnly(hour,0));
        List<Task<double>> tasks = new List<Task<double>>();
        foreach (var priceService in priceServices)
        {
            tasks.Add(priceService.GetPrice(finalDateTime, cancellationToken));
        }

        var results = await Task.WhenAll(tasks);

        double finalResult = results.Sum() / results.Length;

        return Results.Ok(finalResult);
    }

    private static IResult GetPersisted(DateTime start, DateTime end)
    {
        return Results.Ok(15);
    }
}