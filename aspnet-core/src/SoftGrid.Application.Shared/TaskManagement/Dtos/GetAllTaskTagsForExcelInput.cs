using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.TaskManagement.Dtos
{
    public class GetAllTaskTagsForExcelInput
    {
        public string Filter { get; set; }

        public string CustomTagFilter { get; set; }

        public string TagValueFilter { get; set; }

        public int? VerfiedFilter { get; set; }

        public int? MaxSequenceFilter { get; set; }
        public int? MinSequenceFilter { get; set; }

        public string TaskEventNameFilter { get; set; }

        public string MasterTagCategoryNameFilter { get; set; }

        public string MasterTagNameFilter { get; set; }

    }
}