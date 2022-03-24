namespace backend_aoz.Models;

public class AssetPriceTag
{
    public DateTime time { get; set; }
    public string asset_id_base { get; set; }
    public string asset_id_quote { get; set; }
    public decimal rate { get; set; }
}