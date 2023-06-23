namespace PriceChecker.Models;

public class OhlcRoot
{
    public OhlcData Data { get; set; }
}

public class OhlcData
{
    public List<Ohlc>? Ohlc { get; set; }
    public string Pair { get; set; }
}

public class Ohlc
{
    public int Close { get; set; }
    public int High { get; set; }
    public int Low { get; set; }
    public int Open { get; set; }
    public int Timestamp { get; set; }
    public double Volume { get; set; }
}