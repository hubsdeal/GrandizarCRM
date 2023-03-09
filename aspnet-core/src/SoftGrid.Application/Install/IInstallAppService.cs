using System.Threading.Tasks;
using Abp.Application.Services;
using SoftGrid.Install.Dto;

namespace SoftGrid.Install
{
    public interface IInstallAppService : IApplicationService
    {
        Task Setup(InstallDto input);

        AppSettingsJsonDto GetAppSettingsJson();

        CheckDatabaseOutput CheckDatabase();
    }
}