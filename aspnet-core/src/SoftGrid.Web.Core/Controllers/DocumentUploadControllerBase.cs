using Abp.Extensions;
using Abp.IO.Extensions;
using Abp.UI;
using Abp.Web.Models;
using SoftGrid.DemoUiComponents.Dto;
using SoftGrid.Dto;
using SoftGrid.Storage;
using SoftGrid.Url;
using SoftGrid.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SoftGrid.Web.Controllers
{
    public class DocumentUploadControllerBase : SoftGridControllerBase
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;
        protected DocumentUploadControllerBase(
            ITempFileCacheManager tempFileCacheManager)
        {
            _tempFileCacheManager = tempFileCacheManager;
        }

        //file upload controller
        public UploadedFileOutput UploadFile(FileInputDto input)
        {
            try
            {
                var file = Request.Form.Files.First();
                var extension = System.IO.Path.GetExtension(file.FileName);

                //Check input
                if (file == null)
                {
                    throw new UserFriendlyException("File Can't be null");
                }

                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                if (extension.IsIn(".png", ".jpeg", ".jpg"))
                {
                    if (!ImageFormatHelper.GetRawImageFormat(fileBytes).IsIn(ImageFormat.Jpeg, ImageFormat.Png))
                    {
                        throw new Exception(L("IncorrectImageFormat"));
                    }
                    _tempFileCacheManager.SetFile(input.FileToken, fileBytes);
                    using (var bmpImage = new Bitmap(new MemoryStream(fileBytes)))
                    {
                        return new UploadedFileOutput
                        {
                            FileToken = input.FileToken,
                        };
                    }
                }
                _tempFileCacheManager.SetFile(input.FileToken, fileBytes);
                return new UploadedFileOutput
                {
                    FileToken = input.FileToken,
                };
            }
            catch (UserFriendlyException ex)
            {
                return new UploadedFileOutput(new ErrorInfo(ex.Message));
            }
        }
    }
}
