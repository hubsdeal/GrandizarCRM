using Abp.AspNetCore.Mvc.Authorization;
using SoftGrid.Authorization.Users.Profile;
using SoftGrid.Graphics;
using SoftGrid.Storage;

namespace SoftGrid.Web.Controllers
{
    [AbpMvcAuthorize]
    public class ProfileController : ProfileControllerBase
    {
        public ProfileController(
            ITempFileCacheManager tempFileCacheManager,
            IProfileAppService profileAppService,
            IImageFormatValidator imageFormatValidator) :
            base(tempFileCacheManager, profileAppService, imageFormatValidator)
        {
        }
    }
}