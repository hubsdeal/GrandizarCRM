using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.LookupData.Dtos
{
    public class MasterTagDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Synonyms { get; set; }

        public int? DisplaySequence { get; set; }

        public long MasterTagCategoryId { get; set; }

        public long? PictureMediaLibraryId { get; set; }

    }
}