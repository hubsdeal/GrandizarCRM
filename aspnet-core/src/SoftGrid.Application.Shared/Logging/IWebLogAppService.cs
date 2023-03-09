using Abp.Application.Services;
using SoftGrid.Dto;
using SoftGrid.Logging.Dto;

namespace SoftGrid.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
