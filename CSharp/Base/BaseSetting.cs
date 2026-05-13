using Microsoft.Extensions.Configuration;

namespace Zuhid.Base;

public abstract class BaseSetting
{
    public string Name { get; init; } = default!;
    public string Version { get; init; } = "v1";

    public abstract void Bind(IConfiguration configuration);
}
