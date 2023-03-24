using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetStoreProductMapForEditOutput
    {
        public CreateOrEditStoreProductMapDto StoreProductMap { get; set; }

        public string StoreName { get; set; }

        public string ProductName { get; set; }

    }
}