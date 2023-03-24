using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetStoreBusinessHourForEditOutput
    {
        public CreateOrEditStoreBusinessHourDto StoreBusinessHour { get; set; }

        public string StoreName { get; set; }

        public string MasterTagCategoryName { get; set; }

        public string MasterTagName { get; set; }

    }
}