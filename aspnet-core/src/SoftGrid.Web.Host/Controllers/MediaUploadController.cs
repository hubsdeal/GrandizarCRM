using Abp.Domain.Repositories;
using SoftGrid.LookupData;
using SoftGrid.Shop;
using SoftGrid.Storage;
using SoftGrid.Url;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftGrid.Web.Controllers
{
    public class MediaUploadController : MediaUploadControllerBase
    {
        public MediaUploadController(ITempFileCacheManager tempFileCacheManager, IAppFolders appFolders, IWebUrlService webUrlService, IRepository<MediaLibrary, long> mediaLibraryRepository, IBinaryObjectManager binaryObjectManager) :
            base(tempFileCacheManager, appFolders, webUrlService, mediaLibraryRepository, binaryObjectManager)
        {
        }
    }
}
    