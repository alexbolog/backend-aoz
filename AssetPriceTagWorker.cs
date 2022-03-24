using Newtonsoft.Json;
using backend_aoz.Models;
using backend_aoz.Repos;

namespace backend_aoz.Workers;

public class AssetPriceTagWorker
{
    private static IAssetPriceTagRepo repo;
    private readonly System.Timers.Timer timer;
    private const string ApiKey = "58E1921A-9F8A-43FE-A8C1-28EFE194D358";
    private const int Interval = 1000 * 1000; // 1000 seconds in ms

    public AssetPriceTagWorker(IAssetPriceTagRepo repoInstance)
    {
        repo = repoInstance;

        timer = new System.Timers.Timer();
        timer.Interval = Interval;
        timer.AutoReset = true;
        timer.Enabled = true;
        timer.Elapsed += OnTimedEvent;
    }

    public static Thread GetThread()
    {
        var thread = new Thread(new ThreadStart(() => new AssetPriceTagWorker(new FileAssetPriceTagRepo())));
        return thread;
    }

    private static void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
    {
        using var httpClient = new HttpClient();
        using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://rest.coinapi.io/v1/exchangerate/EGLD/USD");
        httpRequestMessage.Headers.Add("X-CoinAPI-Key", ApiKey);
        var response = httpClient.Send(httpRequestMessage);
        var responseContent = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();

        var obj = JsonConvert.DeserializeObject<AssetPriceTag>(responseContent);
        repo.WriteAssetPriceTag(obj);
        Console.WriteLine($"Updated egld price tag with value: {obj.rate}");
    }
}