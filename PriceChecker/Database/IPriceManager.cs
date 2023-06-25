namespace PriceChecker.Database;

public interface IPriceManager
{
    /// <summary>
    /// Get BTC/USD price for a specific time point
    /// </summary>
    double? GetPrice(DateTime date);

    /// <summary>
    /// Save Price in db
    /// </summary>
    Task PersistPrice(DateTime date, double aggregatedPrice, CancellationToken cancellationToken);

    /// <summary>
    /// Get price for a time range
    /// </summary>
    IEnumerable<Price> GetPrices(DateTime start, DateTime end);
}