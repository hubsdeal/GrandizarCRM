using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.CRM.Dtos
{
    public class BusinessTaskMapDto : EntityDto<long>
    {

        public long BusinessId { get; set; }

        public long TaskEventId { get; set; }

    }
}