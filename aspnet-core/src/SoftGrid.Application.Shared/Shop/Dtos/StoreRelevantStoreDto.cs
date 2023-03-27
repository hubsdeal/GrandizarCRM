using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class StoreRelevantStoreDto : EntityDto<long>
    {
        public long RelevantStoreId { get; set; }

        public long PrimaryStoreId { get; set; }

    }
}