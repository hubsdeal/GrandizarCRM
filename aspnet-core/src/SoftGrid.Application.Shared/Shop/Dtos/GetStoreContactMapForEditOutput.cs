using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetStoreContactMapForEditOutput
    {
        public CreateOrEditStoreContactMapDto StoreContactMap { get; set; }

        public string StoreName { get; set; }

        public string ContactFullName { get; set; }

    }
}