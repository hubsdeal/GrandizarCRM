using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.CRM.Dtos
{
    public class GetAllContactTagsForExcelInput
    {
        public string Filter { get; set; }

        public string CustomTagFilter { get; set; }

        public string TagValueFilter { get; set; }

        public int? VerifiedFilter { get; set; }

        public int? MaxSequenceFilter { get; set; }
        public int? MinSequenceFilter { get; set; }

        public string ContactFullNameFilter { get; set; }

        public string MasterTagCategoryNameFilter { get; set; }

        public string MasterTagNameFilter { get; set; }

    }
}