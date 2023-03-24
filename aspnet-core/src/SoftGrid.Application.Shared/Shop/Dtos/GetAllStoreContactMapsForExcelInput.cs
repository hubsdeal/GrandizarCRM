using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Shop.Dtos
{
    public class GetAllStoreContactMapsForExcelInput
    {
        public string Filter { get; set; }

        public int? PaidCustomerFilter { get; set; }

        public double? MaxLifeTimeSalesAmountFilter { get; set; }
        public double? MinLifeTimeSalesAmountFilter { get; set; }

        public string StoreNameFilter { get; set; }

        public string ContactFullNameFilter { get; set; }

    }
}