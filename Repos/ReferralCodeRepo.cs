using backend_aoz.Models;
using Newtonsoft.Json;

namespace backend_aoz.Repos;

public interface IReferralCodeRepo
{
    void AddReferralData(string erdAddress, string referralCode);
    void AddReferralData(ReferralData referralData);
    Task<ReferralData?> GetReferralDataByCodeAsync(string referralCode);
    Task<ReferralData?> GetReferralDataByErdAsync(string erdAddress);
}

public class FileReferralCodeRepo : BaseFileRepo, IReferralCodeRepo
{
    protected override string dbFileName { get => "referralCodes.json"; }
    private static object padlock = new object();

    public void AddReferralData(string erdAddress, string referralCode)
    {
        AddReferralData(new ReferralData(erdAddress, referralCode));
    }

    public void AddReferralData(ReferralData referralData)
    {
        lock (padlock)
        {
            var data = GetAllData().ToList();
            data.Add(referralData);
            File.WriteAllText(dbFullPath, JsonConvert.SerializeObject(data));
        }
    }

    public async Task<ReferralData?> GetReferralDataByCodeAsync(string referralCode)
    {
        return (await GetAllDataAsync())?.FirstOrDefault(d => d.referralCode == referralCode);
    }

    public async Task<ReferralData?> GetReferralDataByErdAsync(string erdAddress)
    {
        return (await GetAllDataAsync())?.FirstOrDefault(d => d.erdAddress == erdAddress);
    }

    private async Task<IEnumerable<ReferralData>> FetchAllDataAsync()
    {
        if (!File.Exists(dbFullPath))
        {
            return Array.Empty<ReferralData>();
        }

        var content = await File.ReadAllTextAsync(dbFullPath);
        return JsonConvert.DeserializeObject<IEnumerable<ReferralData>>(content) ?? Array.Empty<ReferralData>();
    }

    private async Task<IEnumerable<ReferralData>> GetAllDataAsync()
    {
        var retryCount = 0;
        while (retryCount < 3)
        {
            try
            {
                return await FetchAllDataAsync();
            }
            catch (Exception)
            {
                retryCount++;
                await Task.Delay(100);
            }
        }

        return Array.Empty<ReferralData>();
    }

    private IEnumerable<ReferralData> GetAllData()
    {
        var retryCount = 0;
        while (retryCount < 3)
        {
            try
            {
                return FetchAllData();
            }
            catch (Exception)
            {
                retryCount++;
                Thread.Sleep(100);
            }
        }

        return Array.Empty<ReferralData>();
    }

    private IEnumerable<ReferralData> FetchAllData()
    {
        if (!File.Exists(dbFullPath))
        {
            return Array.Empty<ReferralData>();
        }

        var content = File.ReadAllText(dbFullPath);
        return JsonConvert.DeserializeObject<IEnumerable<ReferralData>>(content) ?? Array.Empty<ReferralData>();
    }
}