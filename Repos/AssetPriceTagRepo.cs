using backend_aoz.Models;
using Newtonsoft.Json;

namespace backend_aoz.Repos;

public interface IAssetPriceTagRepo 
{
    void WriteAssetPriceTag(AssetPriceTag tag);
    Task<AssetPriceTag?> GetAssetPriceTagAsync();
}

public class FileAssetPriceTagRepo : BaseFileRepo, IAssetPriceTagRepo
{
    protected override string dbFileName => "egldPriceTag.json";

    public async Task<AssetPriceTag?> GetAssetPriceTagAsync()
    {
        var content = await File.ReadAllTextAsync(dbFullPath);
        return JsonConvert.DeserializeObject<AssetPriceTag>(content);
    }

    public void WriteAssetPriceTag(AssetPriceTag tag)
    {
        File.WriteAllTextAsync(dbFullPath, JsonConvert.SerializeObject(tag));
    }
}

