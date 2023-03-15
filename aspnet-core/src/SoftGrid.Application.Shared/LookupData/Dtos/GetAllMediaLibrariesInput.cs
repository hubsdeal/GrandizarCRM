using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.LookupData.Dtos
{
    public class GetAllMediaLibrariesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string SizeFilter { get; set; }

        public string FileExtensionFilter { get; set; }

        public string DimensionFilter { get; set; }

        public string VideoLinkFilter { get; set; }

        public string SeoTagFilter { get; set; }

        public string AltTagFilter { get; set; }

        public string VirtualPathFilter { get; set; }

        public Guid? BinaryObjectIdFilter { get; set; }

        public string MasterTagCategoryNameFilter { get; set; }

        public string MasterTagNameFilter { get; set; }

    }
}