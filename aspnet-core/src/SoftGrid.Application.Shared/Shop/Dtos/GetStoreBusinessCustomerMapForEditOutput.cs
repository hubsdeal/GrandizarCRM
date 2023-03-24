using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetStoreBusinessCustomerMapForEditOutput
    {
        public CreateOrEditStoreBusinessCustomerMapDto StoreBusinessCustomerMap { get; set; }

        public string StoreName { get; set; }

        public string BusinessName { get; set; }

    }
}