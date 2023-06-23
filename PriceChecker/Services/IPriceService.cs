namespace PriceChecker.Services;

public interface IPriceService
{
    Task<double> GetPrice(DateTime timestamp, CancellationToken cancellationToken);
}