namespace MusicStore.Entities;

public class AppSettings
{
    public StorageConfiguration StorageConfiguration { get; set; } = default!;

    public Jwt Jwt { get; set; } = default!;
    
    public SmtpConfiguration SmtpConfiguration { get; set; } = default!;

}

public class Jwt
{
    public string Secret { get; set; } = default!;
    public string Audience { get; set; } = default!;

    public string Issuer { get; set; } = default!;
}

public class SmtpConfiguration
{
    public string ApiKey { get; set; } = default!;
    public string From { get; set; } = default!;

    public string FromName { get; set; } = default!;
}

public class StorageConfiguration
{
    public string Path { get; set; } = default!;
    public string PublicUrl { get; set; } = default!;
}

