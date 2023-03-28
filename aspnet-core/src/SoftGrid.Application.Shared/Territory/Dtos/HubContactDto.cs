using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Territory.Dtos
{
    public class HubContactDto : EntityDto<long>
    {
        public int? DisplayScore { get; set; }

        public long HubId { get; set; }

        public long ContactId { get; set; }

    }
}