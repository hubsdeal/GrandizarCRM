using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Territory.Dtos
{
    public class HubStoreDto : EntityDto<long>
    {
        public bool Published { get; set; }

        public int? DisplaySequence { get; set; }

        public long HubId { get; set; }

        public long StoreId { get; set; }

    }
}