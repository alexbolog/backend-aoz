using Microsoft.AspNetCore.Mvc;
using backend_aoz.Repos;
using backend_aoz.Models;

namespace backend_aoz.Controllers;

[ApiController]
[Route("[controller]")]
public class ReferralController : ControllerBase
{
    private readonly ILogger<ReferralController> _logger;
    private readonly IReferralCodeRepo codeRepo;
    private static Random random = new Random();

    public ReferralController(ILogger<ReferralController> logger, IReferralCodeRepo codeRepo)
    {
        _logger = logger;
        this.codeRepo = codeRepo;
    }

    [HttpGet("code/{erdAddress}")]
    public async Task<string> GetReferralCodeAsync(string erdAddress)
    {
        var existingData = await codeRepo.GetReferralDataByErdAsync(erdAddress);
        return existingData?.referralCode ?? string.Empty;
    }

    [HttpGet("generateCode/{erdAddress}")]
    public async Task<string> GenerateReferralCodeAsync(string erdAddress)
    {
        var existingData = await codeRepo.GetReferralDataByErdAsync(erdAddress);
        if (existingData != null)
        {
            return existingData.referralCode;
        }
        var data = GenerateNewData(erdAddress);
        codeRepo.AddReferralData(data);

        return data.referralCode;
    }

    [HttpGet("erd/{code}")]
    public async Task<string> GetReferralErdByCodeAsync(string code)
    {
        return (await codeRepo.GetReferralDataByCodeAsync(code))?.erdAddress ?? string.Empty;
    }

    private ReferralData GenerateNewData(string erdAddress) => new ReferralData(erdAddress, RandomString(10));
    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
