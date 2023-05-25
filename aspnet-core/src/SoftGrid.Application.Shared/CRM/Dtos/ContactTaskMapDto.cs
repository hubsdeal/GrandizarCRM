using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.CRM.Dtos
{
    public class ContactTaskMapDto : EntityDto<long>
    {

        public long ContactId { get; set; }

        public long TaskEventId { get; set; }

    }
}