using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.LookupData.Dtos
{
    public class CreateOrEditMediaLibraryDto : EntityDto<long?>
    {

        [Required]
        [StringLength(MediaLibraryConsts.MaxNameLength, MinimumLength = MediaLibraryConsts.MinNameLength)]
        public string Name { get; set; }

        [StringLength(MediaLibraryConsts.MaxSizeLength, MinimumLength = MediaLibraryConsts.MinSizeLength)]
        public string Size { get; set; }

        [StringLength(MediaLibraryConsts.MaxFileExtensionLength, MinimumLength = MediaLibraryConsts.MinFileExtensionLength)]
        public string FileExtension { get; set; }

        [StringLength(MediaLibraryConsts.MaxDimensionLength, MinimumLength = MediaLibraryConsts.MinDimensionLength)]
        public string Dimension { get; set; }

        [StringLength(MediaLibraryConsts.MaxVideoLinkLength, MinimumLength = MediaLibraryConsts.MinVideoLinkLength)]
        public string VideoLink { get; set; }

        [StringLength(MediaLibraryConsts.MaxSeoTagLength, MinimumLength = MediaLibraryConsts.MinSeoTagLength)]
        public string SeoTag { get; set; }

        [StringLength(MediaLibraryConsts.MaxAltTagLength, MinimumLength = MediaLibraryConsts.MinAltTagLength)]
        public string AltTag { get; set; }

        public string VirtualPath { get; set; }

        public Guid BinaryObjectId { get; set; }

        public long? MasterTagCategoryId { get; set; }

        public long? MasterTagId { get; set; }

        public string FileToken { get; set; }

    }
}