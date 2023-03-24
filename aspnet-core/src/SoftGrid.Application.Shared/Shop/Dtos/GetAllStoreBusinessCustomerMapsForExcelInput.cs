using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Shop.Dtos
{
    public class GetAllStoreBusinessCustomerMapsForExcelInput
    {
        public string Filter { get; set; }

        public int? PaidCustomerFilter { get; set; }

        public double? MaxLifeTimeSalesAmountFilter { get; set; }
        public double? MinLifeTimeSalesAmountFilter { get; set; }

        public string StoreNameFilter { get; set; }

        public string BusinessNameFilter { get; set; }

    }
}