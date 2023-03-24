using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class StoreBusinessCustomerMapDto : EntityDto
    {
        public bool PaidCustomer { get; set; }

        public double? LifeTimeSalesAmount { get; set; }

        public long StoreId { get; set; }

        public long BusinessId { get; set; }

    }
}