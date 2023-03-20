using SoftGrid.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftGrid.Web.Controllers
{
    public class DocumentUploadController : DocumentUploadControllerBase
    {
        public DocumentUploadController(ITempFileCacheManager tempFileCacheManager) :
            base(tempFileCacheManager)
        {
        }
    }
}
