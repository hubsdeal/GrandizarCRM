using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.SalesLeadManagement.Dtos
{
    public class GetAllLeadTagsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string CustomTagFilter { get; set; }

        public string TagValueFilter { get; set; }

        public int? MaxDisplaySequenceFilter { get; set; }
        public int? MinDisplaySequenceFilter { get; set; }

        public string LeadTitleFilter { get; set; }

        public string MasterTagCategoryNameFilter { get; set; }

        public string MasterTagNameFilter { get; set; }

    }
}