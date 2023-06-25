using PriceChecker.Models;

namespace PriceChecker.Services;

public class BitstampService : IPriceService
{
    private readonly HttpClient _client;

    public BitstampService(HttpClient client)
    {
        _client = client;
    }

    public async Task<double?> GetPrice(DateTime timestamp, CancellationToken cancellationToken)
    {
        long unixTime = ((DateTimeOffset)timestamp).ToUnixTimeSeconds();

        try
        {
            var response = await _client.GetAsync($"/api/v2/ohlc/btcusd/?step=3600&limit=1&start={unixTime}", cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<OhlcRoot>(cancellationToken: cancellationToken);
                var ohlc = data?.Data.Ohlc?.FirstOrDefault();
                if (ohlc != null)
                {
                    return ohlc.Close;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return null;
    }
}