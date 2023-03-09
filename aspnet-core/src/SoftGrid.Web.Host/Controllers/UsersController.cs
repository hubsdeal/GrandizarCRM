using Abp.AspNetCore.Mvc.Authorization;
using SoftGrid.Authorization;
using SoftGrid.Storage;
using Abp.BackgroundJobs;

namespace SoftGrid.Web.Controllers
{
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Users)]
    public class UsersController : UsersControllerBase
    {
        public UsersController(IBinaryObjectManager binaryObjectManager, IBackgroundJobManager backgroundJobManager)
            : base(binaryObjectManager, backgroundJobManager)
        {
        }
    }
}