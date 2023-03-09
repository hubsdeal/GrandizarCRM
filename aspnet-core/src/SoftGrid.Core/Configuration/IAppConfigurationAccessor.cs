using Microsoft.Extensions.Configuration;

namespace SoftGrid.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
