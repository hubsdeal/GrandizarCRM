using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SoftGrid.EntityFrameworkCore;

namespace SoftGrid.HealthChecks
{
    public class SoftGridDbContextHealthCheck : IHealthCheck
    {
        private readonly DatabaseCheckHelper _checkHelper;

        public SoftGridDbContextHealthCheck(DatabaseCheckHelper checkHelper)
        {
            _checkHelper = checkHelper;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            if (_checkHelper.Exist("db"))
            {
                return Task.FromResult(HealthCheckResult.Healthy("SoftGridDbContext connected to database."));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("SoftGridDbContext could not connect to database"));
        }
    }
}
