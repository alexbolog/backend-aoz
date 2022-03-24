using Microsoft.AspNetCore.Mvc;
using backend_aoz.Repos;
using backend_aoz.Models;

namespace backend_aoz.Controllers;

[ApiController]
[Route("[controller]")]
public class PriceTagController : ControllerBase
{
    private readonly ILogger<PriceTagController> _logger;
    private readonly IAssetPriceTagRepo repo;
    public PriceTagController(ILogger<PriceTagController> logger)
    {
        _logger = logger;
        repo = new FileAssetPriceTagRepo();
    }

    [HttpGet]
    public async Task<decimal> GetPriceTagAsync()
    {
        var priceTag = await repo.GetAssetPriceTagAsync();
        return priceTag?.rate ?? 0m;
    }
}
