using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.SalesLeadManagement.Dtos
{
    public class LeadTagDto : EntityDto<long>
    {
        public string CustomTag { get; set; }

        public string TagValue { get; set; }

        public int? DisplaySequence { get; set; }

        public long LeadId { get; set; }

        public long? MasterTagCategoryId { get; set; }

        public long? MasterTagId { get; set; }

    }
}