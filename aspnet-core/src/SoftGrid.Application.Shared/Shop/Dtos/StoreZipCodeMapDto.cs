using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class StoreZipCodeMapDto : EntityDto<long>
    {
        public string ZipCode { get; set; }

        public long StoreId { get; set; }

        public long? ZipCodeId { get; set; }

    }
}