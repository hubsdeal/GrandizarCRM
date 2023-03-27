using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.SalesLeadManagement.Dtos
{
    public class LeadSourceDto : EntityDto<long>
    {
        public string Name { get; set; }

    }
}