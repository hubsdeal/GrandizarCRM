using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Shop.Dtos
{
    public class GetAllStoreRelevantStoresForExcelInput
    {
        public string Filter { get; set; }

        public long? MaxRelevantStoreIdFilter { get; set; }
        public long? MinRelevantStoreIdFilter { get; set; }

        public string StoreNameFilter { get; set; }

    }
}