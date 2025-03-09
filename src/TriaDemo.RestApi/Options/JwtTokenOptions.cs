namespace TriaDemo.RestApi.Options;

public sealed class JwtTokenOptions
{
    public const string SectionName = "JwtToken";

    public string? Audience { get; set; }

    public string? Issuer { get; set; }
    
    public string? SecretKey { get; set; }
}