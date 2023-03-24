using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetStoreProductCategoryMapForEditOutput
    {
        public CreateOrEditStoreProductCategoryMapDto StoreProductCategoryMap { get; set; }

        public string StoreName { get; set; }

        public string ProductCategoryName { get; set; }

    }
}