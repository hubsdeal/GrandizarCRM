using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.LookupData.Dtos
{
    public class MediaLibraryDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Size { get; set; }

        public string FileExtension { get; set; }

        public string Dimension { get; set; }

        public string VideoLink { get; set; }

        public string SeoTag { get; set; }

        public string AltTag { get; set; }

        public string VirtualPath { get; set; }

        public Guid BinaryObjectId { get; set; }

        public long? MasterTagCategoryId { get; set; }

        public long? MasterTagId { get; set; }

    }
}