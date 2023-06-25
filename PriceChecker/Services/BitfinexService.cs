namespace PriceChecker.Services;

public class BitfinexService : IPriceService
{
    private static readonly int HourMilliseconds = 3600000;
    private readonly HttpClient _client;

    public BitfinexService(HttpClient client)
    {
        _client = client;
    }

    public async Task<double?> GetPrice(DateTime timestamp, CancellationToken cancellationToken)
    {
        var startTime = ((DateTimeOffset)timestamp).ToUnixTimeMilliseconds();
        var endTime = startTime + HourMilliseconds;

        try
        {
            var response = await _client.GetAsync($"/v2/candles/trade:1h:tBTCUSD/hist?start={startTime}&end={endTime}&limit=1", cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<List<List<double>>>(cancellationToken: cancellationToken);
                if (data?.FirstOrDefault() != null)
                {
                    return data.FirstOrDefault()![2];
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