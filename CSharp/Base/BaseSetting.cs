using Microsoft.Extensions.Configuration;

namespace Zuhid.Base;

public abstract class BaseSetting
{
    public string Name { get; init; } = default!;
    public string Version { get; init; } = "v1";
    public JwtOptions Jwt { get; set; } = default!;

    protected BaseSetting(IConfiguration configuration)
    {
        configuration.Bind(this);
    }

    public class JwtOptions
    {
        public string PrivateKeyPath { get; set; } = default!;
        public string PublicKeyPath { get; set; } = default!;
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
        public int ExpiryInMinutes { get; set; }
    }
}
