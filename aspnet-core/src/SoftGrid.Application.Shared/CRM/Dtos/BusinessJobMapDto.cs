using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.CRM.Dtos
{
    public class BusinessJobMapDto : EntityDto<long>
    {

        public long BusinessId { get; set; }

        public long JobId { get; set; }

    }
}