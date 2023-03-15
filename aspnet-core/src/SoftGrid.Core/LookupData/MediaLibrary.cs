using SoftGrid.LookupData;
using SoftGrid.LookupData;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.LookupData
{
    [Table("MediaLibraries")]
    public class MediaLibrary : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(MediaLibraryConsts.MaxNameLength, MinimumLength = MediaLibraryConsts.MinNameLength)]
        public virtual string Name { get; set; }

        [StringLength(MediaLibraryConsts.MaxSizeLength, MinimumLength = MediaLibraryConsts.MinSizeLength)]
        public virtual string Size { get; set; }

        [StringLength(MediaLibraryConsts.MaxFileExtensionLength, MinimumLength = MediaLibraryConsts.MinFileExtensionLength)]
        public virtual string FileExtension { get; set; }

        [StringLength(MediaLibraryConsts.MaxDimensionLength, MinimumLength = MediaLibraryConsts.MinDimensionLength)]
        public virtual string Dimension { get; set; }

        [StringLength(MediaLibraryConsts.MaxVideoLinkLength, MinimumLength = MediaLibraryConsts.MinVideoLinkLength)]
        public virtual string VideoLink { get; set; }

        [StringLength(MediaLibraryConsts.MaxSeoTagLength, MinimumLength = MediaLibraryConsts.MinSeoTagLength)]
        public virtual string SeoTag { get; set; }

        [StringLength(MediaLibraryConsts.MaxAltTagLength, MinimumLength = MediaLibraryConsts.MinAltTagLength)]
        public virtual string AltTag { get; set; }

        public virtual string VirtualPath { get; set; }

        public virtual Guid BinaryObjectId { get; set; }

        public virtual long? MasterTagCategoryId { get; set; }

        [ForeignKey("MasterTagCategoryId")]
        public MasterTagCategory MasterTagCategoryFk { get; set; }

        public virtual long? MasterTagId { get; set; }

        [ForeignKey("MasterTagId")]
        public MasterTag MasterTagFk { get; set; }

    }
}