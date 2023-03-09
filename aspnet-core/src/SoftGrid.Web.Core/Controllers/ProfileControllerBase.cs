using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetZeroCore.Net;
using Abp.Extensions;
using Abp.IO.Extensions;
using Abp.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftGrid.Authorization.Users.Profile;
using SoftGrid.Dto;
using SoftGrid.Graphics;
using SoftGrid.Storage;

namespace SoftGrid.Web.Controllers
{
    public abstract class ProfileControllerBase : SoftGridControllerBase
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IProfileAppService _profileAppService;
        private readonly IImageFormatValidator _imageFormatValidator;
        
        private const int MaxProfilePictureSize = 5242880; //5MB

        protected ProfileControllerBase(
            ITempFileCacheManager tempFileCacheManager,
            IProfileAppService profileAppService, 
            IImageFormatValidator imageFormatValidator)
        {
            _tempFileCacheManager = tempFileCacheManager;
            _profileAppService = profileAppService;
            _imageFormatValidator = imageFormatValidator;
        }
        
        public void UploadProfilePicture(FileDto input)
        {
            var profilePictureFile = Request.Form.Files.First();

            //Check input
            if (profilePictureFile == null)
            {
                throw new UserFriendlyException(L("ProfilePicture_Change_Error"));
            }

            if (profilePictureFile.Length > MaxProfilePictureSize)
            {
                throw new UserFriendlyException(L("ProfilePicture_Warn_SizeLimit",
                    AppConsts.MaxProfilePictureBytesUserFriendlyValue));
            }

            byte[] fileBytes;
            using (var stream = profilePictureFile.OpenReadStream())
            {
                fileBytes = stream.GetAllBytes();
                _imageFormatValidator.Validate(fileBytes);
            }

            _tempFileCacheManager.SetFile(input.FileToken, fileBytes);
        }

        [AllowAnonymous]
        public FileResult GetDefaultProfilePicture()
        {
            return GetDefaultProfilePictureInternal();
        }

        public async Task<FileResult> GetProfilePictureByUser(long userId)
        {
            var output = await _profileAppService.GetProfilePictureByUser(userId);
            if (output.ProfilePicture.IsNullOrEmpty())
            {
                return GetDefaultProfilePictureInternal();
            }

            return File(Convert.FromBase64String(output.ProfilePicture), MimeTypeNames.ImageJpeg);
        }

        protected FileResult GetDefaultProfilePictureInternal()
        {
            return File(Path.Combine("Common", "Images", "default-profile-picture.png"), MimeTypeNames.ImagePng);
        }
    }
}