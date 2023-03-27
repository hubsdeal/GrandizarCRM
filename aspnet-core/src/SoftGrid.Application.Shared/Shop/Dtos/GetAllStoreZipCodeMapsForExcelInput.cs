using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Shop.Dtos
{
    public class GetAllStoreZipCodeMapsForExcelInput
    {
        public string Filter { get; set; }

        public string ZipCodeFilter { get; set; }

        public string StoreNameFilter { get; set; }

        public string ZipCodeNameFilter { get; set; }

    }
}