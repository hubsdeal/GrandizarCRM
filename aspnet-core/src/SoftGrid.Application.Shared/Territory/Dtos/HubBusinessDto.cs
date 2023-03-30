using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Territory.Dtos
{
    public class HubBusinessDto : EntityDto<long>
    {
        public bool Published { get; set; }

        public int? DisplayScore { get; set; }

        public long HubId { get; set; }

        public long BusinessId { get; set; }

    }
}