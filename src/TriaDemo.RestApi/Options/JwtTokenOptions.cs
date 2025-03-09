namespace TriaDemo.RestApi.Options;

public sealed class JwtTokenOptions
{
    public const string SectionName = "JwtToken";

    public required string Audience { get; init; }

    public required string Issuer { get; init; }
    
    public required string SecretKey { get; init; }
}