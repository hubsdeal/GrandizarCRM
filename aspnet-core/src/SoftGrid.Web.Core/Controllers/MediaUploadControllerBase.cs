using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.IO.Extensions;
using Abp.UI;
using Abp.Web.Models;
using Microsoft.AspNetCore.Http;
using MimeKit;
using SoftGrid.Authorization.Users.Profile.Dto;
using SoftGrid.LookupData;
using SoftGrid.Shop;
using SoftGrid.Shop.Dtos;
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
    public class MediaUploadControllerBase : SoftGridControllerBase
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IAppFolders _appFolders;
        private readonly IWebUrlService _webUrlService;
        private const int MaxProfilePictureSize = 5242880; //5MB
        private readonly IRepository<MediaLibrary, long> _mediaLibraryRepository;
        private readonly IBinaryObjectManager _binaryObjectManager;

        protected MediaUploadControllerBase(
            ITempFileCacheManager tempFileCacheManager,
            IAppFolders appFolders,
            IWebUrlService webUrlService,
            IRepository<MediaLibrary, long> mediaLibraryRepository,
            IBinaryObjectManager binaryObjectManager)
        {
            _tempFileCacheManager = tempFileCacheManager;
            _appFolders = appFolders;
            _webUrlService = webUrlService;
            _mediaLibraryRepository = mediaLibraryRepository;
            _binaryObjectManager = binaryObjectManager;
        }

        //i,age upload controller
        public UploadMediaPictureOutput UploadPicture(MediaLibraryInput input)
        {
            try
            {
                var pictureFile = Request.Form.Files.First();

                //Check input
                if (pictureFile == null)
                {
                    throw new UserFriendlyException(L("Picture_Change_Error"));
                }

                //if (profilePictureFile.Length > MaxProfilePictureSize)
                //{
                //    throw new UserFriendlyException(L("ProfilePicture_Warn_SizeLimit", AppConsts.MaxProfilPictureBytesUserFriendlyValue));
                //}

                byte[] fileBytes;
                using (var stream = pictureFile.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                if (!ImageFormatHelper.GetRawImageFormat(fileBytes).IsIn(ImageFormat.Jpeg, ImageFormat.Png, ImageFormat.Gif, ImageFormat.Bmp, ImageFormat.Emf, ImageFormat.Exif, ImageFormat.Icon))
                {
                    throw new Exception(L("IncorrectImageFormat"));
                }

                _tempFileCacheManager.SetFile(input.FileToken, fileBytes);

                using (var bmpImage = new Bitmap(new MemoryStream(fileBytes)))
                {
                    return new UploadMediaPictureOutput
                    {
                        FileToken = input.FileToken,
                    };
                }
            }
            catch (UserFriendlyException ex)
            {
                return new UploadMediaPictureOutput(new ErrorInfo(ex.Message));
            }
        }

        public async void UploadBulkMedia(IFormFile file, string seoTag, string altTag, long mediaLibraryId)
        {
            try
            {
                var contentType = "";
                var pictureFile = Request.Form.Files.First();
                if (pictureFile == null)
                {
                    throw new UserFriendlyException("Picture Upload Error");
                }

                byte[] fileBinary;
                using (var stream = pictureFile.OpenReadStream())
                {
                    fileBinary = stream.GetAllBytes();
                }

                var fileExtension = Path.GetExtension(pictureFile.FileName);
                if (!String.IsNullOrEmpty(fileExtension))
                    fileExtension = fileExtension.ToLowerInvariant();
                //contentType is not always available 
                //that's why we manually update it here
                //http://www.sfsu.edu/training/mimetype.htm
                if (String.IsNullOrEmpty(contentType))
                {
                    switch (fileExtension)
                    {
                        case ".bmp":
                            contentType = MimeTypes.GetMimeType(pictureFile.FileName);
                            break;
                        case ".gif":
                            contentType = MimeTypes.GetMimeType(pictureFile.FileName);
                            break;
                        case ".jpeg":
                        case ".jpg":
                        case ".jpe":
                        case ".jfif":
                        case ".pjpeg":
                        case ".pjp":
                            contentType = MimeTypes.GetMimeType(pictureFile.FileName);
                            break;
                        case ".png":
                            contentType = MimeTypes.GetMimeType(pictureFile.FileName);
                            break;
                        case ".tiff":
                        case ".tif":
                            contentType = MimeTypes.GetMimeType(pictureFile.FileName);
                            break;
                        default:
                            break;
                    }
                }

                MediaLibrary createOrEditMediaLibraryDto = new MediaLibrary
                {
                    Name = pictureFile.FileName,
                    FileExtension = contentType,
                    SeoTag = seoTag,
                    AltTag = altTag,
                    MasterTagCategoryId = 1,
                    MasterTagId = mediaLibraryId
                };

                Image image = Image.FromStream(new MemoryStream(fileBinary));
                createOrEditMediaLibraryDto.Dimension = image.Width.ToString() + "*" + image.Height.ToString();
                createOrEditMediaLibraryDto.Size = (fileBinary.Length / 1024).ToString() + " kb";
                var storedFile = new BinaryObject(AbpSession.TenantId, fileBinary);
                await _binaryObjectManager.SaveAsync(storedFile);
                createOrEditMediaLibraryDto.BinaryObjectId = storedFile.Id;
                await _mediaLibraryRepository.InsertAsync(createOrEditMediaLibraryDto);
            }
            catch (UserFriendlyException ex)
            {
                throw new UserFriendlyException(ex.ToString());
            }
        }
    }
}
