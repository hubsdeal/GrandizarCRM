using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class CreateOrEditStoreZipCodeMapDto : EntityDto<long?>
    {

        [StringLength(StoreZipCodeMapConsts.MaxZipCodeLength, MinimumLength = StoreZipCodeMapConsts.MinZipCodeLength)]
        public string ZipCode { get; set; }

        public long StoreId { get; set; }

        public long? ZipCodeId { get; set; }

    }
}