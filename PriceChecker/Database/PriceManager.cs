namespace PriceChecker.Database;

public class PriceManager : IPriceManager
{
    private readonly PriceDb _db;

    public PriceManager(PriceDb db)
    {
        _db = db;
    }

    public double? GetPrice(DateTime date)
    {
        return _db.Prices.FirstOrDefault(p => p.DateId == date)?.Close;
    }

    public async Task PersistPrice(DateTime date, double aggregatedPrice, CancellationToken cancellationToken)
    {
        var newPrice = new Price { DateId = date, Name = "BTC/USD", Close = aggregatedPrice };
        _db.Prices.Add(newPrice);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public IEnumerable<Price> GetPrices(DateTime start, DateTime end)
    {
        return _db.Prices.Where(p => p.DateId >= start && p.DateId <= end).OrderBy(p => p.DateId).ToList();
    }
}