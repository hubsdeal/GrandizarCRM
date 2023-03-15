using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.LookupData.Dtos
{
    public class MasterTagCategoryDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public long? PictureMediaLibraryId { get; set; }

    }
}