namespace backend_aoz.Models;

public class ReferralData
{
    public ReferralData()
    {
        referralCode = string.Empty;
        erdAddress = string.Empty;
    }
    public ReferralData(string erdAddress, string referralCode)
    {
        this.erdAddress = erdAddress;
        this.referralCode = referralCode;
    }
    public string erdAddress { get; set; }

    public string referralCode {get;set;}
}