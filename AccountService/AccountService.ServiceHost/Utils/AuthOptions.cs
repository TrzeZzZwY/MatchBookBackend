namespace AccountService.ServiceHost.Utils;

public class AuthOptions
{
    public string Secret { get; init; }

    public string Issuer { get; init; }

    public string Audience { get; init; }
}
