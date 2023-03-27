using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetStoreZipCodeMapForEditOutput
    {
        public CreateOrEditStoreZipCodeMapDto StoreZipCodeMap { get; set; }

        public string StoreName { get; set; }

        public string ZipCodeName { get; set; }

    }
}