using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class CreateOrEditStoreContactMapDto : EntityDto<long?>
    {

        public bool PaidCustomer { get; set; }

        public double? LifeTimeSalesAmount { get; set; }

        public long StoreId { get; set; }

        public long ContactId { get; set; }

    }
}